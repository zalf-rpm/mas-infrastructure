using Blazor.Diagrams.Core.Extensions;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using SvgPathProperties;
using System;
using Blazor.Diagrams;
using Blazor.Diagrams.Components;

namespace BlazorDrawFBP.Renderers;

public class CapnpLinkLabelRenderer : ComponentBase, IDisposable
{
  [CascadingParameter]
  public BlazorDiagram BlazorDiagram { get; set; }

  [Parameter]
  public LinkLabelModel Label { get; set; }

  [Parameter]
  public SvgPath Path { get; set; }

  public void Dispose()
  {
    Label.Changed -= OnLabelChanged;
    Label.VisibilityChanged -= OnLabelChanged;
  }

  protected override void OnInitialized()
  {
    Label.Changed += OnLabelChanged;
    Label.VisibilityChanged += OnLabelChanged;
  }

  protected override void BuildRenderTree(RenderTreeBuilder builder)
  {
    if (!Label.Visible)
      return;
    var position = FindPosition();
    if (position == null)
      return;
    var type = BlazorDiagram.GetComponent(Label);
    if ((object) type == null)
      type = typeof (DefaultLinkLabelWidget);
    var componentType = type;
    builder.OpenElement(0, "foreignObject");
    builder.AddAttribute(1, "class", "diagram-link-label");
    var x = position.X;
    var offset1 = Label.Offset;
    var num1 = (object) offset1 != null ? offset1.X : 0.0;
    var invariantString1 = (x + num1).ToInvariantString();
    builder.AddAttribute(2, "x", invariantString1);
    var y = position.Y;
    var offset2 = Label.Offset;
    var num2 = (object) offset2 != null ? offset2.Y : 0.0;
    var invariantString2 = (y + num2).ToInvariantString();
    builder.AddAttribute(3, "y", invariantString2);
    builder.OpenComponent(4, componentType);
    builder.AddAttribute(5, "Label", (object) Label);
    builder.CloseComponent();
    builder.CloseElement();
  }

  private void OnLabelChanged(Model _)
  {
    InvokeAsync(StateHasChanged);
  }

  private Blazor.Diagrams.Core.Geometry.Point? FindPosition()
  {
    var length = Path.Length;
    var distance = Label.Distance;
    double fractionLength;
    if (distance.HasValue)
    {
      var valueOrDefault = distance.GetValueOrDefault();
      if (valueOrDefault <= 1.0)
      {
        fractionLength = valueOrDefault >= 0.0 ? Label.Distance.Value * length : length + Label.Distance.Value;
        goto label_6;
      }
      else if (valueOrDefault > 1.0)
      {
        fractionLength = Label.Distance.Value;
        goto label_6;
      }
    }

    fractionLength = length * (Label.Parent.Labels.IndexOf(Label) + 1) / (Label.Parent.Labels.Count + 1);
    label_6:
    var pointAtLength = Path.GetPointAtLength(fractionLength);
    return new Blazor.Diagrams.Core.Geometry.Point(pointAtLength.X, pointAtLength.Y);
  }
}
