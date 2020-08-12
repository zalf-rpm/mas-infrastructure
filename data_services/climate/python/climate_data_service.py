from scipy.interpolate import NearestNDInterpolator
import numpy as np
import sys
import os
from datetime import date, timedelta
import pandas as pd
from pyproj import Proj, transform
import json
import time

import capnp
capnp.add_import_hook(additional_paths=["../capnproto_schemas/", "../capnproto_schemas/capnp_schemas/"])
import common_capnp as c
import model_capnp as m
import climate_data_old_capnp as cd

def read_header(path_to_ascii_grid_file):
    "read metadata from esri ascii grid file"
    metadata = {}
    header_str = ""
    with open(path_to_ascii_grid_file) as _:
        for i in range(0, 6):
            line = _.readline()
            header_str += line
            sline = [x for x in line.split() if len(x) > 0]
            if len(sline) > 1:
                metadata[sline[0].strip().lower()] = float(sline[1].strip())
    return metadata, header_str

def create_ascii_grid_interpolator(arr, meta, ignore_nodata=True):
    "read an ascii grid into a map, without the no-data values"

    rows, cols = arr.shape

    cellsize = int(meta["cellsize"])
    xll = int(meta["xllcorner"])
    yll = int(meta["yllcorner"])
    nodata_value = meta["nodata_value"]

    xll_center = xll + cellsize // 2
    yll_center = yll + cellsize // 2
    yul_center = yll_center + (rows - 1)*cellsize

    points = []
    values = []

    for row in range(rows):
        for col in range(cols):
            value = arr[row, col]
            if ignore_nodata and value == nodata_value:
                continue
            r = xll_center + col * cellsize
            h = yul_center - row * cellsize
            points.append([r, h])
            values.append(value)

    return NearestNDInterpolator(np.array(points), np.array(values))

def read_file_and_create_interpolator(path_to_grid, dtype=int, skiprows=6, confirm_creation=False):
    "read file and metadata and create interpolator"

    metadata, _ = read_header(path_to_grid)
    grid = np.loadtxt(path_to_grid, dtype=dtype, skiprows=skiprows)
    interpolate = create_ascii_grid_interpolator(grid, metadata)
    if confirm_creation: 
        print("created interpolator from:", path_to_grid)
    return (interpolate, grid, metadata)

wgs84 = Proj(init="epsg:4326")
gk3 = Proj(init="epsg:3396")
gk5 = Proj(init="epsg:31469")
utm21s = Proj(init="epsg:32721")
utm32n = Proj(init="epsg:25832")

cdict = {}
def create_climate_gk5_interpolator_from_json_file(path_to_latlon_to_rowcol_file, wgs84, gk5):
    "create interpolator from json list of lat/lon to row/col mappings"
    with open(path_to_latlon_to_rowcol_file) as _:
        points = []
        values = []

        for latlon, rowcol in json.load(_):
            row, col = rowcol
            clat, clon = latlon
            try:
                cr_gk5, ch_gk5 = transform(wgs84, gk5, clon, clat)
                cdict[(row, col)] = (round(clat, 4), round(clon, 4))
                points.append([cr_gk5, ch_gk5])
                values.append((row, col))
                #print("row:", row, "col:", col, "clat:", clat, "clon:", clon, "h:", h, "r:", r, "val:", values[i])
            except:
                continue

        return NearestNDInterpolator(np.array(points), np.array(values))

#climate_data_to_gk5_interpolator = {}
#for run_id in run_setups:
#    setup = setups[run_id]
#    climate_data = setup["climate_data"]
#    if not climate_data in climate_data_to_gk5_interpolator:
#        path = paths["path-to-climate-dir"] + climate_data + "/csvs/latlon-to-rowcol.json"
#        climate_data_to_gk5_interpolator[climate_data] = create_climate_gk5_interpolator_from_json_file(path, wgs84, gk5)
#        print "created climate_data to gk5 interpolator: ", path

def geo_coord_to_latlon(geo_coord):

    if not hasattr(geo_coord_to_latlon, "gk_cache"):
        geo_coord_to_latlon.gk_cache = {}
    if not hasattr(geo_coord_to_latlon, "utm_cache"):
        geo_coord_to_latlon.utm_cache = {}

    which = geo_coord.which()
    if which == "gk":
        meridian = geo_coord.gk.meridianNo
        if meridian not in geo_coord_to_latlon.gk_cache:
            geo_coord_to_latlon.gk_cache[meridian] = Proj(init="epsg:" + str(cd.Geo.EPSG["gk" + str(meridian)]))
        lon, lat = transform(geo_coord_to_latlon.gk_cache[meridian], wgs84, geo_coord.gk.r, geo_coord.gk.h)
    elif which == "latlon":
        lat, lon = geo_coord.latlon.lat, geo_coord.latlon.lon
    elif which == "utm":
        utm_id = str(geo_coord.utm.zone) + geo_coord.utm.latitudeBand
        if meridian not in geo_coord_to_latlon.utm_cache:
            geo_coord_to_latlon.utm_cache[utm_id] = \
                Proj(init="epsg:" + str(cd.Geo.EPSG["utm" + utm_id]))
        lon, lat = transform(geo_coord_to_latlon.utm_cache[utm_id], wgs84, geo_coord.utm.r, geo_coord.utm.h)

    return lat, lon


class Isimip_CSV_Station(cd.Climate.Station.Server):

    def __init__(self, sim, id, geo_coord, name=None, description=None):
        self.sim = sim
        self.id = id
        self.name = name if name else id
        self.description = description if description else ""
        self.time_series_s = []
        self.geo_coord = geo_coord

    def info(self):
        return c.Common.IdInformation.new_message(id=self.id, name=self.name, description=self.description) 

    def info_context(self, context): # -> (info :IdInformation);
        context.results.info = self.info()

    def simulationInfo_context(self, context): # -> (simInfo :IdInformation);
        context.results.simInfo = self.sim.info()

    def heightNN_context(self, context): # -> (heightNN :Int32);
        context.results.heightNN = 0

    def geoCoord_context(self, context): # -> (geoCoord :Geo.Coord);
        pass
        #return {"gk": {"meridianNo": 5, "r": 1, "h": 2}}

    def allTimeSeries_context(self, context): # -> (allTimeSeries :List(TimeSeries));
        # get all time series available at this station 
        
        if len(self.time_series_s) == 0:
            for scen in self.sim.scenarios:
                for real in scen.realizations:
                    self.time_series_s.append(real.closest_time_series_at(self.geo_coord))
        
        context.results.init("allTimeSeries", len(self.time_series_s))
        for i, ts in enumerate(self.time_series_s):
            context.result.allTimeSeries[i] = ts


    def timeSeriesFor_context(self, context): # (scenarioId :Text, realizationId :Text) -> (timeSeries :TimeSeries);
        pass


def create_date(capnp_date):
    return date(capnp_date.year, capnp_date.month, capnp_date.day)

def create_capnp_date(py_date):
    return {
        "year": py_date.year if py_date else 0,
        "month": py_date.month if py_date else 0,
        "day": py_date.day if py_date else 0
    }
    
class Isimip_CSV_TimeSeries(cd.Climate.TimeSeries.Server): 

    def __init__(self, realization, dataframe, headers=None, start_date=None, end_date=None):
        self._df = dataframe.rename(columns={"windspeed": "wind"})
        self._data = None
        self._headers = headers
        self._start_date = start_date if start_date else \
            (date.fromisoformat(dataframe.index[0]) if len(dataframe) > 0 else None)
        self._end_date = end_date if end_date else \
            (date.fromisoformat(dataframe.index[-1]) if len(dataframe) > 0 else None)
        self._real = realization

    @classmethod
    def create_time_series(cls, realization, path_to_rows_dir, row, col):
        df = pd.read_csv(path_to_rows_dir + "row-" + str(row) + "/col-" + str(col) + ".csv", skiprows=[1], index_col=0)
        time_series = Isimip_CSV_TimeSeries(realization, df)
        return time_series

    def resolution_context(self, context): # -> (resolution :TimeResolution);
        context.results.resolution = cd.Climate.TimeResolution.daily

    def range_context(self, context): # -> (startDate :Date, endDate :Date);
        context.results.startDate = create_capnp_date(self._start_date)
        context.results.endDate = create_capnp_date(self._end_date)
        
    def header(self, **kwargs): # () -> (header :List(Element));
        return self._df.columns.tolist()

    def data(self, **kwargs): # () -> (data :List(List(Float32)));
        return self._df.to_numpy().tolist()

    def dataT(self, **kwargs): # () -> (data :List(List(Float32)));
        return self._df.T.to_numpy().tolist()
                
    def subrange_context(self, context): # (from :Date, to :Date) -> (timeSeries :TimeSeries);
        from_date = create_date(getattr(context.params, "from"))
        to_date = create_date(context.params.to)

        sub_df = self._df.loc[str(from_date):str(to_date)]

        context.results.timeSeries = Isimip_CSV_TimeSeries( \
            self._real, sub_df, headers=self._headers, \
            start_date=from_date, end_date=to_date)
        
    def subheader_context(self, context): # (elements :List(Element)) -> (timeSeries :TimeSeries);
        sub_headers = [str(e) for e in context.params.elements]
        sub_df = self._df.loc[:, sub_headers]

        context.results.timeSeries = Isimip_CSV_TimeSeries( \
            self._real, sub_df, headers=sub_headers, \
            start_date=self._start_date, end_date=self._end_date)
        


class Isimip_CSV_Simulation(cd.Climate.Simulation.Server): 

    def __init__(self, id, path_to_sim_dir, name=None, description=None):
        self._id = id
        self._name = name if name else self._id
        self._description = description if description else ""
        self._scens = None # []
        self._stations = None # []
        self._gk5_interpol = None
        self._path_to_sim_dir = path_to_sim_dir
        

    def info(self):
        return c.Common.IdInformation.new_message(id=self._id, name=self._name, description=self._description) 

    def info_context(self, context): # -> (info :IdInformation);
        context.results.info = self.info()

    @classmethod
    def create_isimip_simulations(cls, path_to_isimip_dir):
        sims = []
        for dirname in os.listdir(path_to_isimip_dir):
            if os.path.isdir(path_to_isimip_dir + dirname):
                sims.append(cls(dirname.lower(), path_to_isimip_dir + dirname + "/", name=dirname))
        return sims

    @property    
    def gk5_interpolator(self):
        if not self._gk5_interpol:
            self._gk5_interpol = create_climate_gk5_interpolator_from_json_file\
                (self._path_to_sim_dir + "../latlon-to-rowcol.json", wgs84, gk5)

        return self._gk5_interpol

    @property
    def scenarios(self):
        if not self._scens:
            self._scens = Isimip_CSV_Scenario.create_isimip_scenarios(self, self._path_to_sim_dir)
        return self._scens

    def scenarios_context(self, context): # -> (scenarios :List(Scenario));
        context.results.init("scenarios", len(self.scenarios))
        for i, scen in enumerate(self.scenarios):
            context.results.scenarios[i] = scen
        

    @property
    def stations(self):
        pass

    def stations_context(self, context): # -> (stations :List(Station));
        pass
        



class Isimip_CSV_Scenario(cd.Climate.Scenario.Server):

    def __init__(self, sim, id, path_to_scen_dir, name=None, description=None):
        self._sim = sim
        self._id = id
        self._name = name if name else self._id
        self._description = description if description else ""
        self._reals = None # []
        self._path_to_scen_dir = path_to_scen_dir

    def info(self):
        return c.Common.IdInformation.new_message(id=self._id, name=self._name, description=self._description) 

    def info_context(self, context): # -> (info :IdInformation);
        context.results.info = self.info()
        

    @classmethod
    def create_isimip_scenarios(cls, sim, path_to_isimip_sim_dir):
        scens = []
        for dirname in os.listdir(path_to_isimip_sim_dir):
            if os.path.isdir(path_to_isimip_sim_dir + dirname):
                scens.append(cls(sim, dirname.lower(), path_to_isimip_sim_dir + dirname + "/", dirname))
        return scens

    @property
    def simulation(self):
        return self._sim    

    def simulation_context(self, context): # -> (simulation :Simulation);
        context.results.simulation = self._sim
        
    @property
    def realizations(self):
        if not self._reals:
            self._reals = Isimip_CSV_Realization.create_isimip_realization(self, self._path_to_scen_dir)
        return self._reals

    def realizations_context(self, context): # -> (realizations :List(Realization));
        context.results.init("realizations", len(self.realizations))
        for i, real in enumerate(self.realizations):
            context.results.realizations[i] = real
        


class Isimip_CSV_Realization(cd.Climate.Realization.Server):

    def __init__(self, scen, path_to_csvs, id=None, name=None, description=None):
        self._scen = scen
        self._path_to_csvs = path_to_csvs
        self._id = id if id else "1"
        self._name = name if name else self._id
        self._description = description if description else ""

    def info(self):
        return c.Common.IdInformation.new_message(id=self._id, name=self._name, description=self._description) 

    def info_context(self, context): # -> (info :IdInformation);
        context.results.info = self.info()

    @classmethod
    def create_isimip_realization(cls, scen, path_to_isimip_scen_dir):
        return [cls(scen, path_to_isimip_scen_dir + "germany/")]
        
    @property
    def scenario(self):
        return self._scen

    def scenario_context(self, context): # -> (scenario :Scenario);
        context.results.scenario = self._scen
        
    def closest_time_series_at(self, geo_coord):

        if geo_coord.which() == "gk" and geo_coord.meridianNo == 5:
            gk5_r, gk5_h = geo_coord.r, geo_coord.h
        else:
            lat, lon = geo_coord_to_latlon(geo_coord)
            gk5_r, gk5_h = transform(wgs84, gk5, lon, lat)

        interpol = self.scenario.simulation.gk5_interpolator
        row, col = interpol(gk5_r, gk5_h)

        closest_time_series = Isimip_CSV_TimeSeries.create_time_series(self, self._path_to_csvs, row, col)

        return closest_time_series
       # header.header, ts.data()) \


    def closestTimeSeriesAt_context(self, context): # (geoCoord :Geo.Coord) -> (timeSeries :TimeSeries);
        # closest TimeSeries object which represents the whole time series 
        # of the climate realization at the give climate coordinate

        context.results.timeSeries = self.closest_time_series_at(context.params.geoCoord)
        



class YearlyTavg(m.Model.ClimateInstance.Server):

    def __init__(self):
        pass

    def runSet(self, dataset, **kwargs): # (dataset :List(TimeSeries)) -> (result :XYPlusResult);
        pass

    def run(self, timeSeries, _context, **kwargs): # (timeSeries :TimeSeries) -> (result :XYResult);
        #return timeSeries.header().then(lambda res: setattr(_context.results, "result", {"xs": [1,2,3], "ys": [2,3,4]}))
        return capnp.join_promises([timeSeries.header(), timeSeries.data(), timeSeries.range()]) \
            .then(lambda res: setattr(_context.results, "result", \
                self.calc_yearly_tavg(res[2].startDate, res[2].endDate, res[0].header, res[1].data))) 

        
    def calc_yearly_tavg(self, start_date, end_date, headers, data):
        "calculate the average temperature for all the years in the data"
        
        start_date = create_date(start_date)
        end_date = create_date(end_date)

        current_year = start_date.year
        current_sum_t = 0
        current_day_count = 0
        years = []
        tavgs = []
        for day in range((end_date - start_date).days + 1):

            current_date = start_date + timedelta(days=day)

            if current_year != current_date.year:
                years.append(current_year)
                tavgs.append(round(current_sum_t / current_day_count, 2))
                current_year = current_date.year
                current_sum_t = 0
                current_day_count = 0

            current_sum_t += data[day][0]
            current_day_count += 1

        return {"xs": years, "ys": tavgs}



class DataServiceImpl(cd.Climate.DataService.Server):
    "Implementation of the Climate.DataService Cap'n Proto interface."

    def __init__(self, path_to_data_dir):
        self._path_to_data_dir = path_to_data_dir
        self._sims = None # []
    
    @property
    def simulations(self):
        if not self._sims:
            self._sims = Isimip_CSV_Simulation.create_isimip_simulations(self._path_to_data_dir + "climate/isimip/csvs/")
        return self._sims

    
    def simulations_context(self, context): # -> (simulations :List(Simulation));
        context.results.init("simulations", len(self.simulations))
        for i, sim in enumerate(self.simulations):
            context.results.simulations[i] = sim


    def models_context(self, context): # -> (models :List(Model));
        context.results.init("models", 1)
        context.results.models[0] = YearlyTavg() 
        

    def getAvailableSoilDataServices(self, _context, **kwargs): 
        msg = data_services_capnp.SoilDataServiceInfo.new_message()
        msg.id = 1
        msg.name = "BUEK1000"
        msg.service = self.soil_data_service_instance
        return [msg]
    
    #def evaluate(self, expression, _context, **kwargs):
    #    return evaluate_impl(expression).then(lambda value: setattr(_context.results, 'value', ValueImpl(value)))

    #def defFunction(self, paramCount, body, _context, **kwargs):
    #    return FunctionImpl(paramCount, body)

    #def getOperator(self, op, **kwargs):
    #    return OperatorImpl(op)



def main():
    #address = parse_args().address

    #server = capnp.TwoPartyServer("*:8000", bootstrap=DataServiceImpl("/home/berg/archive/data/"))
    server = capnp.TwoPartyServer("*:8000", bootstrap=DataServiceImpl("D:/"))
    server.run_forever()

if __name__ == '__main__':
    main()