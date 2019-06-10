using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Ironbug.Core.OpenStudio
{
    public static class OpenStudioHelper
    {
        public static string SupportedVersion { get; set; } = "2.5.0";

        public static bool LoadAssemblies(Action<string> MessageLogger)
        {
            return LoadAssemblies(MessageLogger, SupportedVersion);
        }

        public static bool LoadAssemblies(Action<string> MessageLogger, string Version)
        {
            Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
            var possibleOpsDll = asms.Where(_ => _.GetName().Name.ToUpper() == "OPENSTUDIO");
            var isLoaded = false;

            if (!possibleOpsDll.Any())
            {
                var possiblePath = new List<string>();
                //possiblePath.Add(@"C:\Ironbug");
                //possiblePath.Add(@"C:\openstudio-2.7.0\CSharp\openstudio");
                possiblePath.Add($@"C:\openstudio-{Version}\CSharp\openstudio");

                var file = "OpenStudio.dll";

                var path = possiblePath.FirstOrDefault(_ => File.Exists(Path.Combine(_, file)));

                if (string.IsNullOrEmpty(path))
                {
                    throw new FileNotFoundException($"Cannot find OpenStudio {Version} installed at C:\\openstudio-{Version}!\n\nIronbug works with this specific version of OpenStudio.");
                }

                var asmFile = Path.Combine(path, file);
                var versionFound = AssemblyName.GetAssemblyName(asmFile).Version.ToString();
                try
                {
                    Assembly.LoadFile(asmFile);
                    isLoaded = true;
                    MessageLogger(string.Format("OpenStudio library {0} loaded from {1}", versionFound, path));
                }
                catch (Exception)
                {
                    throw new FileNotFoundException(string.Format("Cannot find {0} ({1}) in {2}", file, Version, path));
                }
            }

            return isLoaded;
        }

    }
}