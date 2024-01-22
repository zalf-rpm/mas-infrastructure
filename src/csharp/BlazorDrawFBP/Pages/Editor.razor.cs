using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blazor.Diagrams;
using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Controls;
using Blazor.Diagrams.Core.Controls.Default;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.PathGenerators;
using Blazor.Diagrams.Core.Routers;
using Blazor.Diagrams.Options;
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

            _components = JObject.Parse(File.ReadAllText("Data/components.json"));
            foreach (var (cat, value) in _components)
            {
                if (value is not JArray components) continue;
                foreach (var component in components)
                {
                    if (component is JObject comp) _componentDict.Add(comp["id"]?.ToString() ?? "", comp);
                }
            }

            Diagram.RegisterComponent<CapnpFbpComponentModel, PythonFbpComponentWidget>();
            Diagram.RegisterComponent<UpdatePortNameNode, UpdatePortNameNodeWidget>();
            Diagram.RegisterComponent<PortOptionsNode, PortOptionsNodeWidget>();
            Diagram.RegisterComponent<NodeInformationControl, NodeInformationControlWidget>();
            Diagram.RegisterComponent<LinkInformationControl, LinkInformationControlWidget>();
            Diagram.RegisterComponent<AddPortControl, AddPortControlWidget>();
            Diagram.RegisterComponent<RemoveProcessControl, RemoveProcessControlWidget>();

            RegisterEvents();

            // var node = Diagram.Nodes.Add(new AddTwoNumbersNode(new Point(80, 80)));
            // node.AddPort(PortAlignment.Top);
            // node.AddPort(PortAlignment.Bottom);

            // var firstNode = Diagram.Nodes.Add(new NodeModel(position: new Point(50, 50))
            // {
            //     Title = "Node 1"
            // });
            // var secondNode = Diagram.Nodes.Add(new NodeModel(position: new Point(200, 100))
            // {
            //     Title = "Node 2"
            // });
            // var leftPort = secondNode.AddPort(PortAlignment.Left);
            // var rightPort = secondNode.AddPort(PortAlignment.Right);
            //
            // // The connection point will be the intersection of
            // // a line going from the target to the center of the source
            // var sourceAnchor = new ShapeIntersectionAnchor(firstNode);
            // // The connection point will be the port's position
            // var targetAnchor = new SinglePortAnchor(leftPort);
            // var link = Diagram.Links.Add(new LinkModel(sourceAnchor, targetAnchor));
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
                Diagram.Controls.AddFor(l, ControlsType.OnHover).Add(new LinkInformationControl());
                if (l.Source.Model is CapnpFbpPortModel pm)
                {
                    switch (pm.ThePortType)
                    {
                        case CapnpFbpPortModel.PortType.In:
                            l.Labels.Add(new LinkLabelModel(l, pm.Name, 40));
                            l.Labels.Add(new LinkLabelModel(l, "OUT", -40));
                            l.SourceMarker = LinkMarker.Arrow;
                            l.TargetChanged += (link, oldTarget, newTarget) =>
                            {
                                if (newTarget.Model is not CapnpFbpPortModel outPort) return;
                                link.Labels[1].Content = outPort.Name;
                                link.Refresh();
                            };
                            break;
                        case CapnpFbpPortModel.PortType.Out:
                            l.Labels.Add(new LinkLabelModel(l, pm.Name, 40));
                            l.Labels.Add(new LinkLabelModel(l, "IN", -40));
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
            //var dia = JObject.Parse(File.ReadAllText("Data/diagram_new.json"));
            var oldNodeIdToNewNode = new Dictionary<string, NodeModel>();
            foreach (var node in dia["nodes"] ?? new JArray())
            {
                if (node is not JObject obj) continue;

                var position = new Point(obj["location"]?["x"]?.Value<double>() ?? 0, 
                    obj["location"]?["y"]?.Value<double>() ?? 0);
                
                var component = obj["id"]?.Type switch
                {
                    JTokenType.Null => obj["component_data"] as JObject,
                    _ => _componentDict[obj["id"]?.ToString() ?? ""]
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
                if (sourcePort == null || targetPort == null) continue;
                Diagram.Links.Add(new LinkModel(sourcePort, targetPort));
            }
        }
        
        protected async Task SaveDiagram()
        {
            var dia = JObject.Parse(File.ReadAllText("Data/diagram_template.json"));
            HashSet<string> linkSet = new();
            foreach(var node in Diagram.Nodes)
            {
                if (node is not CapnpFbpComponentModel fbpNode) continue;

                var cmdParams = new JObject();
                foreach (var line in fbpNode.CmdParamString.Split('\n'))
                {
                    var kv = line.Split('=');
                    var k = kv[0].Trim();
                    var v = kv.Length == 2 ? kv[1].Trim() : "";
                    if(k.Length > 0 && v.Length > 0) cmdParams.Add(k, v);
                }
                var jn = new JObject()
                {
                    { "node_id", fbpNode.Id },
                    { "component_id", fbpNode.ComponentId },
                    { "process_name", fbpNode.ProcessName },
                    { "location", new JObject() { { "x", fbpNode.Position.X }, { "y", fbpNode.Position.Y } } },
                    {
                        "data", new JObject()
                        {
                            { "cmd_params", cmdParams }
                        }
                    }
                };
                if (fbpNode.ComponentId == null)
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
                    
                    jn.Add("component_data", new JObject()
                    {
                        { "type", "CapnpFbpComponent" },
                        { "interpreter", fbpNode.PathToInterpreter },
                        { "description", fbpNode.ShortDescription },
                        { "inputs", new JArray(inputs) },
                        { "outputs", new JArray(outputs) },
                        { "path", fbpNode.PathToFile }
                    });
                }
                if (dia["nodes"] is JArray nodes) nodes.Add(jn);

                foreach (var pl in node.PortLinks)
                {
                    if (!pl.IsAttached) continue;

                    if (pl.Source.Model is not CapnpFbpPortModel sourceCapnpPort ||
                        pl.Target.Model is not CapnpFbpPortModel targetCapnpPort) continue;

                    // make sure the source port is the out port
                    // because BlazorDiagrams doesn't care about the direction of the link
                    var outPort = sourceCapnpPort.ThePortType == CapnpFbpPortModel.PortType.Out
                        ? sourceCapnpPort
                        : targetCapnpPort;
                    var inPort = targetCapnpPort.ThePortType == CapnpFbpPortModel.PortType.In
                        ? targetCapnpPort
                        : sourceCapnpPort;
                    
                    // make sure the link is only stored once
                    var checkOut = $"{outPort.Parent.Id}.{outPort.Name}";
                    var checkIn = $"{inPort.Parent.Id}.{inPort.Name}";
                    if (linkSet.Contains($"{checkOut}->{checkIn}") ||
                        linkSet.Contains($"{checkIn}->{checkOut}")) continue;
                    else linkSet.Add($"{checkOut}->{checkIn}");

                    // for storing the links use the naming convention of BlazorDiagrams (source and target)
                    // but the direction of the link is determined by the port type and FBP convention
                    var jl = new JObject()
                    {
                        { "source", new JObject()
                            {
                                { "node_id", sourceCapnpPort.Parent.Id }, 
                                { "port", sourceCapnpPort.Name }
                            } 
                        },
                        { "target", new JObject()
                            {
                                { "node_id", targetCapnpPort.Parent.Id }, 
                                { "port", targetCapnpPort.Name }
                            } 
                        }
                    };
                    if (dia["links"] is JArray links) links.Add(jl);
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
                    foreach(var param in component["cmd_params"] ?? new JArray())
                    {
                        var paramName = param["name"]?.ToString();
                        if (paramName == null) continue;
                        cmdParams.Append(paramName);
                        cmdParams.Append('=');
                        //if(initCmdParams[paramName]) 
                        cmdParams.Append(initCmdParams.GetValue(paramName) ?? param["default"] ?? "");
                        cmdParams.AppendLine();
                    }

                    var pathToFile = initData["path"]?.ToString() ?? component["path"]?.ToString() ?? "";
                    var compId = component["id"]?.Type switch
                    {
                        JTokenType.Null => null,
                        _ => component["id"]?.ToString()
                    };
                    var procName = initNode != null ? initNode["process_name"]?.Type switch
                    {
                        JTokenType.Null => null,
                        _ => initNode["process_name"]?.ToString()
                    } : null;
                    var node = new CapnpFbpComponentModel(new Point(position.X, position.Y))
                    {
                        ComponentId = compId,
                        ProcessName = procName ?? $"{compId ?? "new"} {CapnpFbpComponentModel.ProcessNo++}",
                        PathToInterpreter = component["interpreter"]?.ToString(), 
                        PathToFile = pathToFile,
                        ShortDescription = component["description"]?.ToString() ?? "",
                        CmdParamString = cmdParams.ToString(),
                        Editable = pathToFile.Length == 0
                    };

                    Diagram.Controls.AddFor(node).Add(new AddPortControl(0.2, -0.2, -33)
                    {
                        Label = "IN",
                        PortType = CapnpFbpPortModel.PortType.In,
                        NodeModel = node,
                    });
                    Diagram.Controls.AddFor(node).Add(new AddPortControl(0.8, -0.2, -41)
                    {
                        Label = "OUT",
                        PortType = CapnpFbpPortModel.PortType.Out,
                        NodeModel = node,
                    });
                    Diagram.Controls.AddFor(node).Add(new RemoveProcessControl(0.5, -0.2, -20));
                    
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
            }

            return null;
        }
    }
}