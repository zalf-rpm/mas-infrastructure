using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;

namespace BlazorDrawFBP.Models;

public class ChannelLinkLabelModel : LinkLabelModel
{
    public ChannelLinkLabelModel(
        BaseLinkModel parent,
        string id,
        string content,
        double? distance = null,
        Point offset = null)
        : base(parent, id, content, distance, offset)
    {
    }

    public ChannelLinkLabelModel(BaseLinkModel parent, string content, double? distance = null, Point offset = null)
    : base(parent, content, distance, offset)
    {
    }

    public bool InteractiveMode { get; set; } = false;
    
    public void ToggleInteractiveMode(bool interactiveMode)
    {
        InteractiveMode = interactiveMode;
        Refresh();
    }
}
