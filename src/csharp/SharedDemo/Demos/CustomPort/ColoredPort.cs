using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;

namespace SharedDemo.Demos.CustomPort
{
    public class ColoredPort : PortModel
    {
        public ColoredPort(NodeModel parent, PortAlignment alignment, bool isRed) : base(parent, alignment, null, null)
        {
            IsRed = isRed;
        }

        public bool IsRed { get; set; }

        public override bool CanAttachTo(ILinkable port)
        {
            // Checks for same-node/port attachements
            if (!base.CanAttachTo(port))
                return false;

            // Only able to attach to the same port type
            if (!(port is ColoredPort cp))
                return false;

            return IsRed == cp.IsRed;
        }
    }
}
