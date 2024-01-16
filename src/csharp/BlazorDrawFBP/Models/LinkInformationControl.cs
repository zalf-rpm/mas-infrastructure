using Blazor.Diagrams.Core.Controls;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;

namespace BlazorDrawFBP.Models;

public class LinkInformationControl : Control
{
    public override Point GetPosition(Model model)
    {
        // We want the information to be under the node
        if (model is LinkModel link)
        {
            return link.GetBounds()?.Center.Add(0, 10);
        }
        return null;
    }
}