using Blazor.Diagrams.Core.Controls.Default;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorDrawFBP.Controls;

public class RemoveLinkControlWidget : ComponentBase
{
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (Model is SvgNodeModel)
        {
            builder.AddMarkupContent(0, "<circle r=\"12\" fill=\"rgba(255,152,0,1)\"></circle>");
            builder.AddMarkupContent(0, "<path d=\"M0 0h24v24H0z\" fill=\"none\"></path>");
            builder.AddMarkupContent(0, "<path d=\"M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z\"></path>");
        }
        else
        {
            builder.AddMarkupContent(1,
                "<svg width=\"24\" height=\"24\" viewBox=\"0 0 24 24\" style=\"overflow: visible;\"><circle cx=\"12\" cy=\"12\" r=\"20\" fill=\"rgba(255,152,0,1)\"></circle><path d=\"M0 0h24v24H0z\" fill=\"none\"></path><path fill=\"white\" d=\"M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z\"></path></svg>");
        }
    }

    [Parameter]
    public RemoveLinkControl Control { get; set; }

    [Parameter]
    public Model Model { get; set; }
}
