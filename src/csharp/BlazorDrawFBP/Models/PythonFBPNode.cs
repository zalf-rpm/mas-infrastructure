using System.Collections.Generic;
using System.Linq;

namespace BlazorDrawFBP.Models;

using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

public class PythonFBPNode : NodeModel
{
    public PythonFBPNode(Point position = null) : base(position) { }
    
    public string UserId { get; set; }
    public string ShortDescription { get; set; }
    public string PathToPythonFile { get; set; }

    public struct CmdParam
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public void AddEmptyCmdParam()
    {
        CmdParameters.Add(new CmdParam { Name = "", Value = "" });
    }
    public readonly List<CmdParam> CmdParameters = new();
    public string CmdParamString { get; set; }
}