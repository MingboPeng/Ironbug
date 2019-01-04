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
        public static bool LoadAssemblies(string Version = "2.7.0.0")
        {
            Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
            var possibleOpsDll = asms.Where(_ => _.GetName().Name.ToUpper() == "OPENSTUDIO");
            var isLoaded = false;

            if (!possibleOpsDll.Any())
            {
                var possiblePath = new List<string>();
                possiblePath.Add(@"C:\Ironbug"); 
                possiblePath.Add(@"C:\openstudio-2.7.0\CSharp\openstudio");
                possiblePath.Add(@"C:\openstudio-2.5.0\CSharp\openstudio");

                var file = "OpenStudio.dll";

                var path = possiblePath.FirstOrDefault(_ => File.Exists(Path.Combine(_, file)));

                

                if (string.IsNullOrEmpty(path))
                {
                    throw new FileNotFoundException(string.Format("Cannot find {0} in {1}", file, @"C:\Ironbug"));
                }

                var asmFile = Path.Combine(path, file);
                var versionFound = AssemblyName.GetAssemblyName(asmFile).Version.ToString();
                if (versionFound == Version)
                {
                    Assembly.LoadFile(asmFile);
                    isLoaded = true;
                }
                else
                {
                    try
                    {
                        Assembly.LoadFile(asmFile);
                        isLoaded = true;
                    }
                    catch (Exception)
                    {

                        throw new FileNotFoundException(string.Format("Cannot find {0} ({1}) in {2}", file, Version), path);
                    }
                    
                }

            }

            return isLoaded;

        }

    }
}
