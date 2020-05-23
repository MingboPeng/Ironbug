using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Ironbug.LBHB_Legacy
{
    public abstract class CommandExecuteBase
    {
        protected virtual string RadString { get; set; }
        protected virtual string RadBinPath { get; set; }
        protected virtual string RadLibPath { get; set; }

        public virtual string Execute()
        {
            var radString = this.RadString;
            int sp = radString.IndexOf(' ');
            var exeName = radString.Substring(0, sp).Trim();
            var arguments = radString.Substring(sp).Trim();

            Process cmd = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = exeName,
                    Arguments = arguments,
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                }

            };

            if (IsNTSystem())
            {
                cmd.StartInfo.EnvironmentVariables["PATH"] += String.Format(";{0}", this.RadBinPath);
                cmd.StartInfo.EnvironmentVariables["RAYPATH"] += String.Format(";{0}", this.RadLibPath);
            }

            cmd.Start();

            string outputs = cmd.StandardOutput.ReadLine();
            string err = cmd.StandardError.ReadToEnd();

            cmd.WaitForExit();
            cmd.Close();

            if (!string.IsNullOrEmpty(err))
            {
                throw new Exception(err);
            }
            
            return outputs;
        }

        private static bool IsNTSystem()
        {
            return Environment.OSVersion.ToString().ToUpper().Contains("NT");
        }
    }
}
