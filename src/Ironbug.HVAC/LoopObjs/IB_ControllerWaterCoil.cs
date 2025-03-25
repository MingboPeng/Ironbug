using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ControllerWaterCoil : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ControllerWaterCoil();


        private static ControllerWaterCoil NewDefaultOpsObj(Model model) {
           
            var dummyLoop = new PlantLoop(model);
            var dummyCoil = new CoilCoolingWater(model);
            dummyLoop.addDemandBranchForComponent(dummyCoil);
            // get a default controllerWaterCoil
            return dummyCoil.controllerWaterCoil().get();
        }

        public IB_ControllerWaterCoil() : base(NewDefaultOpsObj)
        {

        }

        public ModelObject ToOS(Model model)
        {
            //var newObj = this.OnNewOpsObj(NewDefaultOpsObj, model);
            //return newObj;

            //ControllerWaterCoil will never be added to model individually.
            //It will be added to WaterCoil when it is added to a plant loop
            return null;
        }

    }

    public sealed class IB_ControllerWaterCoil_FieldSet
        : IB_FieldSet<IB_ControllerWaterCoil_FieldSet, ControllerWaterCoil>
    {
        private IB_ControllerWaterCoil_FieldSet() {}
        
        
    }
}
