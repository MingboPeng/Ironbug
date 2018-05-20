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

        private static SizingZone InitMethod(Model model) => new SizingZone(model, new ThermalZone(model));
        
        public IB_SizingZone():base(InitMethod(new Model()))
        {
        }
        
        /// <summary>
        /// This is the base Duplicate() for IB_SizingZone, you need to call SetSizingZone in IB_ThermalZone to link SizingZone to ThermalZone; 
        /// Or you can use DuplicateToZone(IB_ThermalZone ThermalZone) instead.
        /// </summary>
        /// <returns>IB_ModelObject</returns>
        public override IB_ModelObject Duplicate()
        {
            return base.DuplicateIBObj(IB_InitSelf);
        }
        

        public ModelObject ToOS(ThermalZone thermalZone)
        {
            //create a sizingZone to target thermalZone
            var targetModel = thermalZone.model();
            return base.OnInitOpsObj((Model model)=> new SizingZone(model, thermalZone), targetModel);
        }

        //this is replaced by above method
        protected override ModelObject InitOpsObj(Model model)
        {
            throw new NotImplementedException();
        }
        
    }

    public sealed class IB_SizingZone_DataFieldSet 
        : IB_DataFieldSet<IB_SizingZone_DataFieldSet, SizingZone>
    {
        private IB_SizingZone_DataFieldSet() { }
    }
}
