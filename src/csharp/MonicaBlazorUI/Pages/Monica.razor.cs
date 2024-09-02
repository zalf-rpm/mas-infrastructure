using Capnp;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json.Linq;
using ExtType = Mas.Schema.Model.Monica.Event.ExternalType;
using Crop = Mas.Schema.Crop;
using Soil = Mas.Schema.Soil;
using Mas.Infrastructure.Common;
using Mas.Schema.Common;
using Mas.Schema.Geo;
using Mas.Schema.Model.Monica;
using Mas.Schema.Persistence;
using MonicaBlazorUI.Services;
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
        public string MonicaSturdyRef { get; set; } = "";
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
        private Climate.ITimeSeries? PlainTimeSeriesCap { get; set; } // plain cap if wrapper is used
        [Parameter]
        public Climate.ITimeSeries? TimeSeriesCap { get; set; }
        private Climate.IAlterTimeSeriesWrapper? AlterTimeSeriesWrapperCap { get; set; }
        [Parameter]
        public string TimeSeriesSturdyRef { get; set; } = "";
        [Parameter]
        public EventCallback<Climate.ITimeSeries> TimeSeriesCapChanged { get; set; }

        private async Task TimeSeriesCapabilityChanged(Climate.ITimeSeries timeSeries)
        {
            if (timeSeries == null) return;

            if (TimeSeriesCap != timeSeries) TimeSeriesCap?.Dispose();
            TimeSeriesCap = timeSeries;

            if (AlterTimeSeriesWrapperCap != null) await AlterTimeSeriesWrapperCap.ReplaceWrappedTimeSeries(Capnp.Rpc.Proxy.Share(TimeSeriesCap));

        }
        #endregion time series capability

        #region time series wrapper factory capability
        [Parameter]
        public Climate.IAlterTimeSeriesWrapperFactory? TimeSeriesFactoryCap { get; set; }

        [Parameter]
        public string TimeSeriesFactorySturdyRef { get; set; } = "";// = "capnp://10.10.24.86:11006"; //"capnp://login01.cluster.zalf.de:11006";//

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
            PlainTimeSeriesCap = TimeSeriesCap; //store plain time series

            //store wrapper cap
            if (AlterTimeSeriesWrapperCap != wts) 
                AlterTimeSeriesWrapperCap?.Dispose();
            AlterTimeSeriesWrapperCap = wts;

            TimeSeriesCap = AlterTimeSeriesWrapperCap;
        }
        #endregion time series wrapper factory capability

        private List<Soil.Layer> _profileLayers = [];

        [Parameter]
        public (double, double) LatLng { get; set; } = (52.52, 14.11);

        //private string monicaResult;
        private string? _monicaErrorMessage;

        #region init
        protected override async Task OnInitializedAsync()
        {
            Console.WriteLine("OnInitialized Monica SR: " + MonicaSturdyRef);

            _simJsonTxt = File.ReadAllText("Data/sim_template.json");
            _cropJsonTxt = File.ReadAllText("Data/crop_template.json");
            _siteJsonTxt = File.ReadAllText("Data/site_template.json");
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

        private async Task MarkDefaultChips(MudChip<string>[] chips, Action<MudChip<string>[]>? action = null)
        {
            if (chips.All(c => c == null)) return;

            var selected = new List<MudChip<string>>();
            foreach (var c in chips)
            {
                c.Selected = c.Default ?? true;
                if (c.Selected) selected.Add(c);
            }

            action?.Invoke(selected.ToArray());
            StateHasChanged();
        }

        #region events / outputs

        private List<(string, List<EditOutputConfig.OId>, bool)> _events =
        [
            ("daily", new List<EditOutputConfig.OId>
            {
                EditOutputConfig.OId.Out("Date"), EditOutputConfig.OId.Out("Crop"), EditOutputConfig.OId.Out("Stage"), EditOutputConfig.OId.Out("Yield"),
                EditOutputConfig.OId.OutL("Mois", 1, 3), EditOutputConfig.OId.OutL("SOC", 1, 6, EditOutputConfig.Agg.AVG), EditOutputConfig.OId.Out("Tavg"),
                EditOutputConfig.OId.Out("Precip")
            }, false)
        ];

        private JArray CreateSingleEventsSection(List<EditOutputConfig.OId> oids)
        {
            JArray section = [];
            foreach (var oid in oids)
            {
                var name = string.IsNullOrEmpty(oid.DisplayName) ? oid.Name : $"{oid.Name}|{oid.DisplayName}";
                if (oid.From.HasValue)
                {
                    JArray? ft = null;
                    if (oid.To.HasValue)
                    {
                        ft = [oid.From, oid.To];
                        if (oid.LayerAgg.HasValue) ft.Add(oid.LayerAgg.ToString());
                    }

                    JArray a = [name, ft == null ? oid.From : ft];
                    if (oid.TimeAgg.HasValue) a.Add(oid.TimeAgg.ToString());

                    section.Add(a);
                }
                else if(oid.Organ.HasValue)
                {
                    JArray a = [name, oid.Organ?.ToString().Replace("strukt", "struct")];
                    if (oid.TimeAgg.HasValue) a.Add(oid.TimeAgg.ToString());
                    section.Add(a);
                }
                else
                {
                    if (oid.TimeAgg.HasValue) section.Add(new JArray() { name, oid.TimeAgg.ToString() });
                    else section.Add(name);
                }
            }

            return section;
        }

        private JArray CreateEvents()
        {
            JArray es = [];
            foreach (var (sectionName, oids, _) in _events)
            {
                es.Add(sectionName);
                es.Add(CreateSingleEventsSection(oids));
            }
            return es;
        }

        #endregion events / outputs

        #region crop rotation
        private List<Mgmt.Event> _cropRotation =
        [
            new Mgmt.Event()
            {
                TheType = ExtType.sowing,
                At = new Event.at { Date = new Mas.Schema.Common.Date { Year = 0, Month = 9, Day = 23 } },
                Params = new Mgmt.Params.Sowing { Cultivar = "winter wheat" }
            },

            new Mgmt.Event()
            {
                TheType = ExtType.harvest,
                At = new Event.at { Date = new Mas.Schema.Common.Date { Year = 1, Month = 7, Day = 27 } }
            }
        ];

        private System.Globalization.TextInfo _textInfo = new System.Globalization.CultureInfo("de-DE", false).TextInfo;

        private string NameFromEvent(Mgmt.Event e)
        {
            var typeStr = e.TheType.ToString();

            var type = e.Info == null ? typeStr : Helper.Capitalize(e.Info.Name);
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
            foreach(var e in _cropRotation)
            {
                switch (e.TheType)
                {
                    case ExtType.sowing:
                        if (e.Params is Mgmt.Params.Sowing s)
                        {
                            if (wss.Any())
                            {
                                cr.Add(cm);
                                wss = [];
                                cm = new JObject { { "worksteps", wss } };
                            }

                            JObject? cropParams = null;
                            if(s.Crop != null)
                            {
                                s.Cultivar = (await s.Crop.Cultivar()).Id;
                                var cps = await s.Crop.Parameters();
                                if (cps != null && cps is string cpss) cropParams = JObject.Parse(cpss);
                            }

                            var ws = new JObject()
                            {
                                { "type", "Sowing" },
                                { "date", Helper.CommonDate2IsoDateString(e.At.Date) },
                                { "crop", cropParams == null ? new JArray { "ref", "crops", s.Cultivar.ToString() } : cropParams },
                            };
                            if (s.PlantDensity > 0) ws.Add("PlantDensity", s.PlantDensity);
                            wss.Add(ws);
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

                            JObject? cropParams = null;
                            // if(sa.Sowing.Crop != null)
                            // {
                            //     sa.Sowing.Cultivar = (await sa.Sowing.Crop.Cultivar()).Id;
                            //     var cps = await sa.Sowing.Crop.Parameters();
                            //     if (cps != null && cps is string cpss) cropParams = JObject.Parse(cpss);
                            // }

                            var ws = new JObject()
                            {
                                { "type", "AutomaticSowing" },
                                { "earliest-date", Helper.CommonDate2IsoDateString(e.Between.Earliest) },
                                { "latest-date", Helper.CommonDate2IsoDateString(e.Between.Latest) },
                                { "crop", cropParams == null ? new JArray { "ref", "crops", sa.Sowing?.Cultivar.ToString() } : cropParams },
                                { "min-temp", sa.MinTempThreshold },
                                { "days-in-temp-window", sa.DaysInTempWindow },
                                { "min-%-asw", sa.MinPercentASW },
                                { "max-%-asw", sa.MaxPercentASW },
                                { "max-3d-precip-sum", sa.Max3dayPrecipSum },
                                { "max-curr-day-precip", sa.MaxCurrentDayPrecipSum },
                                { "temp-sum-above-base-temp", sa.TempSumAboveBaseTemp },
                                { "base-temp", sa.BaseTemp }
                            };
                            if (sa.Sowing?.PlantDensity > 0) ws.Add("PlantDensity", sa.Sowing.PlantDensity);
                            if(sa.TheAvgSoilTemp != null)
                            {
                                ws["avg-soil-temp"] = new JObject {
                                    { "depth", sa.TheAvgSoilTemp.SoilDepthForAveraging },
                                    { "days", sa.TheAvgSoilTemp.DaysInSoilTempWindow },
                                    { "Tavg", sa.TheAvgSoilTemp.SowingIfAboveAvgSoilTemp } 
                                };
                            }
                            wss.Add(ws);
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
                                var organ = Helper.Capitalize(cs.Organ.ToString().Replace("strukt", "struct"));
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
        public EventCallback<(Dictionary<string, IEnumerable<DateTime>>, Dictionary<string, Dictionary<string, IEnumerable<float>>>)> ResultChanged { get; set; }

        private MudChip<string>[] _defaultSelectedSectionChips = new MudChip<string>[1];

        private Dictionary<string, Dictionary<string, IEnumerable<float>>> _section2Oid2Data = new();

        private Dictionary<string, IEnumerable<DateTime>> _section2Dates = new();

        private string _selectedResultSection = "";

        private string _simJsonTxt = "";
        private string _cropJsonTxt = "";
        private string _siteJsonTxt = "";
        //private string climateCsv = "";

        private bool _monicaResultsChanged = false;

        private class SoilProfile : Soil.IProfile
        {
            public List<Soil.Layer> Layers { get; set; } = [];
            
            public void Dispose()
            {
            }

            public Task<Persistent.SaveResults> Save(Persistent.SaveParams arg_, CancellationToken cancellationToken_ = default)
            {
                throw new NotImplementedException();
            }

            public Task<IdInformation> Info(CancellationToken cancellationToken_ = default)
            {
                throw new NotImplementedException();
            }

            public Task<Soil.ProfileData> Data(CancellationToken cancellationToken_ = default)
            {
                Console.WriteLine("SoilProfile.Data: layers: " + Layers);
                return Task.FromResult(new Soil.ProfileData { Layers = Layers });
            }

            public Task<LatLonCoord> GeoLocation(CancellationToken cancellationToken_ = default)
            {
                throw new NotImplementedException();
            }
        }

        private async Task RunMonicaModel()
        {
            if (MonicaInstanceCap == null || TimeSeriesCap == null) return;

            //var files = new List<string> {
            //    "Data-Full/sim-min.json", "Data-Full/crop-min.json", "Data-Full/site-min.json", "Data-Full/climate-min.csv"
            //};

            var simj = JObject.Parse(_simJsonTxt);
            var cropj = JObject.Parse(_cropJsonTxt);
            var sitej = JObject.Parse(_siteJsonTxt);

            //update crop rotation (before resolving references)
            if (OverwriteCropRotation)
            {
                var cr = await CreateCropRotation();
                var str = cr.ToString();
                cropj["cropRotation"] = cr;
            }

            var (envj, createEnvError) = RunMonica.CreateMonicaEnv(simj, cropj, sitej, "");//, new Core.Share.UserSetting(), Core.Share.Enums.MonicaParametersBasePathTypeEnum.LocalServer);
            if (envj == null || createEnvError.Length > 0)
            {
                _monicaErrorMessage = createEnvError;
                return;
            }

            if (OverwriteOutputConfig)
            {
                //var events = new JArray();
                //keep events in files and append the onces defined via UI
                //foreach (var jt in envj["events"]) events.Add(jt);
                //foreach (var jt in CreateEvents()) events.Add(jt);
                var events = CreateEvents();
                //var str = events.ToString();
                envj?.Value<JArray>("events")?.Replace(events);
            }

            envj?.Value<JObject>("params")?.Value<JObject>("siteParameters")?.Value<JValue>("Latitude")?.Replace(LatLng.Item1);

            var menv = new Model.Env<StructuredText>()
            {
                TimeSeries = Capnp.Rpc.Proxy.Share(TimeSeriesCap),
                SoilProfile = OverwriteSoilProfile && _profileLayers.Count != 0 ? new SoilProfile { Layers = _profileLayers } : null,
                Rest = new StructuredText()
                {
                    Structure = new StructuredText.structure { which = StructuredText.structure.WHICH.Json },
                    Value = envj?.ToString() ?? ""
                }
            };

            _section2Oid2Data.Clear();
            _section2Dates.Clear();

            try
            {
                //Console.WriteLine($"T{Environment.CurrentManagedThreadId} Monica.razor::RunMonicaModel OverwriteSoilProfile: {OverwriteSoilProfile} _profileLayers.Count: {_profileLayers.Count} | -> await MonicaInstanceCap.Run(menv)");
                var res = await MonicaInstanceCap.Run(menv);
                if (res == null) throw new Capnp.Rpc.RpcException("MonicaInstanceCap.Run return null result.");
                var resj = JObject.Parse(res.Value);
                var data = resj["data"]; //list
                foreach (var section in data?.Select(s => s.Value<JObject>()) ?? new List<JObject>())
                {
                    if (section == null) continue;

                    var oids = section["outputIds"]?.Select(oid => {
                        //Console.WriteLine("oid: " + oid);
                        var dn = oid["displayName"]?.Value<string>();
                        if (string.IsNullOrEmpty(dn)) return oid["name"]?.Value<string>() ?? "no-name";
                        return dn;
                    }).ToArray() ?? [];
                    //Console.WriteLine("oids: " + string.Join(",", oids));

                    var sectionName = section["origSpec"]?.Value<string>()?.Trim(['\"']);
                    if (sectionName == null) continue;
                    _section2Oid2Data[sectionName] = new Dictionary<string, IEnumerable<float>>();

                    var results = section["results"];
                    if (results == null) continue;
                    foreach (var (name, result) in oids.Zip(results))
                    {
                        //Console.WriteLine("path: " + result.Path);
                        var type = result.First?.Type;
                        switch (type ?? JTokenType.None)
                        {
                            case JTokenType.Integer:
                            case JTokenType.Float:
                                //Console.WriteLine("result type is float: name = " + name);
                                _section2Oid2Data[sectionName][name] = result.Select(v => v.Value<float>());
                                break;
                            case JTokenType.String:
                                //Console.WriteLine("result type is string: name = " + name);
                                try
                                {
                                    var date = result.First?.Value<DateTime>();
                                    if (!date.HasValue) continue;
                                }
                                catch (FormatException) { continue; }
                                goto case JTokenType.Date;
                            case JTokenType.Date:
                                //Console.WriteLine("result type is date: name = " + name);
                                _section2Dates[sectionName] = result.Select(v => v.Value<DateTime>());
                                break;
                            case JTokenType.Array:
                                //Console.WriteLine("result is array: name = " + name + " result.Count = " + result.First!.AsEnumerable().Count());
                                var elCount = result.Count();
                                for(var i = 0; i < result.First!.AsEnumerable().Count(); i++)
                                {
                                    var arr = new float[elCount];
                                    foreach (var (a, j) in result.Select((a, j) => (a, j)))
                                    {
                                        arr[j] = a[i]?.Value<float>() ?? 0;
                                    }
                                    _section2Oid2Data[sectionName][$"{name}_{i}"] = arr;
                                }
                                break;
                            case JTokenType.None:
                            case JTokenType.Object:
                            case JTokenType.Constructor:
                            case JTokenType.Property:
                            case JTokenType.Comment:
                            case JTokenType.Boolean:
                            case JTokenType.Null:
                            case JTokenType.Undefined:
                            case JTokenType.Raw:
                            case JTokenType.Bytes:
                            case JTokenType.Guid:
                            case JTokenType.Uri:
                            case JTokenType.TimeSpan:
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }
                var error = resj["error"];
                if (error != null)
                {
                    _monicaErrorMessage = error.ToString();
                }
                else
                {
                    //workaround to mark the default selected chip
                    _defaultSelectedSectionChips = new MudChip<string>[_section2Oid2Data.Count];

                    _selectedResultSection = _section2Oid2Data.ContainsKey("daily") ? "daily" : _section2Oid2Data.FirstOrDefault().Key;

                    if (ResultChanged.HasDelegate) _ = ResultChanged.InvokeAsync((_section2Dates, _section2Oid2Data));

                    _monicaResultsChanged = true;
                }
            }
            catch (Capnp.Rpc.RpcException e)
            {
                _monicaErrorMessage = e.ToString();
            }

            StateHasChanged();
            await MarkDefaultChips(_defaultSelectedSectionChips);
        }
        #endregion run monica

        #region implement IDisposable
        public void Dispose()
        {
            Console.WriteLine("Disposing Monica SR: " + MonicaSturdyRef + " cap: " + MonicaInstanceCap);
            MonicaInstanceCap?.Dispose();
            if(PlainTimeSeriesCap != TimeSeriesCap)
            {
                Console.WriteLine("Disposing PlainTimeSeries SR:" + TimeSeriesSturdyRef + " cap: " + TimeSeriesCap);
                PlainTimeSeriesCap?.Dispose();
            }
            Console.WriteLine("Disposing TimeSeries SR:" + TimeSeriesSturdyRef + " cap: " + TimeSeriesCap);
            TimeSeriesCap?.Dispose();
            Console.WriteLine("Disposing TimeSeriesFactory SR:" + TimeSeriesFactorySturdyRef + " cap: " + TimeSeriesFactoryCap);
            TimeSeriesFactoryCap?.Dispose();
            Console.WriteLine("Disposing Monica.CropRegistryCap SR:" + CropServiceSturdyRef + " cap: " + CropServiceCap);
            CropServiceCap?.Dispose();
        }
        #endregion implement IDisposable




    }
}

