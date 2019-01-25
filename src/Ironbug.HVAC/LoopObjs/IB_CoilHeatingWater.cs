using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingWater: IB_CoilHeatingBasic, IIB_DualLoopObj, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilHeatingWater();

        private static CoilHeatingWater NewDefaultOpsObj(Model model) => new CoilHeatingWater(model);
        
        public IB_CoilHeatingWater() : base(NewDefaultOpsObj(new Model()))
        {
        }
        
        public override bool AddToNode(Node node)
        {
            var model = node.model();
            var result = ((CoilHeatingWater)this.NewOpsObj(model)).addToNode(node);
            return result;
            
        }


        protected override ModelObject NewOpsObj(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            return obj;
        }


    }

    public sealed class IB_CoilHeatingWater_DataFieldSet
        : IB_FieldSet<IB_CoilHeatingWater_DataFieldSet, CoilHeatingWater>
    {
        private IB_CoilHeatingWater_DataFieldSet() {}


        //https://bigladdersoftware.com/epx/docs/8-0/input-output-reference/page-042.html#coilheatingwater
        public IB_Field Name { get; }
            = new IB_BasicField("Name", "Name")
            {
                DetailedDescription = "A unique identifying name for each coil."
            };
        

        public IB_Field RatedInletWaterTemperature { get; }
            = new IB_BasicField("RatedInletWaterTemperature", "InWaterTemp")
            {
                DetailedDescription = "The inlet water temperature (degrees C) corresponding to the rated heating capacity. " +
                "The default is 82.2 degrees C (180 degrees F)."
            };

        public IB_Field RatedInletAirTemperature { get; }
            = new IB_BasicField("RatedInletAirTemperature", "InAirTemp");
            

        public IB_Field RatedOutletWaterTemperature { get; }
            = new IB_BasicField("RatedOutletWaterTemperature", "OutWaterTemp");

        public IB_Field RatedOutletAirTemperature { get; }
            = new IB_BasicField("RatedOutletAirTemperature", "OutAirTemp");

        public IB_Field UFactorTimesAreaValue { get; }
            = new IB_BasicField("UFactorTimesAreaValue", "UFactor");

        public IB_Field MaximumWaterFlowRate { get; }
            = new IB_BasicField("MaximumWaterFlowRate", "MaxFlow")
            {
                DetailedDescription = "The maximum possible water flow rate (m3/sec) through the coil. " +
                "This field is used when Coil Performance Input Method = UFactorTimesAreaAndDesignWaterFlowRate. " +
                "This field is autosizable.",

            };

        public IB_Field RatedRatioForAirAndWaterConvection { get; }
            = new IB_BasicField("RatedRatioForAirAndWaterConvection", "AirWaterRatio");
        
    }
    
    

}
