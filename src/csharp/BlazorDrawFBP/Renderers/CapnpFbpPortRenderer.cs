using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazor.Diagrams;
using Blazor.Diagrams.Components.Renderers;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Extensions;
using Blazor.Diagrams.Models;
using BlazorDrawFBP.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace BlazorDrawFBP.Renderers;

public class CapnpFbpPortRenderer : ComponentBase, IDisposable
{
    private ElementReference _element;
    private bool _isParentSvg;
    private bool _shouldRefreshPort;
    private bool _shouldRender = true;
    private bool _updatingDimensions;

    [CascadingParameter] public BlazorDiagram BlazorDiagram { get; set; }

    [Inject] private IJSRuntime JSRuntime { get; set; }

    [Parameter] public CapnpFbpPortModel Port { get; set; }

    [Parameter] public string Class { get; set; }

    [Parameter] public string Style { get; set; }

    [Parameter] public RenderFragment ChildContent { get; set; }

    public void Dispose()
    {
        this.Port.Changed -= new Action<Model>(this.OnPortChanged);
        this.Port.VisibilityChanged -= new Action<Model>(this.OnPortChanged);
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        this.Port.Changed += new Action<Model>(this.OnPortChanged);
        this.Port.VisibilityChanged += new Action<Model>(this.OnPortChanged);
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        this._isParentSvg = this.Port.Parent is SvgNodeModel;
    }

    protected override bool ShouldRender()
    {
        if (!this._shouldRender)
            return false;
        this._shouldRender = false;
        return true;
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (!this.Port.Visible)
            return;
        
        var offsetString = "";
        if (Port.Offset != 0)
        {
            offsetString = "offset-" + (Port.Offset < 0 ? "m" : "") + (int)Math.Round(Math.Abs(Port.Offset) / 10.0) * 10;
        }
        
        builder.OpenElement(0, _isParentSvg ? "g" : "div");
        builder.AddAttribute(1, "style", Style);
        builder.AddAttribute(2, "class",
            "diagram-port " + Port.Alignment.ToString().ToLower() + " " + 
            Port.ThePortType.ToString().ToLower() + " " +
            offsetString + " " + (Port.Links.Count > 0 ? "has-links" : "") + " " + Class);
        builder.AddAttribute(3, "data-port-id", Port.Id);
        builder.AddAttribute<PointerEventArgs>(4, "onpointerdown",
            EventCallback.Factory.Create<PointerEventArgs>((object)this,
                new Action<PointerEventArgs>(OnPointerDown)));
        builder.AddEventStopPropagationAttribute(5, "onpointerdown", true);
        builder.AddAttribute<PointerEventArgs>(6, "onpointerup",
            EventCallback.Factory.Create<PointerEventArgs>((object)this,
                new Action<PointerEventArgs>(OnPointerUp)));
        builder.AddEventStopPropagationAttribute(7, "onpointerup", true);
        builder.AddElementReferenceCapture(8, (Action<ElementReference>)(value => _element = value));
        builder.AddContent(9, ChildContent);
        builder.CloseElement();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (Port.Initialized)
            return;
        await UpdateDimensions();
    }

    private void OnPointerDown(PointerEventArgs e)
    {
        BlazorDiagram.TriggerPointerDown((Model)Port, EventsExtensions.ToCore(e));
    }

    private void OnPointerUp(PointerEventArgs e)
    {
        BlazorDiagram.TriggerPointerUp(
            e.PointerType == "mouse" ? (Model)Port : (Model)FindPortOn(e.ClientX, e.ClientY),
            EventsExtensions.ToCore(e));
    }

    private PortModel FindPortOn(double clientX, double clientY)
    {
        foreach (var portOn in BlazorDiagram.Nodes
                     .SelectMany<NodeModel,
                         PortModel>((Func<NodeModel, IEnumerable<PortModel>>)(n => (IEnumerable<PortModel>)n.Ports))
                     .Union<PortModel>(BlazorDiagram.Groups.SelectMany<GroupModel, PortModel>(
                         (Func<GroupModel, IEnumerable<PortModel>>)(g => (IEnumerable<PortModel>)g.Ports))))
        {
            if (!portOn.Initialized) continue;
            var relativeMousePoint = BlazorDiagram.GetRelativeMousePoint(clientX, clientY);
            if (portOn.GetBounds().ContainsPoint(relativeMousePoint))
                return portOn;
        }

        return (PortModel)null;
    }

    private async Task UpdateDimensions()
    {
        Point pan;
        if (BlazorDiagram.Container == null)
        {
            pan = (Point)null;
        }
        else
        {
            _updatingDimensions = true;
            var zoom = BlazorDiagram.Zoom;
            pan = BlazorDiagram.Pan;
            var boundingClientRect = await JSRuntime.GetBoundingClientRect(_element);
            Port.Size = new Size(boundingClientRect.Width / zoom, boundingClientRect.Height / zoom);
            Port.Position = new Point((boundingClientRect.Left - BlazorDiagram.Container.Left - pan.X) / zoom,
                (boundingClientRect.Top - BlazorDiagram.Container.Top - pan.Y) / zoom);
            Port.Initialized = true;
            _updatingDimensions = false;
            if (_shouldRefreshPort)
            {
                _shouldRefreshPort = false;
                Port.RefreshAll();
                pan = (Point)null;
            }
            else
            {
                Port.RefreshLinks();
                pan = (Point)null;
            }
        }
    }

    private async void OnPortChanged(Model _)
    {
        var portRenderer = this;
        if (portRenderer._updatingDimensions)
            portRenderer._shouldRefreshPort = true;
        if (portRenderer.Port.Initialized)
        {
            portRenderer._shouldRender = true;
            await portRenderer.InvokeAsync(new Action(portRenderer.StateHasChanged));
        }
        else
            await portRenderer.UpdateDimensions();
    }
}