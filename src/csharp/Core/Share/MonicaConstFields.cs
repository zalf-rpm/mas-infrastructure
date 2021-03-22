using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Share
{
    public class MonicaConstFields
    {
        public const string OrigSpec = "origSpec";
        public const string Daily = "daily";
        public const string OutputIds = "outputIds";
        public const string JsonInput = "jsonInput";
        public const string Date = "Date";
        public const string Results = "results";
        public const string Id = "Id";
        public const string LayerAggOp = "layerAggOp";
        public const string Name = "name";
        public const string Organ = "organ";
        public const string TimeAggOp = "timeAggOp";
        public const string ToLayer = "toLayer";
        public const string Type = "type";
        public const string Unit = "unit";
        public const string Crop = "Crop";
        //added
        public const string Monthly = "monthly";
        public const string Yearly = "yearly";
        public const string Year = "year";
        public const string Month = "month";

        public const string DefaultParametersPath = "Data/monica-parameters";

        public readonly List<string> XAxiesNames;

        public MonicaConstFields()
        {
            XAxiesNames = new List<string>();
            XAxiesNames.Add(Date);
            XAxiesNames.Add(Year);
            XAxiesNames.Add(Month);
        }
    }
}
