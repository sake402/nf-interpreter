using NanoTest.SimCom;
using System;
using System.Threading.Tasks;

namespace NanoTest.Remote.SIM800
{
    public class Sim800Connector 
    {
        SimCom.SIM800 sim800;
        public Sim800Connector(string server)
        {
            sim800 = new SimCom.SIM800(2);
            sim800.UnsolicitedDataReceived += OnDataReceived;
            _ = Connect(server);
        }

        public SimCom.SIM800 Sim800 => sim800;

        async Task Connect(string server)
        {
            while (true)
            {
                //if (await sim800.GPRS.ConnectAsync("") != null)
                {
                    if (await sim800.Socket.SetAPNAsync("").ConfigureAwait(false))
                    {
                        if (await sim800.Socket.StartAsync().ConfigureAwait(false))
                        {
                            //if (await sim800.Socket.GetIPAsync().ConfigureAwait(false) != null)
                            //{
                            //    if (await sim800.Socket.ConnectAsync(Socket.Connection.TCP, server, 1883).ConfigureAwait(false))
                            //    {
                            //        Connected = true;
                            //        NotifySubscribers();
                            //    }
                            //}
                        }
                    }
                }
                await Task.Delay(5000).ConfigureAwait(false);
            }
        }

        public Task<byte[]> SendAsync(byte[] buffer, bool receive = false)
        {
            return sim800.Socket.SendAsync(buffer, receive);
        }

        public event EventHandler<byte[]> OnPacketReceived;
        async void OnDataReceived(object sender, byte[] data)
        {
            if (data[0] == '+' && data[0] == 'R' && data[0] == 'E' && data[0] == 'C' && data[0] == 'E' && data[0] == 'I' && data[0] == 'V' && data[0] == 'E')
            {
                byte[] bytes = await sim800.SendCommand((byte[])null);
                OnPacketReceived?.Invoke(this, bytes);
            }
        }
    }
}
