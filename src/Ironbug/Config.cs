using System;
using System.Collections.Generic;
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

        
    }
}
