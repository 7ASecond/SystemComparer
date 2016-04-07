using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace SystemCompare.Classes
{
    internal class IpConfig
    {

        private readonly string _binnPath = Application.StartupPath + "\\Binn";

        public void DumpIpConfig(string snapshotFolder)
        {
            var snapshotOutput = Path.Combine(Application.StartupPath, snapshotFolder);
            snapshotOutput = "\"" + snapshotOutput + "\\" + DateTime.UtcNow.Ticks + "-IP.txt\"";

            var batCommand = _binnPath + "\\DumpIPConfig.bat";

            var p = Process.Start(new ProcessStartInfo(batCommand, snapshotOutput));
            p?.WaitForExit();
        }

    }
}
