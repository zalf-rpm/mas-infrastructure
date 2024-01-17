using Blazor.Diagrams.Core;

namespace BlazorDrawFBP.Models;

using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

public class PortOptionsNode : NodeModel
{
    public PortOptionsNode(Point position = null) : base(position) { }

    public string Label { get; set; }
    public CapnpFbpPortModel PortModel { get; set; }
    
    public NodeModel NodeModel { get; set; }
    
    public Diagram Container { get; set; }
}