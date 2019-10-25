using System;
using System.Diagnostics;
using Ironbug.Honeybee.Radiance.Command;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Ironbug.Core.Tests
{
    public class PValueTest
    {
        ITestOutputHelper _output;
        public PValueTest(ITestOutputHelper output)
        {
            this._output = output;
        }

        [Fact]
        public void PValueTest_Exe()
        {
            Process cmd = new Process()
            {

                StartInfo = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = "/C pvalue -o -d -h -b Room.HDR",
                    WorkingDirectory = @"C:\Users\mingo\OneDrive\Desktop\",
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                }

            };

            cmd.StartInfo.EnvironmentVariables["PATH"] += String.Format(";{0}", RadianceBaseCommand.normspace(Config.RadbinPath));
            cmd.StartInfo.EnvironmentVariables["RAYPATH"] += String.Format(";{0}", RadianceBaseCommand.normspace(Config.RadlibPath));

            cmd.Start();
            //cmd.BeginOutputReadLine();
            string outputs = cmd.StandardOutput.ReadToEnd();
            string err = cmd.StandardError.ReadToEnd();

            //Console.WriteLine(outputs);
            
            Console.WriteLine(err);

            //TestContext.WriteLine(outputs);
            //TestContext.WriteLine("Error: "+ err);

            cmd.WaitForExit();

            //while (!cmd.HasExited)
            //{
            //    int milliseconds = 25;
            //    Thread.Sleep(milliseconds);
            //}

            cmd.Close();

            var lines = outputs.Split(Environment.NewLine.ToCharArray());
            Console.WriteLine("Total length: "+lines.Length);
            Console.WriteLine(lines[0]);
            Assert.True(lines.Length > 0);
        }

        [Fact]
        public void PValueLegacy_Test()
        {
            var hdrFile = @"C:\Users\mingo\OneDrive\Desktop\Room.HDR";
            var pvalue = new PValue_Legacy(hdrFile);

            var cmdString = pvalue.ToString();
            //Console.WriteLine("cmdString: " + cmdString);

            var output = pvalue.Execute().ToArray();
            var counts = output.Length;

            Console.WriteLine("TotalLineCount: " + counts);
            Console.WriteLine("X: " + pvalue.X);
            Console.WriteLine("Y: " + pvalue.Y);

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("output: " + output[i]);
            }
            Console.WriteLine("....... ");
            for (int i = counts-5; i < counts; i++)
            {
                Console.WriteLine("output: " + output[i]);
            }


            Assert.True(counts > 0);
        }
        
    }
}
