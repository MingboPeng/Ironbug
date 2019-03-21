using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingWater: IB_CoilHeatingBasic, IIB_DualLoopObj, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilHeatingWater();

        private static CoilHeatingWater NewDefaultOpsObj(Model model) => new CoilHeatingWater(model);
        private IB_ControllerWaterCoil Controller => this.Children.Get<IB_ControllerWaterCoil>();
        public IB_CoilHeatingWater() : base(NewDefaultOpsObj(new Model()))
        {
        }
        public IB_CoilHeatingWater(IB_ControllerWaterCoil Controller) : base(NewDefaultOpsObj(new Model()))
        {
            AddChild(Controller);
        }

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
        public override bool AddToNode(Node node)
        {
            var model = node.model();
            var obj = ToOS(model);
            var success = obj.addToNode(node);
            if (success)
            {
                var optionalCtrl = ((CoilHeatingWater)obj).controllerWaterCoil();
                if (optionalCtrl.is_initialized())
                {
                    optionalCtrl.get().SetCustomAttributes(this.Controller.CustomAttributes);
                }
            }
            return success;
        }


    }

    public sealed class IB_CoilHeatingWater_FieldSet
        : IB_FieldSet<IB_CoilHeatingWater_FieldSet, CoilHeatingWater>
    {
        private IB_CoilHeatingWater_FieldSet() {}

        
        public IB_Field Name { get; }
            = new IB_BasicField("Name", "Name");


        public IB_Field RatedInletWaterTemperature { get; }
            = new IB_BasicField("RatedInletWaterTemperature", "InWaterTemp");

        public IB_Field RatedInletAirTemperature { get; }
            = new IB_BasicField("RatedInletAirTemperature", "InAirTemp");
            

        public IB_Field RatedOutletWaterTemperature { get; }
            = new IB_BasicField("RatedOutletWaterTemperature", "OutWaterTemp");

        public IB_Field RatedOutletAirTemperature { get; }
            = new IB_BasicField("RatedOutletAirTemperature", "OutAirTemp");

        public IB_Field UFactorTimesAreaValue { get; }
            = new IB_BasicField("UFactorTimesAreaValue", "UFactor");

        public IB_Field MaximumWaterFlowRate { get; }
            = new IB_BasicField("MaximumWaterFlowRate", "MaxFlow");

        public IB_Field RatedRatioForAirAndWaterConvection { get; }
            = new IB_BasicField("RatedRatioForAirAndWaterConvection", "AirWaterRatio");
        
    }
    
    

}
