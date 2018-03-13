using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenStudio;

namespace Ironbug.HVAC
{
    public abstract class IB_ZoneEquipment : IB_ModelObject, IIB_ToOPSable
    {
        public abstract ModelObject ToOS(Model model);

        public IB_ZoneEquipment(HVACComponent GhostOSObject) : base(GhostOSObject)
        {

        }
    }
}
