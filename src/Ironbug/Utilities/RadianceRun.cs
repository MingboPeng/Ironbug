using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Ironbug.Utilities
{
    public static class RadianceRun
    {
        public static string RadRun()
        {
            // full path of python interpreter 
            string rad = @"C:\Radiance\bin\ra_tiff.exe";

            // app to call 
            string myPythonApp = @"C:\Users\Mingbo\Documents\GitHub\HoneybeeCSharp\doc\testFile\test.py";

            // dummy parameters to send
            var inHdr = @"C:\Users\Mingbo\Documents\GitHub\HoneybeeCSharp\doc\testFile\AcceleRad_test_IMG_Perspective_CPU@fc.HDR";
            var outTiff = inHdr.Remove(inHdr.Length - 4) + ".tiff";

            // Create new process start info 
            ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(rad);

            // make sure we can read the output from stdout 
            myProcessStartInfo.UseShellExecute = false;
            myProcessStartInfo.RedirectStandardOutput = true;
            
            myProcessStartInfo.Arguments = inHdr + " " + outTiff;

            Process myProcess = new Process();
            myProcess.StartInfo = myProcessStartInfo;
            
            // start the process 
            myProcess.Start();

            // Read the standard output of the app we called.  
            // in order to avoid deadlock we will read output first 
            // and then wait for process terminate: 
            StreamReader myStreamReader = myProcess.StandardOutput;
            string myString = myStreamReader.ReadLine();

            /*if you need to read multiple lines, you might use: 
                string myString = myStreamReader.ReadToEnd() */

            // wait exit signal from the app we called and then close it. 
            myProcess.WaitForExit();
            myProcess.Close();



            // write the output we got from python app 

            Console.WriteLine("Value received from script: " + myString);
            return myString;

        }
    }
}
