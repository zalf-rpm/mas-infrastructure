//using MonicaBlazorZmqUI.Services.Github;
//using Core.Share;
using Newtonsoft.Json.Linq;
using System.Text;

namespace MonicaBlazorUI.Services
{
    public class MonicaIO
    {
        private int OP_AVG = 0;
        private int OP_MEDIAN = 1;
        private int OP_SUM = 2;
        private int OP_MIN = 3;
        private int OP_MAX = 4;
        private int OP_FIRST = 5;
        private int OP_LAST = 6;
        private int OP_NONE = 7;
        private int OP_UNDEFINED_OP_ = 8;
        private int ORGAN_ROOT = 0;
        private int ORGAN_LEAF = 1;
        private int ORGAN_SHOOT = 2;
        private int ORGAN_FRUIT = 3;
        private int ORGAN_STRUCT = 4;
        private int ORGAN_SUGAR = 5;
        private int ORGAN_UNDEFINED_ORGAN_ = 6;

        //protected readonly IGithubService _githubService;

        //public UserSetting? UserSettings { get; set; }

        public MonicaIO()//IGithubService githubService)
        {
            //_githubService = githubService;
        }

        private bool OidIsOrgan(dynamic oid)
        {
            return Convert.ToInt32(oid["organ"]) != ORGAN_UNDEFINED_ORGAN_;
        }
        private bool OidIsRange(dynamic oid)
        {
            return (Convert.ToInt32(oid["fromLayer"]) >= 0 && Convert.ToInt32(oid["toLayer"]) >= 0);
        }
        private string OpToString(int op)
        {
            if (op == OP_AVG) return "AVG";
            if (op == OP_MEDIAN) return "MEDIAN";
            if (op == OP_SUM) return "SUM";
            if (op == OP_MIN) return "MIN";
            if (op == OP_MAX) return "MAX";
            if (op == OP_FIRST) return "FIRST";
            if (op == OP_LAST) return "LAST";
            if (op == OP_NONE) return "NONE";

            return "undef";
        }
        private string OrganToString(int organ)
        {
            if (organ == ORGAN_ROOT) return "Root";
            if (organ == ORGAN_LEAF) return "Leaf";
            if (organ == ORGAN_SHOOT) return "Shoot";
            if (organ == ORGAN_FRUIT) return "Fruit";
            if (organ == ORGAN_STRUCT) return "Struct";
            if (organ == ORGAN_SUGAR) return "Sugar";

            return "undef";
        }
        private string OidToString(dynamic oid, bool include_time_agg)
        {
            StringBuilder oss = new StringBuilder();
            oss.Append("[");
            oss.Append(oid["name"]);
            if (OidIsOrgan(oid))
            {
                oss.Append(", " + OrganToString(Convert.ToInt32(oid["organ"])));
            }
            else if (OidIsRange(oid))
            {
                oss.Append(", [");
                oss.Append((Convert.ToInt32(oid["fromLayer"]) + 1).ToString());
                oss.Append(", ");
                oss.Append((Convert.ToInt32(oid["toLayer"]) + 1).ToString());
                oss.Append(Convert.ToInt32(oid["layerAggOp"]) != OP_NONE ? ", " + OpToString(Convert.ToInt32(oid["layerAggOp"])) : "");
                oss.Append("]");
            }
            else if (Convert.ToInt32(oid["fromLayer"]) >= 0)
            {
                oss.Append(", " + (Convert.ToInt32(oid["fromLayer"]) + 1).ToString());
            }
            if (include_time_agg)
            {
                oss.Append(", " + OpToString(Convert.ToInt32(oid["timeAggOp"])));
            }
            oss.Append("]");

            return oss.ToString();
        }
        public JArray WriteOutputHeaderRows(
            JArray output_ids,
            bool include_header_row = true,
            bool include_units_row = true,
            bool include_time_agg = false)
        {
            var row1 = new JArray();
            var row2 = new JArray();
            var row3 = new JArray();
            var row4 = new JArray();
            foreach (var __item in output_ids)
            {
                //string key = __item.Key;
                if (__item is JObject oid)
                {
                    var from_layer = Convert.ToInt32(oid["fromLayer"]);
                    var to_layer = Convert.ToInt32(oid["toLayer"]);
                    var is_organ = OidIsOrgan(oid);
                    var is_range = OidIsRange(oid) && Convert.ToInt32(oid["layerAggOp"]) == OP_NONE;
                    if (is_organ)
                    {
                        // organ is being represented just by the value of fromLayer currently
                        //to_layer = Convert.ToInt32(oid["organ"]);
                        to_layer = from_layer = Convert.ToInt32(oid["organ"]);
                    }
                    else if (is_range)
                    {
                        from_layer += 1;
                        to_layer += 1;
                    }
                    else
                    {
                        to_layer = from_layer;
                    }
                    //foreach (var i in Enumerable.Range(from_layer, to_layer + 1 - from_layer))
                    for (int i = from_layer; i <= to_layer; i++)
                    {
                        var str1 = "";
                        if (is_organ)
                        {
                            // str1 += oid["displayName"].Count() == 0 ? oid["name"] + "/" + organ_to_string(Convert.ToInt32(oid["organ"])) : oid["displayName"];
                            str1 += ((oid["displayName"] ?? "").ToString().Length == 0) ? oid["name"] + "/" + OrganToString(Convert.ToInt32(oid["organ"])) : oid["displayName"];
                        }
                        else if (is_range)
                        {
                            // str1 += oid["displayName"].Count() == 0 ? oid["name"] + "_" + i.ToString() : oid["displayName"];
                            str1 += ((oid["displayName"] ?? "").ToString().Length == 0) ? oid["name"] + "_" + i.ToString() : oid["displayName"];
                        }
                        else
                        {
                            //str1 += oid["displayName"].Count() == 0 ? oid["name"] : oid["displayName"];
                            str1 += ((oid["displayName"] ?? "").ToString().Length == 0) ? oid["name"] : oid["displayName"];
                        }
                        row1.Add(str1);
                        //row4.Add("j:" + oid["jsonInput"].ToString().Replace("\"", ""));
                        var row4value = "j:" + (oid["jsonInput"] ?? "").ToString().Replace("\"", "");
                        if (row4value.Contains("["))
                            row4.Add("\"" + row4value + "\"");
                        else
                            row4.Add(row4value);
                        row3.Add("m:" + OidToString(oid, include_time_agg));
                        row2.Add("[" + oid["unit"] + "]");
                    }
                }
            }
            var res = new JArray();
            if (include_header_row)
            {
                res.Add(row1);
            }
            if (include_units_row)
            {
                res.Add(row4);
            }
            if (include_time_agg)
            {
                res.Add(row3);
                res.Add(row2);
            }
            return res;
        }
        public JArray WriteOutput(JArray outputIds, JArray values)
        {
            var res = new JArray();
            if (values.Count > 0)
            {
                foreach (var k in Enumerable.Range(0, values[0].Count()))
                {
                    var i = 0;
                    var row = new JArray();
                    foreach (var _ in outputIds)
                    {
                        var cju = values[i][k];
                        if (cju is JArray cjua)
                        {
                            foreach (var jv_ in cjua) row.Add(jv_); 
                        }
                        else
                        {
                            if(cju != null) row.Add(cju);
                        }
                        i += 1;
                    }
                    res.Add(row);
                }
            }
            return res;
        }
        private bool IsAbsolutePath(string p)
        {
            return p.StartsWith("/") || p.Length == 2 && p[1] == ':' || p.Length > 2 && p[1] == ':' && (p[2] == '\\' || p[2] == '/');
        }
        private bool IsGithubPath(string p)
        {
            return false;
            //return p.StartsWith("https://github.com") || p.StartsWith("http://github.com") || p.StartsWith("https://raw.githubusercontent.com");
        }
        private string FixSystemSeparator(string path)
        {
            path = path.Replace("\\", "/");
            var newPath = path;
            while (true)
            {
                newPath = path.Replace("//", "/");
                if (newPath == path)
                {
                    break;
                }
                path = newPath;
            }
            return newPath;
        }
        private string ReplaceEnvVars(string path)
        {
            var startToken = "${";
            var endToken = "}";
            var startPos = path.IndexOf(startToken);
            while (startPos > -1)
            {
                var endPos = path.IndexOf(endToken, startPos + 1);
                if (endPos > -1)
                {
                    var nameStart = startPos + 2;
                    var envVarName = path.Substring(nameStart, endPos - startPos);
                    var envVarContent = Environment.GetEnvironmentVariable(envVarName);
                    if (envVarContent != null)
                    {
                        path = path.Replace(path.Substring(startPos, (endPos + 1) - startPos), envVarContent);
                        startPos = path.IndexOf(startToken);
                    }
                    else
                    {
                        startPos = path.IndexOf(startToken, endPos + 1);
                    }
                }
                else
                {
                    break;
                }
            }
            return path;
        }
        private  string DefaultValue(JObject dic, string key, string @default)
        {
            return dic.ContainsKey(key) ? (dic[key] ?? "").ToString() : @default;
        }
        private  JObject ReadAndParseJsonFile(string path, bool isGithubPath = false)
        {
            var res = new JObject();
            try
            {
                JObject ssjsn;
                // if (isGithubPath)
                // { 
                //     // if(_githubService.IsExistPath(path, UserSettings.GithubUserName, UserSettings.GithubToken, UserSettings.MonicaResultsPathOnGithub))
                //     // {
                //     //     //_githubService.get
                //     //     ssjsn = JObject.Parse(_githubService.GetFileContent(path, UserSettings.GithubUserName, UserSettings.GithubToken));
                //     // }
                //     // else
                //     // {
                //     //     throw new FileNotFoundException();
                //     // }
                // }
                // else
                // {
                    ssjsn = JObject.Parse(File.ReadAllText(path));
                //}

                res.Add("result", ssjsn);
                res.Add("errors", new JArray());
                res.Add("success", true);
            }
            catch(Exception)
            {
                res.Add("result", new JObject());
                res.Add("errors", "Error opening file with path: '" + path + "'!");
                res.Add("success", false);
            }
            return res;
        }

        private JObject ParseJsonString(string jsonString)
        {
            return new JObject {
                { "result", JObject.Parse(jsonString) },
                { "errors", new JArray() },
                { "success", true }
            };
        }
        private bool IsStringType(JValue? j)
        {
            if (j == null) return false;
            return j.Type == JTokenType.String;
        }

        public JObject FindAndReplaceReferences(JObject? root, JToken? j)
        {
            if (root == null || j == null) return new JObject();

            JObject res;
            var success = true;
            var errors = new List<string>();
            if (j is JArray ja && ja.Count > 0)
            {
                var arr = new List<object>();
                var arrayIsReferenceFunction = false;
                if (j[0] is JValue j0 && IsStringType(j0))
                {
                    if (Contains(j0.ToString()))
                    {
                        string function = j0.ToString();
                        arrayIsReferenceFunction = true;

                        //check for nested function invocations in the arguments
                        var funcArr = new JArray();
                        foreach (var i in j)
                        {
                            res = FindAndReplaceReferences(root, i);
                            success = success && Convert.ToBoolean(res["success"]);
                            if (!Convert.ToBoolean(res["success"]))
                            {
                                foreach (var err in res["errors"] ?? new JObject())
                                {
                                    errors.Add(err.ToString());
                                }
                            }
                            var r = res["result"];
                            if(r != null) funcArr.Add(r);
                        }
                        //invoke function
                        var jaes = GetValue(function, root, funcArr);
                        success = success && Convert.ToBoolean(jaes["success"]);
                        if (!Convert.ToBoolean(jaes["success"]))
                        {
                            foreach (var err in jaes["errors"] ?? new JObject())
                            {
                                errors.Add(err.ToString());
                            }
                        }
                        //if successful try to recurse into result for functions in result
                        if (Convert.ToBoolean(jaes["success"]))
                        {
                            var jaesr = jaes["result"];
                            if (jaesr != null)
                            {
                                res = FindAndReplaceReferences(root, jaesr);
                                success = success && Convert.ToBoolean(res["success"]);
                                if (!Convert.ToBoolean(res["success"]))
                                {
                                    foreach (var err in res["errors"] ?? new JObject())
                                    {
                                        errors.Add(err.ToString());
                                    }
                                }
                                return new JObject{
                                    { "result", res["result"] },
                                    { "errors", new JArray(errors.ToArray()) },
                                    { "success", errors.Count == 0 }
                                };
                            }
                        }
                        else
                        {
                            return new JObject{
                                { "result", new JArray() },
                                { "errors", new JArray(errors.ToArray()) },
                                { "success", errors.Count == 0 }
                            };
                        }
                    }
                }
                if (!arrayIsReferenceFunction)
                {
                    foreach (var jv in j)
                    {
                        res = FindAndReplaceReferences(root, jv);
                        success = success && Convert.ToBoolean(res["success"]);
                        if (!Convert.ToBoolean(res["success"]))
                        {
                            foreach (var err in res["errors"] ?? new JObject())
                            {
                                errors.Add(err.ToString());
                            }
                        }
                        var r = res["result"];
                        if (r != null) arr.Add(r);
                    }
                }
                return new JObject {
                    { "result", new JArray(arr.ToArray()) },
                    { "errors", new JArray(errors.ToArray()) },
                    { "success", errors.Count == 0 }
                };
            }
            else if (j is JObject jo)
            {
                var obj = new JObject();
                foreach (var item in jo)
                {
                    var k = item.Key;
                    var v = item.Value;
                    var r = v != null ? FindAndReplaceReferences(root, v) : new JObject();
                    success = success && Convert.ToBoolean(r["success"]);
                    if (!Convert.ToBoolean(r["success"]))
                    {
                        foreach (var e in r["errors"] ?? new JObject())
                        {
                            errors.Add(e.ToString());
                        }
                    }
                    var rr = r["result"];
                    if(rr != null) obj.Add(k, rr);
                }
                return new JObject{
                    { "result", obj },
                    { "errors", new JArray(errors.ToArray()) },
                    { "success", errors.Count == 0 }
                };
            }
            return new JObject{
                { "result", j },
                { "errors", new JArray(errors.ToArray()) },
                { "success", errors.Count == 0 }
            };
        }
        private static bool PrintPossibleErrors(JObject errs, bool includeWarnings = false)
        {
            var succ = errs["success"];
            if (succ != null && !(bool)succ)
            {
                foreach (var err in errs["errors"] ?? new JObject())
                {
                    Console.WriteLine(err);
                }
            }
            if (includeWarnings && errs.ContainsKey("warnings"))
            {
                foreach (var war in errs["warnings"] ?? new JObject())
                {
                    Console.WriteLine(war);
                }
            }
            return (bool)(succ ?? false);
        }

        public JObject? CreateEnvJsonFromJsonConfig(JObject cropSiteSim, string parametersPath)
        {
            JToken? jTkn;
            string key;
            foreach (var item in cropSiteSim)
            {
                key = item.Key;
                jTkn = item.Value;
                if (jTkn == null)
                {
                    return null;
                }
            }

            var pathToParameters = parametersPath; //crop_site_sim["sim"]["include-file-base-path"];
            //var path_to_parameters = "Data/monica-parameters/";

            var cropSiteSim2 = new JObject();

            //collect all errors in all files and don't stop as early as possible
            var errors = new List<string>();
            foreach (var item in cropSiteSim)
            {
                key = item.Key;
                jTkn = item.Value;
                if (key == "climate") continue;
                AddBasePath(jTkn as JObject, pathToParameters);
                var res = FindAndReplaceReferences(jTkn as JObject, jTkn);
                if (Convert.ToBoolean(res["success"]))
                {
                    var rr = res["result"];
                    if(rr != null) cropSiteSim2.Add(key, rr);
                }
                else
                {
                    var ers = res.Value<JArray>("errors");
                    if (ers != null)
                    {
                        for (int ii = 0; ii < ers.Count; ii++)
                        {
                            errors.Add(ers[ii].ToString());
                        }
                    }
                }
            }

            if (errors.Count > 0)
            {
                /*foreach (var err in errors)
                {
                    Console.WriteLine(err);
                }*/
                return null;
            }
            var cropj = cropSiteSim2["crop"] ?? new JObject();
            var sitej = cropSiteSim2["site"] ?? new JObject();
            var simj = cropSiteSim2["sim"] ?? new JObject();
            var env = new JObject
            {
                { "type", "Env" },
                //store debug mode in env, take from sim.json, but prefer params map
                { "debugMode", simj["debug?"] ?? false },
                { "cropRotation", cropj["cropRotation"] ?? new JObject() },
                { "cropRotations", cropj["cropRotations"] ?? new JObject() },
                { "events", simj.Value<JObject>("output")?.Value<JArray>("events") ?? new JArray() },
                { "pathToClimateCSV", simj["climate.csv"] ?? "" },
                { "csvViaHeaderOptions", simj["climate.csv-options"] ?? new JObject() },
                { "climateCSV", cropSiteSim["climate"] ?? ""}
            };
            env.Value<JObject>("csvViaHeaderOptions")?.Value<JValue>("latitude")?.Replace(sitej.Value<JObject>("SiteParameters")?.Value<JValue>("Latitude") ?? new JValue(0.0));
            var cpp = new JObject()
            {{
                "type",
                "CentralParameterProvider"},
            {
                "userCropParameters",
                cropj["CropParameters"]},
            {
                "userEnvironmentParameters",
                sitej["EnvironmentParameters"] ?? new JObject()},
            {
                "userSoilMoistureParameters",
                sitej["SoilMoistureParameters"] ?? new JObject()},
            {
                "userSoilTemperatureParameters",
                sitej["SoilTemperatureParameters"] ?? new JObject()},
            {
                "userSoilTransportParameters",
                sitej["SoilTransportParameters"] ?? new JObject()},
            {
                "userSoilOrganicParameters",
                sitej["SoilOrganicParameters"] ?? new JObject()},
            {
                "simulationParameters",
                simj},
            {
                "siteParameters",
                sitej["SiteParameters"] ?? new JObject()}
            };
            env.Add("params", cpp);
            return env;
        }

        private void AddBasePath(JObject? j, JToken? basePath)
        {
            if (j == null || basePath == null) return;
            if (!j.ContainsKey("include-file-base-path")) j.Add("include-file-base-path", basePath);
            else j["include-file-base-path"] = basePath;
        }

        public JObject Ref(JObject? root, JArray? j)
        {
            if (root == null || j == null) return new JObject { { "success", false } };

            if (j.Count == 3 && IsStringType(j[1] as JValue) && IsStringType(j[2] as JValue))
            {
                var key1 = j[1].ToString();
                var key2 = j[2].ToString();
                var r12 = (root[key1] ?? new JObject())[key2];
                if(r12 != null) return FindAndReplaceReferences(root, r12);
                return new JObject();

            }
            return new JObject {
                { "result", j },
                { "errors", new JArray("Couldn't resolve reference: " + j.ToString() + "!") },
                { "success", false }
            };
        }

        public JObject FromFile(JObject? root, JArray? j)
        {
            if (root == null || j == null) return new JObject { { "success", false } };

            if (j.Count == 2 && IsStringType(j.Value<JValue>(1)))
            {
                var basePath = DefaultValue(root, "include-file-base-path", ".");
                var pathToFile = j[1].ToString();
                bool isGitHubPath = false;

                if (IsGithubPath(basePath))
                {
                    // _githubService.SetRepoInfo(base_path);
                    // isGitHubPath = true;
                }
                else
                {
                    if (!IsAbsolutePath(pathToFile)) pathToFile = basePath + "/" + pathToFile;
                    pathToFile = ReplaceEnvVars(pathToFile);
                    pathToFile = FixSystemSeparator(pathToFile);
                }
                // here can check if this path exists in our array,if so then replace the path with our path like: Data/upload/temp-monica-parameters/....json
                var jo_ = ReadAndParseJsonFile(pathToFile, isGitHubPath);
                if (Convert.ToBoolean(jo_["success"]) && jo_["result"] != null)
                {
                    return new JObject {
                        { "result", jo_["result"] },
                        { "errors", new JArray() },
                        { "success", true }
                    };
                }
                return new JObject {
                    {"result", j },
                    { "errors", new JArray("Couldn't include file with path: '" + pathToFile + "'!") },
                    { "success", false }
                };
            }
            return new JObject {
                { "result", j },
                { "errors", new JArray("Couldn't include file with function: " + j.ToString() + "!") },
                { "success", false }
            };
        }

        public JObject LdToTrd(JObject? root, JArray? j)
        {
            if (root == null || j == null) return new JObject { { "success", false } };

            if (j.Count == 3 && IsInt(j[1] as JValue) && IsFloat(j[2] as JValue))
            {
                return new JObject {
                    { "result", SoilIO.bulk_density_class_to_raw_density((int)j[1], (double)j[2]) },
                    { "errors", new JArray() },
                    { "success", true }
                };
            }
            return new JObject{
                { "result", j },
                { "errors", new JArray("Couldn't convert bulk density class to raw density using function: " + j.ToString() + "!") },
                { "success", false }
            };
        }

        public JObject KA5ToClay(JObject? root, JArray? j)
        {
            if (root == null || j == null) return new JObject { { "success", false } };

            if (j.Count == 2 && IsStringType(j[1] as JValue))
            {
                return new JObject{
                    { "result", SoilIO.ka5_texture_to_clay(j[1].ToString()) },
                    { "errors", new JArray() },
                    { "success", true }
                };
            }
            return new JObject{
                { "result", j },
                { "errors", new JArray("Couldn't get soil clay content from KA5 soil class: " + j.ToString() + "!") },
                { "success", false }
            };
        }

        public JObject KA5ToSand(JObject? root, JArray? j)
        {
            if (root == null || j == null) return new JObject { { "success", false } };

            if (j.Count == 2 && IsStringType(j[1] as JValue))
            {
                return new JObject {
                    { "result", SoilIO.ka5_texture_to_sand(j[1].ToString()) },
                    { "errors", new JArray() },
                    { "success", true }
                };
            }
            return new JObject{
                { "result", j },
                { "errors", new JArray("Couldn't get soil sand content from KA5 soil class: " + j.ToString() + "!") },
                { "success", false }
            };
        }

        public JObject SandClayToLambda(JObject? root, JArray? j)
        {
            if (root == null || j == null) return new JObject { { "success", false } };

            if (j.Count == 2 && IsFloat(j[1] as JValue) && IsFloat(j[2] as JValue))
            {
                return new JObject {
                    { "result", SoilIO.sand_and_clay_to_lambda((double)j[1], (double)j[2]) },
                    { "errors", new JArray() },
                    { "success", true }
                };
            }
            return new JObject {
                { "result", j },
                { "errors", new JArray("Couldn't get lambda value from soil sand and clay content: " + j.ToString() + "!") },
                { "success", false }
            };
        }

        public JObject Percent(JObject? root, JArray? j)
        {
            if (root == null || j == null) return new JObject { { "success", false } };

            if (j.Count == 2 && IsFloat(j[1] as JValue))
            {
                return new JObject {
                    { "result", (float)j[1] / 100.0 },
                    { "errors", new JArray() },
                    { "success", true }
                };
            }
            return new JObject {
                { "result", j },
                { "errors", new JArray("Couldn't convert percent to decimal percent value: " + j.ToString() + "!") },
                { "success", false }
            };
        }

        public JObject HumusToCorg(JObject? root, JArray? j)
        {
            if (root == null || j == null) return new JObject { { "success", false } };

            if (j.Count == 2 && IsInt(j[1] as JValue))
            {
                return new JObject{
                    { "result", SoilIO.humus_class_to_corg((int)j[1]) },
                    { "errors", new JArray() },
                    { "success", true }
                };
            }
            return new JObject {
                { "result", j },
                { "errors", new JArray("Couldn't convert humus level to corg: " + j.ToString() + "!") },
                { "success", false }
            };
        }

        private bool IsInt(JValue? value)
        {
            if (value == null) return false;
            return value.Type == JTokenType.Integer;
        }
        private bool IsFloat(JValue? value)
        {
            if (value == null) return false;
            return value.Type == JTokenType.Float;
        }

        public JObject GetValue(string function, JObject? root, JToken? j)
        {
            if (root == null || j == null) return new JObject { { "success", false } };

            return function switch
            {
                "include-from-file" => FromFile(root, j as JArray),
                "ref" => Ref(root, j as JArray),
                "humus_st2corg" => HumusToCorg(root, j as JArray),
                "ld_eff2trd" => LdToTrd(root, j as JArray),
                "bulk-density-class->raw-density" => LdToTrd(root, j as JArray),
                "KA5TextureClass2clay" => KA5ToClay(root, j as JArray),
                "KA5-texture-class->clay" => KA5ToClay(root, j as JArray),
                "KA5TextureClass2sand" => KA5ToSand(root, j as JArray),
                "KA5-texture-class->sand" => KA5ToSand(root, j as JArray),
                "sandAndClay2lambda" => SandClayToLambda(root, j as JArray),
                "sand-and-clay->lambda" => SandClayToLambda(root, j as JArray),
                "%" => Percent(root, j as JArray),
                _ => throw new Exception("invalid pattern name!"),
            };
        }
        public bool Contains(string function)
        {
            return function switch
            {
                "include-from-file" or "ref" or "humus_st2corg" or "ld_eff2trd" or "bulk-density-class->raw-density" 
                or "KA5TextureClass2clay" or "KA5-texture-class->clay" or "KA5TextureClass2sand" 
                or "KA5-texture-class->sand" or "sandAndClay2lambda" 
                or "sand-and-clay->lambda" or "%" => true,
                _ => false,
            };
        }
    }
}
