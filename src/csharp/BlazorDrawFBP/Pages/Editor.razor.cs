using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Blazor.Diagrams;
using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.PathGenerators;
using Blazor.Diagrams.Core.Routers;
using Blazor.Diagrams.Options;
using BlazorDrawFBP.Models;
using Microsoft.AspNetCore.Components.Web;
using Newtonsoft.Json.Linq;
using SharedDemo.Demos;

namespace BlazorDrawFBP.Pages
{
    public partial class Editor
    {
        private static readonly Random Random = new();
        private BlazorDiagram Diagram { get; set; } = null!;
        //private readonly List<string> events = new List<string>();

        private JObject _nodeConfigs = null!;
        
        protected override void OnInitialized()
        {
            var options = new BlazorDiagramOptions
            {
                AllowMultiSelection = true,
                Zoom = { Enabled = true, },
                Links =
                {
                    DefaultRouter = new NormalRouter(),
                    DefaultPathGenerator = new SmoothPathGenerator()
                },
                Groups = { Enabled = true }
            };

            Diagram = new BlazorDiagram(options);

            var nodeConfigs = File.ReadAllText("Data/node_configs.json");
            _nodeConfigs = JObject.Parse(nodeConfigs);
            
            Diagram.RegisterComponent<PythonFbpNode, PythonFbpNodeWidget>();
            Diagram.RegisterComponent<UpdatePortNameNode, UpdatePortNameNodeWidget>();

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

            // Diagram.PointerClick += (m, e) =>
            // {
            //     events.Add($"MouseClick, Type={m?.GetType().Name}, ModelId={m?.Id}");
            //     StateHasChanged();
            // };

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
                            Model = link.Labels[labelIndex],
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
            var node = new PythonFbpNode(new Point(x, y));
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

        protected void Download()
        {
            var node = Diagram.Nodes.FirstOrDefault(n => n.Selected);
            if (node == null) return;
            Diagram.Nodes.Remove(node);
        }
        
        protected void AddPort(CapnpFbpPortModel.PortType portType)
        {
            var node = Diagram.Nodes.FirstOrDefault(n => n.Selected);
            if (node == null) return;

            foreach (PortAlignment portAlignment in Enum.GetValues(typeof(PortAlignment)))
            {
                if (node.GetPort(portAlignment) != null) continue;
                var port = new CapnpFbpPortModel(node, portType, portAlignment);
                node.AddPort(port);
                node.Refresh();
                break;
            }
        }

        protected void RemovePort()
        {
            var node = Diagram.Nodes.FirstOrDefault(n => n.Selected);
            if (node == null) return;

            if (node.Ports.Count == 0) return;

            var i = Random.Next(0, node.Ports.Count);
            var port = node.Ports[i];

            Diagram.Links.Remove(port.Links.ToArray());
            node.RemovePort(port);
            node.Refresh();
        }

        protected void AddLink()
        {
            var selectedNodes = Diagram.Nodes.Where(n => n.Selected).ToArray();
            if (selectedNodes.Length != 2) return;

            var node1 = selectedNodes[0];
            var node2 = selectedNodes[1];

            if (node1 == null || node1.Ports.Count == 0 || node2 == null || node2.Ports.Count == 0)
                return;

            var sourcePort = node1.Ports[Random.Next(0, node1.Ports.Count)];
            var targetPort = node2.Ports[Random.Next(0, node2.Ports.Count)];
            Diagram.Links.Add(new LinkModel(sourcePort, targetPort));
        }
        
        private string _draggedNodeType;
        private string _draggedNodeName;
        
        private void OnNodeDragStart(string nodeType, string nodeName)
        {
            _draggedNodeType = nodeType;
            _draggedNodeName = nodeName;
        }

        private void OnNodeDrop(DragEventArgs e)
        {
            if (_draggedNodeType == null) // Unkown item
                return;

            var position = Diagram.GetRelativeMousePoint(e.ClientX, e.ClientY);
            switch (_draggedNodeType)
            {
                case "PythonFbpNode":
                {
                    _nodeConfigs.TryGetValue(_draggedNodeName, out var nodeConfig);
                    if (nodeConfig == null) return;

                    var cmdParams = new StringBuilder();
                    foreach(var param in nodeConfig["cmd_params"] ?? new JArray())
                    {
                        var paramName = param["name"];
                        if (paramName == null) continue;
                        cmdParams.Append(paramName);
                        cmdParams.Append('=');
                        if (param["default"] != null)
                        {
                            cmdParams.Append(param["default"]);
                        }
                        cmdParams.AppendLine();
                    }
                    var node = new PythonFbpNode(new Point(position.X, position.Y))
                    {
                        PathToPythonFile = nodeConfig["path"]?.ToString() ?? "",
                        ShortDescription = nodeConfig["description"]?.ToString() ?? "",
                        CmdParamString = cmdParams.ToString()
                    };

                    foreach(var (i, input) in (nodeConfig["inputs"] ?? new JArray()).
                            Select((inp, i) => (i, inp)))
                    {
                        var alignment = i switch
                        {
                            10 => PortAlignment.TopLeft,
                            > 10 => PortAlignment.Top,
                            _ => PortAlignment.Left
                        };
                        var port = new CapnpFbpPortModel(node, CapnpFbpPortModel.PortType.In, alignment)
                        {
                            Name = input["name"]?.ToString() ?? "IN",
                            OrderNo = i > 10 ? i-11 : i,
                        };
                        node.AddPort(port);
                    }

                    foreach(var (i, output) in (nodeConfig["outputs"] ?? new JArray()).
                            Select((outp, i) => (i, outp)))
                    {
                        var alignment = i switch
                        {
                            10 => PortAlignment.BottomRight,
                            > 10 => PortAlignment.Bottom,
                            _ => PortAlignment.Right
                        };
                        var port = new CapnpFbpPortModel(node, CapnpFbpPortModel.PortType.Out, alignment)
                        {
                            Name = output["name"]?.ToString() ?? "OUT",
                            OrderNo = i > 10 ? i-11 : i,
                        };
                        node.AddPort(port);
                    }
                    Diagram.Nodes.Add(node);
                    break;
                }
            }
            _draggedNodeType = null;
        }
        
    }
}