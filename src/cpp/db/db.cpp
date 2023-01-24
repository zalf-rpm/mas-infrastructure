/* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

/*
Authors: 
Michael Berg <michael.berg@zalf.de>

Maintainers: 
Currently maintained by the authors.

This file is part of the util library used by models created at the Institute of 
Landscape Systems Analysis at the ZALF.
Copyright (C) Leibniz Centre for Agricultural Landscape Research (ZALF)
*/

#include "db.h"

#include <iostream>
#include <cstring>
#include <algorithm>
#include <sstream>

#include "tools/helper.h"

using namespace std;
using namespace Db;

#ifndef NO_MYSQL
MysqlDB::MysqlDB(const string& host, const string& user, const string& pwd,
     const string& schema, unsigned int port)
: _host(host)
, _user(user)
, _pwd(pwd)
, _schema(schema)
, _port(port) {
  //commented out to make the DB connections initialized lazily
  //init();
}

MysqlDB::~MysqlDB() {
  freeResultSet();
  mysql_close(_connection);
}

void MysqlDB::lazyInit() {
  if(!_initialized) {
   init();
   _initialized = true;
  }
}

void MysqlDB::init() {
  _connection = mysql_init(NULL);
  my_bool reconnect(1);
  mysql_options(_connection, MYSQL_OPT_RECONNECT, &reconnect);
  unsigned int timeout = 10; //seconds
  mysql_options(_connection, MYSQL_OPT_READ_TIMEOUT, &timeout);
//  mysql_options(_connection, MYSQL_OPT_CONNECT_TIMEOUT, &timeout);

  if(_connection) {
  _isConnected = true;
  if(!mysql_real_connect(_connection, _host.c_str(),
               _user.c_str(), _pwd.c_str(), _schema.c_str(),
               _port, NULL, 0)) {
    _isConnected = false;
    cout << "Error connecting to database: " << mysql_error(_connection)
       << endl;
  } else {
    //cerr << "Connected to database: " << dbase << endl;
  }
  } else {
  cout << "Couldn't even initialize database connection" << endl;
  }
  _initialized = true;
}

bool MysqlDB::exec(const char* query) {
  lazyInit();

  //cout << "query: " << sel << endl;
  int t = mysql_real_query(_connection, query, (unsigned int)strlen(query));
  if(t != 0) {
  cout << "Error during query: " << query
  << " \nerror: " << mysql_error(_connection) << endl;
  return false;
  } else {
  //cerr << "Done query: " << sel << endl;
  }

  //in order to not create a memory leak, on repeatedly calling select
  freeResultSet();

  _resultSet = mysql_store_result(_connection);
  return true;
}

MYSQL_FIELD* MysqlDB::getFields() {
  lazyInit();
  return _resultSet ? mysql_fetch_fields(_resultSet) : NULL;
}

MYSQL_FIELD* MysqlDB::getNextField() {
  lazyInit();
  return _resultSet ? mysql_fetch_field(_resultSet) : NULL;
}

size_t MysqlDB::getNumberOfFields() {
  lazyInit();
  return _resultSet ? size_t(mysql_num_fields(_resultSet)) : 0;
}

size_t MysqlDB::getNumberOfRows() {
  lazyInit();
  return _resultSet ? size_t(mysql_num_rows(_resultSet)) : 0;
}

DBRow MysqlDB::getRow() {
  lazyInit();

  DBRow row;
  if(!_resultSet) return row;

  MYSQL_ROW mrow = mysql_fetch_row(_resultSet);
  if(mrow != 0) {
    for(int i = 0, noOfFields = getNumberOfFields(); i < noOfFields; i++) {
      row.push_back(mrow[i] ? mrow[i] : string());
    }
  }

  return row;
}

MYSQL_ROW MysqlDB::getMysqlRow() {
  lazyInit();
  return _resultSet ? mysql_fetch_row(_resultSet) : NULL;
}

void MysqlDB::freeResultSet() {
  lazyInit();

  if(_resultSet) {
  mysql_free_result(_resultSet);
  _resultSet = NULL;
  }
}

std::string MysqlDB::toDBDate(Tools::Date date) const {
  ostringstream s;
  s << date.year()
  << "-" << (date.month() < 10 ? "0" : "") << date.month()
  << "-" << (date.day() < 10 ? "0" : "") << date.day();
  return s.str();
}

void MysqlDB::setCharacterSet(const char* charsetName) {
  lazyInit();

  if(mysql_set_character_set(_connection, charsetName)){
  cout << "Couldn't set new client character set: " << charsetName << endl;
  //<< mysql_character_set_name(&_connection) << endl;
  }
}

int MysqlDB::insertId() {
  lazyInit();
  return mysql_insert_id(_connection);
}
#endif

//------------------------------------------------------------------------------

#ifndef NO_SQLITE	
SqliteDB::SqliteDB(const string& filename) : _filename(filename) {
  //commented out to make the DB connections initialized lazily
  //init();
}

SqliteDB::~SqliteDB() {
  freeResultSet();
  sqlite3_close(_db);
}

void SqliteDB::lazyInit() {
  if(!_initialized) {
   init();
   _initialized = true;
  }
}

void SqliteDB::init() {
  string utf8filename = _filename; // linux default codepage is utf-8 
#ifdef WIN32
  utf8filename = Tools::winStringSystemCodepageToutf8(_filename);
#endif
  int rc = sqlite3_open_v2(utf8filename.c_str(), &_db, SQLITE_OPEN_READONLY, NULL);
  _isConnected = rc == SQLITE_OK;
  if(rc) {
    cout << "Can't open sqlite database: " << _filename << ". Error: " << sqlite3_errmsg(_db) << endl;
    sqlite3_close(_db);
    exit(1);
  }

  addNeededSQLFunctions();
  _initialized = true;
}

bool SqliteDB::exec(const char* query) {
  lazyInit();

  //first finalize potential previous statement
  freeResultSet();

  int rc = sqlite3_prepare_v2(_db, query, -1, &_ppStmt, NULL);
  if(rc != SQLITE_OK) {
    cout << "Error during query: " << query << endl
      << ". Error: " << sqlite3_errmsg(_db) << endl;
    //sqlite3_free(zErrMsg);
    return false;
  } else {
    _query = query;
  //cerr << "Done query: " << sel << endl;
  }

  //in order to not create a memory leak, on repeatedly calling select
  //freeResultSet();
  
  return true;
}

bool SqliteDB::inUpDel(const char* inUpDelStatement) {
  if(exec(inUpDelStatement))
  {
    int rc2 = sqlite3_step(_ppStmt);
    switch(rc2) {
    case SQLITE_DONE:
      return true;
    case SQLITE_ROW:
      cerr << "Error during executing sql statement. Seams "
            "it was no insert/update/delete statement." << endl;
      return false;
    case SQLITE_ERROR:
      return false;
    default:;
    }
  }

  return false;
}

string SqliteDB::errorMsg() {
  return sqlite3_errmsg(_db);
}

size_t SqliteDB::getNumberOfFields() {
  lazyInit();
  return size_t(sqlite3_column_count(_ppStmt));
}

size_t SqliteDB::getNumberOfRows() {
  lazyInit();

  //old position of cursor is stored in _currentRowNo
  size_t noOfRows = _currentRowNo;
  while(sqlite3_step(_ppStmt) != SQLITE_DONE) ++noOfRows;

  sqlite3_reset(_ppStmt);
  //restore old cursor position 
  for(uint i = 1; i <= _currentRowNo; i++) sqlite3_step(_ppStmt);
  
  return noOfRows;
}

DBRow SqliteDB::getRow() {
  lazyInit();

  DBRow row;

  int rc = sqlite3_step(_ppStmt);
  switch(rc) {
  case SQLITE_ROW: {
    _currentRowNo++;
    int colCount = sqlite3_column_count(_ppStmt);
    for(int i = 0; i < colCount; i++) {
      int type = sqlite3_column_type(_ppStmt, i);
      switch(type) {
      case SQLITE_INTEGER:
      //row.push_back(to_string(sqlite3_column_int(_ppStmt, i)));
        row.push_back((const char*)sqlite3_column_text(_ppStmt, i));
        break;
      case SQLITE_FLOAT:
        //row.push_back(to_string(sqlite3_column_double(_ppStmt, i)));
        row.push_back((const char*)sqlite3_column_text(_ppStmt, i));
        break;
      case SQLITE_TEXT:
      case SQLITE_BLOB:
        row.push_back((const char*)sqlite3_column_text(_ppStmt, i));
        break;
      case SQLITE_NULL:
        row.push_back("");
        break;
      default:;
      }
  //    const char* col = (const char*)sqlite3_column_text(_ppStmt, i);
  //			row.push_back(col ? col : string());
    }
  }
  break;
  case SQLITE_DONE: 
    sqlite3_reset(_ppStmt);
    _currentRowNo = 0;
    break;
  default: //error
    cerr << "Error during getRow in: " << _query << endl
         << ". Error: " << sqlite3_errmsg(_db) << endl;
  }

  return row;
}

void SqliteDB::freeResultSet() {
  lazyInit();

  if(_ppStmt) {
    int rc = sqlite3_finalize(_ppStmt);
    if(rc != SQLITE_OK) {
      cerr << "Error while finalizing sqlite prepared statement belonging to query: " << _query << endl
        << ". Error: " << sqlite3_errmsg(_db) << endl;
            
    }
  _ppStmt = NULL;
  }
}

std::string SqliteDB::toDBDate(Tools::Date date) const {
  ostringstream s;
  s << date.year()
  << "-" << (date.month() < 10 ? "0" : "") << date.month()
  << "-" << (date.day() < 10 ? "0" : "") << date.day();
  return s.str();
}

void SqliteDB::setCharacterSet(const char* charsetName) {
  lazyInit();
  return;
  
  //if(mysql_set_character_set(_connection, charsetName))
  //	cout << "Couldn't set new client character set: " << charsetName << endl;
    //<< mysql_character_set_name(&_connection) << endl;
}

int SqliteDB::insertId() {
  lazyInit();
  return int(sqlite3_last_insert_rowid(_db));
}

namespace _ {
  void sqlite_mod(sqlite3_context* c, int argc, sqlite3_value** argv) {
    if(argc == 2) {
      int left = sqlite3_value_int(argv[0]);
      int right = sqlite3_value_int(argv[1]);
      sqlite3_result_int(c, left % right);
    }
    else sqlite3_result_error(c, "Wrong numer of arguments to mod function.", -1);
  }
}

void SqliteDB::addNeededSQLFunctions() {
  sqlite3_create_function(_db, "mod", 2, SQLITE_ANY, NULL, _::sqlite_mod, NULL, NULL);
}

bool SqliteDB::attachDB(std::string pathToDB, std::string alias) {
  ostringstream s;
  s << "attach database '" << pathToDB << "' as '" << alias << "'";
  return inUpDel(s.str().c_str());
}
#endif

//--------------------------------------------------------------------------------------------

DB* Db::newConnection(const DBConData& cd) {
  DB* db = NULL;

#ifndef NO_SQLITE
  if(cd.isSqliteDB()) db = new SqliteDB(cd.filename);
#endif

#ifndef NO_MYSQL
  if(cd.isMysqlDB()) db = new MysqlDB(cd.host, cd.user, cd.pwd, cd.schema, cd.port);
#endif

  if(db) db->setAbstractSchemaName(cd.abstractSchemaName);

  return db;
}

//------------------------------------------------------------------------------

string DBConData::toString() const {
  ostringstream s;
  if(isSqliteDB()) s << "Sqlite-DB filename: " << filename;
  else if(isMysqlDB()) {
    s << "host: " << host << " user: " << user << " pwd: " << pwd
    << " schema: " << schema << " port: " << port;
  } else s << "Invalid DB connection data.";
  return s.str();
}

