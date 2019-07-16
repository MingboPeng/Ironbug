using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingFourPipeBeam : IB_CoilCoolingBasic, IIB_DualLoopObj, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilHeatingFourPipeBeam();

        private static CoilHeatingFourPipeBeam NewDefaultOpsObj(Model model) => new CoilHeatingFourPipeBeam(model);

        public IB_CoilHeatingFourPipeBeam() : base(NewDefaultOpsObj(new Model()))
        {
        }
        

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
        
    }
    public sealed class IB_CoilHeatingFourPipeBeam_FieldSet
        : IB_FieldSet<IB_CoilHeatingFourPipeBeam_FieldSet, CoilHeatingFourPipeBeam>
    {
        private IB_CoilHeatingFourPipeBeam_FieldSet() { }
        
    }

}
