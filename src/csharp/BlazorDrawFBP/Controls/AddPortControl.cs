using System.Collections.Generic;
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

public class AddPortControl : ExecutableControl
{
    private readonly IPositionProvider _positionProvider;
    public string Label { get; set; } = "Port";

    public CapnpFbpPortModel.PortType PortType { get; set; } = CapnpFbpPortModel.PortType.In;

    public CapnpFbpComponentModel NodeModel { get; set; } = null;
    
    public AddPortControl(double x, double y, double offsetX = 0.0, double offsetY = 0.0)
        : this(new BoundsBasedPositionProvider(x, y, offsetX, offsetY))
    {
    }

    public AddPortControl(IPositionProvider positionProvider)
    {
        _positionProvider = positionProvider;
    }

    public override Point GetPosition(Model model) => _positionProvider.GetPosition(model);

    public override async ValueTask OnPointerDown(Diagram diagram, Model model, PointerEventArgs _)
    {
        var ports = 
            NodeModel.Ports.Where(p => p is CapnpFbpPortModel cp && cp.ThePortType == PortType)
            .OrderBy(p => p is CapnpFbpPortModel cp ? cp.OrderNo : 0);
        if (ports.Any())
        {
            if (ports.Last() is CapnpFbpPortModel lastPort)
            {
                var newOrderNo = lastPort.OrderNo + 1;
                CreateAndAddPort(NodeModel, PortType, newOrderNo);
            }
            NodeModel.RefreshAll();
        }
    }

    public static CapnpFbpPortModel CreateAndAddPort(CapnpFbpComponentModel node,
        CapnpFbpPortModel.PortType portType, int orderNo, string name = null)
    {
        if (orderNo > 19) return null;
        var alignment = PortAlignmentForOrderNo(portType, orderNo);
        var port = new CapnpFbpPortModel(node, portType, alignment)
        {
            Name = name ?? (portType == CapnpFbpPortModel.PortType.In ? "IN" : "OUT"),
            OrderNo = orderNo,
        };
        node.AddPort(port);
        node.RefreshAll();
        return port;
    }
    
    private static PortAlignment PortAlignmentForOrderNo(CapnpFbpPortModel.PortType portType, int orderNo)
    {
        if (portType == CapnpFbpPortModel.PortType.In)
        {
            return orderNo switch
            {
                10 => PortAlignment.TopLeft,
                > 10 => PortAlignment.Top,
                _ => PortAlignment.Left
            };
        }
        else
        {
            return orderNo switch
            {
                10 => PortAlignment.BottomRight,
                > 10 => PortAlignment.Bottom,
                _ => PortAlignment.Right
            };
        }
    }
    
    private static readonly Dictionary<int, List<int>> LeftRightOffsetsMap = new()
    {
        { 1, new List<int> { 0 } },
        { 2, new List<int> { 10, -10 } },
        { 3, new List<int> { 20, 0, -20 } },
        { 4, new List<int> { 30, 10, -10, -30 } },
        { 5, new List<int> { 30, 15, 0, -15, -30 } },
        { 6, new List<int> { 30, 15, 0, -15, -30, -45 } },
        { 7, new List<int> { 30, 20, 10, 0, -10, -20, -30 } },
        { 8, new List<int> { 35, 25, 15, 5, -5, -15, -25, -35 } },
        { 9, new List<int> { 40, 30, 20, 10, 0, -10, -20, -30, -40 } },
        { 10, new List<int> { -45, -35, -25, -15, -5, 5, 15, 25, 35, 45 } },
        { 11, new List<int> { 50, 40, 30, 20, 10, 0, -10, -20, -30, -40, -50 } },
    };

    private static readonly List<int> TopBottomOffsets = new() { -40, -30, -20, -10, 0, 10, 20, 30, 40 };
}