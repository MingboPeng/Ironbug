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
        
        public IB_SizingZone():base(NewDefaultOpsObj(new Model()))
        {
        }
        
        public ModelObject ToOS(ThermalZone thermalZone)
        {
            //create a sizingZone to target thermalZone
            var targetModel = thermalZone.model();
            return base.OnNewOpsObj((Model model)=> new SizingZone(model, thermalZone), targetModel);
        }
        
        
    }

    public sealed class IB_SizingZone_DataFieldSet 
        : IB_FieldSet<IB_SizingZone_DataFieldSet, SizingZone>
    {
        private IB_SizingZone_DataFieldSet() { }
    }
}
