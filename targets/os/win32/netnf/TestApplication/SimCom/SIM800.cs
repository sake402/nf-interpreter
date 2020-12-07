using System;
using System.Threading.Tasks;

namespace NanoTest.SimCom
{
    public class SIM800
    {
        //SerialDevice serialPort;
        //DataWriter outputDataWriter;
        //DataReader inputDataReader;
        public SIM800(int port)
        {
            //Configuration.SetPinFunction(Gpio.IO04, DeviceFunction.COM2_TX);
            //Configuration.SetPinFunction(Gpio.IO05, DeviceFunction.COM2_RX);
            //serialPort = SerialDevice.FromId($"COM{port}");
            //serialPort.BaudRate = 9600;
            //serialPort.Parity = SerialParity.None;
            //serialPort.StopBits = SerialStopBitCount.One;
            //serialPort.Handshake = SerialHandshake.None;
            //serialPort.DataBits = 8;
            //serialPort.WriteTimeout = new TimeSpan(0, 0, 5);
            //serialPort.ReadTimeout = new TimeSpan(0, 0, 4);
            //serialPort.WatchChar = '\r';
            //serialPort.DataReceived += SerialReceived;

            //outputDataWriter = new DataWriter(serialPort.OutputStream);

            //inputDataReader = new DataReader(serialPort.InputStream);
            //inputDataReader.InputStreamOptions = InputStreamOptions.Partial;
        }

        public event EventHandler<byte[]> UnsolicitedDataReceived;

        TaskCompletionSource<byte[]> btcs;
        TaskCompletionSource<string> stcs;
        //void SerialReceived(object sender, SerialDataReceivedEventArgs e)
        //{
        //    if (e.EventType == SerialData.WatchChar)
        //    {
        //        if (stcs != null)
        //        {
        //            var bytesRead = inputDataReader.Load(128);
        //            var data = inputDataReader.ReadString(bytesRead);
        //            stcs.SetResult(data);
        //            stcs = null;
        //        }
        //        else if (btcs != null)
        //        {
        //            serialPort.ReadTimeout = TimeSpan.Zero;
        //            var bytesRead = inputDataReader.Load(128);
        //            byte[] b = new byte[bytesRead];
        //            inputDataReader.ReadBytes(b);
        //            btcs.SetResult(b);
        //            btcs = null;
        //        }
        //        else //unsolicited sata
        //        {
        //            serialPort.ReadTimeout = TimeSpan.Zero;
        //            var bytesRead = inputDataReader.Load(128);
        //            byte[] b = new byte[bytesRead];
        //            inputDataReader.ReadBytes(b);
        //            UnsolicitedDataReceived?.Invoke(this, b);
        //        }
        //    }
        //}

        internal Task<byte[]> SendCommand(byte[] data, int timeOut = -1)
        {
            btcs = new TaskCompletionSource<byte[]>();
            //outputDataWriter.WriteBytes(data);
            //outputDataWriter.Store();
            return btcs.Task;
        }

        internal Task<string> SendCommand(string command, int timeOut = -1)
        {
            stcs = new TaskCompletionSource<string>();
            //if (command.IndexOf("AT") != 0)
            //{
            //    outputDataWriter.WriteString("AT");
            //}
            //outputDataWriter.WriteString(command);
            //outputDataWriter.Store();
            return stcs.Task;
        }

        internal async Task<bool> SendOKCommand(string command, int timeOut = -1)
        {
            var result = await SendCommand(command, timeOut).ConfigureAwait(false);
            return result == "OK";
        }

        internal async Task<bool> SendPatternedResponseCommand(string command, string response, int timeOut = -1)
        {
            var result = await SendCommand(command, timeOut).ConfigureAwait(false);
            return response.IndexOf(result) != -1;
        }

        /// <summary>
        /// Check if modem is detected by sending AT and expecting OK
        /// </summary>
        /// <returns></returns>
        public Task<bool> IsConnectedAsync()
        {
            return SendOKCommand("");
        }

        GPRS gprs;
        public GPRS GPRS
        {
            get
            {
                if (gprs == null)
                    gprs = new GPRS(this, 1);
                return gprs;
            }
        }

        Socket socket;
        public Socket Socket
        {
            get
            {
                if (socket == null)
                    socket = new Socket(this, 1);
                return socket;
            }
        }
    }
}
