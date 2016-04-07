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
using Environment = SystemCompare.Classes.Environment;

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
            Application.DoEvents();
            ScanFiles();

            // Dump Registry 
            Application.DoEvents();
            DumpRegistry();

            // Dump Tasks
            lblStatus.Text = @"Generating Tasks Snapshot";
            Application.DoEvents();
            DumpTasks();

            // Dump Environment Variables
            lblStatus.Text = @"Generating Environment Variables Snapshot";
            Application.DoEvents();
            DumpEnvironment();

            // Dump IP Config
            lblStatus.Text = @"Generating IP Configuration Snapshot";
            Application.DoEvents();
            DumpIpConfig();

            // Dump NetStats
            lblStatus.Text = @"Generating Network Snapshot";
            Application.DoEvents();
            DumpNetStats();

            // Dump Drivers
            lblStatus.Text = @"Generating Drivers Snapshot";
            Application.DoEvents();
            DumpDrivers();

            // Dump Scheduled Tasks
            lblStatus.Text = @"Generating Scheduled Tasks Snapshot";
            Application.DoEvents();
            DumpScheduledTasks();

            // Dump Startup Applications
            lblStatus.Text = @"Generating Startup Applications Snapshot";
            Application.DoEvents();
            DumpStartupApplications();

            Application.DoEvents();
            lblStatus.Text = @"Done";
            ToggleButtons();
        }

        private void DumpStartupApplications()
        {
            StartupApplications sa = new StartupApplications();
            sa.DumpStartupApplications(SnapshotName);
        }

        private void DumpScheduledTasks()
        {
            ScheduledTasks st = new ScheduledTasks();
            st.DumpScheduledTasks(SnapshotName);
        }

        private void DumpDrivers()
        {
            Drivers d = new Drivers();
            d.DumpDrivers(SnapshotName);
        }

        private void DumpNetStats()
        {
            NetStat ns = new NetStat();
            ns.DumpNetStat(SnapshotName);
        }

        private void DumpIpConfig()
        {
            IpConfig ip = new IpConfig();
            ip.DumpIpConfig(SnapshotName);
        }

        private void DumpEnvironment()
        {
            Environment env = new Environment();
            env.DumpEnvironment(SnapshotName);
        }

        private void DumpTasks()
        {
            Tasks tasks = new Tasks();
            tasks.DumpTasks(SnapshotName);
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
                Application.DoEvents();
                lblStatus.Text = @"Generating Hive: " + hive + @" Snapshot";
                r.DumpRegistryHive(hive, SnapshotName);
            }
        }

        private void ScanFiles()
        {
            // Scan C:\ 
            lblStatus.Text = @"Generating File System Snapshot Please Wait.";

            var files = new Files(SnapshotName);
            files.DumpFileSystem(SnapshotName);
        }

        private void FileScanAttributes(List<string> fileList, TextWriter sw)
        {
            if (fileList == null) throw new ArgumentNullException(nameof(fileList));

            foreach (var file in fileList)
            {
                Progress.Value++;
                try
                {
                    var finfo = new FileInfo(file);
                    var timeStamp = finfo.CreationTimeUtc + " " + finfo.LastAccessTimeUtc + " " + finfo.LastWriteTimeUtc;
                    var size = finfo.Length;
                    sw.WriteLine("[" + timeStamp + "]\t [" + size + "]\t" + file);
                }
                catch (Exception)
                {
                    //TODO: Requires an Error log so we still know what files exist through diffing the error logs
                }


                lblStatus.Text = @"Processing File " + Progress.Value.ToString("N0");
                Application.DoEvents();
            }
        }

        private void FileScanHash(List<string> fileList, TextWriter sw)
        {
            if (fileList == null) throw new ArgumentNullException(nameof(fileList));
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

            var oldText = LoadSnapshotFile();
            var newText = LoadSnapshotFile();

            if (_isRegistry)
                RegistryDiff(oldText, newText);
            else
                SnapshotDiff(oldText, newText);

            ToggleButtons();
        }

        private void SnapshotDiff(string oldText, string newText)
        {
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
            d = null;
        }

        private void RegistryDiff(string oldText, string newText)
        {
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
            d = null;
        }

        string _previousFilesDirectory = "";
        string _previousTaskImageName = "";
        private void UpdateDiffFile(StreamWriter sw, DiffPiece line, DiffPaneModel result, int linecount)
        {
            int curPosition;
            if (line.Position == null)
                curPosition = linecount;
            else
                curPosition = (int)line.Position;

            var baseLine = result.Lines.ElementAt((int)curPosition);

            if (_isRegistry)
            {
                while (!baseLine.Text.Contains("[HKEY_")) // Roll back to find which Key this line is associated with.
                {
                    curPosition--;
                    baseLine = result.Lines.ElementAt((int)curPosition);
                }
                sw.WriteLine("[" + baseLine.Type + "]\t [" + baseLine.Position + "]\t" + baseLine.Text);
            }
            else if (_isFiles)
            {

                while (!baseLine.Text.ToLower().Contains(" directory of ")) // Roll back to find which Key this line is associated with.
                {
                    curPosition--;
                    baseLine = result.Lines.ElementAt((int)curPosition);
                }
                if (_previousFilesDirectory != baseLine.Text)
                {
                    sw.WriteLine("[" + baseLine.Type + "]\t [" + baseLine.Position + "]\t" + baseLine.Text);
                    _previousFilesDirectory = baseLine.Text;
                }
            }
            else if (_isTasks)
            {

                while (!baseLine.Text.ToLower().Contains("image name: ")) // Roll back to find which Key this line is associated with.
                {
                    curPosition--;
                    baseLine = result.Lines.ElementAt((int)curPosition);
                }
                if (_previousTaskImageName != baseLine.Text)
                {
                    sw.WriteLine("[" + baseLine.Type + "]\t [" + baseLine.Position + "]\t" + baseLine.Text);
                    _previousTaskImageName = baseLine.Text;
                }
            }

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

        bool _isRegistry;
        bool _isFiles;
        bool _isTasks;
        private string LoadSnapshotFile()
        {
            _isRegistry = false;
            _isFiles = false;
            _isTasks = false;

            var contents = "";

            var dr = OFileD.ShowDialog();
            if (dr != DialogResult.OK) return contents;

            contents = File.ReadAllText(OFileD.FileName);
            _baseFileName = OFileD.FileName;
            if (_baseFileName.ToLower().Contains("-hk")) _isRegistry = true;
            if (_baseFileName.ToLower().Contains("-files.txt")) _isFiles = true;
            if (_baseFileName.ToLower().Contains("-tasks.txt")) _isTasks = true;

            return contents;
        }
    }
}
