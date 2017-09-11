using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;


namespace Ironbug.Radiance.Command
{
    public abstract class RadianceBaseCommand
    {
        private static string radlibPath;


        protected static string RadlibPath
        {
            get { return radlibPath; }
            private set { radlibPath = value; }
        }

        private static string radbinPath;

        protected static string RadbinPath
        {
            get { return radbinPath; }
            private set { radbinPath = value; }
        }

        private string exeName;

        protected RadianceBaseCommand(string executableName)
        {
            exeName = executableName;
            RadlibPath = Config.RadlibPath;
            RadbinPath = Config.RadbinPath;
           
        }

        private bool checkExecutable(string radbinPath, bool raiseException = false)
        {
            throw new NotImplementedException();
        }


        protected abstract string ToRadString(bool relativePath = false);

        public bool Execute()
        {
            Process cmd = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = "/C " + this.ToRadString(),
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput =true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                }
                
            };

            if (isNTSystem())
            {
                cmd.StartInfo.EnvironmentVariables["PATH"] += String.Format(";{0}", normspace(Config.RadbinPath));
                cmd.StartInfo.EnvironmentVariables["RAYPATH"] += String.Format(";{0}", normspace(Config.RadlibPath));
            }

            cmd.Start();
            //cmd.BeginOutputReadLine();
            string outputs = cmd.StandardOutput.ReadLine();
            string err = cmd.StandardError.ReadToEnd();
            Console.WriteLine(outputs);

            cmd.WaitForExit();

            //while (!cmd.HasExited)
            //{
            //    int milliseconds = 25;
            //    Thread.Sleep(milliseconds);
            //}

            cmd.Close();
            Console.WriteLine("\n\nPress any key to exit.");
            Console.ReadLine();
            return true;

            
        }

        private static void checkFiles()
        {
            throw new NotImplementedException();
        }

        public static string normspace(string path)
        {
            //Norm white spaces in path.
            //Return path with quotation marks if there is whitespace in path.
            //https://github.com/ladybug-tools/honeybee/blob/master/honeybee/radiance/command/_commandbase.py#L71

            if (path.Trim().Contains(" "))
            {
                string wrapper = isNTSystem()? "\"":"'";
                return String.Format("{0}{1}{0}", wrapper, path);
            }
            else
            {
                return path;
            }
            
        }

        private static bool isNTSystem()
        {
            return Environment.OSVersion.ToString().ToUpper().Contains("NT");
        }
        
    }
}
