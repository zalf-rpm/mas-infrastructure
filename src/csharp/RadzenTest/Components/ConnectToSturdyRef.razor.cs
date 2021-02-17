using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace RadzenTest.Components
{
    public partial class ConnectToSturdyRef<TCapnpInterface> where TCapnpInterface : class, IDisposable
    {
        /*
        private String sturdyRef = "capnp://localhost:56081/";

        [Parameter]
        public TCapnpInterface Capability { get; set; }

        [Parameter]
        public EventCallback<TCapnpInterface> CapabilityChanged { get; set; }

        private async Task Connect()
        {
            Capability = await ConMan.Connect<TCapnpInterface>(sturdyRef);
            await CapabilityChanged.InvokeAsync(Capability);
        }
        */
    }
}
