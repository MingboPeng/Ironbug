using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Ironbug.Core.Honeybee.Radiance.Command
{
    public abstract class CommandBase
    {
        public dynamic RawObj;

        public string RadbinPath
        {
            get { return this.RawObj.radbinPath; }
            set { this.RawObj.radbinPath = value; }
        }

        public string RadlibPath
        {
            get { return this.RawObj.radlibPath; }
            set { this.RawObj.radlibPath = value; }
        }

        //public virtual string Execute()
        //{
        //    this.RawObj.execute();
        //    return "";
        //}

        public virtual string Execute(bool RunFromPython=true)
        {
            if (RunFromPython)
            {
                this.RawObj.execute();
            }
            else
            {
                string radString = this.ToRadString();
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
                    cmd.StartInfo.EnvironmentVariables["PATH"] += String.Format(";{0}", this.RawObj.normspace(this.RadbinPath));
                    cmd.StartInfo.EnvironmentVariables["RAYPATH"] += String.Format(";{0}", this.RawObj.normspace(this.RadlibPath));
                }

                cmd.Start();

                string outputs = cmd.StandardOutput.ReadLine();
                string err = cmd.StandardError.ReadToEnd();
                Console.WriteLine(outputs);
                Console.WriteLine(err);

                cmd.WaitForExit();
                

                cmd.Close();
            }

            Console.WriteLine(ToRadString());
            return ToRadString();
        }

        public virtual string ToRadString(bool relativePath = false)
        {
            return RawObj.toRadString(relativePath);
        }

        public override string ToString()
        {
            return ToRadString();
        }

        private static bool IsNTSystem()
        {
            return Environment.OSVersion.ToString().ToUpper().Contains("NT");
        }
    }
}
