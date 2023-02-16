using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json.Linq;
using ExtType = Mas.Schema.Model.Monica.Event.ExternalType;
using Crop = Mas.Schema.Crop;
using Soil = Mas.Schema.Soil;
using Mas.Infrastructure.Common;
using Climate = Mas.Schema.Climate;
using Model = Mas.Schema.Model;
using Monica = Mas.Schema.Model.Monica;
using Mgmt = Mas.Schema.Model.Monica;

namespace Mas.Infrastructure.BlazorComponents
{
    public partial class Monica : IDisposable
    {
        #region Monica capability

        [Parameter]
        public Model.IEnvInstance<Schema.Common.StructuredText, Schema.Common.StructuredText>? MonicaInstanceCap { get; set; }
        [Parameter]
        public String MonicaSturdyRef { get; set; } = "";
        [Parameter]
        public EventCallback<Model.IEnvInstance<Schema.Common.StructuredText, Schema.Common.StructuredText>> MonicaInstanceCapChanged { get; set; }

        private async Task MonicaCapabilityChanged(Model.IEnvInstance<Schema.Common.StructuredText, Schema.Common.StructuredText> monicaInstance)
        {
            if (monicaInstance == null) return;

            if (MonicaInstanceCap != monicaInstance) MonicaInstanceCap?.Dispose();
            MonicaInstanceCap = monicaInstance;
        }
        #endregion Monica capability

        [Parameter]
        public bool HideSturdyRefConnectors { get; set; } = false;

        [Parameter]
        public bool TryConnectOnInit { get; set; } = true;

        #region time series capability
        [Parameter]
        public Climate.ITimeSeries? TimeSeriesCap { get; set; }
        private Climate.IAlterTimeSeriesWrapper? AlterTimeSeriesWrapperCap { get; set; }
        [Parameter]
        public String TimeSeriesSturdyRef { get; set; } = "";
        [Parameter]
        public EventCallback<Climate.ITimeSeries> TimeSeriesCapChanged { get; set; }

        private async Task TimeSeriesCapabilityChanged(Climate.ITimeSeries timeSeries)
        {
            if (timeSeries == null) return;

            if (TimeSeriesCap != timeSeries) TimeSeriesCap?.Dispose();
            TimeSeriesCap = timeSeries;
        }
        #endregion time series capability

        #region time series wrapper factory capability
        [Parameter]
        public Climate.IAlterTimeSeriesWrapperFactory? TimeSeriesFactoryCap { get; set; }

        [Parameter]
        public String TimeSeriesFactorySturdyRef { get; set; } = "";// = "capnp://10.10.24.86:11006"; //"capnp://login01.cluster.zalf.de:11006";//

        [Parameter]
        public EventCallback<Climate.IAlterTimeSeriesWrapperFactory> TimeSeriesFactoryCapChanged { get; set; }

        private async Task TimeSeriesFactoryCapabilityChanged(Climate.IAlterTimeSeriesWrapperFactory factory)
        {
            if (factory == null) return;

            if (TimeSeriesFactoryCap != factory) TimeSeriesFactoryCap?.Dispose();
            TimeSeriesFactoryCap = factory;
        }

        private async Task WrapTimeSeries()
        {
            if (TimeSeriesFactoryCap == null || TimeSeriesCap == null) return;

            var wts = await TimeSeriesFactoryCap.Wrap(Capnp.Rpc.Proxy.Share(TimeSeriesCap));
            if (wts == null) return;

            //store wrapper cap
            if (AlterTimeSeriesWrapperCap != wts) 
                AlterTimeSeriesWrapperCap?.Dispose();
            AlterTimeSeriesWrapperCap = wts;

            //let wrapper point also to general time series, but dispose ref to old time series - just remote wrapper still holds ref
            if (TimeSeriesCap != AlterTimeSeriesWrapperCap) 
                TimeSeriesCap?.Dispose();
            TimeSeriesCap = AlterTimeSeriesWrapperCap;
        }
        #endregion time series wrapper factory capability

        #region soil service
        [Parameter]
        public Soil.IService? SoilServiceCap { get; set; }

        [Parameter]
        public String SoilServiceSturdyRef { get; set; } = "";

        [Parameter]
        public EventCallback<Climate.ITimeSeries> SoilServiceCapChanged { get; set; }
        
        private async Task SoilServiceCapabilityChanged(Soil.IService service)
        {
            if (service == null) return;

            if (SoilServiceCap != service) 
                SoilServiceCap?.Dispose();
            SoilServiceCap = service;
        }

        private List<Soil.Layer> profileLayers = new();

        private SoilService? soilServiceRef;
        #endregion soil service cap

        #region climate service cap
        [Parameter]
        public Climate.IService? ClimateServiceCap { get; set; }

        [Parameter]
        public string ClimateServiceSturdyRef { get; set; } = "";

        [Parameter]
        public EventCallback<Climate.ITimeSeries> ClimateServiceCapChanged { get; set; }

        private async Task ClimateServiceCapabilityChanged(Climate.IService service)
        {
            if (service == null) return;

            if (ClimateServiceCap != service) ClimateServiceCap?.Dispose();
            ClimateServiceCap = service;
        }
        #endregion climate service cap

        [Parameter]
        public (double, double) LatLng { get; set; } = (52.52, 14.11);

        //private String monicaResult;
        private string? monicaErrorMessage;

        #region init
        protected override async Task OnInitializedAsync()
        {
            Console.WriteLine("OnInitialized Monica SR: " + MonicaSturdyRef);

            /*
            var query = new Uri(NavigationManager.Uri).Query;
            var qps = QueryHelpers.ParseQuery(query);

            if (MonicaSturdyRef.Length == 0) MonicaSturdyRef = qps.GetValueOrDefault("monicaSturdyRef", "");
            if (MonicaSturdyRef.Length > 0)
            {
                HideSturdyRefConnectors = true;
                TryConnectOnInit = true;
            }

            if (TimeSeriesSturdyRef.Length == 0) TimeSeriesSturdyRef = qps.GetValueOrDefault("timeSeriesSturdyRef", "");
            if (TimeSeriesSturdyRef.Length > 0)
            {
                HideSturdyRefConnectors = true;
                TryConnectOnInit = true;
            }

            if (SoilServiceSturdyRef.Length == 0) SoilServiceSturdyRef = qps.GetValueOrDefault("soilServiceSturdyRef", "");
            if (SoilServiceSturdyRef.Length > 0)
            {
                HideSturdyRefConnectors = true;
                TryConnectOnInit = true;
            }

            if (ClimateServiceSturdyRef.Length == 0) ClimateServiceSturdyRef = qps.GetValueOrDefault("climateServiceSturdyRef", "");
            if (ClimateServiceSturdyRef.Length > 0)
            {
                HideSturdyRefConnectors = true;
                TryConnectOnInit = true;
            }
            */

            simJsonTxt = File.ReadAllText("Data/sim_template.json");
            cropJsonTxt = File.ReadAllText("Data/crop_template.json");
            siteJsonTxt = File.ReadAllText("Data/site_template.json");
            //climateCsv = File.ReadAllText("Data-Full/climate-min.csv");

            availableOIds.Sort();

            //_ = Task.Delay(1000).ContinueWith(_ => soilServiceRef?.GetAllAvailableSoilProperties());
        }

        #endregion init


        private async Task ClimateCSVInputChanged(string csv)
        {
            if (csv.Length == 0 || TimeSeriesFactoryCap == null || TimeSeriesCap == null) return;

            /*
            var (tsCap, error) = await TimeSeriesFactoryCap.Create(csv, new Climate.CSVTimeSeriesFactory.CSVConfig());
            if (tsCap == null && error.Length != 0)
            {
                monicaErrorMessage = error;
                return;
            }
            */

            var ts = await TimeSeriesFactoryCap.Wrap(TimeSeriesCap);
            /*
            var r = await ts.Range();
            var hs = await ts.Header();
            var loc = await ts.Location();
            var md = await ts.Metadata();
            var d = await ts.Data();
            var dt = await ts.DataT();
            */

            if (TimeSeriesCap != ts) 
                TimeSeriesCap?.Dispose();
            TimeSeriesCap = ts;
        }

        private async Task MarkDefaultChips(MudChip[] chips, Action<MudChip[]>? action = null)
        {
            if (chips.All(c => c == null)) return;

            var selected = new List<MudChip>();
            foreach (var c in chips)
            {
                c.IsSelected = c.Default ?? true;
                if (c.IsSelected) selected.Add(c);
            }
            if (action != null) action(selected.ToArray());
            StateHasChanged();
        }

        #region events / outputs
        public enum Agg { AVG, MEDIAN, SUM, MIN, MAX, FIRST, LAST, NONE }
        public class OId : IComparable<OId>
        {
            public static OId Out(string name) { return new OId { Name = name }; }

            public static OId Out(string name, string desc, string unit = "")
            { return new OId { Name = name, Desc = desc, Unit = unit}; }

            public static OId Out(string name, string desc, Mgmt.PlantOrgan o, string unit = "")
            { return new OId { Name = name, Desc = desc, Unit = unit, Organ = o }; }
            
            public static OId Out(string name, string desc, int layer, string unit = "")
            { return new OId { Name = name, Desc = desc, Unit = unit, From = layer }; }

            public static OId OutL(string name, int from, int? to = null, Agg agg = Agg.NONE)
            { return new OId { Name = name, From = from, To = to, LayerAgg = agg }; }

            public static OId OutT(string name, Agg agg = Agg.AVG)
            { return new OId { Name = name, TimeAgg = agg }; }

            public static OId OutLT(string name, int from, int? to = null, Agg layerAgg = Agg.NONE, Agg timeAgg = Agg.AVG)
            { return new OId { Name = name, From = from, To = to, LayerAgg = layerAgg, TimeAgg = timeAgg }; }

            /*
            public override string ToString()
            {
                if (From.HasValue && To.HasValue && LayerAgg.HasValue && TimeAgg.HasValue)
                    return $"[{Name},[{From},{To},{LayerAgg}],{TimeAgg}]";
                else if (From.HasValue && To.HasValue && LayerAgg.HasValue)
                    return $"[{Name},[{From},{To},{LayerAgg}]]";
                else if (TimeAgg.HasValue)
                    return $"[{Name},{TimeAgg}]";
                return Name;
            }
            */

            public int CompareTo(OId? other)
            {
                // A null value means that this object is greater.
                if (other == null)
                    return 1;
                if (Name != null) 
                    return Name.CompareTo(other.Name);
                return 0;
            }

            public override string ToString() => Name ?? "";
            
            public string Name { get; set; } = "";
            public string Desc { get; set; } = "";
            public string Unit { get; set; } = "";
            public int? From { get; set; }
            public int? To { get; set; }
            public Agg? LayerAgg { get; set; }
            public Agg? TimeAgg { get; set; }
            public Mgmt.PlantOrgan? Organ { get; set; }
        }
        private OId editOId = OId.Out("");

        private List<(String, List<OId>, bool)> events = new() 
        { 
            ("daily", new List<OId> { 
                OId.Out("Date"), OId.Out("Crop"), OId.Out("Stage"), OId.Out("Yield"), 
                OId.OutL("Mois", 1, 3), OId.OutL("SOC", 1, 6, Agg.AVG), OId.Out("Tavg"), 
                OId.Out("Precip") 
            }, false) 
        };

        private List<(String, bool)> eventShortcuts = new() 
        { 
            ("daily", false), 
            ("crop", true), 
            ("monthly", true), 
            ("yearly", true), 
            ("run", true), 
            ("Sowing", false), 
            ("AutomaticSowing", false), 
            ("Harvest", false), 
            ("AutomaticHarvest", false), 
            ("Cutting", false), 
            ("emergence", false), 
            ("anthesis", false), 
            ("maturity", false), 
            ("Stage-1", false), 
            ("Stage-2", false), 
            ("Stage-3", false), 
            ("Stage-4", false),
            ("Stage-5", false),
            ("Stage-6", false), 
            ("Stage-7", false)
        };

        private JArray CreateSingleEventsSection(List<OId> oids)
        {
            JArray section = new();
            foreach (var oid in oids)
            {
                if (oid.From.HasValue)
                {
                    JArray? ft = null;
                    if (oid.To.HasValue)
                    {
                        ft = new JArray { oid.From, oid.To };
                        if (oid.LayerAgg.HasValue) ft.Add(oid.LayerAgg.ToString());
                    }

                    var a = new JArray { oid.Name, ft == null ? oid.From : ft };
                    if (oid.TimeAgg.HasValue) a.Add(oid.TimeAgg.ToString());

                    section.Add(a);
                }
                else if(oid.Organ.HasValue)
                {
                    var a = new JArray { oid.Name, oid.Organ?.ToString().Replace("strukt", "struct") };
                    if (oid.TimeAgg.HasValue) a.Add(oid.TimeAgg.ToString());
                    section.Add(a);
                }
                else
                {
                    if (oid.TimeAgg.HasValue)
                        section.Add(new JArray() { oid.Name, oid.TimeAgg.ToString() });
                    else
                        section.Add(oid.Name);
                }
            }

            return section;
        }

        private JArray CreateEvents()
        {
            JArray es = new();
            foreach (var (sectionName, oids, _) in events)
            {
                es.Add(sectionName);
                es.Add(CreateSingleEventsSection(oids));
            }
            return es;
        }

        #endregion events / outputs

        private string Capitalize(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            return s.Length == 1 ? char.ToUpper(s[0]).ToString() : char.ToUpper(s[0]) + s.Substring(1);
        }

        #region crop rotation
        private List<Mgmt.Event> cropRotation = new()
        {
            new Mgmt.Event()
            {
                TheType = ExtType.sowing,
                At = new() { Date = new Mas.Schema.Common.Date { Year = 0, Month = 9, Day = 23 } },
                Params = new Mgmt.Params.Sowing { Cultivar = "winter wheat" }
            },
            new Mgmt.Event()
            {
                TheType = ExtType.harvest,
                At = new() { Date = new Mas.Schema.Common.Date { Year = 1, Month = 7, Day = 27 } }
            }
        };

        private System.Globalization.TextInfo textInfo = new System.Globalization.CultureInfo("de-DE", false).TextInfo;

        private string NameFromEvent(Mgmt.Event e)
        {
            var typeStr = e.TheType.ToString();

            var type = e.Info == null ? typeStr : Capitalize(e.Info.Name);
            var crop = (e.Params is Mgmt.Params.Sowing s) ? s.Cultivar.ToString() : "";
            if (!string.IsNullOrEmpty(crop)) type = $"{type} : {crop}";
            var date = e.which == Mgmt.Event.WHICH.At ? Helper.CommonDate2IsoDateString(e.At.Date) : null;
            var amount = (e.Params is Schema.Management.Params.MineralFertilization m)
                ? m.Amount.ToString() : (e.Params is Schema.Management.Params.OrganicFertilization o)
                ? o.Amount.ToString() : "";

            return string.IsNullOrEmpty(amount) ? $"{type} @ {date}" : $"{type} @ {date} = {amount}";
        }

        private async Task<JArray> CreateCropRotation()
        {
            var cr = new JArray();
            var wss = new JArray();
            var cm = new JObject { { "worksteps", wss } };
            foreach(var e in cropRotation)
            {
                switch (e.TheType)
                {
                    case ExtType.sowing:
                        if (e.Params is Mgmt.Params.Sowing s)
                        {
                            if (wss.Any())
                            {
                                cr.Add(cm);
                                wss = new JArray();
                                cm = new JObject { { "worksteps", wss } };
                            }

                            if(s.Crop != null)
                            {
                                s.Cultivar = (await s.Crop.Cultivar()).Id;
                            }

                            wss.Add(new JObject()
                            {
                                { "type", "Sowing" },
                                { "date", Helper.CommonDate2IsoDateString(e.At.Date) },
                                { "crop", new JArray { "ref", "crops", s.Cultivar.ToString() } },
                                { "PlantDensity", s.PlantDensity }
                            });
                        }
                        break;
                    case ExtType.automaticSowing:
                        if (e.Params is Mgmt.Params.AutomaticSowing sa)
                        {
                            if (wss.Any())
                            {
                                cr.Add(cm);
                                wss = new JArray();
                                cm = new JObject { { "worksteps", wss } };
                            }

                            var o = new JObject()
                            {
                                { "type", "AutomaticSowing" },
                                { "earliest-date", Helper.CommonDate2IsoDateString(e.Between.Earliest) },
                                { "latest-date", Helper.CommonDate2IsoDateString(e.Between.Latest) },
                                { "crop", new JArray { "ref", "crops", sa.Sowing?.Cultivar.ToString() } },
                                { "min-temp", sa.MinTempThreshold },
                                { "days-in-temp-window", sa.DaysInTempWindow },
                                { "min-%-asw", sa.MinPercentASW },
                                { "max-%-asw", sa.MaxPercentASW },
                                { "max-3d-precip-sum", sa.Max3dayPrecipSum },
                                { "max-curr-day-precip", sa.MaxCurrentDayPrecipSum },
                                { "temp-sum-above-base-temp", sa.TempSumAboveBaseTemp },
                                { "base-temp", sa.BaseTemp }
                            };
                            if(sa.TheAvgSoilTemp != null)
                            {
                                o["avg-soil-temp"] = new JObject {
                                    { "depth", sa.TheAvgSoilTemp.SoilDepthForAveraging },
                                    { "days", sa.TheAvgSoilTemp.DaysInSoilTempWindow },
                                    { "Tavg", sa.TheAvgSoilTemp.SowingIfAboveAvgSoilTemp } 
                                };
                            }
                            wss.Add(o);
                        }
                        break;
                    case ExtType.harvest:
                        if (e.Params is Mgmt.Params.Harvest h)
                        {
                            var o = new JObject()
                            {
                                { "type", "Harvest" },
                                { "date", Helper.CommonDate2IsoDateString(e.At.Date) },
                                { "exported", h.Exported }
                            };
                            if (h.OptCarbMgmtData != null)
                            {
                                o.Add("opt-carbon-conservation", h.OptCarbMgmtData.OptCarbonConservation);
                                o.Add("crop-impact-on-humus-balance", h.OptCarbMgmtData.CropImpactOnHumusBalance);
                                o.Add("crop-usage", h.OptCarbMgmtData.CropUsage == Mgmt.Params.Harvest.CropUsage.greenManure ? "green-manure" : "biomass-production");
                                o.Add("residue-heq", h.OptCarbMgmtData.ResidueHeq);
                                o.Add("organic-fertilizer-heq", h.OptCarbMgmtData.OrganicFertilizerHeq);
                                o.Add("max-residue-recover-fraction", h.OptCarbMgmtData.MaxResidueRecoverFraction);
                            }
                            wss.Add(o);
                        }
                        break;
                    case ExtType.automaticHarvest:
                        if (e.Params is Mgmt.Params.AutomaticHarvest ha)
                        {
                            var o = new JObject()
                            {
                                { "type", "AutomaticHarvest" },
                                { "exported", ha.Harvest.Exported },
                                { "latest-date", Helper.CommonDate2IsoDateString(e.Between.Latest) },
                                { "min-%-asw", ha.MinPercentASW },
                                { "max-%-asw", ha.MaxPercentASW },
                                { "max-3d-precip-sum", ha.Max3dayPrecipSum },
                                { "max-curr-day-precip", ha.MaxCurrentDayPrecipSum },
                                { "harvest-time", ha.HarvestTime.ToString() }
                            };
                            var h2 = ha.Harvest;
                            if (h2.OptCarbMgmtData != null)
                            {
                                o.Add("opt-carbon-conservation", h2.OptCarbMgmtData.OptCarbonConservation);
                                o.Add("crop-impact-on-humus-balance", h2.OptCarbMgmtData.CropImpactOnHumusBalance);
                                o.Add("crop-usage", h2.OptCarbMgmtData.CropUsage == Mgmt.Params.Harvest.CropUsage.greenManure ? "green-manure" : "biomass-production");
                                o.Add("residue-heq", h2.OptCarbMgmtData.ResidueHeq);
                                o.Add("organic-fertilizer-heq", h2.OptCarbMgmtData.OrganicFertilizerHeq);
                                o.Add("max-residue-recover-fraction", h2.OptCarbMgmtData.MaxResidueRecoverFraction);
                            }
                            wss.Add(o);
                        }
                        break;
                    case ExtType.cutting:
                        if (e.Params is Mgmt.Params.Cutting c)
                        {
                            Func<Mgmt.Params.Cutting.Unit, string> toUnit = u =>
                            {
                                var res = "";
                                switch (u)
                                {
                                    case Mgmt.Params.Cutting.Unit.percentage: res = "%"; break;
                                    case Mgmt.Params.Cutting.Unit.biomass: res = "kg ha-1"; break;
                                    case Mgmt.Params.Cutting.Unit.lai: res = "m2 m-2"; break;
                                }
                                return res;
                            };

                            var organs = new JObject();
                            var exports = new JObject();
                            foreach(var cs in c.CuttingSpec)
                            {
                                var organ = Capitalize(cs.Organ.ToString().Replace("strukt", "struct"));
                                organs[organ] = new JArray { cs.Value, toUnit(cs.Unit), cs.CutOrLeft.ToString() };
                                organs[organ] = cs.ExportPercentage;
                            }

                            wss.Add(new JObject()
                            {
                                { "type", "Cutting" },
                                { "date", Helper.CommonDate2IsoDateString(e.At.Date) },
                                { "organs", organs },
                                { "exports", exports },
                                { "cut-max-assimilation-rate", c.CutMaxAssimilationRatePercentage }
                            });
                        }
                        break;
                    case ExtType.mineralFertilization:
                        if (e.Params is Mgmt.Params.MineralFertilization mf)
                        {
                            wss.Add(new JObject()
                            {
                                { "type", "MineralFertilization" },
                                { "date", Helper.CommonDate2IsoDateString(e.At.Date) },
                                { "amount", mf.Amount },
                                { "partition", new JObject() {
                                    { "type", "MineralFertiliserParameters" },
                                    { "id", mf.Partition.Id ?? "" },
                                    { "name", mf.Partition.Name ?? "" },
                                    { "Carbamid", mf.Partition.Carbamid / 100.0 },
                                    { "NH4", mf.Partition.Nh4 / 100.0 },
                                    { "NO3", mf.Partition.No3 / 100.0 } } }
                            });
                        }
                        break;
                    case ExtType.organicFertilization:
                        if (e.Params is Mgmt.Params.OrganicFertilization of)
                        {
                            wss.Add(new JObject()
                            {
                                { "type", "OrganicFertilization" },
                                { "date", Helper.CommonDate2IsoDateString(e.At.Date) },
                                { "amount", of.Amount },
                                { "partition", new JObject() {
                                    { "type", "OrganicFertiliserParameters" },
                                    { "id", of.Params.Id ?? ""},
                                    { "name", of.Params.Name ?? "" },
                                    { "AOM_DryMatterContent", of.Params.Params.AomDryMatterContent },
                                    { "AOM_FastDecCoeffStandard", of.Params.Params.AomFastDecCoeffStandard },
                                    { "AOM_NH4Content", of.Params.Params.AomNH4Content },
                                    { "AOM_NO3Content", of.Params.Params.AomNO3Content },
                                    { "AOM_SlowDecCoeffStandard", of.Params.Params.AomSlowDecCoeffStandard },
                                    { "CN_Ratio_AOM_Fast", of.Params.Params.CnRatioAOMFast },
                                    { "CN_Ratio_AOM_Slow", of.Params.Params.CnRatioAOMSlow },
                                    { "NConcentration", of.Params.Params.NConcentration },
                                    { "PartAOM_Slow_to_SMB_Fast", of.Params.Params.PartAOMSlowToSMBFast },
                                    { "PartAOM_Slow_to_SMB_Slow", of.Params.Params.PartAOMSlowToSMBSlow },
                                    { "PartAOM_to_AOM_Fast", of.Params.Params.PartAOMToAOMFast },
                                    { "PartAOM_to_AOM_Slow", of.Params.Params.PartAOMToAOMSlow } } }
                            });
                        }
                        break;
                    case ExtType.irrigation:
                        if (e.Params is Mgmt.Params.Irrigation i)
                        {
                            wss.Add(new JObject()
                            {
                                { "type", "Irrigation" },
                                { "date", Helper.CommonDate2IsoDateString(e.At.Date) },
                                { "amount", i.Amount },
                                { "parameters", new JObject() {
                                    { "nitrateConcentration", i.Params.NitrateConcentration },
                                    { "sulfateConcentration", i.Params.NitrateConcentration } } }
                            });
                        }
                        break;
                    case ExtType.tillage:
                        if (e.Params is Mgmt.Params.Tillage t)
                        {
                            wss.Add(new JObject()
                            {
                                { "type", "Tillage" },
                                { "date", Helper.CommonDate2IsoDateString(e.At.Date) },
                                { "depth", t.Depth }
                            });
                        }
                        break;
                }
            }

            if (wss.Any()) cr.Add(cm);
            return cr;
        }
        #endregion crop rotation

        #region run monica
        [Parameter]
        public EventCallback<(Dictionary<String, IEnumerable<DateTime>>, Dictionary<String, Dictionary<String, IEnumerable<float>>>)> ResultChanged { get; set; }

        private MudChip[] defaultSelectedSectionChips = new MudChip[1];

        private Dictionary<String, Dictionary<String, IEnumerable<float>>> Section2Oid2Data = new();

        private Dictionary<String, IEnumerable<DateTime>> Section2Dates = new();

        private String selectedResultSection = "";

        private String simJsonTxt = "";
        private String cropJsonTxt = "";
        private String siteJsonTxt = "";
        //private String climateCsv = "";

        private bool MonicaResultsChanged = false;

        private async Task RunMonicaModel()
        {
            if (MonicaInstanceCap == null || TimeSeriesCap == null) return;

            //var files = new List<String> {
            //    "Data-Full/sim-min.json", "Data-Full/crop-min.json", "Data-Full/site-min.json", "Data-Full/climate-min.csv"
            //};

            var simj = JObject.Parse(simJsonTxt);
            var cropj = JObject.Parse(cropJsonTxt);
            var sitej = JObject.Parse(siteJsonTxt);

            //update crop rotation (before resolving references)
            if (overwriteCropRotation)
            {
                var cr = await CreateCropRotation();
                var str = cr.ToString();
                cropj["cropRotation"] = cr;
            }

            var envj = RunMonica.CreateMonicaEnv(simj, cropj, sitej, "");//, new Core.Share.UserSetting(), Core.Share.Enums.MonicaParametersBasePathTypeEnum.LocalServer);

            if (overwriteOutputConfig)
            {
                //var events = new JArray();
                //keep events in files and append the onces defined via UI
                //foreach (var jt in envj["events"]) events.Add(jt);
                //foreach (var jt in CreateEvents()) events.Add(jt);
                var events = CreateEvents();
                var str = events.ToString();
                envj?.Value<JArray>("events")?.Replace(events);
            }

            envj?.Value<JObject>("params")?.Value<JObject>("siteParameters")?.Value<JValue>("Latitude")?.Replace(LatLng.Item1);
            //envj["params"]["siteParameters"]["Latitude"] = LatLng.Item1;

            var menv = new Model.Env<Schema.Common.StructuredText>()
            {
                TimeSeries = Capnp.Rpc.Proxy.Share(TimeSeriesCap),
                SoilProfile = overwriteSoilProfile && profileLayers.Any() ? new Soil.Profile() { Layers = profileLayers } : null,
                Rest = new Schema.Common.StructuredText()
                {
                    Structure = new Schema.Common.StructuredText.structure { which = Schema.Common.StructuredText.structure.WHICH.Json },
                    Value = envj?.ToString() ?? ""
                }
            };

            Section2Oid2Data.Clear();
            Section2Dates.Clear();

            try
            {
                //var datat = await TimeSeriesCap.DataT();
                var res = await MonicaInstanceCap.Run(menv);
                if (res == null) throw new Capnp.Rpc.RpcException("MonicaInstanceCap.Run return null result.");
                var resj = JObject.Parse(res.Value);
                var data = resj["data"]; //list
                foreach (var section in data?.Select(s => s.Value<JObject>()) ?? new List<JObject>())
                {
                    if (section == null) continue;

                    var oids = section["outputIds"]?.Select(oid => {
                        var dn = oid["displayName"]?.Value<String>();
                        if (dn == null || dn.Length == 0) return oid["name"]?.Value<String>() ?? "no-name";
                        else return dn;
                    });

                    var sectionName = section["origSpec"]?.Value<String>()?.Trim(new char[] { '\"' });
                    if (sectionName != null)
                    {
                        Section2Oid2Data[sectionName] = new Dictionary<String, IEnumerable<float>>();

                        var results = section["results"];
                        if (results != null && oids != null)
                        {
                            foreach (var (name, result) in oids.Zip(results))
                            {
                                var type = result.First?.Type;
                                switch (type ?? JTokenType.None)
                                {
                                    case JTokenType.Integer:
                                    case JTokenType.Float: Section2Oid2Data[sectionName][name] = result.Select(v => v.Value<float>()); break;

                                    case JTokenType.String:
                                        try
                                        {
                                            var date = result.First?.Value<DateTime>();
                                            if (!date.HasValue) continue;
                                        }
                                        catch (System.FormatException) { continue; }
                                        goto case JTokenType.Date;
                                    case JTokenType.Date: Section2Dates[sectionName] = result.Select(v => v.Value<DateTime>()); break;
                                }
                            }
                        }
                    }
                }
                var error = resj["error"];

                //workaround to mark the default selected chip
                defaultSelectedSectionChips = new MudChip[Section2Oid2Data.Count()];

                selectedResultSection = Section2Oid2Data.ContainsKey("daily") ? "daily" : Section2Oid2Data.FirstOrDefault().Key;

                if (ResultChanged.HasDelegate) _ = ResultChanged.InvokeAsync((Section2Dates, Section2Oid2Data));

                MonicaResultsChanged = true;
            }
            catch (Capnp.Rpc.RpcException e)
            {
                monicaErrorMessage = e.ToString();
            }

            StateHasChanged();
            await MarkDefaultChips(defaultSelectedSectionChips);
        }
        #endregion run monica

        #region implement IDisposable
        public void Dispose()
        {
            Console.WriteLine("Disposing Monica SR: " + MonicaSturdyRef + " cap: " + MonicaInstanceCap);
            MonicaInstanceCap?.Dispose();
            Console.WriteLine("Disposing TimeSeries SR:" + TimeSeriesSturdyRef + " cap: " + TimeSeriesCap);
            TimeSeriesCap?.Dispose();
            Console.WriteLine("Disposing TimeSeriesFactory SR:" + TimeSeriesFactorySturdyRef + " cap: " + TimeSeriesFactoryCap);
            TimeSeriesFactoryCap?.Dispose();
            Console.WriteLine("Disposing SoilService SR:" + SoilServiceSturdyRef + " cap: " + SoilServiceCap);
            SoilServiceCap?.Dispose();
            Console.WriteLine("Disposing Monica.CropRegistryCap SR:" + CropRegistrySturdyRef + " cap: " + CropRegistryCap);
            CropRegistryCap?.Dispose();
        }
        #endregion implement IDisposable


        private List<OId> availableOIds = new()
        {
            OId.Out("Count", "output 1 for counting things"),
            OId.Out("CM-count", "output the order number of the current cultivation method"),
            OId.Out("Date", "output current date"),
            OId.Out("days-since-start", "output number of days since simulation start"),
            OId.Out("DOY", "output current day of year"),
            OId.Out("Month", "output current Month"),
            OId.Out("Year", "output current Year"),
            OId.Out("Crop", "crop name"),
            OId.Out("TraDef", "TranspirationDeficit", ""),
            OId.Out("Tra", "ActualTranspiration", "mm"),
            OId.Out("NDef", "CropNRedux, indicates N availability: 1 no stress, 0 no N available", ""),
            OId.Out("HeatRed", "HeatStressRedux", ""),
            OId.Out("FrostRed", "FrostStressRedux", ""),
            OId.Out("OxRed", "OxygenDeficit", ""),
            OId.Out("Stage", "DevelopmentalStage", ""),
            OId.Out("TempSum", "CurrentTemperatureSum", "°Cd"),
            OId.Out("VernF", "VernalisationFactor", ""),
            OId.Out("DaylF", "DaylengthFactor", ""),
            OId.Out("IncRoot", "OrganGrowthIncrement root", "kg/ha"),
            OId.Out("IncLeaf", "OrganGrowthIncrement leaf", "kg/ha"),
            OId.Out("IncShoot", "OrganGrowthIncrement shoot", "kg/ha"),
            OId.Out("IncFruit", "OrganGrowthIncrement fruit", "kg/ha"),
            OId.Out("RelDev", "RelativeTotalDevelopment", ""),
            OId.Out("LT50", "LT50", "°C"),
            OId.Out("AbBiom", "AbovegroundBiomass", "kg/ha"),
            OId.Out("OrgBiom", "OrganBiomass", Mgmt.PlantOrgan.leaf, "kg-DM/ha"),
            OId.Out("OrgGreenBiom", "OrganGreenBiomass", Mgmt.PlantOrgan.leaf, "kg-DM/ha"),
            OId.Out("Yield", "get_PrimaryCropYield", "kg-DM/ha"),
            OId.Out("SumYield", "get_AccumulatedPrimaryCropYield", "kg-DM/ha"),
            OId.Out("sumExportedCutBiomass", "return sum(across cuts) of exported cut biomass for current crop", "kg-DM/ha"),
            OId.Out("exportedCutBiomass", "return exported cut biomass for current crop and cut", "kg-DM/ha"),
            OId.Out("sumResidueCutBiomass", "return sum(across cuts) of residue cut biomass for current crop", "kg-DM/ha"),
            OId.Out("residueCutBiomass", "return residue cut biomass for current crop and cut", "kg-DM/ha"),
            OId.Out("optCarbonExportedResidues", "return exported part of the residues according to optimal carbon balance", "kg-DM/ha"),
            OId.Out("optCarbonReturnedResidues", "return returned to soil part of the residues according to optimal carbon balance", "kg-DM/ha"),
            OId.Out("humusBalanceCarryOver", "return humus balance carry over according to optimal carbon balance", "Heq-NRW/ha"),
            OId.Out("SecondaryYield", "SecondaryCropYield", "kg-DM/ha"),
            OId.Out("GroPhot", "GrossPhotosynthesisHaRate", "kg-CH2O/ha"),
            OId.Out("NetPhot", "NetPhotosynthesis", "kg-CH2O/ha"),
            OId.Out("MaintR", "MaintenanceRespirationAS", "kg-CH2O/ha"),
            OId.Out("GrowthR", "GrowthRespirationAS", "kg-CH2O/ha"),
            OId.Out("StomRes", "StomataResistance", "s/m"),
            OId.Out("Height", "CropHeight", "m"),
            OId.Out("LAI", "LeafAreaIndex", "m2/m2"),
            OId.Out("RootDep", "RootingDepth", "Layer#"),
            OId.Out("EffRootDep", "Effective RootingDepth", "m"),
            OId.Out("TotBiomN", "TotalBiomassNContent", "kg-N/ha"),
            OId.Out("AbBiomN", "AbovegroundBiomassNContent", "kg-N/ha"),
            OId.Out("SumNUp", "SumTotalNUptake", "kg-N/ha"),
            OId.Out("ActNup", "ActNUptake", "kg-N/ha"),
            OId.Out("PotNup", "PotNUptake", "kg-N/ha"),
            OId.Out("NFixed", "NFixed", "kg-N/ha"),
            OId.Out("Target", "TargetNConcentration", "kg-N/ha"),
            OId.Out("CritN", "CriticalNConcentration", "kg-N/ha"),
            OId.Out("AbBiomNc", "AbovegroundBiomassNConcentration", "kg-N/ha"),
            OId.Out("Nstress", "NitrogenStressIndex", ""),
            OId.Out("YieldNc", "PrimaryYieldNConcentration", "kg-N/ha"),
            OId.Out("YieldN", "PrimaryYieldNContent", "kg-N/ha"),
            OId.Out("Protein", "RawProteinConcentration", "kg/kg"),
            OId.Out("NPP", "NPP", "kg-C/ha"),
            OId.Out("NPP-Organs", "organ specific NPP", Mgmt.PlantOrgan.leaf, "kg-C/ha"),
            OId.Out("GPP", "GPP", "kg-C/ha"),
            OId.Out("Ra", "autotrophic respiration", "kg-C/ha"),
            OId.Out("Ra-Organs", "organ specific autotrophic respiration", Mgmt.PlantOrgan.leaf, "kg-C/ha"),
            OId.Out("Mois", "Soil moisture content", 1, "m3/m3"),
            OId.Out("ActNupLayer", "ActNUptakefromLayer", 1, "kg-N/ha"),
            OId.Out("Irrig", "Irrigation", "mm"),
            OId.Out("Infilt", "Infiltration", "mm"),
            OId.Out("Surface", "Surface water storage", "mm"),
            OId.Out("RunOff", "Surface water runoff", "mm"),
            OId.Out("SnowD", "Snow depth", "mm"),
            OId.Out("FrostD", "Frost front depth in soil", "m"),
            OId.Out("ThawD", "Thaw front depth in soil", "m"),
            OId.Out("PASW", "Plant Available Soil Water", 1, "m3/m3"),
            OId.Out("SurfTemp", "Surface temperature", "°C"),
            OId.Out("STemp", "Soil temperature", 1, "°C"),
            OId.Out("Act_Ev", "Actual evaporation", "mm"),
            OId.Out("Pot_ET", "Potential evapotranspiration", "mm"),
            OId.Out("Act_ET", "Actual evapotranspiration", "mm"),
            OId.Out("Act_ET2", "ActualEvaporation + Transpiration", "mm"),
            OId.Out("ET0", "ET0", "mm"),
            OId.Out("Kc", "Kc", ""),
            OId.Out("AtmCO2", "Atmospheric CO2 concentration", "ppm"),
            OId.Out("AtmO3", "Atmospheric O3 concentration", "ppb"),
            OId.Out("Groundw", "Groundwater level", "m"),
            OId.Out("Recharge", "Groundwater recharge", "mm"),
            OId.Out("NLeach", "N leaching", "kg-N/ha"),
            OId.Out("NO3", "Soil NO3 content", 1, "kg-N/m3"),
            OId.Out("Carb", "Soil Carbamid content", 1, "kg-N/m3"),
            OId.Out("NH4", "Soil NH4 content", 1, "kg-N/m3"),
            OId.Out("NO2", "NO2", "kg-N/m3"),
            OId.Out("SOC", "Soil organic carbon content", 1, "kg-C/kg"),
            OId.Out("SOC-X-Y", "SOC*SoilBulkDensity*LayerThickness*1000", 1, "g-C/m2"),
            OId.Out("AOMf", "AOM_FastSum", 1, "kg-C/m3"),
            OId.Out("AOMs", "AOM_SlowSum", 1, "kg-C/m3"),
            OId.Out("SMBf", "SMB_Fast", 1, "kg-C/m3"),
            OId.Out("SMBs", "SMB_Slow", 1, "kg-C/m3"),
            OId.Out("SOMf", "SOM_Fast", 1, "kg-C/m3"),
            OId.Out("SOMs", "SOM_Slow", 1, "kg-C/m3"),
            OId.Out("CBal", "get_CBalance", 1, "kg-C/m3"),
            OId.Out("Nmin", "NetNMineralisationRate", 1, "kg-N/ha"),
            OId.Out("NetNmin", "NetNminRate for the layers defined in the parameter MaxMineralizationDepth(general/soil-organic.json)", "kg-N/ha"),
            OId.Out("Denit", "Amount of N resulting from denitrification", "kg-N/ha"),
            OId.Out("actnitrate", "N production rate resulting from nitrification(N2O STICS module)", "kg-N/m3"),
            OId.Out("N2O", "Total N2O produced(Monica's original approach)", "kg-N/ha"),
            OId.Out("N2Onit", "N2O produced through nitrification (N2O STICS module)", "kg-N/ha"),
            OId.Out("N2Odenit", "N2O produced through denitrification(N2O STICS module)", "kg-N/ha"),
            OId.Out("SoilpH", "SoilpH", ""),
            OId.Out("NEP", "NEP", "kg-C/ha"),
            OId.Out("NEE", "NEE", "kg-C/ha"),
            OId.Out("Rh", "Rh", "kg-C/ha"),
            OId.Out("Tmin", "Daily minimum temperature", "°C"),
            OId.Out("Tavg", "Daily average temperature", "°C"),
            OId.Out("Tmax", "Daily maximum temperature", "°C"),
            OId.Out("Precip", "Daily precipitation", "mm"),
            OId.Out("Wind", "Daily average windspeed", "m/s"),
            OId.Out("Globrad", "Daily global radiation", "MJ/m2"),
            OId.Out("Relhumid", "Daily relative humidity", "%"),
            OId.Out("Sunhours", "If available? Daily number of sunshine hours", "h"),
            OId.Out("BedGrad", "PercentageSoilCoverage", ""),
            OId.Out("N", "Nitrate", 1, "kg-N/m3"),
            OId.Out("Co", "Co", 1, "kg-C/m3"),
            OId.Out("NH3", "NH3_Volatilised", "kg-N/ha"),
            OId.Out("NFert", "dailySumFertiliser", "kg-N/ha"),
            OId.Out("SumNFert", "sum of N fertilizer applied during cropping period", "kg-N/ha"),
            OId.Out("NOrgFert", "dailySumOrgFertiliser", "kg-N/ha"),
            OId.Out("SumNOrgFert", "sum of N of organic fertilizer applied during cropping period", "kg-N/ha"),
            OId.Out("WaterContent", "soil water content", "%nFC"),
            OId.Out("AWC", "available water capacity", "m3/m3"),
            OId.Out("CapillaryRise", "Capillary rise", 1, "mm"),
            OId.Out("PercolationRate", "percolation rate", 1, "mm"),
            OId.Out("SMB-CO2-ER", "SMB_CO2EvolutionRate", 1, ""),
            OId.Out("Evapotranspiration", "", "mm"),
            OId.Out("Evaporation", "", "mm"),
            OId.Out("ETa/ETc", "actual evapotranspiration / potential evapotranspiration", ""),
            OId.Out("Transpiration", "", "mm"),
            OId.Out("GrainN", "FruitBiomassNContent", "kg/ha"),
            OId.Out("Fc", "Field capacity", 1, "m3/m3"),
            OId.Out("Pwp", "Permanent wilting point", 1, "m3/m3"),
            OId.Out("Sat", "saturation", 1, "m3/m3"),
            OId.Out("Nresid", "Nitrogen content in crop residues", "kg-N/ha"),
            OId.Out("Sand", "Soil sand content", "kg/kg"),
            OId.Out("Clay", "Soil clay content", "kg/kg"),
            OId.Out("Silt", "Soil silt content", "kg/kg"),
            OId.Out("Stone", "Soil stone content", "kg/kg"),
            OId.Out("pH", "Soil pH content", ""),
            OId.Out("rootDensity", "Root density at layer", 1, ""),
            OId.Out("rootingZone", "Layer into which roots reach", "Layer#"),
            OId.Out("actammoxrate", "actAmmoniaOxidationRate", "kgN/m3/d"),
            OId.Out("actnitrate", "actNitrificationRate", "kgN/m3/d"),
            OId.Out("actdenitrate", "actDenitrificationRate", "kgN/m3/d"),
            OId.Out("O3-short-damage", "short term ozone induced reduction of Ac", ""),
            OId.Out("O3-long-damage", "long term ozone induced senescence", ""),
            OId.Out("O3-WS-gs-reduction", "water stress impact on stomatal conductance", ""),
            OId.Out("O3-total-uptake", "total O3 uptake", "?"),
            OId.Out("NO3conv", "Convection", ""),
            OId.Out("NO3disp", "Dispersion", ""),
            OId.Out("noOfAOMPools", "number of AOM pools in existence currently", "#")
        };

    }


}

