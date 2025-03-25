using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_CoilCoolingFourPipeBeam : IB_CoilCoolingBasic, IIB_DualLoopObj, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilCoolingFourPipeBeam();

        private static CoilCoolingFourPipeBeam NewDefaultOpsObj(Model model) => new CoilCoolingFourPipeBeam(model);

        public IB_CoilCoolingFourPipeBeam() : base(NewDefaultOpsObj)
        {
        }
        

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
        
    }
    public sealed class IB_CoilCoolingFourPipeBeam_FieldSet
        : IB_FieldSet<IB_CoilCoolingFourPipeBeam_FieldSet>
    {
        private IB_CoilCoolingFourPipeBeam_FieldSet() { }
        
    }

}
