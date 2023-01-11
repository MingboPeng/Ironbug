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

                if (!commandArgs.Any() || commandArgs.Count() != 2)
                {
                    Console.WriteLine("Hello, this is Ironbug Console app!");
                    return;
                }
                var osm = System.IO.Path.GetFullPath(commandArgs.FirstOrDefault());
                var hvac = System.IO.Path.GetFullPath(commandArgs.LastOrDefault());

                //var osm = @"D:\Dev\Ironbug\src\Ironbug.HVAC_Tests\TestSource\Integration Testing\FourOfficeBuilding - Copy.osm";
                //var hvac = @"D:\Dev\Ironbug\src\Ironbug.HVAC_Tests\TestSource\Integration Testing\Sys02_PTHP_Advanced.json";

                Console.WriteLine($"[INFO] Input osm file: {osm}");
                Console.WriteLine($"[INFO] Input ironbug HVAC json file: {hvac}");

                // set the current directory so that it can find all openstudio files on Linux
                var currDir = System.IO.Path.GetDirectoryName(typeof(Program).Assembly.Location);
                System.IO.Directory.SetCurrentDirectory(currDir);

                var done = HVAC.IB_HVACSystem.SaveHVAC(osm, hvac);
                if (done)
                    Console.WriteLine($"[INFO] HVAC is added to osm file:\n {osm}");
                else
                    throw new ArgumentException("Failed to save HVAC to osm file");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
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



