using System.Threading.Tasks;

namespace NanoTest.SimCom
{
    public class GPRS
    {
        SIM800 sim800;
        int connectionId;

        public GPRS(SIM800 sim800, int connectionId)
        {
            this.sim800 = sim800;
            this.connectionId = connectionId;
        }

        static string[] GPRSTags = { "", "CONTYPE", "APN", "USER", "PWD", "PHONENUM", "RATE" };
        enum GPRSCommand { CloseBearer, OpenBearer, QueryBearer, SetupBearer, GetBearer };
        enum GPRSTag { None, ConnectionType, APN, UserName, Password, PhoneNumber, DataRate };
        Task<bool> SetupAsync(GPRSCommand type, GPRSTag tag = GPRSTag.None, string value = null)
        {
            if (tag != GPRSTag.None)
                return sim800.SendOKCommand($"+SAPBR={(int)type},{connectionId},\"{GPRSTags[(int)tag]}\",\"{value}\"");
            else
                return sim800.SendOKCommand($"+SAPBR={(int)type},{connectionId}", type == GPRSCommand.OpenBearer ? 10000 : -1);
        }

        /// <summary>
        /// return connection IP address or null if not connected
        /// </summary>
        /// <returns></returns>
        public async Task<string> IsConnected()
        {
            var response = await sim800.SendCommand($"SAPBR=2,{connectionId}").ConfigureAwait(false);
            if (response.IndexOf($"+SAPBR: {connectionId},1") != -1)
            {

            }
            return null;
        }

        public async Task<string> ConnectAsync(string apn, string password = null)
        {
            var conn = await IsConnected().ConfigureAwait(false);
            if (conn == null)
            {
                if (await SetupAsync(GPRSCommand.SetupBearer, GPRSTag.ConnectionType, "GPRS").ConfigureAwait(false))
                {
                    if (await SetupAsync(GPRSCommand.SetupBearer, GPRSTag.APN, apn).ConfigureAwait(false))
                    {
                        await SetupAsync(GPRSCommand.OpenBearer).ConfigureAwait(false);
                        conn = await IsConnected().ConfigureAwait(false);
                    }
                }
            }
            return conn;
        }

        public Task<bool> DisconnectAsync()
        {
            return SetupAsync(GPRSCommand.CloseBearer, GPRSTag.None, null);
        }
    }
}
