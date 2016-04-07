using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace SystemCompare.Classes
{
    class ScheduledTasks
    {
        private readonly string _binnPath = Application.StartupPath + "\\Binn";

        public void DumpScheduledTasks(string snapshotFolder)
        {
            var snapshotOutput = Path.Combine(Application.StartupPath, snapshotFolder);
            snapshotOutput = "\"" + snapshotOutput + "\\" + DateTime.UtcNow.Ticks + "-ScheduledTasks.txt\"";

            var batCommand = _binnPath + "\\DumpScheduledTasks.bat";

            var p = Process.Start(new ProcessStartInfo(batCommand, snapshotOutput));
            p?.WaitForExit();
        }
    }
}
