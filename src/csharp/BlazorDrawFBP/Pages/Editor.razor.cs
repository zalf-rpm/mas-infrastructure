using System;
using System.Linq;
using Blazor.Diagrams;
using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.PathGenerators;
using Blazor.Diagrams.Core.Routers;
using Blazor.Diagrams.Options;

namespace BlazorDrawFBP.Pages
{
    public partial class Editor
    {
        private static readonly Random Random = new();
        private BlazorDiagram Diagram { get; set; } = null!;

        protected override void OnInitialized()
        {
            var options = new BlazorDiagramOptions
            {
                AllowMultiSelection = true,
                Zoom = { Enabled = false,},
                Links =
                {
                    DefaultRouter = new NormalRouter(),
                    DefaultPathGenerator = new SmoothPathGenerator()
                },
                Groups = { Enabled = true }
            };

            Diagram = new BlazorDiagram(options);

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
        
        
        protected void AddNode()
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

        protected void AddPort()
        {
            var node = Diagram.Nodes.FirstOrDefault(n => n.Selected);
            if (node == null) return;

            foreach(PortAlignment portAlignment in Enum.GetValues(typeof(PortAlignment)))
            {
                if (node.GetPort(portAlignment) != null) continue;
                node.AddPort(portAlignment);
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
    }
}