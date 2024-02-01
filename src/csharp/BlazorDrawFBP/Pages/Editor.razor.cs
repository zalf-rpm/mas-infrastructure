using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blazor.Diagrams;
using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Behaviors;
using Blazor.Diagrams.Core.Controls;
using Blazor.Diagrams.Core.Controls.Default;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.PathGenerators;
using Blazor.Diagrams.Core.Routers;
using Blazor.Diagrams.Options;
using BlazorDrawFBP.Behaviors;
using BlazorDrawFBP.Controls;
using BlazorDrawFBP.Models;
using Mas.Schema.Climate;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharedDemo.Demos;

namespace BlazorDrawFBP.Pages
{
    public partial class Editor
    {
        private static readonly Random Random = new();
        private BlazorDiagram Diagram { get; set; } = null!;
        //private readonly List<string> events = new List<string>();

        private JObject _components = null!;
        private Dictionary<string, JObject> _componentDict = new();
        
        protected override void OnInitialized()
        {
            var options = new BlazorDiagramOptions
            {
                AllowMultiSelection = true,
                Zoom = { Enabled = true, Inverse = true },
                Links =
                {
                    DefaultRouter = new NormalRouter(),
                    DefaultPathGenerator = new SmoothPathGenerator()
                },
                Groups = { Enabled = true }
            };

            Diagram = new BlazorDiagram(options);
            var ksb = Diagram.GetBehavior<KeyboardShortcutsBehavior>();
            ksb?.RemoveShortcut("Delete", false, false, false);
            ksb?.SetShortcut("Delete", false, true, false, KeyboardShortcutsDefaults.DeleteSelection);
            
            _components = JObject.Parse(File.ReadAllText("Data/components.json"));
            foreach (var (cat, value) in _components)
            {
                if (value is not JArray components) continue;
                foreach (var component in components)
                {
                    if (component is JObject comp) _componentDict.Add(comp["id"]?.ToString() ?? "", comp);
                }
            }

            Diagram.RegisterComponent<CapnpFbpComponentModel, CapnpFbpComponentWidget>();
            Diagram.RegisterComponent<CapnpFbpIipModel, CapnpFbpIipWidget>();
            Diagram.RegisterComponent<UpdatePortNameNode, UpdatePortNameNodeWidget>();
            Diagram.RegisterComponent<PortOptionsNode, PortOptionsNodeWidget>();
            Diagram.RegisterComponent<NodeInformationControl, NodeInformationControlWidget>();
            Diagram.RegisterComponent<LinkInformationControl, LinkInformationControlWidget>();
            Diagram.RegisterComponent<AddPortControl, AddPortControlWidget>();
            Diagram.RegisterComponent<ToggleEditNodeControl, ToggleEditNodeControlWidget>();
            Diagram.RegisterComponent<RemoveProcessControl, RemoveProcessControlWidget>();
            Diagram.RegisterComponent<RemoveLinkControl, RemoveLinkControlWidget>();
            Diagram.RegisterComponent<LinkModel, FbpLinkWidget>(true);
            RegisterEvents();
            
            //var oldDragNewLinkBehavior = Diagram.GetBehavior<DragNewLinkBehavior>()!;
            Diagram.UnregisterBehavior<DragNewLinkBehavior>();
            Diagram.RegisterBehavior(new FbpDragNewLinkBehavior(Diagram));
        }

        private void RegisterEvents()
        {
            // Diagram.Changed += () =>
            // {
            //     events.Add("Changed");
            //     StateHasChanged();
            // };

            // Diagram.Nodes.Added += (n) => events.Add($"NodesAdded, NodeId={n.Id}");
            // Diagram.Nodes.Removed += (n) => events.Add($"NodesRemoved, NodeId={n.Id}");

            // Diagram.SelectionChanged += (m) =>
            // {
            //     events.Add($"SelectionChanged, Id={m.Id}, Type={m.GetType().Name}, Selected={m.Selected}");
            //     StateHasChanged();
            // };

            Diagram.Links.Added += (l) =>
            {
                Diagram.Controls.AddFor(l).Add(new RemoveLinkControl(0.5, 0.5));
                switch (l.Source.Model)
                {
                    case CapnpFbpPortModel sourcePort:
                    {
                        var targetPort = l.Target.Model as CapnpFbpPortModel;
                        switch (sourcePort.ThePortType)
                        {
                            case CapnpFbpPortModel.PortType.In:
                                l.Labels.Add(new LinkLabelModel(l, sourcePort.Name, 40));
                                l.Labels.Add(new LinkLabelModel(l, targetPort?.Name ?? "OUT", -40));
                                l.SourceMarker = LinkMarker.Arrow;
                                l.TargetChanged += (link, oldTarget, newTarget) =>
                                {
                                    if (newTarget.Model is not CapnpFbpPortModel outPort) return;
                                    link.Labels[1].Content = outPort.Name;
                                    link.Refresh();
                                };
                                break;
                            case CapnpFbpPortModel.PortType.Out:
                                l.Labels.Add(new LinkLabelModel(l, sourcePort.Name, 40));
                                l.Labels.Add(new LinkLabelModel(l, targetPort?.Name ?? "IN", -40));
                                l.TargetMarker = LinkMarker.Arrow;
                                l.TargetChanged += (link, oldTarget, newTarget) =>
                                {
                                    if (newTarget.Model is not CapnpFbpPortModel inPort) return;
                                    link.Labels[1].Content = inPort.Name;
                                    link.Refresh();
                                };
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        break;
                    }
                    case CapnpFbpIipPortModel:
                    {
                        var targetPort = l.Target.Model as CapnpFbpPortModel;
                        l.Labels.Add(new LinkLabelModel(l, targetPort?.Name ?? "IN", -40));
                        l.TargetMarker = LinkMarker.Arrow;
                        l.TargetChanged += (link, oldTarget, newTarget) =>
                        {
                            if (newTarget.Model is not CapnpFbpPortModel inPort) return;
                            link.Labels[0].Content = inPort.Name;
                            link.Refresh();
                        };
                        break;
                    }
                }

                // Console.WriteLine($"Links.Added, LinkId={l.Id}, Source={l.Source}, Target={l.Target}");
                // events.Add($"Links.Added, LinkId={l.Id}");
            };

            // Diagram.Links.Removed += (l) => events.Add($"Links.Removed, LinkId={l.Id}");

            // Diagram.PointerDown += (m, e) =>
            // {
            //     //Console.WriteLine($"MouseDown, Type={m?.GetType().Name}, ModelId={m?.Id}, Position=({e.ClientX}/{e.ClientY}");
            //     events.Add($"MouseDown, Type={m?.GetType().Name}, ModelId={m?.Id}");
            //     StateHasChanged();
            // };

            // Diagram.PointerUp += (m, e) =>
            // {
            //     events.Add($"MouseUp, Type={m?.GetType().Name}, ModelId={m?.Id}");
            //     StateHasChanged();
            // };

            // Diagram.PointerEnter += (m, e) =>
            // {
            //     //Console.WriteLine($"TouchStart, Type={m?.GetType().Name}, ModelId={m?.Id}, Position=({e.ClientX}/{e.ClientY}");
            //     events.Add($"TouchStart, Type={m?.GetType().Name}, ModelId={m?.Id}");
            //     StateHasChanged();
            // };

            // Diagram.PointerLeave += (m, e) =>
            // {
            //     events.Add($"TouchEnd, Type={m?.GetType().Name}, ModelId={m?.Id}");
            //     StateHasChanged();
            // };

            Diagram.PointerClick += (m, e) =>
            {
                if (m is CapnpFbpPortModel port)
                {
                    var relativePt = Diagram.GetRelativeMousePoint(e.ClientX, e.ClientY);

                    // find closest port, assuming the user will click on the label he actually wants to change
                    var node = new PortOptionsNode(relativePt)
                    {
                        Label = $"Change {port.Name}",
                        PortModel = port,
                        NodeModel = port.Parent as CapnpFbpComponentModel,
                        Container = Diagram
                    };
                    Diagram.Nodes.Add(node);
                }
                
                //Console.WriteLine($"MouseClick, Type={m?.GetType().Name}, ModelId={m?.Id}, Position=({e.ClientX}/{e.ClientY}");
                //events.Add($"MouseClick, Type={m?.GetType().Name}, ModelId={m?.Id}");
                StateHasChanged();
            };

            Diagram.PointerDoubleClick += (m, e) =>
            {
                if (m is LinkModel link)
                {
                    if (link.Source.Model is CapnpFbpPortModel source &&
                        link.Target.Model is CapnpFbpPortModel target)
                    {
                        var relativePt = Diagram.GetRelativeMousePoint(e.ClientX, e.ClientY);

                        // find closest port, assuming the user will click on the label he actually wants to change
                        var sourceToPoint = relativePt.DistanceTo(source.MiddlePosition);
                        var targetToPoint = relativePt.DistanceTo(target.MiddlePosition);
                        var labelIndex = sourceToPoint < targetToPoint ? 0 : 1; 
                        var node = new UpdatePortNameNode(relativePt)
                        {
                            Label = $"Change {link.Labels[labelIndex].Content}",
                            LabelModel = link.Labels[labelIndex],
                            PortModel = sourceToPoint < targetToPoint ? source : target,
                            Container = Diagram
                        };
                        Diagram.Nodes.Add(node);
                    }
                }

                // Console.WriteLine(
                //     $"MouseDoubleClick, Type={m?.GetType().Name}, ModelId={m?.Id}, Position=({e.ClientX}/{e.ClientY}");
                // events.Add($"MouseDoubleClick, Type={m?.GetType().Name}, ModelId={m?.Id}");
                StateHasChanged();
            };
        }

        protected void AddNode()
        {
            var x = Random.Next(0, (int)Diagram.Container.Width - 120);
            var y = Random.Next(0, (int)Diagram.Container.Height - 100);
            AddNode(x, y);
        }

        protected void AddNode(double x, double y)
        {
            var node = new CapnpFbpComponentModel(new Point(x, y));
            Diagram.Nodes.Add(node);
        }

        protected void AddDefaultNode()
        {
            var x = Random.Next(0, (int)Diagram.Container.Width - 120);
            var y = Random.Next(0, (int)Diagram.Container.Height - 100);
            Diagram.Nodes.Add(new NodeModel(new Point(x, y)));
        }

        protected void RemoveNode()
        {
            var node = Diagram.Nodes.FirstOrDefault(n => n.Selected);
            if (node == null) return;
            Diagram.Nodes.Remove(node);
        }

        protected async Task LoadDiagram(IBrowserFile file)
        {
            var s = file.OpenReadStream();
            if (s.Length > 1*1024*1024) return; // 1 MB
            var dia = JObject.Parse(await new StreamReader(s).ReadToEndAsync());
            var oldNodeIdToNewNode = new Dictionary<string, NodeModel>();
            foreach (var node in dia["nodes"] ?? new JArray())
            {
                if (node is not JObject obj) continue;

                var position = new Point(obj["location"]?["x"]?.Value<double>() ?? 0, 
                    obj["location"]?["y"]?.Value<double>() ?? 0);
                
                var component = obj["component_id"]?.Type switch
                {
                    null => obj["inline_component"] as JObject,
                    JTokenType.Null => obj["inline_component"] as JObject,
                    _ => _componentDict[obj["component_id"]?.ToString() ?? ""]
                } ?? new JObject() { { "type", "CapnpFbpComponent" } };
                var diaNode = AddFbpNode(position, component, obj);
                oldNodeIdToNewNode.Add(obj["node_id"]?.ToString() ?? "", diaNode);
            }

            foreach (var link in dia["links"] ?? new JArray())
            {
                if (link["source"] is not JObject source || link["target"] is not JObject target) continue;
                
                var sourcePortName = source["port"]?.ToString();
                var targetPortName = target["port"]?.ToString();
                if (sourcePortName == null || targetPortName == null) continue;
                
                var sourceNode = oldNodeIdToNewNode[source["node_id"]?.ToString() ?? ""];
                var targetNode = oldNodeIdToNewNode[target["node_id"]?.ToString() ?? ""];
                if (sourceNode == null || sourceNode.Ports.Count == 0 || 
                    targetNode == null || targetNode.Ports.Count == 0) continue;

                var sourcePort = sourceNode.Ports.Where(p => 
                        p is CapnpFbpPortModel capnpPort && capnpPort.Name == sourcePortName)
                    .DefaultIfEmpty(null).First();
                var targetPort = targetNode.Ports.Where(p => 
                        p is CapnpFbpPortModel capnpPort && capnpPort.Name == targetPortName)
                    .DefaultIfEmpty(null).First();
                // might be an IIP
                sourcePort ??= sourceNode.Ports.Where(p =>
                        p is CapnpFbpIipPortModel iipPort && iipPort.Alignment.ToString() == sourcePortName)
                    .DefaultIfEmpty(null).First();
                if (sourcePort == null || targetPort == null) continue;
                Diagram.Links.Add(new LinkModel(sourcePort, targetPort));
            }
        }
        
        protected async Task SaveDiagram()
        {
            var dia = JObject.Parse(await File.ReadAllTextAsync("Data/diagram_template.json"));
            HashSet<string> linkSet = new();
            foreach(var node in Diagram.Nodes)
            {
                switch (node)
                {
                    case CapnpFbpComponentModel fbpNode:
                    {
                        var cmdParams = new JObject();
                        foreach (var line in fbpNode.CmdParamString.Split('\n'))
                        {
                            var kv = line.Split('=');
                            var k = kv[0].Trim();
                            var v = kv.Length == 2 ? kv[1].Trim() : "";
                            if (k.Length > 0 && v.Length > 0) cmdParams.Add(k, v);
                        }

                        var jn = new JObject()
                        {
                            { "node_id", fbpNode.Id },
                            { "process_name", fbpNode.ProcessName },
                            { "location", new JObject() { { "x", fbpNode.Position.X }, { "y", fbpNode.Position.Y } } },
                            { "editable", fbpNode.Editable },
                            { "parallel_processes", fbpNode.InParallelCount },
                            {
                                "data", new JObject()
                                {
                                    { "cmd_params", cmdParams }
                                }
                            }
                        };
                        if (string.IsNullOrEmpty(fbpNode.ComponentId) ||
                            !_componentDict.ContainsKey(fbpNode.ComponentId))
                        {
                            // create inputs
                            var inputs = fbpNode.Ports.Where(p => p is CapnpFbpPortModel cp
                                                                  && cp.ThePortType == CapnpFbpPortModel.PortType.In)
                                .Select(p => p as CapnpFbpPortModel)
                                .Select(p => new JObject() { { "name", p!.Name } });

                            //create outputs
                            var outputs = fbpNode.Ports.Where(p => p is CapnpFbpPortModel cp
                                                                   && cp.ThePortType == CapnpFbpPortModel.PortType.Out)
                                .Select(p => p as CapnpFbpPortModel)
                                .Select(p => new JObject() { { "name", p!.Name } });

                            jn.Add("inline_component", new JObject()
                            {
                                { "id", fbpNode.ComponentId },
                                { "type", "CapnpFbpComponent" },
                                { "interpreter", fbpNode.PathToInterpreter },
                                { "description", fbpNode.ShortDescription },
                                { "inputs", new JArray(inputs) },
                                { "outputs", new JArray(outputs) },
                                { "path", fbpNode.PathToFile }
                            });
                        }
                        else
                        {
                            jn.Add("component_id", fbpNode.ComponentId);
                        }

                        if (dia["nodes"] is JArray nodes) nodes.Add(jn);

                        break;
                    }
                    case CapnpFbpIipModel iipNode:
                    {
                        var jn = new JObject()
                        {
                            { "node_id", iipNode.Id },
                            { "component_id", iipNode.ComponentId },
                            { "location", new JObject() { { "x", iipNode.Position.X }, { "y", iipNode.Position.Y } } },
                            {
                                "data", new JObject()
                                {
                                    { "content", iipNode.Content }
                                }
                            },
                        };
                        if (dia["nodes"] is JArray nodes) nodes.Add(jn);
                        break;
                    }
                    default:
                        continue;
                }
                
                foreach (var pl in node.PortLinks)
                {
                    if (!pl.IsAttached) continue;

                    CapnpFbpIipPortModel outIipPort = null;
                    CapnpFbpPortModel outCapnpPort = null;
                    CapnpFbpPortModel inCapnpPort = null;
                    switch (pl.Target.Model)
                    {
                        case CapnpFbpIipPortModel targetIipPort
                            when pl.Source.Model is CapnpFbpPortModel sourceCapnpPort:
                            outIipPort = targetIipPort;
                            inCapnpPort = sourceCapnpPort;
                            break;
                        case CapnpFbpPortModel targetCapnpPort
                            when pl.Source.Model is CapnpFbpIipPortModel sourceIipPort:
                        {
                            outIipPort = sourceIipPort;
                            inCapnpPort = targetCapnpPort;
                            break;
                        }
                        case CapnpFbpPortModel targetCapnpPort
                            when pl.Source.Model is CapnpFbpPortModel sourceCapnpPort:
                            outCapnpPort = sourceCapnpPort;
                            inCapnpPort = targetCapnpPort;
                            break;
                    }

                    if (outIipPort != null && inCapnpPort != null)
                    {
                        // make sure the link is only stored once
                        var checkOut = $"{outIipPort.Parent.Id}.{outIipPort.Alignment.ToString()}";
                        var checkIn = $"{inCapnpPort.Parent.Id}.{inCapnpPort.Name}";
                        if (linkSet.Contains($"{checkOut}->{checkIn}") ||
                            linkSet.Contains($"{checkIn}->{checkOut}")) continue;
                        linkSet.Add($"{checkOut}->{checkIn}");

                        var jl = new JObject()
                        {
                            {
                                "source", new JObject()
                                {
                                    { "node_id", outIipPort.Parent.Id },
                                    { "port", outIipPort.Alignment.ToString() }
                                }
                            },
                            {
                                "target", new JObject()
                                {
                                    { "node_id", inCapnpPort.Parent.Id },
                                    { "port", inCapnpPort.Name }
                                }
                            }
                        };
                        if (dia["links"] is JArray links) links.Add(jl);
                    } 
                    else if (outCapnpPort != null && inCapnpPort != null)
                    {
                        // make sure the link is only stored once
                        var checkOut = $"{outCapnpPort.Parent.Id}.{outCapnpPort.Name}";
                        var checkIn = $"{inCapnpPort.Parent.Id}.{inCapnpPort.Name}";
                        if (linkSet.Contains($"{checkOut}->{checkIn}") ||
                            linkSet.Contains($"{checkIn}->{checkOut}")) continue;
                        linkSet.Add($"{checkOut}->{checkIn}");

                        var jl = new JObject()
                        {
                            {
                                "source", new JObject()
                                {
                                    { "node_id", outCapnpPort.Parent.Id },
                                    { "port", outCapnpPort.Name }
                                }
                            },
                            {
                                "target", new JObject()
                                {
                                    { "node_id", inCapnpPort.Parent.Id },
                                    { "port", inCapnpPort.Name }
                                }
                            }
                        };
                        if (dia["links"] is JArray links) links.Add(jl);
                    }
                }
            }
            
            //File.WriteAllText("Data/diagram_new.json", dia.ToString());
            await JsRuntime.InvokeVoidAsync("saveAsBase64", "diagram.json", 
                Convert.ToBase64String(Encoding.UTF8.GetBytes(dia.ToString())));
        }
        
        protected async Task CreatePythonFlow()
        {
            // var dia = JObject.Parse(File.ReadAllText("Data/diagram_template.json"));
            // HashSet<string> linkSet = new();
            // foreach(var node in Diagram.Nodes)
            // {
            //     if (node is not PythonFbpComponentModel fbpNode) continue;
            //
            //     var cmdParams = new JObject();
            //     foreach (var line in fbpNode.CmdParamString.Split('\n'))
            //     {
            //         var kv = line.Split('=');
            //         cmdParams.Add(kv[0].Trim(), kv.Length == 2 ? kv[1].Trim() : "");
            //     }
            //     var jn = new JObject()
            //     {
            //         { "node_id", fbpNode.Id },
            //         { "component_id", fbpNode.ComponentId },
            //         { "user_name", fbpNode.UserName },
            //         { "location", new JObject() { { "x", fbpNode.Position.X }, { "y", fbpNode.Position.Y } } },
            //         {
            //             "data", new JObject()
            //             {
            //                 { "path", fbpNode.PathToPythonFile },
            //                 { "cmd_params", cmdParams }
            //             }
            //         }
            //     };
            //     if (dia["nodes"] is JArray nodes) nodes.Add(jn);
            //
            //     foreach (var pl in node.PortLinks)
            //     {
            //         if (!pl.IsAttached) continue;
            //
            //         if (pl.Source.Model is not CapnpFbpPortModel sourceCapnpPort ||
            //             pl.Target.Model is not CapnpFbpPortModel targetCapnpPort) continue;
            //
            //         var checkS = $"{sourceCapnpPort.Parent.Id}.{sourceCapnpPort.Name}";
            //         var checkT = $"{targetCapnpPort.Parent.Id}.{targetCapnpPort.Name}";
            //         if (linkSet.Contains($"{checkS}->{checkT}") ||
            //             linkSet.Contains($"{checkT}->{checkS}")) continue;
            //         else linkSet.Add($"{checkS}->{checkT}");
            //
            //         var jl = new JObject()
            //         {
            //             { "source", new JObject()
            //                 {
            //                     { "node_id", sourceCapnpPort.Parent.Id }, 
            //                     { "port", sourceCapnpPort.Name }
            //                 } 
            //             },
            //             { "target", new JObject()
            //                 {
            //                     { "node_id", targetCapnpPort.Parent.Id }, 
            //                     { "port", targetCapnpPort.Name }
            //                 } 
            //             }
            //         };
            //         if (dia["links"] is JArray links) links.Add(jl);
            //     }
            // }
            //
            // //File.WriteAllText("Data/diagram_new.json", dia.ToString());
            // await JsRuntime.InvokeVoidAsync("saveAsBase64", "diagram.json", 
            //     Convert.ToBase64String(Encoding.UTF8.GetBytes(dia.ToString())));
        }
        
        private JObject _draggedComponent;
        
        private void OnNodeDragStart(JObject component)//string nodeType, string nodeName)
        {
            _draggedComponent = component;
        }

        private void OnNodeDrop(DragEventArgs e)
        {
            if (_draggedComponent == null) return;
            var position = Diagram.GetRelativeMousePoint(e.ClientX, e.ClientY);
            AddFbpNode(position, _draggedComponent);
            _draggedComponent = null;
        }
        
        private NodeModel AddFbpNode(Point position, JObject component, JObject initNode = null)
        {
            var initData = initNode == null ? new JObject() : initNode["data"] as JObject ?? new JObject();
            
            switch (component["type"]?.ToString())
            {
                case null:
                    return null;
                case "CapnpFbpComponent":
                {
                    var cmdParams = new StringBuilder();
                    var initCmdParams = initData["cmd_params"] as JObject ?? new JObject();
                    var compCmdParams = component["cmd_params"] as JObject ?? new JObject();
                    var cmdParamNames = ((IDictionary<string, JToken>)compCmdParams).Keys.ToHashSet();
                        cmdParamNames.UnionWith(((IDictionary<string, JToken>)initCmdParams).Keys.ToHashSet());
                    foreach(var paramName in cmdParamNames)
                    {
                        cmdParams.Append(paramName);
                        cmdParams.Append('=');
                        //if(initCmdParams[paramName]) 
                        cmdParams.Append(initCmdParams.GetValue(paramName) ?? compCmdParams["default"] ?? "");
                        cmdParams.AppendLine();
                    }

                    var pathToFile = initData["path"]?.ToString() ?? component["path"]?.ToString() ?? "";
                    var compId = component["id"]?.Type switch
                    {
                        JTokenType.Null => null,
                        _ => component["id"]?.ToString()
                    };
                    var procName = initNode?.GetValue("process_name")?.Type switch
                    {
                        JTokenType.Null => null,
                        _ => initNode?.GetValue("process_name")?.ToString()
                    };
                    var node = new CapnpFbpComponentModel(new Point(position.X, position.Y))
                    {
                        ComponentId = compId,
                        ProcessName = procName ?? $"{compId ?? "new"} {CapnpFbpComponentModel.ProcessNo++}",
                        PathToInterpreter = component["interpreter"]?.ToString(), 
                        PathToFile = pathToFile,
                        ShortDescription = component["description"]?.ToString() ?? "",
                        CmdParamString = cmdParams.ToString(),
                        Editable = initNode?.GetValue("editable")?.Value<bool>() ?? pathToFile.Length == 0,
                        InParallelCount = initNode?.GetValue("parallel_processes")?.Value<int>() ?? 1
                    };

                    Diagram.Controls.AddFor(node).Add(new AddPortControl(0.2, 0, -33, -50)
                    {
                        Label = "IN",
                        PortType = CapnpFbpPortModel.PortType.In,
                        NodeModel = node,
                    });
                    Diagram.Controls.AddFor(node).Add(new AddPortControl(0.8, 0, -41, -50)
                    {
                        Label = "OUT",
                        PortType = CapnpFbpPortModel.PortType.Out,
                        NodeModel = node,
                    });
                    Diagram.Controls.AddFor(node).Add(new RemoveProcessControl(0.5, 0, -20, -50));
                    Diagram.Controls.AddFor(node).Add(new ToggleEditNodeControl(1.1, 0, -20, -50)
                    {
                        NodeModel = node
                    });
                    
                    foreach(var (i, input) in (component["inputs"] ?? new JArray()).
                            Select((inp, i) => (i, inp)))
                    {
                        AddPortControl.CreateAndAddPort(node, CapnpFbpPortModel.PortType.In, i, 
                            input["name"]?.ToString());
                    }
                    
                    foreach(var (i, output) in (component["outputs"] ?? new JArray()).
                            Select((outp, i) => (i, outp)))
                    {
                        AddPortControl.CreateAndAddPort(node, CapnpFbpPortModel.PortType.Out, i, 
                            output["name"]?.ToString());
                    }
                    Diagram.Nodes.Add(node);
                    return node;
                }
                case "CapnpFbpIIP":
                {
                    var compId = component["id"]?.Type switch
                    {
                        JTokenType.Null => null,
                        _ => component["id"]?.ToString()
                    };
                    var node = new CapnpFbpIipModel(new Point(position.X, position.Y))
                    {
                        ComponentId = compId,
                        Content = initData["content"]?.ToString() ?? ""
                    };
                    Diagram.Nodes.Add(node);
                    Diagram.Controls.AddFor(node).Add(new RemoveProcessControl(0.5, 0, -20, -50));
                    node.AddPort(new CapnpFbpIipPortModel(node, PortAlignment.Top));
                    node.AddPort(new CapnpFbpIipPortModel(node, PortAlignment.Bottom));
                    node.AddPort(new CapnpFbpIipPortModel(node, PortAlignment.Left));
                    node.AddPort(new CapnpFbpIipPortModel(node, PortAlignment.Right));
                    node.RefreshAll();
                    return node;
                }
            }

            return null;
        }
    }
}