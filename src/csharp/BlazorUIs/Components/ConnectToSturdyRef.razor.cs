using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace BlazorUIs.Components
{
    public partial class ConnectToSturdyRef<TCapnpInterface> where TCapnpInterface : class, IDisposable
    {
    }
}
