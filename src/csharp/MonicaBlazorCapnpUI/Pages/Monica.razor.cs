using System;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json.Linq;
using Mas.Rpc;
using Microsoft.AspNetCore.WebUtilities;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mgmt = Mas.Rpc.Management;
using ExtType = Mas.Rpc.Management.Event.ExternalType;
using Mas.Infrastructure.Common;

namespace Mas.Infrastructure.BlazorComponents
{
    public partial class Monica : IDisposable
    {
        #region Monica capability

        [Parameter]
        public Model.IEnvInstance<Rpc.Common.StructuredText, Rpc.Common.StructuredText> MonicaInstanceCap { get; set; }
        [Parameter]
        public String MonicaSturdyRef { get; set; } = "";
        [Parameter]
        public EventCallback<Model.IEnvInstance<Rpc.Common.StructuredText, Rpc.Common.StructuredText>> MonicaInstanceCapChanged { get; set; }

        private async Task MonicaCapabilityChanged(Model.IEnvInstance<Rpc.Common.StructuredText, Rpc.Common.StructuredText> monicaInstance)
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
        public Climate.ITimeSeries TimeSeriesCap { get; set; }
        private Climate.IAlterTimeSeriesWrapper AlterTimeSeriesWrapperCap { get; set; }
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
        public Climate.IAlterTimeSeriesWrapperFactory TimeSeriesFactoryCap { get; set; }

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
        public Soil.IService SoilServiceCap { get; set; }

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

        private SoilService soilServiceRef;
        #endregion soil service cap

        #region climate service cap
        [Parameter]
        public Climate.IService ClimateServiceCap { get; set; }

        [Parameter]
        public string ClimateServiceSturdyRef { get; set; } = "";//"capnp://login01.cluster.zalf.de:9998";

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
        private string monicaErrorMessage;

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

        private async Task MarkDefaultChips(MudChip[] chips, Action<MudChip[]> action = null)
        {
            if (chips.All(c => c == null)) return;

            var selected = new List<MudChip>();
            foreach (var c in chips)
            {
                c.IsSelected = c.Default;
                if (c.IsSelected) selected.Add(c);
            }
            if (action != null) action(selected.ToArray());
            StateHasChanged();
        }

        #region events / outputs
        public enum Agg { AVG, MEDIAN, SUM, MIN, MAX, FIRST, LAST, NONE }
        public class OId
        {
            public static OId Out(string name) { return new OId { Name = name }; }

            public static OId OutL(string name, int from, int to, Agg agg = Agg.NONE)
            { return new OId { Name = name, From = from, To = to, LayerAgg = agg }; }

            public static OId OutT(string name, Agg agg = Agg.AVG)
            { return new OId { Name = name, TimeAgg = agg }; }

            public static OId OutLT(string name, int from, int to, Agg layerAgg = Agg.NONE, Agg timeAgg = Agg.AVG)
            { return new OId { Name = name, From = from, To = to, LayerAgg = layerAgg, TimeAgg = timeAgg }; }

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

            public string Name { get; set; } = "";
            public int? From { get; set; }
            public int? To { get; set; }
            public Agg? LayerAgg { get; set; }
            public Agg? TimeAgg { get; set; }
        }
        private OId editOId = OId.Out("");

        private List<(String, List<OId>)> events = new() { ("daily", new List<OId> { OId.Out("Date"), OId.Out("Crop"), OId.Out("Stage"), OId.Out("Yield"), OId.OutL("Mois", 1, 3), OId.OutL("SOC", 1, 6, Agg.AVG), OId.Out("Tavg"), OId.Out("Precip") }) };

        private List<String> eventShortcuts = new() { "daily", "crop", "monthly", "yearly", "run", "Sowing", "AutomaticSowing", "Harvest", "AutomaticHarvest", "Cutting", "emergence", "anthesis", "maturity", "Stage-1", "Stage-2", "Stage-3", "Stage-4", "Stage-5", "Stage-6", "Stage-7" };

        private JArray CreateSingleEventsSection(List<OId> oids)
        {
            JArray section = new();
            foreach (var oid in oids)
            {
                if (oid.From.HasValue && oid.To.HasValue)
                {
                    var ft = new JArray { oid.From, oid.To };
                    if (oid.LayerAgg.HasValue) ft.Add(oid.LayerAgg.ToString());

                    var a = new JArray { oid.Name, ft };
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
            foreach (var (sectionName, oids) in events)
            {
                es.Add(sectionName);
                es.Add(CreateSingleEventsSection(oids));
            }
            return es;
        }

        #endregion events / outputs

        #region crop rotation
        private List<Mgmt.Event> cropRotation = new()
        {
            new Mgmt.Event()
            {
                TheType = ExtType.sowing,
                At = new() { Date = new Mas.Common.Date { Year = 0, Month = 9, Day = 23 } },
                Params = new Mgmt.Params.Sowing { Cultivar = Mgmt.Cultivar.wheatWinter }
            },
            new Mas.Rpc.Management.Event()
            {
                TheType = ExtType.harvest,
                At = new() { Date = new Mas.Common.Date { Year = 1, Month = 7, Day = 27 } }
            }
        };

        private System.Globalization.TextInfo textInfo = new System.Globalization.CultureInfo("de-DE", false).TextInfo;

        private string NameFromEvent(Mas.Rpc.Management.Event e)
        {
            var typeStr = e.TheType.ToString();

            var type = e.Info == null ? typeStr : textInfo.ToTitleCase(e.Info.Name);
            var crop = (e.Params is Mas.Rpc.Management.Params.Sowing s) ? s.Cultivar.ToString() : "";
            if (!string.IsNullOrEmpty(crop)) type = $"{type} : {crop}";
            var date = e.which == Mgmt.Event.WHICH.At ? Helper.CommonDate2IsoDateString(e.At.Date) : null;
            var amount = (e.Params is Rpc.Management.Params.MineralFertilization m)
                ? m.Amount.ToString() : (e.Params is Rpc.Management.Params.OrganicFertilization o)
                ? o.Amount.ToString() : "";

            return string.IsNullOrEmpty(amount) ? $"{type} @ {date}" : $"{type} @ {date} = {amount}";
        }

        private JArray CreateCropRotation()
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

                            wss.Add(new JObject()
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
                                { "base-temp", sa.BaseTemp },
                                { "avg-soil-temp", new JObject {
                                    { "depth", sa.AvgSoilTemp.SoilDepthForAveraging },
                                    { "days", sa.AvgSoilTemp.DaysInSoilTempWindow },
                                    { "Tavg", sa.AvgSoilTemp.SowingIfAboveAvgSoilTemp } } }

                            });
                        }
                        break;
                    case ExtType.harvest:
                        wss.Add(new JObject()
                        {
                            { "type", "Harvest" },
                            { "date", Helper.CommonDate2IsoDateString(e.At.Date) },
                        });
                        break;
                    case ExtType.automaticHarvest:
                        if (e.Params is Mgmt.Params.AutomaticHarvest ha)
                        {
                            wss.Add(new JObject()
                            {
                                { "type", "Harvest" },
                                { "latest-date", Helper.CommonDate2IsoDateString(e.Between.Latest) },
                                { "min-%-asw", ha.MinPercentASW },
                                { "max-%-asw", ha.MaxPercentASW },
                                { "max-3d-precip-sum", ha.Max3dayPrecipSum },
                                { "max-curr-day-precip", ha.MaxCurrentDayPrecipSum },
                                { "harvest-time", ha.HarvestTime.ToString() }
                            });
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
                                var organ = textInfo.ToTitleCase(cs.Organ.ToString().Replace("strukt", "struct"));
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
                                    { "id", mf.Partition.Id },
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
                                    { "id", of.Params.Id },
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
                                { "type", "MineralFertilization" },
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
                var cr = CreateCropRotation();
                cropj["cropRotation"] = cr;
            }

            var envj = RunMonica.CreateMonicaEnv(simj, cropj, sitej, null, new Core.Share.UserSetting(), Core.Share.Enums.MonicaParametersBasePathTypeEnum.LocalServer);

            var events = new JArray();
            //keep events in files and append the onces defined via UI
            foreach (var jt in envj["events"]) events.Add(jt);
            foreach (var jt in CreateEvents()) events.Add(jt);
            envj["events"] = events;

            envj["params"]["siteParameters"]["Latitude"] = LatLng.Item1;

            var menv = new Model.Env<Rpc.Common.StructuredText>()
            {
                TimeSeries = Capnp.Rpc.Proxy.Share(TimeSeriesCap),
                SoilProfile = profileLayers.Any() ? new Soil.Profile() { Layers = profileLayers } : null,
                Rest = new Rpc.Common.StructuredText()
                {
                    Structure = new Rpc.Common.StructuredText.structure { which = Rpc.Common.StructuredText.structure.WHICH.Json },
                    Value = envj.ToString()
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
                foreach (var section in data.Select(s => s.Value<JObject>()))
                {
                    var oids = section["outputIds"].Select(oid =>
                    oid["displayName"].Value<String>().Length == 0
                    ? oid["name"].Value<String>()
                    : oid["displayName"].Value<String>());

                    var sectionName = section["origSpec"].Value<String>().Trim(new char[] { '\"' });
                    Section2Oid2Data[sectionName] = new Dictionary<String, IEnumerable<float>>();

                    var results = section["results"];
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
        }
        #endregion implement IDisposable
    }


}

