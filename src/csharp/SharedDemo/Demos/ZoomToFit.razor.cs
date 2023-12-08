﻿using Blazor.Diagrams;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core;
using Microsoft.AspNetCore.Components;
using Blazor.Diagrams.Core.Geometry;

namespace SharedDemo.Demos
{
    public class ZoomToFitComponent : ComponentBase
    {
        protected readonly BlazorDiagram diagram = new();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c += 2)
                {
                    var node1 = new NodeModel(new Point(350 + c * 80 + c * 120, 150 + r * 120));
                    var node2 = new NodeModel(new Point(350 + (c + 1) * 200, 150 + r * 120));

                    var sourcePort = node1.AddPort(PortAlignment.Right);
                    var targetPort = node2.AddPort(PortAlignment.Left);

                    diagram.Nodes.Add(new[] { node1, node2 });
                    diagram.Links.Add(new LinkModel(sourcePort, targetPort));
                }
            }
        }
    }
}
