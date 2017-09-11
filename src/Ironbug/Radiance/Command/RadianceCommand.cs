using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;


namespace Ironbug.Radiance.Command
{
    public abstract class RadianceCommand
    {
        private static string radlibPath;

        public static string RadlibPath
        {
            get { return radlibPath; }
            private set { radlibPath = value; }
        }

        private static string radbinPath;

        public static string RadbinPath
        {
            get { return radbinPath; }
            private set { radbinPath = value; }
        }

        private string exeName;

        public RadianceCommand(string executableName)
        {
            exeName = executableName;
            RadlibPath = Config.RadlibPath;
            RadbinPath = Config.RadbinPath;
           
        }

        private bool checkExecutable(string radbinPath, bool raiseException = false)
        {
            throw new NotImplementedException();
        }


        public abstract string ToRadString(bool relativePath = false);

        public bool Execute()
        {
            //checkFiles();

            //var cmdString = string.Join("&", command);

            Process cmd = new Process()
            {
                StartInfo = new ProcessStartInfo("cmd.exe")
                {
                    Arguments = "/C " + this.ToRadString(),
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput =true,
                    UseShellExecute = false
                }
                
            };

            if (isNTSystem())
            {
                cmd.StartInfo.EnvironmentVariables["PATH"] += String.Format(";{0}", normspace(Config.RadbinPath));
                cmd.StartInfo.EnvironmentVariables["RAYPATH"] += String.Format(";{0}", normspace(Config.RadlibPath));
            }

            cmd.Start();
            cmd.StandardOutput.ReadLine();

            //using (StreamWriter sw = cmd.StandardInput)
            //{
            //    if (sw.BaseStream.CanWrite)
            //    {
            //        sw.WriteLine(cmdString);
                    
            //    }
            //}
            
            cmd.WaitForExit();

            //while (!cmd.HasExited)
            //{
            //    int milliseconds = 25;
            //    Thread.Sleep(milliseconds);
            //}

            cmd.Close();
            
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
