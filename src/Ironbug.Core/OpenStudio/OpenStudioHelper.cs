using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Ironbug.Core.OpenStudio
{
    public static class OpenStudioHelper
    {
        //public static string SupportedVersion { get; set; } = "2.8.1";

        //public static bool LoadAssemblies(Action<string> MessageLogger)
        //{
        //    return LoadAssemblies(MessageLogger, SupportedVersion);
        //}

        public static bool LoadAssemblies(Action<string> MessageLogger)
        {
            Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
            var possibleOpsDll = asms.Where(_ => _.GetName().Name.ToUpper() == "OPENSTUDIO");
            var isLoaded = false;

            if (!possibleOpsDll.Any())
            {
                var possiblePath = new List<string>();
                possiblePath.Add(@"C:\Ironbug");
                //possiblePath.Add(@"C:\openstudio-2.7.0\CSharp\openstudio");
                //possiblePath.Add($@"C:\openstudio-{Version}\CSharp\openstudio");
                var opsPaths = Directory.GetDirectories(@"C:\")
                    .Where(s => s.ToLower().StartsWith("c:\\openstudio"))
                    .Select(_=>_+ @"\CSharp\openstudio").ToList();
                opsPaths.Sort();
                opsPaths.Reverse();
                possiblePath.AddRange(opsPaths);

                var file = "OpenStudio.dll";

                var path = possiblePath.FirstOrDefault(_ => File.Exists(Path.Combine(_, file)));
                if (string.IsNullOrEmpty(path))
                {
                    throw new FileNotFoundException($"Cannot find OpenStudio 2.8 or newer version installed!\n\nIronbug works with this specific version of OpenStudio.");
                }

                var asmFile = Path.Combine(path, file);
                var versionFound = CheckOpsVersionIfValid(asmFile);

                try
                {
                    Assembly.LoadFile(asmFile);
                    isLoaded = true;
                    MessageLogger($"OpenStudio library {versionFound} loaded from {path}");
                }
                catch (Exception)
                {
                    throw new FileNotFoundException($"Cannot find {file} (2.8 or newer) in {path}");
                }
            }

            return isLoaded;
        }

        public static Version CheckOpsVersionIfValid(string Path)
        {
            var v1 = new System.Version("2.5.0");
            var v2 = new System.Version("2.8.0");
            var versionFound = AssemblyName.GetAssemblyName(Path).Version;

            //2.5.0
            if (versionFound.CompareTo(v1) == 0)
            {
                return v1;
            }
            else if (versionFound.CompareTo(v2) >= 0)
            {
                return versionFound;
            }
            else
            {
                throw new FileNotFoundException($"Cannot find OpenStudio (2.8 or newer) installed!");
                //return v2;
            }
            
        }


    }
}