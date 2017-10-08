using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Ironbug
{
    public static class Config
    {
        //private static string radlibPath;

        public static string RadlibPath
        {
            get {
                var radlibPath = @"C:\Radiance\lib";
                return radlibPath;
            }
            //private set { radlibPath = value; }
        }

        //private static string radbinPath;

        public static string RadbinPath
        {
            get {
                var radbinPath = @"C:\Radiance\bin";

                return radbinPath;
            }
            //private set { radbinPath = value; }
        }

//        public static string GetRADPath()
//        {
//            string radPath = @"C:\Radiance\bin";
//            if (Directory.Exists(radPath))
//            {
//                return radPath;
//            }

//            //only for when "C:\Radiance\bin" doesn't exist
//            var pyRun = Rhino.Runtime.PythonScript.Create();
//            string pyScript = @"
//import scriptcontext as sc;
//RADPath = sc.sticky['honeybee_folders']['RADPath'];
//";

//            try
//            {
//                pyRun.ExecuteScript(pyScript);
//                radPath = pyRun.GetVariable("RADPath") as string;
//            }
//            catch (Exception)
//            {

//            }

//            return radPath;
//        }


    }
}
