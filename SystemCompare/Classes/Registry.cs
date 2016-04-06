using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SystemCompare.Classes
{
    internal class Registry
    {
       private readonly string _binnPath = Application.StartupPath + "\\Binn";

        public void DumpRegistryHive(string hive, string snapshotFolder)
        {
            var psi = new ProcessStartInfo
            {
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = true,
                FileName = Path.Combine(_binnPath, "reg.exe"),
                Arguments =
                    " export " + hive + " " + Path.Combine(snapshotFolder, DateTime.UtcNow.Ticks + "-" + hive + ".txt")
            };
            //   psi.RedirectStandardError = true;
            //   psi.RedirectStandardOutput = true;

            using (var process = Process.Start(psi))
            {
                process?.WaitForExit();
            }

        }

        public static List<string> GetRegistryHives()
        {
            // [ HKLM | HKCU | HKCR | HKU | HKCC ]
            var registryHives = new List<string> {"HKLM", "HKCU", "HKCR", "HKU", "HKCC"};
            
            return registryHives.ToList();
        }

    }
}
