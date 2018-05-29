using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_CoilCoolingCooledBeam : IB_CoilBasic, IIB_ShareableObj, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilCoolingCooledBeam();

        private static CoilCoolingCooledBeam InitMethod(Model model) => new CoilCoolingCooledBeam(model);

        public IB_CoilCoolingCooledBeam() : base(InitMethod(new Model()))
        {
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((CoilCoolingCooledBeam)this.ToOS(model)).addToNode(node);
        }
        

        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethod, model).to_CoilCoolingCooledBeam().get();
        }
    }
    public sealed class IB_CoilCoolingCooledBeam_DataFieldSet
        : IB_DataFieldSet<IB_CoilCoolingCooledBeam_DataFieldSet, CoilCoolingCooledBeam>
    {
        private IB_CoilCoolingCooledBeam_DataFieldSet() { }
        
    }

}
