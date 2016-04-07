using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace SystemCompare.Classes
{
    class Drivers
    {
        private readonly string _binnPath = Application.StartupPath + "\\Binn";

        public void DumpDrivers(string snapshotFolder)
        {
            var snapshotOutput = Path.Combine(Application.StartupPath, snapshotFolder);
            snapshotOutput = "\"" + snapshotOutput + "\\" + DateTime.UtcNow.Ticks + "-Drivers.txt\"";

            var batCommand = _binnPath + "\\DumpDrivers.bat";

            var p = Process.Start(new ProcessStartInfo(batCommand, snapshotOutput));
            p?.WaitForExit();
        }

    }
}
