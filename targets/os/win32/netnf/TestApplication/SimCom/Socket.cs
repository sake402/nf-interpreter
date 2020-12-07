using System;
using System.Threading.Tasks;

namespace NanoTest.SimCom
{
    public class Socket
    {
        SIM800 sim800;
        static int socketCount;
        int connectionId;
        public Socket(SIM800 sim800, int connectionId)
        {
            this.sim800 = sim800;
            this.connectionId = connectionId;
            socketCount++;
        }

        public Task<bool> StartAsync()
        {
            return sim800.SendOKCommand($"+CIICR", 5000);
        }

        public Task<bool> StopAsync()
        {
            return sim800.SendPatternedResponseCommand($"+CIPSHUT", "SHUT OK");
        }

        public Task<bool> EnableMultiConectionAsync(bool enable)
        {
            socketCount++;
            return sim800.SendOKCommand($"+CIPMUX={(enable ? 1 : 0)}");
        }

        public Task<bool> SetTransparentModeAsync(bool enable)
        {
            return sim800.SendOKCommand($"+CIPMODE={(enable ? 1 : 0)}");
        }

        public Task<bool> SetAPNAsync(string apn, string username = null, string password = null)
        {
            if (apn != null && password != null)
            {
                return sim800.SendOKCommand($"+CSTT=\"{apn}\",\"{username}\",\"{password}\"");
            }
            else
            {
                return sim800.SendOKCommand($"+CSTT=\"{apn}\"");
            }
        }

        //public async Task<IPAddress> GetIPAsync()
        //{
        //    var ip = await sim800.SendCommand($"+CIFSR");
        //    return IPAddress.Parse(ip);
        //}

        public enum Connection { TCP, UDP }
        public async Task<bool> ConnectAsync(Connection type, string address, int portNumber)
        {
            bool ok = false;
            if (socketCount > 1)
            {
                ok = await sim800.SendOKCommand($"+CIPSTART={connectionId},\"{type}\",\"{address}\",\"{portNumber}\"");
            }
            else
            {
                ok = await sim800.SendOKCommand($"+CIPSTART=\"{type}\",\"{address}\",\"{portNumber}\"");
            }
            if (ok)
            {
                ok = await sim800.SendPatternedResponseCommand(null, "CONNECT OK", 10000);
            }
            return ok;
        }

        public Task<bool> IsConnectedAsync()
        {
            if (socketCount > 1)
            {
                return sim800.SendPatternedResponseCommand($"+CIPSTATUS={connectionId}", "CONNECTED");
            }
            else
            {
                return sim800.SendPatternedResponseCommand($"+CIPSTATUS", "STATE: CONNECT OK");
            }
        }

        public Task<bool> DisconnectAsync()
        {
            if (socketCount > 1)
            {
                return sim800.SendPatternedResponseCommand($"+CIPCLOSE={connectionId},1", "CLOSE OK");
            }
            else
            {
                return sim800.SendPatternedResponseCommand($"+CIPCLOSE=1", "CLOSE OK");
            }
        }

        public async Task<byte[]> SendAsync(byte[] data, bool receive = true)
        {
            bool ok = false;
            if (socketCount > 1)
            {
                ok = await sim800.SendPatternedResponseCommand($"+CIPSEND={connectionId},{data.Length}", ">");
            }
            else
            {
                ok = await sim800.SendPatternedResponseCommand($"+CIPSEND={data.Length}", ">");
            }
            if (ok)
            {
                _ = sim800.SendCommand(data);
                _ = sim800.SendCommand(new byte[] { 0x1A });
                ok = await sim800.SendPatternedResponseCommand(null, "SEND OK", 5000);
                if (receive)
                {
                    if (ok)
                    {
                        int cId, len = 0;
                        if (socketCount > 1)
                        {
                            ok = false;
                            var result = await sim800.SendCommand((string)null).ConfigureAwait(false);
                            if (result.IndexOf("+RECEIVE") == 0)
                            {
                                var split = result.Split(',');
                                cId = int.Parse(split[1]);
                                len = int.Parse(split[2]);
                                if (cId == connectionId)
                                {
                                    ok = true;
                                }
                            }
                        }
                        if (ok)
                        {
                            byte[] bytes = await sim800.SendCommand((byte[])null);
                            return bytes;
                        }
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            throw new Exception("Error");
        }
    }
}
