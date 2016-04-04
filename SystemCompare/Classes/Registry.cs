using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;

namespace SystemCompare.Classes
{
    internal class Registry
    {
        private readonly string _exeFolder = Application.StartupPath;
        private readonly string _exePath = Application.StartupPath + "\\reg.exe";
        private readonly string _binnPath = Application.StartupPath + "\\Binn";

        public void DumpRegistryHive(string hive, string snapshotFolder)
        {
            var p = Path.Combine(_exeFolder, snapshotFolder);

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.UseShellExecute = true;
            psi.FileName = Path.Combine(_binnPath, "reg.exe");
            //   psi.RedirectStandardError = true;
            //   psi.RedirectStandardOutput = true;
            psi.Arguments = " export " + hive + " " + Path.Combine(snapshotFolder, DateTime.UtcNow.Ticks + "-" + hive + ".txt");

            using (Process process = Process.Start(psi))
            {
                process.WaitForExit();
            }

        }

        public static List<string> GetRegistryHives()
        {
            List<string> registryHives = new List<string>();

            // [ HKLM | HKCU | HKCR | HKU | HKCC ]
            registryHives.Add("HKLM");
            registryHives.Add("HKCU");
            registryHives.Add("HKCR");
            registryHives.Add("HKU");
            registryHives.Add("HKCC");

            return registryHives.ToList();
        }

    }
}
