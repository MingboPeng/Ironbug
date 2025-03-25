using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_SizingZone : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SizingZone();

        private static SizingZone NewDefaultOpsObj(Model model) => new SizingZone(model, new ThermalZone(model));
        
        public IB_SizingZone():base(NewDefaultOpsObj)
        {
        }
        
        public ModelObject ToOS(Model model, ThermalZone thermalZone)
        {
            var sz = thermalZone.sizingZone();
            sz.SetCustomAttributes(model, this.CustomAttributes);
            return sz;
        }
        
        
    }

    public sealed class IB_SizingZone_FieldSet 
        : IB_FieldSet<IB_SizingZone_FieldSet, SizingZone>
    {
        private IB_SizingZone_FieldSet() { }
    }
}
