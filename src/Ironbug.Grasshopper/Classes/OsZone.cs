using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Types
{
    public class OsZone: GH_Brep
    {
        public string ZoneName { get; private set; }

        public override string TypeName => "Imported OSThermalZone";

        public override string TypeDescription => "This is a visual representation of the thermal zone for energy modeling with HVAC systems";

        public OsZone(Brep brep , string ZoneName):base(brep)
        {
            this.ZoneName = ZoneName;
        }

        public override string ToString()
        {
            return ZoneName + "(OsZone)";
        }

      
    }

   
}