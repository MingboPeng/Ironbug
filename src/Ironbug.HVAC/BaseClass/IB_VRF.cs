using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_VRFSystem : IB_ModelObject, IIB_ToOPSable, IIB_ZoneEquipment
    {
        
        public abstract ModelObject ToOS(Model model);

        public IB_VRFSystem(HVACComponent GhostOSObject) : base(GhostOSObject)
        {

        }
        


    }

    
}
