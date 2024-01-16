using System.Threading.Tasks;
using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Controls;
using Blazor.Diagrams.Core.Controls.Default;
using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Positions;

namespace BlazorDrawFBP.Controls;

public class AddPortControl : ExecutableControl
{
    private readonly IPositionProvider _positionProvider;

    public AddPortControl(double x, double y, double offsetX = 0.0, double offsetY = 0.0)
        : this((IPositionProvider)new BoundsBasedPositionProvider(x, y, offsetX, offsetY))
    {
    }

    public AddPortControl(IPositionProvider positionProvider)
    {
        this._positionProvider = positionProvider;
    }

    public override Point? GetPosition(Model model) => this._positionProvider.GetPosition(model);

    public override async ValueTask OnPointerDown(Diagram diagram, Model model, PointerEventArgs _)
    {
        if (!await AddPortControl.ShouldDeleteModel(diagram, model))
            return;
        AddPortControl.DeleteModel(diagram, model);
    }

    private static void DeleteModel(Diagram diagram, Model model)
    {
        switch (model)
        {
            case GroupModel group:
                diagram.Groups.Delete(group);
                break;
            case NodeModel nodeModel:
                diagram.Nodes.Remove(nodeModel);
                break;
            case BaseLinkModel baseLinkModel:
                diagram.Links.Remove(baseLinkModel);
                break;
        }
    }

    private static async ValueTask<bool> ShouldDeleteModel(Diagram diagram, Model model)
    {
        if (model.Locked)
            return false;
        bool flag;
        switch (model)
        {
            case GroupModel groupModel:
                flag = await diagram.Options.Constraints.ShouldDeleteGroup(groupModel);
                break;
            case NodeModel nodeModel:
                flag = await diagram.Options.Constraints.ShouldDeleteNode(nodeModel);
                break;
            case BaseLinkModel baseLinkModel:
                flag = await diagram.Options.Constraints.ShouldDeleteLink(baseLinkModel);
                break;
            default:
                flag = false;
                break;
        }

        return flag;
    }
}