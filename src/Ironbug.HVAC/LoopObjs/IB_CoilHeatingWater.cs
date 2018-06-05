using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingWater: IB_CoilBasic, IIB_ShareableObj, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilHeatingWater();

        private static CoilHeatingWater InitMethod(Model model) => new CoilHeatingWater(model);
        
        public IB_CoilHeatingWater() : base(InitMethod(new Model()))
        {
        }
        
        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((CoilHeatingWater)this.InitOpsObj(model)).addToNode(node);
            
        }


        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethod, model).to_CoilHeatingWater().get();
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
            = new IB_ProField("RatedInletAirTemperature", "InAirTemp");
            

        public IB_Field RatedOutletWaterTemperature { get; }
            = new IB_BasicField("RatedOutletWaterTemperature", "OutWaterTemp");

        public IB_Field RatedOutletAirTemperature { get; }
            = new IB_ProField("RatedOutletAirTemperature", "OutAirTemp");

        public IB_Field UFactorTimesAreaValue { get; }
            = new IB_ProField("UFactorTimesAreaValue", "UFactor");

        public IB_Field MaximumWaterFlowRate { get; }
            = new IB_ProField("MaximumWaterFlowRate", "MaxFlow")
            {
                DetailedDescription = "The maximum possible water flow rate (m3/sec) through the coil. " +
                "This field is used when Coil Performance Input Method = UFactorTimesAreaAndDesignWaterFlowRate. " +
                "This field is autosizable.",

            };

        public IB_Field RatedRatioForAirAndWaterConvection { get; }
            = new IB_ProField("RatedRatioForAirAndWaterConvection", "AirWaterRatio");
        
    }
    
    

}
