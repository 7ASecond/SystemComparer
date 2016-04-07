using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace SystemCompare.Classes
{
    class Files
    {
        readonly string _path;
        private FileStream _fs;
        private StreamWriter _sw;

        public Files(string snapshotFolder)
        {
            _path = Path.Combine(snapshotFolder, DateTime.UtcNow.Ticks + "-files.txt");
        }

        public StreamWriter OpenFilesLog()
        {
            _fs = new FileStream(_path, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.ReadWrite);
            _sw = new StreamWriter(_fs);
            return _sw;
        }

        public void CloseFileLog()
        {
            _sw.Close();
            _fs.Close();
        }

        public void DumpFiles(StreamWriter sw, string file)
        {
            sw.WriteLine(file);
            sw.FlushAsync();
        }


        private string _binnPath = Application.StartupPath + "\\Binn";

        public void DumpFileSystem(string snapshotFolder)
        {
            var snapshotOutput = Path.Combine(Application.StartupPath, snapshotFolder);
            snapshotOutput = "\"" + snapshotOutput + "\\" + DateTime.UtcNow.Ticks + "-files.txt\"";
           
            var batCommand = _binnPath + "\\DumpFileSystem.bat";

            var p = Process.Start(new ProcessStartInfo(batCommand, snapshotOutput));
            p?.WaitForExit();           
        }
    }
}
