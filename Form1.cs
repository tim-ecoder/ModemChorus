using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModemChorus
{
    public partial class Form1 : Form
    {
        public Form1(Dictionary<int, PortState> ports)
        {
            InitializeComponent();
            Ports = ports;

            int initTime = 3000;
            int deltaTime = 3000;
            int rstTime = 40000;
            int rstDeltaTime = 2000;

            foreach (var port in Ports)
            {

                var r1 = new DataGridViewRow();
                r1.Cells.Add(new DataGridViewTextBoxCell { Value = initTime + deltaTime * port.Key });
                r1.Cells.Add(new DataGridViewTextBoxCell { Value = "D" });
                r1.Cells.Add(new DataGridViewTextBoxCell { Value = port.Key });
                dataGridView1.Rows.Add(r1);
            }

            foreach (var port in Ports)
            {

                var r1 = new DataGridViewRow();
                r1.Cells.Add(new DataGridViewTextBoxCell { Value = initTime + rstTime + rstDeltaTime * port.Key });
                r1.Cells.Add(new DataGridViewTextBoxCell { Value = "R" });
                r1.Cells.Add(new DataGridViewTextBoxCell { Value = port.Key });
                dataGridView1.Rows.Add(r1);
            }
        }

        private List<SymphonyItem> _items = new List<SymphonyItem>();
        int doneItem = -1;
        bool cycle = false;
        double startTime;
        public Dictionary<int, PortState> Ports;

        private void button1_Click(object sender, EventArgs e)
        {
            _items.Clear();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Index == dataGridView1.Rows.Count - 1)
                    break;
                var si = new SymphonyItem();
                si.Time = Int32.Parse(row.Cells[0].Value.ToString());
                if (row.Cells[1].Value.ToString() == "D")
                    si.Act = ItemAction.Dial;
                if (row.Cells[1].Value.ToString() == "R")
                    si.Act = ItemAction.ResetPort;
                string[] modems = row.Cells[2].Value.ToString().Split(' ');
                foreach(var mdm in modems)
                {
                    si.ModemIndexes.Add(Int32.Parse(mdm));
                }
                _items.Add(si);
            }

            cycle = false;
            Thread.Sleep(300);

            if(backgroundWorker2 != null) {
                //backgroundWorker2.CancelAsync();
                backgroundWorker2.Dispose();
                
            }
            backgroundWorker2 = new BackgroundWorker();
            this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            ResetCycle();
            
            cycle= true;
            backgroundWorker2.RunWorkerAsync();
        }

        void ResetCycle()
        {
            startTime = GetTime();
            doneItem = 0;
        }

        double GetTime()
        {
            return DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            while (cycle) {

                if (doneItem >= _items.Count)
                    ResetCycle();
                else
                {
                    double timeDelta = GetTime() - startTime;
                    var si = _items[doneItem];
                    if (timeDelta >= si.Time)
                    {
                        DoItemWork(si);
                        doneItem++;
                    }
                }

 
                Thread.Sleep(100);
            }
        }
        private void DoItemWork(SymphonyItem item)
        {
            foreach (int mdm in item.ModemIndexes)
            {
                if (item.Act == ItemAction.Dial)
                {
                    Ports[mdm].SendConnect();
                }
                if(item.Act == ItemAction.ResetPort)
                {
                    Ports[mdm].ResetPort();
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            cycle = false;

            if (backgroundWorker2 != null)
            {
                //backgroundWorker2.CancelAsync();
                backgroundWorker2.Dispose();

            }
        }
    }



    public class SymphonyItem
    {
        public int Time;
        public ItemAction Act;
        public List<int> ModemIndexes = new List<int>();

    }

    public enum ItemAction
    {
        Dial,
        ResetPort
    }
}
