using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ironbug.Core.OpenStudio
{
    public static class OpenStudioHelper
    {
        public static bool LoadAssemblies(string version = "2.7.0.0")
        {
            Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
            var possibleOpsDll = asms.Where(_ => _.GetName().Name.ToUpper() == "OPENSTUDIO");
            var isLoaded = false;

            if (!possibleOpsDll.Any())
            {
                var path = @"C:\openstudio-2.7.0\CSharp\openstudio";
                var file = "OpenStudio.dll";
                var asmFile = Path.Combine(path, file);
                //version = "2.7.0.0";

                if (AssemblyName.GetAssemblyName(asmFile).Version.ToString() == version)
                {
                    Assembly.LoadFile(asmFile);
                    isLoaded = true;
                }
                else
                {
                    throw new FileNotFoundException(string.Format("Cannot find {0} ({1})",file, version));
                }

            }

            return isLoaded;

        }

    }
}
