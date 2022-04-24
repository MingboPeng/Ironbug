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

        public static string FindOpsFolder()
        {
            var possiblePath = new List<string>();
            var root = Path.GetDirectoryName(typeof(OpenStudioHelper).Assembly.Location);
            possiblePath.Add(root);

            if (root.Contains("ladybug_tools"))
            {
                // installed to LBT folder
                var lbt = Path.GetDirectoryName(root);
                var lbtOpenStudio = Path.Combine(lbt, "openstudio", "CSharp", "openstudio");
                if (Directory.Exists(lbtOpenStudio))
                    possiblePath.Add(lbtOpenStudio);
            }
            else
            {
                var userFolder = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                var lbt = Path.Combine(userFolder, "ladybug_tools");
                var lbtOpenStudio = Path.Combine(lbt, "openstudio", "CSharp", "openstudio");
                if (Directory.Exists(lbtOpenStudio))
                    possiblePath.Add(lbtOpenStudio);
            }

            //possiblePath.Add(@"C:\openstudio-2.7.0\CSharp\openstudio");
            //possiblePath.Add($@"C:\openstudio-{Version}\CSharp\openstudio");
            var opsPaths = Directory.GetDirectories(@"C:\")
                .Where(s => s.ToLower().StartsWith("c:\\openstudio"))
                .Select(_ => _ + @"\CSharp\openstudio").ToList();
            opsPaths.Sort();
            opsPaths.Reverse();
            possiblePath.AddRange(opsPaths);

            var file = "OpenStudio.dll";
            var path = possiblePath.FirstOrDefault(_ => File.Exists(Path.Combine(_, file)));
            if (string.IsNullOrEmpty(path))
                throw new FileNotFoundException($"Cannot find OpenStudio 3.3 or newer version installed in C drive!");

            return path;
        }

        public static bool LoadAssemblies(Action<string> MessageLogger)
        {
            Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
            var possibleOpsDll = asms.Where(_ => _.GetName().Name.ToUpper() == "OPENSTUDIO");
            var isLoaded = false;

            if (!possibleOpsDll.Any())
            {
                var file = "OpenStudio.dll";
                var path = FindOpsFolder();

                var asmFile = Path.Combine(path, file);
                var versionFound = CheckOpsVersionIfValid(asmFile);

                try
                {
                    // check openstudiolib.dll, copy it to csharp folder
                    var libFile = "openstudiolib.dll";
                    if (!File.Exists(Path.Combine(path, libFile)))
                    {
                        var opsRoot = Path.GetDirectoryName(Path.GetDirectoryName(path));
                        var libFolder = Path.Combine(opsRoot, "lib");
                        if (Directory.Exists(libFolder))
                        {
                            var libs = Directory.GetFiles(libFolder, "*", SearchOption.TopDirectoryOnly);
                            foreach (var item in libs)
                            {
                                var target = Path.Combine(path, Path.GetFileName(item));
                                File.Copy(item, target, true);
                            }
                        }
                    }
                   

                    Assembly.LoadFile(asmFile);
                    isLoaded = true;
                    MessageLogger($"OpenStudio library {versionFound} from {path}.");
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
            var v1 = new System.Version("2.5.0.0");
            var v2 = new System.Version("2.8.0.0");
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
                throw new FileNotFoundException($"Cannot find OpenStudio (2.8 or newer) installed!\n{versionFound} is found at {Path}");
                //return v2;
            }
            
        }


    }
}