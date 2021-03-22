using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonicaBlazorZmqUI.Services
{
    public class MonicaParameters : IMonicaZmqService
    {

        public Task Recieve()
        {
            throw new NotImplementedException();
        }

        public bool Send(string message, string serverPushAddress, int serverPushPort)
        {
            try
            {
                using (var producer = new PushSocket())
                {
                    producer.Connect(serverPushAddress + ":" + serverPushPort);
                    producer.SendFrame(message);
                    System.Threading.Thread.Sleep(2000);
                    producer.Disconnect(serverPushAddress + ":" + serverPushPort);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
