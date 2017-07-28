using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Ironbug
{
    public static class CMD
    {
        public static bool Execute(List<string> command)
        {
            var cmdString = string.Join("&", command);

            Process cmd = new Process();
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();
            using (StreamWriter sw = cmd.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine(cmdString);
                    
                }
            }
            
            cmd.WaitForExit();

            while (!cmd.HasExited)
            {
                int milliseconds = 50;
                Thread.Sleep(milliseconds);
            }
            
            return true;

            
        }

        //convert the hdr to tiff
        public static void HDR2TIF(List<string> ra_tiffs)
        {
            var cmdStrings = new List<string>();
            cmdStrings.Add(@"SET RAYPATH=.;C:\Radiance\lib&PATH=C:\Radiance\bin;$PATH");
            cmdStrings.AddRange(ra_tiffs);
            CMD.Execute(cmdStrings);
        }
    }
}
