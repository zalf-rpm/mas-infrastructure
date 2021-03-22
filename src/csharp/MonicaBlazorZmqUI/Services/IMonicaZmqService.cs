using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonicaBlazorZmqUI.Services
{
    public interface IMonicaZmqService
    {
        bool Send(string message, string serverPushAddress, int serverPushPort);

        Task Recieve();
    }
}
