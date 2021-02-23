using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MonicaBlazorZmqUI.Services
{
    public static class SoilIO
    {
        public static object soil_parameters(dynamic con, object profile_id)
        {
            ////var query = @"
            ////            select 
            ////                id, 
            ////                layer_depth, 
            ////                soil_organic_carbon, 
            ////                soil_organic_matter, 
            ////                bulk_density, 
            ////                raw_density,
            ////                sand, 
            ////                clay, 
            ////                ph, 
            ////                KA5_texture_class,
            ////                permanent_wilting_point,
            ////                field_capacity,
            ////                saturation,
            ////                soil_water_conductivity_coefficient,
            ////                sceleton,
            ////                soil_ammonium,
            ////                soil_nitrate,
            ////                c_n,
            ////                initial_soil_moisture,
            ////                layer_description,
            ////                is_in_groundwater,
            ////                is_impenetrable
            ////            from soil_profile 
            ////            where id = ? 
            ////            order by id, layer_depth
            ////            ";
            ////var layers = new JObject();
            ////var prev_depth = 0;
            ////con.row_factory = sqlite3.Row;
            ////foreach (var row in con.cursor().execute(query, ValueTuple.Create(profile_id)))
            ////{
            ////    var layer = new Dictionary<object, object> {
            ////    {
            ////        "type",
            ////        "SoilParameters"}};
            ////    if (row["layer_depth"] != null)
            ////    {
            ////        var depth = float(row["layer_depth"]);
            ////        layer["Thickness"] = new List<object> {
            ////        depth - prev_depth,
            ////        "m"
            ////    };
            ////        prev_depth = depth;
            ////    }
            ////    if (row["KA5_texture_class"] != null)
            ////    {
            ////        layer["KA5TextureClass"] = row["KA5_texture_class"];
            ////    }
            ////    if (row["sand"] != null)
            ////    {
            ////        layer["Sand"] = new List<object> {
            ////        float(row["sand"]) / 100.0,
            ////        "% [0-1]"
            ////    };
            ////    }
            ////    if (row["clay"] != null)
            ////    {
            ////        layer["Clay"] = new List<object> {
            ////        float(row["clay"]) / 100.0,
            ////        "% [0-1]"
            ////    };
            ////    }
            ////    if (row["ph"] != null)
            ////    {
            ////        layer["pH"] = float(row["ph"]);
            ////    }
            ////    if (row["sceleton"] != null)
            ////    {
            ////        layer["Sceleton"] = new List<object> {
            ////        float(row["sceleton"]) / 100.0,
            ////        "vol% [0-1]"
            ////    };
            ////    }
            ////    if (row["soil_organic_carbon"] != null)
            ////    {
            ////        layer["SoilOrganicCarbon"] = new List<object> {
            ////        float(row["soil_organic_carbon"]),
            ////        "mass% [0-100]"
            ////    };
            ////    }
            ////    else if (row["soil_organic_matter"] != null)
            ////    {
            ////        layer["SoilOrganicMatter"] = new List<object> {
            ////        float(row["soil_organic_matter"]) / 100.0,
            ////        "mass% [0-1]"
            ////    };
            ////    }
            ////    if (row["bulk_density"] != null)
            ////    {
            ////        layer["SoilBulkDensity"] = new List<object> {
            ////        float(row["bulk_density"]),
            ////        "kg m-3"
            ////    };
            ////    }
            ////    else if (row["raw_density"] != null)
            ////    {
            ////        layer["SoilRawDensity"] = new List<object> {
            ////        float(row["raw_density"]),
            ////        "kg m-3"
            ////    };
            ////    }
            ////    if (row["field_capacity"] != null)
            ////    {
            ////        layer["FieldCapacity"] = new List<object> {
            ////        float(row["field_capacity"]) / 100.0,
            ////        "vol% [0-1]"
            ////    };
            ////    }
            ////    if (row["permanent_wilting_point"] != null)
            ////    {
            ////        layer["PermanentWiltingPoint"] = new List<object> {
            ////        float(row["permanent_wilting_point"]) / 100.0,
            ////        "vol% [0-1]"
            ////    };
            ////    }
            ////    if (row["saturation"] != null)
            ////    {
            ////        layer["PoreVolume"] = new List<object> {
            ////        float(row["saturation"]) / 100.0,
            ////        "vol% [0-1]"
            ////    };
            ////    }
            ////    if (row["initial_soil_moisture"] != null)
            ////    {
            ////        layer["SoilMoisturePercentFC"] = new List<object> {
            ////        float(row["initial_soil_moisture"]),
            ////        "% [0-100]"
            ////    };
            ////    }
            ////    if (row["soil_water_conductivity_coefficient"] != null)
            ////    {
            ////        layer["Lambda"] = float(row["soil_water_conductivity_coefficient"]);
            ////    }
            ////    if (row["soil_ammonium"] != null)
            ////    {
            ////        layer["SoilAmmonium"] = new List<object> {
            ////        float(row["soil_ammonium"]),
            ////        "kg NH4-N m-3"
            ////    };
            ////    }
            ////    if (row["soil_nitrate"] != null)
            ////    {
            ////        layer["SoilNitrate"] = new List<object> {
            ////        float(row["soil_nitrate"]),
            ////        "kg NO3-N m-3"
            ////    };
            ////    }
            ////    if (row["c_n"] != null)
            ////    {
            ////        layer["CN"] = float(row["c_n"]);
            ////    }
            ////    if (row["layer_description"] != null)
            ////    {
            ////        layer["description"] = row["layer_description"];
            ////    }
            ////    if (row["is_in_groundwater"] != null)
            ////    {
            ////        layer["is_in_groundwater"] = Convert.ToInt32(row["is_in_groundwater"]) == 1;
            ////    }
            ////    if (row["is_impenetrable"] != null)
            ////    {
            ////        layer["is_impenetrable"] = Convert.ToInt32(row["is_impenetrable"]) == 1;
            ////    }
            ////    var found = key => layer.Contains(key);
            ////    var layer_is_ok = found("Thickness") && (found("SoilOrganicCarbon") || found("SoilOrganicMatter")) && (found("SoilBulkDensity") || found("SoilRawDensity")) && (found("KA5TextureClass") || found("Sand") && found("Clay") || found("PermanentWiltingPoint") && found("FieldCapacity") && found("PoreVolume") && found("Lambda"));
            ////    if (layer_is_ok)
            ////    {
            ////        layers.append(layer);
            ////    }
            ////    else
            ////    {
            ////        prev_depth -= depth;
            ////        Console.WriteLine("Layer ", layer, " is incomplete. Skipping it!");
            ////    }
            ////}
            ////return layers;

            return null;
        }

        //con = sqlite3.connect("soil.sqlite")
        //x = soil_parameters(con, 197595)
        //print(x)
        // convert humus class to soil organic carbon content
        public static double humus_class_to_corg(int humus_class)
        {
            var ccc = new Dictionary<int, double>
            {{
                0,
                0.0
            },
            {
                1,
                (0.5 / 1.72)
            },
            {
                2,
                (1.5 / 1.72)
            },
            {
                3,
                (3.0 / 1.72)
            },
            {
                4,
                (6.0 / 1.72)
            },
            {
                5,
                (11.5 / 2.0)
            },
            {
                6,
                (17.5 / 2.0)
            },
            {
                7,
                (30.0 / 2.0)
            }
            };
            if (ccc.ContainsKey(humus_class))
                return ccc[humus_class];
            else
                return 0.0;
        }

        // convert a bulk density class to an approximated raw density
        public static double bulk_density_class_to_raw_density(int bulk_density_class, double clay)
        {
            var ccc = new Dictionary<int, double> {
            {
                1,
                1.3},
            {
                2,
                1.5},
            {
                3,
                1.7},
            {
                4,
                1.9},
            {
                5,
                2.1}};
            double xxx = 0;
            if (ccc.ContainsKey(bulk_density_class))
                xxx = ccc[bulk_density_class];

            return (xxx - 0.9 * clay) * 1000.0;
        }

        // roughly calculate lambda value from sand and clay content
        public static double sand_and_clay_to_lambda(double sand, double clay)
        {
            return 2.0 * (sand * sand * 0.575) + clay * 0.1 + (1.0 - sand - clay) * 0.35;
        }

        // get a rough KA5 soil texture class from given sand and soil content
        public static string sand_and_clay_to_ka5_texture(double sand, double clay)
        {
            var silt = 1.0 - sand - clay;
            var soil_texture = "";
            if (silt < 0.1 && clay < 0.05)
            {
                soil_texture = "Ss";
            }
            else if (silt < 0.25 && clay < 0.05)
            {
                soil_texture = "Su2";
            }
            else if (silt < 0.25 && clay < 0.08)
            {
                soil_texture = "Sl2";
            }
            else if (silt < 0.4 && clay < 0.08)
            {
                soil_texture = "Su3";
            }
            else if (silt < 0.5 && clay < 0.08)
            {
                soil_texture = "Su4";
            }
            else if (silt < 0.8 && clay < 0.08)
            {
                soil_texture = "Us";
            }
            else if (silt >= 0.8 && clay < 0.08)
            {
                soil_texture = "Uu";
            }
            else if (silt < 0.1 && clay < 0.17)
            {
                soil_texture = "St2";
            }
            else if (silt < 0.4 && clay < 0.12)
            {
                soil_texture = "Sl3";
            }
            else if (silt < 0.4 && clay < 0.17)
            {
                soil_texture = "Sl4";
            }
            else if (silt < 0.5 && clay < 0.17)
            {
                soil_texture = "Slu";
            }
            else if (silt < 0.65 && clay < 0.17)
            {
                soil_texture = "Uls";
            }
            else if (silt >= 0.65 && clay < 0.12)
            {
                soil_texture = "Ut2";
            }
            else if (silt >= 0.65 && clay < 0.17)
            {
                soil_texture = "Ut3";
            }
            else if (silt < 0.15 && clay < 0.25)
            {
                soil_texture = "St3";
            }
            else if (silt < 0.3 && clay < 0.25)
            {
                soil_texture = "Ls4";
            }
            else if (silt < 0.4 && clay < 0.25)
            {
                soil_texture = "Ls3";
            }
            else if (silt < 0.5 && clay < 0.25)
            {
                soil_texture = "Ls2";
            }
            else if (silt < 0.65 && clay < 0.3)
            {
                soil_texture = "Lu";
            }
            else if (silt >= 0.65 && clay < 0.25)
            {
                soil_texture = "Ut4";
            }
            else if (silt < 0.15 && clay < 0.35)
            {
                soil_texture = "Ts4";
            }
            else if (silt < 0.3 && clay < 0.45)
            {
                soil_texture = "Lts";
            }
            else if (silt < 0.5 && clay < 0.35)
            {
                soil_texture = "Lt2";
            }
            else if (silt < 0.65 && clay < 0.45)
            {
                soil_texture = "Tu3";
            }
            else if (silt >= 0.65 && clay >= 0.25)
            {
                soil_texture = "Tu4";
            }
            else if (silt < 0.15 && clay < 0.45)
            {
                soil_texture = "Ts3";
            }
            else if (silt < 0.5 && clay < 0.45)
            {
                soil_texture = "Lt3";
            }
            else if (silt < 0.15 && clay < 0.65)
            {
                soil_texture = "Ts2";
            }
            else if (silt < 0.3 && clay < 0.65)
            {
                soil_texture = "Tl";
            }
            else if (silt >= 0.3 && clay < 0.65)
            {
                soil_texture = "Tu2";
            }
            else if (clay >= 0.65)
            {
                soil_texture = "Tt";
            }
            else
            {
                soil_texture = "";
            }
            return soil_texture;
        }

        // return sand content given the KA5 soil texture
        public static double ka5_texture_to_sand(string soil_type)
        {
            return ka5_texture_to_sand_and_clay(soil_type)[0];
        }

        // return clay content given the KA5 soil texture
        public static double ka5_texture_to_clay(string soil_type)
        {
            return ka5_texture_to_sand_and_clay(soil_type)[1];
        }

        // return (sand, clay) content given KA5 soil texture
        public static double[] ka5_texture_to_sand_and_clay(string soil_type)
        {
            var xxx = new double[2] { 0.66, 0.0 };
            if (soil_type == "fS")
            {
                xxx[0] = 0.84;
                xxx[1] = 0.02;
            }
            else if (soil_type == "fSms")
            {
                xxx[0] = 0.86;
                xxx[1] = 0.02;
            }
            else if (soil_type == "fSgs")
            {
                xxx[0] = 0.88;
                xxx[1] = 0.02;
            }
            else if (soil_type == "gS")
            {
                xxx[0] = 0.93;
                xxx[1] = 0.02;
            }
            else if (soil_type == "mSgs")
            {
                xxx[0] = 0.96;
                xxx[1] = 0.02;
            }
            else if (soil_type == "mSfs")
            {
                xxx[0] = 0.93;
                xxx[1] = 0.02;
            }
            else if (soil_type == "mS")
            {
                xxx[0] = 0.96;
                xxx[1] = 0.02;
            }
            else if (soil_type == "Ss")
            {
                xxx[0] = 0.93;
                xxx[1] = 0.02;
            }
            else if (soil_type == "Sl2")
            {
                xxx[0] = 0.76;
                xxx[1] = 0.02;
            }
            else if (soil_type == "Sl3")
            {
                xxx[0] = 0.65;
                xxx[1] = 0.1;
            }
            else if (soil_type == "Sl4")
            {
                xxx[0] = 0.6;
                xxx[1] = 0.14;
            }
            else if (soil_type == "Slu")
            {
                xxx[0] = 0.43;
                xxx[1] = 0.12;
            }
            else if (soil_type == "St2")
            {
                xxx[0] = 0.84;
                xxx[1] = 0.11;
            }
            else if (soil_type == "St3")
            {
                xxx[0] = 0.71;
                xxx[1] = 0.21;
            }
            else if (soil_type == "Su2")
            {
                xxx[0] = 0.8;
                xxx[1] = 0.02;
            }
            else if (soil_type == "Su3")
            {
                xxx[0] = 0.63;
                xxx[1] = 0.04;
            }
            else if (soil_type == "Su4")
            {
                xxx[0] = 0.56;
                xxx[1] = 0.04;
            }
            else if (soil_type == "Ls2")
            {
                xxx[0] = 0.34;
                xxx[1] = 0.21;
            }
            else if (soil_type == "Ls3")
            {
                xxx[0] = 0.44;
                xxx[1] = 0.21;
            }
            else if (soil_type == "Ls4")
            {
                xxx[0] = 0.56;
                xxx[1] = 0.21;
            }
            else if (soil_type == "Lt2")
            {
                xxx[0] = 0.3;
                xxx[1] = 0.3;
            }
            else if (soil_type == "Lt3")
            {
                xxx[0] = 0.2;
                xxx[1] = 0.4;
            }
            else if (soil_type == "Lts")
            {
                xxx[0] = 0.42;
                xxx[1] = 0.35;
            }
            else if (soil_type == "Lu")
            {
                xxx[0] = 0.19;
                xxx[1] = 0.23;
            }
            else if (soil_type == "Uu")
            {
                xxx[0] = 0.1;
                xxx[1] = 0.04;
            }
            else if (soil_type == "Uls")
            {
                xxx[0] = 0.3;
                xxx[1] = 0.12;
            }
            else if (soil_type == "Us")
            {
                xxx[0] = 0.31;
                xxx[1] = 0.04;
            }
            else if (soil_type == "Ut2")
            {
                xxx[0] = 0.13;
                xxx[1] = 0.1;
            }
            else if (soil_type == "Ut3")
            {
                xxx[0] = 0.11;
                xxx[1] = 0.14;
            }
            else if (soil_type == "Ut4")
            {
                xxx[0] = 0.09;
                xxx[1] = 0.21;
            }
            else if (soil_type == "Utl")
            {
                xxx[0] = 0.19;
                xxx[1] = 0.23;
            }
            else if (soil_type == "Tt")
            {
                xxx[0] = 0.17;
                xxx[1] = 0.82;
            }
            else if (soil_type == "Tl")
            {
                xxx[0] = 0.17;
                xxx[1] = 0.55;
            }
            else if (soil_type == "Tu2")
            {
                xxx[0] = 0.12;
                xxx[1] = 0.55;
            }
            else if (soil_type == "Tu3")
            {
                xxx[0] = 0.1;
                xxx[1] = 0.37;
            }
            else if (soil_type == "Ts3")
            {
                xxx[0] = 0.52;
                xxx[1] = 0.4;
            }
            else if (soil_type == "Ts2")
            {
                xxx[0] = 0.37;
                xxx[1] = 0.55;
            }
            else if (soil_type == "Ts4")
            {
                xxx[0] = 0.62;
                xxx[1] = 0.3;
            }
            else if (soil_type == "Tu4")
            {
                xxx[0] = 0.05;
                xxx[1] = 0.3;
            }
            else if (soil_type == "L")
            {
                xxx[0] = 0.35;
                xxx[1] = 0.31;
            }
            else if (soil_type == "S")
            {
                xxx[0] = 0.93;
                xxx[1] = 0.02;
            }
            else if (soil_type == "U")
            {
                xxx[0] = 0.1;
                xxx[1] = 0.04;
            }
            else if (soil_type == "T")
            {
                xxx[0] = 0.17;
                xxx[1] = 0.82;
            }
            else if (soil_type == "HZ1")
            {
                xxx[0] = 0.3;
                xxx[1] = 0.15;
            }
            else if (soil_type == "HZ2")
            {
                xxx[0] = 0.3;
                xxx[1] = 0.15;
            }
            else if (soil_type == "HZ3")
            {
                xxx[0] = 0.3;
                xxx[1] = 0.15;
            }
            else if (soil_type == "Hh")
            {
                xxx[0] = 0.15;
                xxx[1] = 0.1;
            }
            else if (soil_type == "Hn")
            {
                xxx[0] = 0.15;
                xxx[1] = 0.1;
            }
            return xxx;
        }
    }

}
