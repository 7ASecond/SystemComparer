using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SystemCompare.Classes;
using SystemCompare.GUI;
using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;
using HashLib;

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

            // Get Snapshot Name where we shall save the log files.
            var getUserInput = new FrmGetUserInput();
            var dr = getUserInput.ShowDialog();
            if (dr == DialogResult.Cancel) return;
            SnapshotName = getUserInput.SnapshotName.Replace(" ", "");

            // Create the Snapshot Directory if it does not exist
            if (!Directory.Exists(SnapshotName)) Directory.CreateDirectory(SnapshotName);

            ScanFiles();

            // Scan Registry 
            DumpRegistry();
            lblStatus.Text = @"Done";

            ToggleButtons();
        }

        private void ToggleButtons()
        {
            btnCompareSnapshots.Enabled = !btnCompareSnapshots.Enabled;
            btnSnapshot.Enabled = !btnSnapshot.Enabled;
        }


        private void DumpRegistry()
        {
            var r = new Registry();
            foreach (var hive in Registry.GetRegistryHives())
            {
                lblStatus.Text = @"Dumping Hive: " + hive;
                r.DumpRegistryHive(hive, SnapshotName);
            }
        }

        private void ScanFiles()
        {
            // Scan C:\ 
            lblStatus.Text = @"Initial Scan Please Wait.";
            var fileList = FileScan.RecursiveScan2("C:\\");
            lblFiles.Text = fileList.Count.ToString("N0");
            Progress.Maximum = fileList.Count;

            //var finfo = new FileInfo(_baseFileName);
            //if (string.IsNullOrEmpty(SnapshotName)) GetSnapshotName(finfo); // Can happen when starting program and doing Diff without doing Snapshot first
            var files = new Files(SnapshotName);
            StreamWriter sw = files.OpenFilesLog();

            // Full Scan
            //  FileScanHash(fileList, sw);

            // Quick File Scan
            FileScanAttributes(fileList, sw);

            files.CloseFileLog();
        }

        private void FileScanAttributes(List<string> fileList, StreamWriter sw)
        {
            foreach (var file in fileList)
            {
                Progress.Value++;

                string TimeStamp = string.Empty;
                long size = 0;

                FileInfo finfo = new FileInfo(file);
                TimeStamp = finfo.CreationTimeUtc + " " + finfo.LastAccessTimeUtc + " " + finfo.LastWriteTimeUtc;
                size = finfo.Length;

                lblStatus.Text = @"Processing File " + Progress.Value.ToString("N0");
                sw.WriteLine("[" + TimeStamp + "]\t [" + size + "]\t" + file);
                Application.DoEvents();
            }
        }

        private void FileScanHash(List<string> fileList, StreamWriter sw)
        {
            foreach (var file in fileList)
            {
                Progress.Value++;

                string hashResult;
                try
                {
                    IHash hash = HashFactory.Crypto.CreateMD5();
                    hashResult = hash.ComputeFile(file).ToString();
                }
                catch (Exception ex)
                {
                    hashResult = "[" + ex.Message + "]";
                }

                lblStatus.Text = @"Processing File " + Progress.Value.ToString("N0");

                sw.WriteLine("[" + hashResult + "]\t" + file);
                Application.DoEvents();
            }
        }

        private void btnCompareSnapshots_Click(object sender, EventArgs e)
        {
            ToggleButtons();

            // Load OldText as string (Base Registry)
            // Load NewText as string (Changed Registry entry)

            var oldText = LoadRegistryDump();
            var newText = LoadRegistryDump();

            var d = new Differ();
            var inlineBuilder = new InlineDiffBuilder(d);
            var result = inlineBuilder.BuildDiffModel(oldText, newText);

            var lineCounter = 1;

            Progress.Maximum = result.Lines.Count;
            var sw = OpenDiffFile();

            foreach (var line in result.Lines)
            {
                Progress.Value = lineCounter;
                switch (line.Type)
                {
                    case ChangeType.Inserted:
                        lblStatus.Text = @"Inserted " + lineCounter.ToString("N0");
                        UpdateDiffFile(sw, line, result, lineCounter);
                        break;
                    case ChangeType.Deleted:
                        lblStatus.Text = @"Deleted " + lineCounter.ToString("N0");
                        UpdateDiffFile(sw, line, result, lineCounter);
                        break;
                    case ChangeType.Imaginary:
                        lblStatus.Text = @"Imaginary " + lineCounter.ToString("N0");
                        UpdateDiffFile(sw, line, result, lineCounter);
                        break;
                    case ChangeType.Modified:
                        lblStatus.Text = @"Modified " + lineCounter.ToString("N0");
                        UpdateDiffFile(sw, line, result, lineCounter);
                        break;
                    case ChangeType.Unchanged:
                        lblStatus.Text = @"Unchanged " + lineCounter.ToString("N0");
                        //if (line.Text.Contains("[HKEY_"))
                        //    UpdateDiffFile(sw, line);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }


                //Console.WriteLine(line.Text);
                Application.DoEvents();
                lineCounter++;
            }
            Progress.Value = 0;
            CloseDiffFile(sw);
            ToggleButtons();
        }

        private static void UpdateDiffFile(StreamWriter sw, DiffPiece line, DiffPaneModel result, int linecount)
        {
            int curPosition;
            if (line.Position == null)
                curPosition = linecount;
            else
                curPosition = (int)line.Position;

            var baseLine = result.Lines.ElementAt((int)curPosition);


            while (!baseLine.Text.Contains("[HKEY_")) // Roll back to find which Key this line is associated with.
            {
                curPosition--;
                baseLine = result.Lines.ElementAt((int)curPosition);
            }


            sw.WriteLine("[" + baseLine.Type + "]\t [" + baseLine.Position + "]\t" + baseLine.Text);
            sw.WriteLine("[" + line.Type + "]\t [" + line.Position + "]\t" + line.Text);
            sw.Flush();
        }

        FileStream _difFileStream;

        private StreamWriter OpenDiffFile()
        {
            var finfo = new FileInfo(_baseFileName);
            var ext = finfo.Extension;
            var name = DateTime.UtcNow.Ticks + "-" + finfo.Name.Replace(ext, "").Replace(" ", "").Replace(".", "") + "-Dif.txt"; // Name of the Diff File

            if (string.IsNullOrEmpty(SnapshotName)) GetSnapshotName(finfo); // Can happen when starting program and doing Diff without doing Snapshot first
            _difFileStream = new FileStream(Path.Combine(SnapshotName, name), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            var sw = new StreamWriter(_difFileStream);
            return sw;
        }

        private void GetSnapshotName(FileInfo name)
        {
            SnapshotName = Path.GetDirectoryName(name.FullName);
        }

        private void CloseDiffFile(StreamWriter sw)
        {
            sw.Close();
            _difFileStream.Close();
        }

        private string _baseFileName = "";

        private string LoadRegistryDump()
        {
            var contents = "";

            var dr = OFileD.ShowDialog();
            if (dr == DialogResult.OK)
            {
                contents = File.ReadAllText(OFileD.FileName);
                _baseFileName = OFileD.FileName;
            }

            return contents;
        }
    }
}
