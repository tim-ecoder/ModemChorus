using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace ModemChorus
{
    public partial class frmSymphony : Form
    {
        public frmSymphony(Dictionary<int, PortState> ports)
        {
            InitializeComponent();
            Ports = ports;

            GenerateOneByOne();

        }

        private void GenerateOneByOne()
        {
            dgvSymphony.Rows.Clear();
            dgvSymphony.Refresh();

            int initTime = 2000;
            int deltaTime = 4000;
            int rstTime = 36000;

            string ports_str = String.Empty;

            foreach (var port in Ports)
            {

                var r0 = new DataGridViewRow();
                r0.Cells.Add(new DataGridViewTextBoxCell { Value = initTime + deltaTime * port.Key });
                r0.Cells.Add(new DataGridViewTextBoxCell { Value = "W1" });
                r0.Cells.Add(new DataGridViewTextBoxCell { Value = port.Key });
                dgvSymphony.Rows.Add(r0);
                ports_str += " " + port.Key;
            }

            ports_str = ports_str.Trim();

            var r1 = new DataGridViewRow();
            r1.Cells.Add(new DataGridViewTextBoxCell { Value = initTime });
            r1.Cells.Add(new DataGridViewTextBoxCell { Value = "D" });
            r1.Cells.Add(new DataGridViewTextBoxCell { Value = ports_str });
            dgvSymphony.Rows.Add(r1);




            var r2 = new DataGridViewRow();
            r2.Cells.Add(new DataGridViewTextBoxCell { Value = rstTime });
            r2.Cells.Add(new DataGridViewTextBoxCell { Value = "R" });
            r2.Cells.Add(new DataGridViewTextBoxCell { Value = ports_str });
            dgvSymphony.Rows.Add(r2);
        }

        private void GenerateAllTogether()
        {
            dgvSymphony.Rows.Clear();
            dgvSymphony.Refresh();

            int initTime = 2000;
            int deltaTime = 4000;
            int rstTime = 36000;

            string ports_str = String.Empty;

            foreach (var port in Ports)
            {
                ports_str += " " + port.Key;
            }

            ports_str = ports_str.Trim();

            var r1 = new DataGridViewRow();
            r1.Cells.Add(new DataGridViewTextBoxCell { Value = initTime });
            r1.Cells.Add(new DataGridViewTextBoxCell { Value = "D" });
            r1.Cells.Add(new DataGridViewTextBoxCell { Value = ports_str });
            dgvSymphony.Rows.Add(r1);




            var r2 = new DataGridViewRow();
            r2.Cells.Add(new DataGridViewTextBoxCell { Value = rstTime });
            r2.Cells.Add(new DataGridViewTextBoxCell { Value = "R" });
            r2.Cells.Add(new DataGridViewTextBoxCell { Value = ports_str });
            dgvSymphony.Rows.Add(r2);
        }

        bool cycle2 = false;
        public Dictionary<int, PortState> Ports;
        List<ModemTrack> ModemTracks = new List<ModemTrack>();


        private static void ParseModemStep(DataGridViewRow row, ModemStep si)
        {
            si.Time = Int32.Parse(row.Cells[0].Value.ToString());
            if (row.Cells[1].Value.ToString() == "D")
                si.Act = ItemAction.Dial;
            if (row.Cells[1].Value.ToString() == "R")
                si.Act = ItemAction.ResetPort;
            if (row.Cells[1].Value.ToString() == "W1")
                si.Act = ItemAction.WiatFirst;
        }

        void InitCycle2()
        {
            foreach (var track in ModemTracks)
            {
                ResetTrack(track, true);
            }
        }

        private void ResetTrack(ModemTrack track, bool firstInit)
        {
            track.startTime = GetTime();
            track.curStep = 0;
            track.prevTimes = 0;
            if (!firstInit && track.firstRun)
                track.firstRun = false;
        }

        double GetTime()
        {
            return DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
        }

        private void DoItemWork(SymphonyItem item)
        {
            foreach (int mdm in item.ModemIndexes)
            {
                RunModemStep(item, mdm);
            }
        }

        private void RunModemStep(ModemStep item, int mdm)
        {
            if (item.Act == ItemAction.Dial)
            {
                Ports[mdm].SendConnect();
            }
            if (item.Act == ItemAction.ResetPort)
            {
                Ports[mdm].ResetPort();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            cycle2 = false;

            if (backgroundWorker1 != null)
            {
                backgroundWorker1.Dispose();

            }
        }

        private void btStart_Click(object sender, EventArgs e)
        {
            ModemTracks.Clear();
            foreach (DataGridViewRow row in dgvSymphony.Rows)
            {
                if (row.Index == dgvSymphony.Rows.Count - 1)
                    break;
                var si = new ModemStep();
                ParseModemStep(row, si);
                string[] modems = row.Cells[2].Value.ToString().Split(' ');
                foreach (var mdm in modems)
                {
                    int mdmIndex = Int32.Parse(mdm);
                    var track = GetTrack(mdmIndex);
                    track.ModemSteps.Add(si);
                }

            }

            cycle2 = false;
            Thread.Sleep(300);

            if (backgroundWorker1 != null)
            {
                //backgroundWorker2.CancelAsync();
                backgroundWorker1.Dispose();

            }
            backgroundWorker1 = new BackgroundWorker();
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            InitCycle2();

            cycle2 = true;
            backgroundWorker1.RunWorkerAsync();
        }

        ModemTrack GetTrack(int mdmIndex)
        {
            if(ModemTracks.Count <= mdmIndex)
            {
                ModemTracks.Add(new ModemTrack() { ModemIndex = mdmIndex });
            }
            return ModemTracks[mdmIndex];
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while(cycle2) {

                foreach (var track in ModemTracks)
                {
                    if (track.curStep >= track.ModemSteps.Count)
                    {
                        ResetTrack(track, false);
                    }
                    else
                    {

                        double timeDelta = GetTime() - track.startTime;

                        var si = track.ModemSteps[track.curStep];
                        if(!track.firstRun && si.Act == ItemAction.WiatFirst)
                        {
                            track.curStep++;
                            si = track.ModemSteps[track.curStep];
                        }
                        if (timeDelta >= track.prevTimes + si.Time)
                        {
                            RunModemStep(si, track.ModemIndex);
                            track.prevTimes += si.Time;
                            track.curStep++;


                            
                        }
                    }
                }

                Thread.Sleep(100);
            }


        }

        private void btOneByOne_Click(object sender, EventArgs e)
        {
            GenerateOneByOne();
        }

        private void btAllTogether_Click(object sender, EventArgs e)
        {
            GenerateAllTogether();
        }
    }

    public class ModemStep
    {
        public int Time;
        public ItemAction Act;
    }


    public class SymphonyItem : ModemStep
    {

        public List<int> ModemIndexes = new List<int>();

    }

    public enum ItemAction
    {
        WiatFirst,
        Dial,
        ResetPort
    }

    public class ModemTrack
    {
        public List<ModemStep> ModemSteps = new List<ModemStep>();
        public int ModemIndex;

        public int curStep = 0;
        public double startTime;
        public int prevTimes = 0;
        public bool firstRun = true;
    }
}
