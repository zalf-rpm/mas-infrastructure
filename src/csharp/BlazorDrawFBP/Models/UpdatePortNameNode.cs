using Blazor.Diagrams.Core;

namespace BlazorDrawFBP.Models;

using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

public class UpdatePortNameNode : NodeModel
{
    public UpdatePortNameNode(Point position = null) : base(position) { }

    public string Label { get; set; }
    public LinkLabelModel LabelModel { get; set; }
    
    public CapnpFbpPortModel PortModel { get; set; }
    
    public Diagram Container { get; set; }
}