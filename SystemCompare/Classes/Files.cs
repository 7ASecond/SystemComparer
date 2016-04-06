using System;
using System.IO;

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
            _fs = new FileStream(_path,FileMode.CreateNew,FileAccess.ReadWrite,FileShare.ReadWrite);
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

    }
}
