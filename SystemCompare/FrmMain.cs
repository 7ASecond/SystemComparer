using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemCompare.Classes;
using SystemCompare.GUI;

namespace SystemCompare
{
    public partial class FrmMain : Form
    {

        private string SnapshotName { get; set; }

        public FrmMain()
        {
            InitializeComponent();
        }

        private void btnSnapshot_Click(object sender, EventArgs e)
        {
            ToggleButtons();
           
            // Get Snapshot Name
            FrmGetUserInput getUserInput = new FrmGetUserInput();
            DialogResult dr = getUserInput.ShowDialog();
            if (dr == DialogResult.Cancel) return;
            SnapshotName = getUserInput.SnapshotName;

            // Create the Snapshot Directory if it does not exist
            if (!Directory.Exists(SnapshotName)) Directory.CreateDirectory(SnapshotName.Replace(" ", ""));

            //   ScanFiles();

            // Scan Registry -> Log all keys and values to somewhere :)
            DumpRegistry();
            lblStatus.Text = @"Done";

            ToggleButtons();
        }

        private void ToggleButtons()
        {
            btnCompareSnapshots.Enabled = !btnCompareSnapshots.Enabled;
            btnSnapshot.Enabled = !btnSnapshot.Enabled;
        }


        private Task DumpRegistry()
        {
            Registry r = new Registry();
            foreach (var hive in Registry.GetRegistryHives())
            {
                lblStatus.Text = @"Dumping Hive: " + hive;
                r.DumpRegistryHive(hive, SnapshotName.Replace(" ", ""));
            }

            return null;
        }

        private void ScanFiles()
        {
            // Scan C:\ -> Log all files to somewhere :)
            lblStatus.Text = @"Initial Scan Please Wait.";
            List<string> fileList = FileScan.RecursiveScan2("C:\\");
            lblFiles.Text = fileList.Count.ToString("N0");
            Progress.Maximum = fileList.Count;

            // Full Scan
            foreach (var file in fileList)
            {
                Progress.Value++;
                lblStatus.Text = @"Processing File " + Progress.Value;

                Application.DoEvents();
            }
        }

        private void btnCompareSnapshots_Click(object sender, EventArgs e)
        {
            ToggleButtons();

            // Load File 1
            // Load File 2
            // Compare

            ToggleButtons();
        }
    }
}
