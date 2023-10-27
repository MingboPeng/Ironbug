using System;
using System.Linq;
using System.Reflection;

namespace Ironbug
{
    class Program
    {

        static void Main(string[] args)
        {
            try
            {
                AppDomain currentDomain = AppDomain.CurrentDomain;
                currentDomain.AssemblyResolve += new ResolveEventHandler(OpsResolveEventHandler);
                var commandArgs = args.Select(x => x.Trim()).Where(_ => !string.IsNullOrEmpty(_));

                var assembly = typeof(Program).Assembly;
                if (!commandArgs.Any() || commandArgs.Count() != 2)
                {
                    var version = assembly.GetName().Version;
                    var date = System.IO.File.GetLastWriteTime(assembly.Location).ToString("MMM dd, yyyy");
                    Console.WriteLine($"Hello, this is Ironbug Console app!{System.Environment.NewLine}v{version} ({date})");
                    return;
                }

             

                var osm = System.IO.Path.GetFullPath(commandArgs.FirstOrDefault());
                var hvac = System.IO.Path.GetFullPath(commandArgs.LastOrDefault());

                //var osm = @"C:\Users\mingo\simulation\20230615_DetailedHVAC\openstudio\generated_files\VAV and Chilled Beams.osm";
                //var hvac = @"C:\Users\mingo\simulation\20230615_DetailedHVAC\openstudio\generated_files\VAV and Chilled Beams.json";

                Console.WriteLine($"[INFO] Input osm file:\n {osm}");
                Console.WriteLine($"[INFO] Input ironbug HVAC json file:\n {hvac}");

                // duplicate a copy
                var osmIn = System.IO.Path.ChangeExtension(osm, "osm.backup");
                System.IO.File.Copy(osm, osmIn, true);
                if (System.IO.File.Exists(osmIn))
                    Console.WriteLine($"[INFO] Backup input file:\n {osmIn}");

                // set the current directory so that it can find all OpenStudio files on Linux
                var currDir = System.IO.Path.GetDirectoryName(assembly.Location);
                System.IO.Directory.SetCurrentDirectory(currDir);

                var done = HVAC.IB_HVACSystem.SaveHVAC(osm, hvac);
                if (done)
                    Console.WriteLine($"[INFO] Done! HVAC is added to osm file:\n {osm}");
                else
                    throw new ArgumentException("Failed to save HVAC to osm file");
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
                //Console.ReadLine();
            }

            //Console.ReadLine();
        }


        private static Assembly OpsResolveEventHandler(object sender, ResolveEventArgs args)
        {

            if (args.Name.StartsWith( "OpenStudio"))
            {
                Ironbug.Core.OpenStudio.OpenStudioHelper.LoadAssemblies((string s) => Console.WriteLine($"[INFO] {s}"), out var ops);
                return ops;
            }
            else
            {
                return null;
            }

        }


    }
}



