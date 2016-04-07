using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace SystemCompare.Classes
{
    class StartupApplications
    {
        private readonly string _binnPath = Application.StartupPath + "\\Binn";

        public void DumpStartupApplications(string snapshotFolder)
        {
            var snapshotOutput = Path.Combine(Application.StartupPath, snapshotFolder);
            snapshotOutput = "\"" + snapshotOutput + "\\" + DateTime.UtcNow.Ticks + "-StartUp.txt\"";

            var batCommand = _binnPath + "\\DumpStartupApplications.bat";

            var p = Process.Start(new ProcessStartInfo(batCommand, snapshotOutput));
            p?.WaitForExit();
        }
    }
}
