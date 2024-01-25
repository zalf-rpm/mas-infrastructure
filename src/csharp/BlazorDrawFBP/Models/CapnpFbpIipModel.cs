using System.Collections.Generic;
using System.Linq;

namespace BlazorDrawFBP.Models;

using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

public class CapnpFbpIipModel : NodeModel
{
    public CapnpFbpIipModel(Point position = null) : base(position) {}
    
    public string Content { get; set; }
}