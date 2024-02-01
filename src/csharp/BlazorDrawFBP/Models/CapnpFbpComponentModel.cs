using System.Collections.Generic;
using System.Linq;

namespace BlazorDrawFBP.Models;

using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

public class CapnpFbpComponentModel : NodeModel
{
    public CapnpFbpComponentModel(Point position = null) : base(position) {}
    
    // the id of component
    public string ComponentId { get; set; }
    public string ProcessName { get; set; }
    public string ShortDescription { get; set; }
    public string PathToFile { get; set; }

    public int InParallelCount { get; set; } = 1;
    
    public bool Editable { get; set; } = true;
    
    public static int ProcessNo { get; set; } = 0;
    
    // if null PathToFile is a standalone executable, else a script needing the interpreter
    public string PathToInterpreter { get; set; } = null;
    
    // public struct CmdParam
    // {
    //     public string Name { get; set; }
    //     public string Value { get; set; }
    // }
    //
    // public void AddEmptyCmdParam()
    // {
    //     CmdParameters.Add(new CmdParam { Name = "", Value = "" });
    // }
    // public readonly List<CmdParam> CmdParameters = new();
    public string CmdParamString { get; set; }
}