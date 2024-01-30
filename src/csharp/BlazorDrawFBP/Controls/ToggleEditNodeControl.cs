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
using MudBlazor.Interfaces;

namespace BlazorDrawFBP.Controls;

public class ToggleEditNodeControl : ExecutableControl
{
    private readonly IPositionProvider _positionProvider;

    public CapnpFbpComponentModel NodeModel { get; set; }
    
    public ToggleEditNodeControl(double x, double y, double offsetX = 0.0, double offsetY = 0.0)
        : this(new BoundsBasedPositionProvider(x, y, offsetX, offsetY))
    {
    }

    public ToggleEditNodeControl(IPositionProvider positionProvider)
    {
        _positionProvider = positionProvider;
    }

    public override Point GetPosition(Model model) => _positionProvider.GetPosition(model);

    public override async ValueTask OnPointerDown(Diagram diagram, Model model, PointerEventArgs _)
    {
        NodeModel.Editable = !NodeModel.Editable;
        NodeModel.RefreshAll();
    }
}