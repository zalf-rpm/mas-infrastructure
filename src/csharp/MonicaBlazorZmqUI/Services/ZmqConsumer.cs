using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;


namespace MonicaBlazorZmqUI.Services
{
    public class ZmqConsumer
    {
        //public static string RunConsumer()            
        //{

        //string outDir = Path.GetDirectoryName(Application.ExecutablePath);
        //    var config = new JObject();
        //config.Add("port", string.IsNullOrEmpty(port) ? "7777" : port);
        //config.Add("server", string.IsNullOrEmpty(server) ? "localhost" : port);
        //config.Add("out", string.IsNullOrEmpty(path_to_output_dir) ? outDir : path_to_output_dir);
        //    config.Add("leave_after_finished_run", leave_after_finished_run);
        //config.Add("shared_id", shared_id);



        //socket.RCVTIMEO = 1000
        // first get the message in blazor then call this method and convert the msg to csv
        // this method can send back the link of csv file to the blazor or fail msg

        private readonly MonicaIO _monicaIO;

        public static int env_count = 0;

        public ZmqConsumer(MonicaIO monicaIO)
        {
            _monicaIO = monicaIO;
        }

        public string RunConsumer(string msg)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            JObject msgObj = JObject.Parse(msg);

            var leave = false;
            if (msgObj["type"].ToString() == "finish")
            {
                //Console.WriteLine("c: received finish message");
                leave = true;
                env_count = 0;
            }
            else
            {
                // Assume that we get all in one frame
               // Console.WriteLine("c: received work result ", env_count, " customId: ", msgObj.ContainsKey("customId") ? msgObj["customId"].ToString() : "");
               // env_count++;


                var varval = "";
                //string csvFile = Path.Combine(config["out"].ToString(), env_count.ToString() + ".csv");
                //string csvFile = ("wwwroot/export/1.csv");   // static address for export for test perpose at first run
                //csvFilePath = (csvPath + ".csv");
                //if (File.Exists(csvFilePath))
                //    varval = File.ReadAllText(csvFilePath);
                //StreamWriter strWrite = new StreamWriter(csvFile);
                StringBuilder strBuild = new StringBuilder("");

                strBuild.AppendLine("");

                JArray ddd = new JArray();
                if (msgObj.ContainsKey("data"))
                    ddd = msgObj["data"] as JArray;
                foreach (var data_ in ddd)
                {
                    var results = new JArray();
                    if (data_["results"] != null)
                        results = data_["results"] as JArray;
                    var orig_spec = "";
                    if (data_["origSpec"] != null)
                        orig_spec = data_["origSpec"].ToString();
                    var output_ids = new JArray();
                    if (data_["outputIds"] != null)
                        output_ids = data_["outputIds"] as JArray;

                    if (results.Count > 0)
                    {
                        strBuild.AppendLine(orig_spec.Replace("\"", ""));
                        foreach (JArray row in _monicaIO.write_output_header_rows(output_ids, true, true, false))
                        {
                            string cline = "";
                            for (int i = 0; i < row.Count; i++)
                            {
                                cline += row[i];
                                if (i < (row.Count - 1))
                                    cline += ",";
                            }
                            strBuild.AppendLine(cline);
                        }
                        foreach (JArray row in _monicaIO.write_output(output_ids, results))
                        {
                            string cline = "";
                            for (int i = 0; i < row.Count; i++)
                            {
                                cline += row[i];
                                if (i < (row.Count - 1))
                                    cline += ",";
                            }
                            strBuild.AppendLine(cline);
                        }
                    }

                    strBuild.AppendLine();
                }
               
                leave = true;
                
                
                //strWrite.Close();
                return strBuild.ToString();  // this method produce csv file and returns the csv path and csv string
            }

            //csvFilePath = "Path Error";
            return "Content Error";
        }
    
    }

}
