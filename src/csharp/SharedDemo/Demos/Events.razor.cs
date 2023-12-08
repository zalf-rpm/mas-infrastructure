﻿using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using Blazor.Diagrams;

namespace SharedDemo.Demos
{
    public class EventsComponent : ComponentBase
    {
        protected readonly BlazorDiagram diagram = new();
        protected readonly List<string> events = new List<string>();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            RegisterEvents();

            var node1 = NewNode(50, 50);
            var node2 = NewNode(300, 300);
            diagram.Nodes.Add(new[] { node1, node2, NewNode(300, 50) });
            diagram.Links.Add(new LinkModel(node1.GetPort(PortAlignment.Right), node2.GetPort(PortAlignment.Left)));
        }

        private void RegisterEvents()
        {
            diagram.Changed += () =>
            {
                events.Add("Changed");
                StateHasChanged();
            };

            diagram.Nodes.Added += (n) => events.Add($"NodesAdded, NodeId={n.Id}");
            diagram.Nodes.Removed += (n) => events.Add($"NodesRemoved, NodeId={n.Id}");

            diagram.SelectionChanged += (m) =>
            {
                events.Add($"SelectionChanged, Id={m.Id}, Type={m.GetType().Name}, Selected={m.Selected}");
                StateHasChanged();
            };

            diagram.Links.Added += (l) => events.Add($"Links.Added, LinkId={l.Id}");

            diagram.Links.Removed += (l) => events.Add($"Links.Removed, LinkId={l.Id}");

            diagram.PointerDown += (m, e) =>
            {
                events.Add($"MouseDown, Type={m?.GetType().Name}, ModelId={m?.Id}");
                StateHasChanged();
            };

            diagram.PointerUp += (m, e) =>
            {
                events.Add($"MouseUp, Type={m?.GetType().Name}, ModelId={m?.Id}");
                StateHasChanged();
            };

            diagram.PointerEnter += (m, e) =>
            {
                events.Add($"TouchStart, Type={m?.GetType().Name}, ModelId={m?.Id}");
                StateHasChanged();
            };

            diagram.PointerLeave += (m, e) =>
            {
                events.Add($"TouchEnd, Type={m?.GetType().Name}, ModelId={m?.Id}");
                StateHasChanged();
            };

            diagram.PointerClick += (m, e) =>
            {
                events.Add($"MouseClick, Type={m?.GetType().Name}, ModelId={m?.Id}");
                StateHasChanged();
            };

            diagram.PointerDoubleClick += (m, e) =>
            {
                events.Add($"MouseDoubleClick, Type={m?.GetType().Name}, ModelId={m?.Id}");
                StateHasChanged();
            };
        }

        private NodeModel NewNode(double x, double y)
        {
            var node = new NodeModel(new Point(x, y));
            node.AddPort(PortAlignment.Bottom);
            node.AddPort(PortAlignment.Top);
            node.AddPort(PortAlignment.Left);
            node.AddPort(PortAlignment.Right);
            return node;
        }
    }
}
