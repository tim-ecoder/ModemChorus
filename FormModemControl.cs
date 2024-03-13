using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ModemChorus
{
    public partial class ModemControl : Form
    {
        private List<String> ExcludePorts = new List<string>();
        private Dictionary<int, PortState> Ports = new Dictionary<int, PortState>();

        Form1 symphonyForm;

        public ModemControl()
        {
            InitializeComponent();


        }

        private void ModemControl_Load(object sender, EventArgs e)
        {

            var value1   = System.Configuration.ConfigurationManager.AppSettings["exclude-ports"];
            var temp = value1.Split(new char[] { ',', ' ', ';', '.', '|'});
            foreach(String port in temp)
            {
                ExcludePorts.Add(port.Trim());
            }

            LogText("CREATING COM PORTS: ");
            //show list of valid com ports
            foreach (string s in SerialPort.GetPortNames())
            {
                if (IsExcludePort(s)) { continue; }

                String portNum = Regex.Match(s, "COM(\\d+)").Groups[1].Value;
                LogText(String.Format("{0} ", portNum));
                PortState ps = PortState.Initialize(s, LogText, UpdateStates);

                ps.ItemNumber = listBox1.Items.Count;
                Ports.Add(ps.ItemNumber, ps);
                listBox1.Items.Add(s);
            }
            LogText("\r\n");


            LogText("OPENING COM PORTS: ");
            foreach (var ps in Ports.Values)
            {
                ps.OpenPort();
                listBox2.Items.Add(ps.State);
 
                LogText(String.Format("{0} ", ps.PortNumber));
            }
            LogText("\r\n");
        }

        private void ModemControl_Shown(object sender, EventArgs e)
        {
            new Thread(ModemStatupInitialization).Start();
            //this.Invoke(new Action(ModemStatupInitialization));
        }

        private void ModemStatupInitialization()
        {

            LogText("WAIT...\r\n");
            WellSleep(750);
            LogText("SEND ATZ\r\n");
            foreach (int i in Ports.Keys)
            {
                var ps = Ports[i];
                ps.SendATZ();
                //WellSleep(100);
            }

            LogText("WAIT...\r\n");
            WellSleep(1000);
            LogText("SEND ATI3\r\n");
            foreach (int i in Ports.Keys)
            {
                var ps = Ports[i];
                ps.SendATI3();
                //WellSleep(100);
            }

            LogText("WAIT...\r\n");
            WellSleep(3000);

            foreach (var ps in Ports.Values)
            {
                ps.State = ModemState.READY;
            }

            UpdateStates();


            //LogText("SEND ATZ\r\n");
            //foreach (int i in Ports.Keys)
            //{
            //    var ps = Ports[i];
            //    ps.SendATZ();
            //}

            //WellSleep(3000);
            LogText("READY.\r\n");


        }

        private void WellSleep(int time)
        {
            //return;
            int K = 2;
            for (int i = 0; i < K; i++)
            {
                Thread.Sleep(time / K);
            }
        }



        private bool IsExcludePort(string port)
        {
            foreach (string port1 in ExcludePorts)
            {
                if (port1.Equals(port))
                    return true;
            }
            return false;
        }


        private void ModemDialup(PortState ps)
        {
            if(!ps.SerialPort.IsOpen)
            {
                textBox1.Text += "PORT " + ps.ComPort + " NOT OPEN";
                return;
            }

            ps.SendConnect();
            return;
        }

        private void ModemControl_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach(var port in Ports.Values)
            {
                if (port.SerialPort == null)
                    continue;
                port.Close();
            }
        }



        private void LogText(String str)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(LogText), new object[] { str });
                return;
            }
            textBox1.AppendText(String.Format("{0}", str));
        }

        public void UpdateStates()
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(UpdateStates));
                return;
            }
            foreach (var ps in Ports)
            {
                if (!String.IsNullOrEmpty(ps.Value.ModemName))
                {
                    listBox1.Items[ps.Value.ItemNumber] = String.Format("{0}: {1}", ps.Value.ComPort, ps.Value.ModemName);
                }
                if (ps.Value.State == ModemState.CONNECTED)
                {
                    listBox2.Items[ps.Value.ItemNumber] = ps.Value.State + " " + ps.Value.ConnectSpeed;
                }
                else
                {
                    listBox2.Items[ps.Value.ItemNumber] = ps.Value.State;
                }
            }
        }

        private void textBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            textBox1.Clear();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            LogText("DIALING PORTS: ");

            if (listBox1.SelectedItems == null || listBox1.SelectedItems.Count == 0)
            {
                foreach (int i in Ports.Keys)
                {
                    var ps = Ports[i];
                    LogText(String.Format("{0} ", ps.PortNumber));
                    ModemDialup(ps);
                    //WellSleep(100);
                }
            }
            else
            {
                foreach (int i in listBox1.SelectedIndices)
                {

                    var ps = Ports[i];
                    LogText(String.Format("{0} ", ps.PortNumber));
                    ModemDialup(ps);
                    //WellSleep(100);
                }
            }
            LogText("\r\n");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LogText("HANGUP PORTS: ");
            if (listBox1.SelectedItems == null || listBox1.SelectedItems.Count == 0)
            {
                foreach (int i in Ports.Keys)
                {
                    var ps = Ports[i];
                    LogText(String.Format("{0} ", ps.PortNumber));
                    ps.SendCommand("ATH0");
                    //WellSleep(100);
                }
            }
            else
            {
                foreach (int i in listBox1.SelectedIndices)
                {
                    var ps = Ports[i];
                    LogText(String.Format("{0} ", ps.PortNumber));
                    ps.SendCommand("ATH0");
                    //WellSleep(100);
                }
            }
            LogText("\r\n");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LogText("ATZ PORTS: ");
            if (listBox1.SelectedItems == null || listBox1.SelectedItems.Count == 0) { 
                foreach (int i in Ports.Keys)
                {
                    var ps = Ports[i];
                    LogText(String.Format("{0} ", ps.PortNumber));
                    ps.SendATZ();
                    //WellSleep(100);
                }
            } else
            {
                foreach (int i in listBox1.SelectedIndices)
                {

                    var ps = Ports[i];
                    LogText(String.Format("{0} ", ps.PortNumber));
                    ps.SendATZ();
                    //WellSleep(100);
                }
            }
            LogText("\r\n");
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue != 13)
                return;

            String cmd;
            LogText("CMD PORTS: ");
            if (String.IsNullOrEmpty(textBox2.Text))
                cmd = "\r";
            else
                cmd = textBox2.Text;

            cmd = cmd.Replace("\\r", "\r").Replace("\\n", "\n");

            if (listBox1.SelectedItems == null || listBox1.SelectedItems.Count == 0)
            {
                foreach (int i in Ports.Keys)
                {
                    var ps = Ports[i];
                    LogText(String.Format("{0} ", ps.PortNumber));
                    ps.SendCommand(cmd);
                    //WellSleep(100);
                }
            }
            else
            {
                foreach (int i in listBox1.SelectedIndices)
                {

                    var ps = Ports[i];
                    LogText(String.Format("{0} ", ps.PortNumber));
                    ps.SendCommand(cmd);
                    //WellSleep(100);
                }
            }

            LogText("\r\n");

        }
                   
        private void button4_Click(object sender, EventArgs e)
        {
            LogText("RESET PORTS: ");
            if (listBox1.SelectedItems == null || listBox1.SelectedItems.Count == 0)
            {
                foreach (int i in Ports.Keys)
                {
                    var ps = Ports[i];
                    LogText(String.Format("{0} ", ps.PortNumber));
                    ps.ResetPort();
                    //WellSleep(100);
                }
            }
            else
            {
                foreach (int i in listBox1.SelectedIndices)
                {

                    var ps = Ports[i];
                    LogText(String.Format("{0} ", ps.PortNumber));
                    ps.ResetPort();
                    //WellSleep(100);
                }
            }
            LogText("\r\n");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (symphonyForm == null || symphonyForm.IsDisposed)
            {
                symphonyForm = new Form1(Ports);
                symphonyForm.Show();
            }
        }
    }
}