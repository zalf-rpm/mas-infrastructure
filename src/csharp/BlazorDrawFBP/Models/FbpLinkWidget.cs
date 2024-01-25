using Blazor.Diagrams;
using Blazor.Diagrams.Components.Renderers;
using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Behaviors;
using Blazor.Diagrams.Core.Extensions;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using SvgPathProperties;
using System;
using System.Runtime.CompilerServices;
using BlazorDrawFBP.Behaviors;

namespace BlazorDrawFBP.Models;

public class FbpLinkWidget : ComponentBase
{
  private bool _hovered;

  [CascadingParameter]
  public BlazorDiagram BlazorDiagram { get; set; }

  [Parameter]
  public LinkModel Link { get; set; }

  private RenderFragment GetSelectionHelperPath(string color, string d, int index)
  {
    return (RenderFragment) (builder =>
    {
      builder.OpenElement(0, "path");
      builder.AddAttribute(1, "class", "selection-helper");
      builder.AddAttribute(2, "stroke", color);
      builder.AddAttribute(3, "stroke-width", (object) 12);
      builder.AddAttribute(4, nameof (d), d);
      builder.AddAttribute(5, "stroke-linecap", "butt");
      builder.AddAttribute(6, "stroke-opacity", _hovered ? "0.05" : "0");
      builder.AddAttribute(7, "fill", "none");
      builder.AddAttribute(8, "onmouseenter", EventCallback.Factory.Create<MouseEventArgs>((object) this, new Action<MouseEventArgs>(OnMouseEnter)));
      builder.AddAttribute(9, "onmouseleave", EventCallback.Factory.Create<MouseEventArgs>((object) this, new Action<MouseEventArgs>(OnMouseLeave)));
      builder.AddAttribute(10, "onpointerdown", EventCallback.Factory.Create<PointerEventArgs>((object) this, (Action<PointerEventArgs>) (e => OnPointerDown(e, index))));
      builder.AddEventStopPropagationAttribute(11, "onpointerdown", Link.Segmentable);
      builder.CloseElement();
    });
  }

  private void OnPointerDown(PointerEventArgs e, int index)
  {
    if (!Link.Segmentable)
      return;
    BlazorDiagram.TriggerPointerDown(CreateVertex(e.ClientX, e.ClientY, index), EventsExtensions.ToCore(e));
  }

  private void OnMouseEnter(MouseEventArgs e) => _hovered = true;

  private void OnMouseLeave(MouseEventArgs e) => _hovered = false;

  private LinkVertexModel CreateVertex(double clientX, double clientY, int index)
  {
    var vertex = new LinkVertexModel(Link, BlazorDiagram.GetRelativeMousePoint(clientX, clientY));
    Link.Vertices.Insert(index, vertex);
    Link.Refresh();
    return vertex;
  }

  protected override void BuildRenderTree(RenderTreeBuilder builder)
  {
    var color = Link.Selected ? Link.SelectedColor 
                                ?? BlazorDiagram.Options.Links.DefaultSelectedColor : Link.Color
      ?? BlazorDiagram.Options.Links.DefaultColor;
    var pathGeneratorResult = Link.PathGeneratorResult;
    if (pathGeneratorResult == null) return;
    var behavior = BlazorDiagram.GetBehavior<FbpDragNewLinkBehavior>();
    if (behavior == null) return;
    var d1 = pathGeneratorResult.FullPath.ToString();
    builder.OpenElement(0, "path");
    builder.AddAttribute(1, "d", d1);
    builder.AddAttribute(2, "stroke-width", Link.Width.ToInvariantString());
    builder.AddAttribute(3, "fill", "none");
    builder.AddAttribute(4, "stroke", color);
    builder.CloseElement();
    if (behavior.OngoingLink == null || behavior.OngoingLink != Link)
    {
      if (Link.Vertices.Count == 0)
      {
        builder.AddContent(5, GetSelectionHelperPath(color, d1, 0));
      }
      else
      {
        for (var index1 = 0; index1 < pathGeneratorResult.Paths.Length; ++index1)
        {
          var d2 = pathGeneratorResult.Paths[index1].ToString();
          var index2 = index1;
          builder.AddContent(6, GetSelectionHelperPath(color, d2, index2));
        }
      }
    }
    double? nullable;
    if (Link.SourceMarker != null)
    {
      nullable = pathGeneratorResult.SourceMarkerAngle;
      if (nullable.HasValue && pathGeneratorResult.SourceMarkerPosition != (Point) null)
      {
        builder.OpenElement(7, "g");
        builder.AddAttribute(8, "transform", FormattableString.Invariant(FormattableStringFactory.Create("translate({0}, {1}) rotate({2})", (object) pathGeneratorResult.SourceMarkerPosition.X, (object) pathGeneratorResult.SourceMarkerPosition.Y, (object) pathGeneratorResult.SourceMarkerAngle)));
        builder.OpenElement(9, "path");
        builder.AddAttribute(10, "d", Link.SourceMarker.Path);
        builder.AddAttribute(11, "fill", color);
        builder.CloseElement();
        builder.CloseElement();
      }
    }
    if (Link.TargetMarker != null)
    {
      nullable = pathGeneratorResult.TargetMarkerAngle;
      if (nullable.HasValue && pathGeneratorResult.TargetMarkerPosition != (Point) null)
      {
        builder.OpenElement(12, "g");
        builder.AddAttribute(13, "transform", FormattableString.Invariant(FormattableStringFactory.Create("translate({0}, {1}) rotate({2})", (object) pathGeneratorResult.TargetMarkerPosition.X, (object) pathGeneratorResult.TargetMarkerPosition.Y, (object) pathGeneratorResult.TargetMarkerAngle)));
        builder.OpenElement(14, "path");
        builder.AddAttribute(15, "d", Link.TargetMarker.Path);
        builder.AddAttribute(16, "fill", color);
        builder.CloseElement();
        builder.CloseElement();
      }
    }
    if (Link.Vertices.Count > 0)
    {
      var str1 = Link.SelectedColor ?? BlazorDiagram.Options.Links.DefaultSelectedColor;
      var str2 = Link.Color ?? BlazorDiagram.Options.Links.DefaultColor;
      foreach (var vertex in Link.Vertices)
      {
        builder.OpenComponent<LinkVertexRenderer>(17);
        builder.AddAttribute(18, "Vertex", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck(vertex));
        builder.AddAttribute(19, "Color", (object) Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck(str2));
        builder.AddAttribute(20, "SelectedColor", (object) Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck(str1));
        builder.SetKey((object) vertex.Id);
        builder.CloseComponent();
      }
    }
    foreach (var label in Link.Labels)
    {
      builder.OpenComponent<LinkLabelRenderer>(21);
      builder.AddAttribute(22, "Label", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck(label));
      builder.AddAttribute(23, "Path", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck(pathGeneratorResult.FullPath));
      builder.SetKey((object) label.Id);
      builder.CloseComponent();
    }
  }
}
