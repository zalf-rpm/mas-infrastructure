using System.Linq;
using System.Threading.Tasks;
using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Controls;
using Blazor.Diagrams.Core.Controls.Default;
using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Positions;
using BlazorDrawFBP.Models;

namespace BlazorDrawFBP.Controls;

public class RemoveLinkControl : ExecutableControl
{
    private readonly IPositionProvider _positionProvider;

    public RemoveLinkControl(double x, double y, double offsetX = 0.0, double offsetY = 0.0)
        : this(new BoundsBasedPositionProvider(x, y, offsetX, offsetY))
    {
    }

    public RemoveLinkControl(IPositionProvider positionProvider)
    {
        _positionProvider = positionProvider;
    }

    public override Point GetPosition(Model model) => _positionProvider.GetPosition(model);

    public override async ValueTask OnPointerDown(Diagram diagram, Model model, PointerEventArgs _)
    {
        if (!await ShouldDeleteModel(diagram, model))
            return;
        DeleteModel(diagram, model);
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
                if (baseLinkModel.Source.Model is NodeModel sourceNode)
                {
                    foreach (var p in sourceNode.Ports)
                    {
                        if (p is CapnpFbpPortModel { ThePortType: CapnpFbpPortModel.PortType.Out } ocp &&
                            ocp.Name == baseLinkModel.Labels[0].Content)
                        {
                            ocp.Visibility = CapnpFbpPortModel.VisibilityState.Visible;
                        }
                    }
                    sourceNode.RefreshAll();
                }
                if (baseLinkModel.Target.Model is NodeModel targetNode)
                {
                    var noOfLinksToInPort = diagram.Links.Count(l => l.Target.Model == targetNode
                    && l.Labels[1].Content == baseLinkModel.Labels[1].Content);

                    if (noOfLinksToInPort == 1)
                    {
                        foreach (var p in targetNode.Ports)
                        {
                            if (p is CapnpFbpPortModel { ThePortType: CapnpFbpPortModel.PortType.In } ocp &&
                                ocp.Name == baseLinkModel.Labels[1].Content)
                            {
                                ocp.Visibility = CapnpFbpPortModel.VisibilityState.Visible;
                            }
                        }
                    }
                    targetNode.RefreshAll();
                }
                
                diagram.Links.Remove(baseLinkModel);
                break;
        }
    }

    private static async ValueTask<bool> ShouldDeleteModel(Diagram diagram, Model model)
    {
        if (model.Locked)
            return false;
        bool flag = model switch
        {
            GroupModel groupModel => await diagram.Options.Constraints.ShouldDeleteGroup(groupModel),
            NodeModel nodeModel => await diagram.Options.Constraints.ShouldDeleteNode(nodeModel),
            BaseLinkModel baseLinkModel => await diagram.Options.Constraints.ShouldDeleteLink(baseLinkModel),
            _ => false
        };

        return flag;
    }
}