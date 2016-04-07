using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystemCompare.Classes
{
    class Environment
    {
        private string _binnPath = Application.StartupPath + "\\Binn";

        public void DumpEnvironment(string snapshotFolder)
        {
            var snapshotOutput = Path.Combine(Application.StartupPath, snapshotFolder);
            snapshotOutput = "\"" + snapshotOutput + "\\" + DateTime.UtcNow.Ticks + "-Environment.txt\"";

            var batCommand = _binnPath + "\\DumpEnvironmentVariables.bat";

            var p = Process.Start(new ProcessStartInfo(batCommand, snapshotOutput));
            p?.WaitForExit();
        }
    }
}
