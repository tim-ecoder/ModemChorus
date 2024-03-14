using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ModemChorus
{
    public partial class frmModemControl : Form
    {
        private List<String> ExcludePorts = new List<string>();
        private Dictionary<int, PortState> Ports = new Dictionary<int, PortState>();

        frmSymphony symphonyForm;

        public frmModemControl()
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

                ps.ItemNumber = lbPorts.Items.Count;
                Ports.Add(ps.ItemNumber, ps);
                lbPorts.Items.Add(s);
            }
            LogText("\r\n");


            LogText("OPENING COM PORTS: ");
            foreach (var ps in Ports.Values)
            {
                ps.OpenPort();
                lbMdmStatus.Items.Add(ps.State);
 
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
            EnableBtSymphony();


        }

        private void EnableBtSymphony()
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(EnableBtSymphony));
                return;
            }
            btSymphony.Enabled = true;
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
                tbConsole.Text += "PORT " + ps.ComPort + " NOT OPEN";
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
            tbConsole.AppendText(String.Format("{0}", str));
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
                    lbPorts.Items[ps.Value.ItemNumber] = String.Format("{0}: {1}", ps.Value.ComPort, ps.Value.ModemName);
                }
                if (ps.Value.State == ModemState.CONNECTED)
                {
                    lbMdmStatus.Items[ps.Value.ItemNumber] = ps.Value.State + " " + ps.Value.ConnectSpeed;
                }
                else
                {
                    lbMdmStatus.Items[ps.Value.ItemNumber] = ps.Value.State;
                }
            }
        }

        private void textBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            tbConsole.Clear();
        }



        private void btDialup_Click(object sender, EventArgs e)
        {
            LogText("DIALING PORTS: ");

            if (lbPorts.SelectedItems == null || lbPorts.SelectedItems.Count == 0)
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
                foreach (int i in lbPorts.SelectedIndices)
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
            if (lbPorts.SelectedItems == null || lbPorts.SelectedItems.Count == 0)
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
                foreach (int i in lbPorts.SelectedIndices)
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
            if (lbPorts.SelectedItems == null || lbPorts.SelectedItems.Count == 0) { 
                foreach (int i in Ports.Keys)
                {
                    var ps = Ports[i];
                    LogText(String.Format("{0} ", ps.PortNumber));
                    ps.SendATZ();
                    //WellSleep(100);
                }
            } else
            {
                foreach (int i in lbPorts.SelectedIndices)
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
            if (String.IsNullOrEmpty(tbAtCmd.Text))
                cmd = "\r";
            else
                cmd = tbAtCmd.Text;

            cmd = cmd.Replace("\\r", "\r").Replace("\\n", "\n");

            if (lbPorts.SelectedItems == null || lbPorts.SelectedItems.Count == 0)
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
                foreach (int i in lbPorts.SelectedIndices)
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
            if (lbPorts.SelectedItems == null || lbPorts.SelectedItems.Count == 0)
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
                foreach (int i in lbPorts.SelectedIndices)
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
                symphonyForm = new frmSymphony(Ports);
                symphonyForm.Show();
            }
        }
    }
}