using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Behaviors;
using BlazorDrawFBP.Models;

namespace BlazorDrawFBP.Behaviors;

using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;

public class FbpDragNewLinkBehavior : Behavior
{
  private PositionAnchor _targetPositionAnchor;

  public BaseLinkModel OngoingLink { get; private set; }

  public FbpDragNewLinkBehavior(Diagram diagram)
    : base(diagram)
  {
    Diagram.PointerDown += OnPointerDown;
    Diagram.PointerMove += OnPointerMove;
    Diagram.PointerUp += OnPointerUp;
  }

  public void StartFrom(ILinkable source, double clientX, double clientY)
  {
    if (OngoingLink != null) return;
    _targetPositionAnchor = new PositionAnchor(CalculateTargetPosition(clientX, clientY));
    OngoingLink = Diagram.Options.Links.Factory(Diagram, source, _targetPositionAnchor);
    if (OngoingLink == null) return;
    Diagram.Links.Add(OngoingLink);
  }

  public void StartFrom(BaseLinkModel link, double clientX, double clientY)
  {
    if (OngoingLink != null) return;
    _targetPositionAnchor = new PositionAnchor(CalculateTargetPosition(clientX, clientY));
    OngoingLink = link;
    OngoingLink.SetTarget(_targetPositionAnchor);
    OngoingLink.Refresh();
    OngoingLink.RefreshLinks();
  }

  private void OnPointerDown(Model model, MouseEventArgs e)
  {
    if (e.Button != 0L) return;
    OngoingLink = null;
    _targetPositionAnchor = null;
    if (model is not PortModel source || source.Locked) return;
    // FBP semantics: allow only one link from an output port
    // at the moment also don't allow multiple links from IIPs even if we could actually support it 
    // due to copy semantics of strings in the future
    switch (model)
    {
      case CapnpFbpPortModel { ThePortType: CapnpFbpPortModel.PortType.Out, Links.Count: > 0 }:
      case CapnpFbpIipPortModel iipPort when iipPort.Parent.Ports.Any(p => p is CapnpFbpIipPortModel { Links.Count: > 0 }):
        return;
    }
    _targetPositionAnchor = new PositionAnchor(CalculateTargetPosition(e.ClientX, e.ClientY));
    OngoingLink = Diagram.Options.Links.Factory(Diagram, source, _targetPositionAnchor);
    if (OngoingLink == null) return;
    OngoingLink.SetTarget(_targetPositionAnchor);
    Diagram.Links.Add(OngoingLink);
  }

  private void OnPointerMove(Model model, MouseEventArgs e)
  {
    if (OngoingLink == null || model != null) return;
    _targetPositionAnchor.SetPosition(CalculateTargetPosition(e.ClientX, e.ClientY));
    if (Diagram.Options.Links.EnableSnapping)
    {
      var nearPortToAttachTo = FindNearPortToAttachTo();
      if (nearPortToAttachTo != null || OngoingLink.Target is not PositionAnchor)
      {
        OngoingLink.SetTarget(nearPortToAttachTo == null
          ? _targetPositionAnchor
          : new SinglePortAnchor(nearPortToAttachTo));
      }
    }
    OngoingLink.Refresh();
    OngoingLink.RefreshLinks();
  }

  private void OnPointerUp(Model model, MouseEventArgs e)
  {
    if (OngoingLink == null) return;
    if (OngoingLink.IsAttached)
    {
      OngoingLink.TriggerTargetAttached();
      OngoingLink = null;
    }
    else
    {
      if (model is ILinkable linkable && 
          (OngoingLink.Source.Model == null || OngoingLink.Source.Model.CanAttachTo(linkable)))
      {
        OngoingLink.SetTarget(Diagram.Options.Links.TargetAnchorFactory(Diagram, OngoingLink, linkable));
        OngoingLink.TriggerTargetAttached();
        OngoingLink.Refresh();
        OngoingLink.RefreshLinks();
      }
      else switch (Diagram.Options.Links.RequireTarget)
      {
        case true:
          Diagram.Links.Remove(OngoingLink);
          break;
        case false:
          OngoingLink.Refresh();
          break;
      }
      OngoingLink = null;
    }
  }

  private Point CalculateTargetPosition(double clientX, double clientY)
  {
    var relativeMousePoint = Diagram.GetRelativeMousePoint(clientX, clientY);
    if (OngoingLink == null) return relativeMousePoint;
    var plainPosition = OngoingLink.Source.GetPlainPosition();
    if (plainPosition == null) return relativeMousePoint;
    var other = relativeMousePoint.Subtract(plainPosition).Normalize().Multiply(5.0);
    return relativeMousePoint.Subtract(other);
  }

  private PortModel FindNearPortToAttachTo()
  {
    if (OngoingLink == null || _targetPositionAnchor == null) return null;
    PortModel nearPortToAttachTo = null;
    var num1 = double.PositiveInfinity;
    var position = _targetPositionAnchor.GetPosition(OngoingLink);
    if (position == null) return null;
    foreach (var other in Diagram.Nodes.SelectMany((Func<NodeModel, IEnumerable<PortModel>>) (n => n.Ports)))
    {
      var num2 = position.DistanceTo(other.Position);
      if (num2 <= Diagram.Options.Links.SnappingRadius)
      {
        var model = OngoingLink.Source.Model;
        if ((model != null ? (model.CanAttachTo(other) ? 1 : 0) : 1) != 0 && num2 < num1)
        {
          num1 = num2;
          nearPortToAttachTo = other;
        }
      }
    }
    return nearPortToAttachTo;
  }

  public override void Dispose()
  {
    Diagram.PointerDown -= OnPointerDown;
    Diagram.PointerMove -= OnPointerMove;
    Diagram.PointerUp -= OnPointerUp;
  }
}

