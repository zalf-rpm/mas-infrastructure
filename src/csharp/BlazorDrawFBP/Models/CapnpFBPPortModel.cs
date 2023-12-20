using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;

namespace BlazorDrawFBP.Models;

public class CapnpFBPPortModel : PortModel
{
    public enum PortType
    {
        In,
        Out
    }

    public PortType ThePortType { get; }
    
    public CapnpFBPPortModel(NodeModel parent, PortType thePortType, PortAlignment alignment = PortAlignment.Bottom,
        Point position = null, Size size = null) : base(parent, alignment, position, size)
    {
        ThePortType = thePortType;
    }

    public CapnpFBPPortModel(string id, NodeModel parent, PortType thePortType, PortAlignment alignment = PortAlignment.Bottom,
        Point position = null, Size size = null) : base(id, parent, alignment, position, size)
    {
        ThePortType = thePortType;
    }

    public override bool CanAttachTo(ILinkable other)
    {
        // default constraints
        if (!base.CanAttachTo(other)) return false;

        if (other is not CapnpFBPPortModel otherPort) return false;

        // Only link Ins with Outs
        return ThePortType != otherPort.ThePortType;
    }
}