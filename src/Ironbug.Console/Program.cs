using System;
using System.Linq;

namespace Ironbug
{
    class Program
    {
        static void Main(string[] args)
        {

            var commandArgs = args.Select(x => x.Trim()).Where(_ => !string.IsNullOrEmpty(_));

            if (!commandArgs.Any() || commandArgs.Count() != 2)
            {
                Console.WriteLine("Hello, this is Ironbug Console app!");
                return;
            }
            var osm = commandArgs.FirstOrDefault();
            var hvac = commandArgs.LastOrDefault();

            //var osm = "in.osm";
            //var hvac = "hvac.txt";

            Console.WriteLine($"Input osm file: {osm}");
            Console.WriteLine($"Input ironbug HVAC json file: {hvac}");

            if (!System.IO.File.Exists(osm) || !osm.ToLower().EndsWith(".osm"))
                throw new ArgumentException("Invalid osm file");
            if (!System.IO.File.Exists(hvac))
                throw new ArgumentException("Invalid hvac file");

            var hvacJson = System.IO.File.ReadAllText(hvac);
            var system = Ironbug.HVAC.IB_HVACSystem.FromJson(hvacJson);

            if (system != null) Console.WriteLine($"HVAC is valid");

            var done = system?.SaveHVAC(osm);
            if (done.GetValueOrDefault())
                Console.WriteLine($"HVAC is added to osm file: {osm}");
            else
                throw new ArgumentException("Failed to save HVAC to osm file");

            //Console.ReadLine();
        }


    }
}



