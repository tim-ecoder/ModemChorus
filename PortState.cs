using System;
using System.IO.Ports;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static ModemChorus.PortState;

namespace ModemChorus
{
    public enum ModemState
    {
        NONE,
        PORT_OPEN,
        PORT_NOT_OPEN,
        ATZ_OK,
        ATZ_SENT,
        DIALUP,
        CONNECTED,
        NO_CARRIER,
        BUSY,
        ATI3,
        READY
    }
    public class PortState
    {
        public String ComPort;
        public int PortNumber;
        public SerialPort SerialPort;
        public int ItemNumber = 0;
        public ModemState State = ModemState.NONE;
        public string ConnectSpeed = "";
        public string ModemName = "";

        public delegate void WriteTextDelegate(String str);
        public WriteTextDelegate LogText;

        public delegate void UpdateStates();
        public UpdateStates updateStates;

        private SerialDataReceivedEventHandler readHendler;

        public List<String> CommandsToSend = new List<string>();

        private Thread SenderThread;
        private bool runSenderCycle = true;

        private String readBuffer = String.Empty;

        public void SenderCycle()
        {
            while(runSenderCycle)
            {
                if(CommandsToSend.Count > 0)
                {
                    var cmd = CommandsToSend[0];
                    CommandsToSend.RemoveAt(0);
                    SendCommandToPort(cmd);
                }
                Thread.Sleep(5);
            }
        }

        public PortState(String comPort, SerialPort sp, WriteTextDelegate logText, UpdateStates updateStates1)
        {
            ComPort= comPort;
            PortNumber = Int32.Parse(Regex.Match(comPort, "COM(\\d+)").Groups[1].Value);
            SerialPort = sp;
            LogText= logText;
            updateStates = updateStates1;

            readHendler = new SerialDataReceivedEventHandler(port_DataReceived); 
        }

        public static PortState Initialize(string s, WriteTextDelegate logText, UpdateStates updateStates1)
        {

            var sp = CreateSerialPort(s);
            return new PortState(s, sp, logText, updateStates1);
        }

        private static SerialPort CreateSerialPort(string s)
        {
            var _serialPort = new SerialPort(s);
            _serialPort.PortName = s;
            _serialPort.BaudRate = 57600;
            _serialPort.Parity = Parity.None;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Handshake = Handshake.None;

            // Set the read/write timeouts
            _serialPort.ReadTimeout = 2000;
            _serialPort.WriteTimeout = 2000;

            _serialPort.DtrEnable = true;
            _serialPort.RtsEnable = true;
            return _serialPort;
        }


        public void StopRead()
        {
            
            SerialPort.DataReceived -= readHendler;
            runSenderCycle = false;
        }

        public void RunReaderAndSender()
        {
            readBuffer = String.Empty;
            SerialPort.DataReceived += readHendler;
            runSenderCycle = true;
            SenderThread = new Thread(SenderCycle);
            SenderThread.Start();
        }

        public void OpenPort()
        {

            var sp = SerialPort;
            
            if (!sp.IsOpen)
            {
                sp.Open();
                //LogText("OPENNING PORT " + sp.PortName);
            }
            if (!sp.IsOpen)
            {
                LogText("PORT " + sp.PortName + " NOT OPEN\r\n");
                State = ModemState.PORT_NOT_OPEN;
                return;
            }
            RunReaderAndSender();
            State = ModemState.PORT_OPEN;

        }

        private void SendCommandToPort(String str) {
            SerialPort.Write(String.Format("{1}\r", ComPort, str, SerialPort.NewLine));
            //LogText(String.Format(">> <{0}>: {1}\r\n", ComPort, str));
        }

        public void SendCommand(String str)
        {
            CommandsToSend.Add(str);
        }

        public void SendATZ()
        {

            SendCommand("ATZ");
            State = ModemState.ATZ_SENT;
            updateStates();
        }

        public void ResetPort()
        {
            Close();
            SerialPort = CreateSerialPort(ComPort);

            OpenPort();

            updateStates();
        }

        public void port_DataReceived(object sender,  SerialDataReceivedEventArgs e)
        {
            readBuffer += SerialPort.ReadExisting();
            if(readBuffer.Length > 1024)
            {
                StopRead();
                LogText(ComPort + ": TOO LONG TEXT. RESET PORT..\n");
                readBuffer = String.Empty;
                ResetPort();
                updateStates();
                return;

            }
            //test for termination character in buffer
            if (readBuffer.EndsWith('\n'))
            {
                ProcessReadString(readBuffer);
                readBuffer = String.Empty;
            }

        }

        private void ProcessReadString(String message)
        {
            //String message = SerialPort.ReadExisting();
            if (String.IsNullOrEmpty(message.Trim()))
                return;

            if (State == ModemState.ATI3)
            {
                ModemName += message.ToUpper().Replace("\r", "").Replace("\n", "").Replace("OK", "").Replace("ATI3", "").Trim();
                LogText(ComPort + ": " + message);
                //updateStates();
                return;
            }

            if (State == ModemState.ATZ_SENT && message.Replace("ATZ","").Trim().Equals("OK"))
            {
                State = ModemState.ATZ_OK;
                LogText(String.Format("<< <{0}>: {1}\r\n", ComPort, message.Replace("\r", "").Replace("\n", "").Trim()));
                updateStates();
                return;
            }
            LogText(String.Format("<< <{0}>: {1}\r\n", ComPort, message));
            if (State == ModemState.DIALUP && message.ToUpper().Contains("CONNECT"))
            {
                var match = Regex.Match(message.ToUpper(), ".*CONNECT (.*)\r");
                if (match.Success)
                {
                    ConnectSpeed = match.Groups[1].Value;
                }
                State = ModemState.CONNECTED;
                
                updateStates();
            }

            if (message.ToUpper().Contains("NO CARRIER"))
            {
                State = ModemState.NO_CARRIER;
                updateStates();
            }
            if (State == ModemState.DIALUP && message.ToUpper().Contains("BUSY"))
            {
                State = ModemState.BUSY;
                updateStates();
            }
        }

        public void SendConnect()
        {
            SendCommand("ATDT "+Program.CALL_NUMBER);
            State = ModemState.DIALUP;
            updateStates();
        }

        public void SendATI3()
        {
            SendCommand("ATI3");
            State = ModemState.ATI3;
            updateStates();
        }

        public void Close()
        {
            readBuffer = String.Empty;
            StopRead();
            if (SerialPort.IsOpen)
            {
                SerialPort.Close();
            }
            SerialPort.Dispose();
        }
    }
}