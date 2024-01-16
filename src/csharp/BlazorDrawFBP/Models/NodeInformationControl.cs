using Blazor.Diagrams.Core.Controls;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;

namespace BlazorDrawFBP.Models;

public class NodeInformationControl : Control
{
    public override Point GetPosition(Model model)
    {
        // We want the information to be under the node
        if (model is NodeModel node)
        {
            if (node.Size == null) return null;
            return node.Position.Add(0, node.Size!.Height + 10);
        }
        else if (model is LinkModel link)
        {
            return link.GetBounds()?.Center.Add(0, 10);
        }
        return null;
    }
}