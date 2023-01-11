using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Ironbug.Core.OpenStudio
{
    public static class OpenStudioHelper
    {
   
        public static string FindOpsFolder()
        {
            var possiblePath = new List<string>();
            var root = Path.GetDirectoryName(typeof(OpenStudioHelper).Assembly.Location);
            possiblePath.Add(root);

            // only works with LBT
            if (root.Contains("ladybug_tools"))
            {
                // installed to LBT folder
                var lbt = root.Substring(0, root.IndexOf("ladybug_tools"));
                var lbtOpenStudio = Path.Combine(lbt, "ladybug_tools", "openstudio", "CSharp", "openstudio");
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


            var file = "OpenStudio.dll";
            var path = possiblePath.FirstOrDefault(_ => File.Exists(Path.Combine(_, file)));
            if (string.IsNullOrEmpty(path))
                throw new FileNotFoundException($"Cannot find OpenStudio installed in ladybug_tools folder!");

            return path;
        }
        public static bool LoadAssemblies(Action<string> messageLogger)
        {
            return LoadAssemblies(messageLogger, out var _);
        }

        public static bool LoadAssemblies(Action<string> messageLogger, out Assembly ops)
        {
            Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
            var possibleOpsDll = asms.Where(_ => _.GetName().Name.ToUpper() == "OPENSTUDIO");
            var isLoaded = possibleOpsDll.Any();

            var file = "OpenStudio.dll";
            ops = null;

            if (!isLoaded)
            {
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


                    ops = Assembly.LoadFile(asmFile);
                    isLoaded = true;
                    messageLogger($"OpenStudio library {versionFound} from {path}.");
                }
                catch (Exception)
                {
                    throw new FileNotFoundException($"Cannot find {file} in {path}");
                }
            }

            return isLoaded;
        }

        public static Version CheckOpsVersionIfValid(string Path)
        {
            var v2 = new System.Version("3.3.0.0");
            var versionFound = AssemblyName.GetAssemblyName(Path).Version;

            if (versionFound.CompareTo(v2) >= 0)
            {
                return versionFound;
            }
            else
            {
                throw new FileNotFoundException($"Cannot find OpenStudio ({v2} or newer) installed!\n{versionFound} is found at {Path}");
                //return v2;
            }
            
        }


    }
}