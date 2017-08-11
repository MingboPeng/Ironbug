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

            Process cmd = new Process()
            {
                StartInfo = new ProcessStartInfo("cmd")
                {
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput =true,
                    UseShellExecute = false

                }
            };

            cmd.Start();

            using (StreamWriter sw = cmd.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine(cmdString);
                    //sw.WriteLine(@"EXIT");

                }
            }

            cmd.WaitForExit();

            while (!cmd.HasExited)
            {
                int milliseconds = 10;
                Thread.Sleep(milliseconds);
            }
            


            //cmd.Start();
            //StreamWriter sw = cmd.StandardInput;
            //if (sw.BaseStream.CanWrite)
            //{
            //    sw.WriteLine(cmdString);
            //}

            //cmd.WaitForExit(20);
            

            //var a = cmd.TotalProcessorTime;
            //while (!(a.TotalMilliseconds>0))
            //{
            //    int milliseconds = 10;
            //    Thread.Sleep(milliseconds);
            //    System.Windows.Forms.MessageBox.Show(a.ToString());
            //}
            //int milliseconds = 1000;
            //Thread.Sleep(milliseconds);

            //Boolean breakFlag = true;
            //while (breakFlag)
            //{
            //    cmd.Dispose();
            //    breakFlag = !cmd.HasExited;
            //    milliseconds = 1000;
            //    Thread.Sleep(milliseconds);
            //}

            cmd.Close();
            return true;

            
        }

        //convert the hdr to tiff
        public static List<string> HDR2TIF(List<string> HDRs, List<string> TargetTIF, string RADPath)
        {
            if (HDRs.IsNullOrEmpty()) return HDRs;

            var cmdStrings = new List<string>();
            var setEnv = string.Format("SET RAYPATH=.;{1}&PATH={0};$PATH", RADPath, RADPath.Replace("bin", "lib"));
            cmdStrings.Add(setEnv);


            for (int i = 0; i < HDRs.Count; i++)
            {
                var filePath = HDRs[i];
                var tiffFile = TargetTIF[i];
                string cmdStr1 = @"ra_tiff " + filePath + " " + tiffFile;
                cmdStrings.Add(setEnv);
            }

            try
            {
                CMD.Execute(cmdStrings);
            }
            catch (Exception)
            {
                throw;
            }

            return TargetTIF;

            //foreach (var item in TargetTIF)
            //{
            //    if (File.Exists(item))
            //    {

            //    }
                
            //}

            
            //var cmdStrings = new List<string>();
            
            //cmdStrings.Add(setEnv);
            //cmdStrings.Add(cmdStr1);
            //CMD.Execute(cmdStrings);
        }
    }
}
