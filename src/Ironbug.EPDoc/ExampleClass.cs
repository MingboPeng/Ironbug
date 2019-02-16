using System.Collections.Generic;

namespace Ironbug.EPDoc
{
    ////This is a test file
    ///Do not delete this, as this is used as an entry.
    public sealed class ExampleClass
    {
        private string _name = "Coil:Cooling:Water";
        private string _note = string.Empty;
        private IEnumerable<string> note = new List<string>() { "The water cooling coil (Coil:Cooling:Water) has the ability to give detailed output with simplified inputs, inputting complicated coil geometry is not required by the user for this model instead the coil is sized in terms of auto-sizeable thermodynamic inputs. The coil requires thermodynamic inputs such as temperatures, mass flow rates and humidity ratios.", "", "The coil is sized using auto-sized/user design input conditions and the UA values are calculated from the design conditions. A rough estimate of the coil area is provided along with percentage of surface wet and/or dry. This model uses the NTU-effectiveness approach to model heat transfer and has two types of flow arrangements cross-flow or counter-flow.", "", "The basic underlying idea is - use auto sizable thermodynamic design inputs, calculate the coil UA s, use these UA values and operating conditions from the nodes connections, calculate the outlet stream conditions, and calculate the heat transfer rates.", "", "See section Cooling Coil Model in the EnergyPlus Engineering Document for further details regarding this model.", "" };

        private static readonly System.Lazy<ExampleClass> instance = new System.Lazy<ExampleClass>(() => new ExampleClass());

        public static ExampleClass Value => instance.Value;
        public static string Name => Value._name;
        public static string Note => Value._note;

        private ExampleClass()
        {
        }
    }

}