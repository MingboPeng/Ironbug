using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingWater: IB_Coil
    {
        private static CoilHeatingWater InitMethod(Model model) => new CoilHeatingWater(model);
        
        public IB_CoilHeatingWater() : base(InitMethod(new Model()))
        {
            
        }

        public override IB_ModelObject Duplicate()
        {
            return base.DuplicateIB_ModelObject(() => new IB_CoilHeatingWater());
        }
        
        //dealing with the real object, use only when it is ready to be added to os model
        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((CoilHeatingWater)this.ToOS(model)).addToNode(node);
            
        }
        
        
        public override ModelObject ToOS(Model model)
        {

            return base.ToOS(InitMethod, model).to_CoilHeatingWater().get();
        }


    }

    public class IB_CoilHeatingWater_DataFieldSet: IB_DataFieldSet
    {
        //private static readonly CoilHeatingWater refObj = new CoilHeatingWater(new Model());
        //public new IddObject IddObject = new CoilHeatingWater(new Model()).iddObject();
        protected override IddObject RefIddObject => new CoilHeatingWater(new Model()).iddObject();

        protected override Type ParentType => typeof(CoilHeatingWater);

        //https://openstudio-sdk-documentation.s3.amazonaws.com/cpp/OpenStudio-2.4.0-doc/model/html/classopenstudio_1_1model_1_1_coil_heating_water.html
        //https://bigladdersoftware.com/epx/docs/8-0/input-output-reference/page-042.html#coilheatingwater
        //Following list items are fields that I want to have picked for GH user to edit
        public static readonly IB_DataField Name
            = new IB_DataField("Name", "Name", strType, true)
            {
                Description = "A unique identifying name for each coil."
            };
        

        public static readonly IB_DataField RatedInletWaterTemperature 
            = new IB_DataField("RatedInletWaterTemperature", "InWaterTemp", dbType, BasicSetting:true)
            {
                Description = "The inlet water temperature (degrees C) corresponding to the rated heating capacity. " +
                "The default is 82.2 degrees C (180 degrees F)."
            };

        public static readonly IB_DataField RatedInletAirTemperature
            = new IB_DataField("RatedInletAirTemperature", "InAirTemp", dbType);
            

        public static readonly IB_DataField RatedOutletWaterTemperature 
            = new IB_DataField("RatedOutletWaterTemperature", "OutWaterTemp", dbType, BasicSetting: true);

        public static readonly IB_DataField RatedOutletAirTemperature
            = new IB_DataField("RatedOutletAirTemperature", "OutAirTemp", dbType);

        public static readonly IB_DataField UFactorTimesAreaValue
            = new IB_DataField("UFactorTimesAreaValue", "UFactor", dbType);

        public static readonly IB_DataField MaximumWaterFlowRate
            = new IB_DataField("MaximumWaterFlowRate", "MaxFlow", dbType)
            {
                Description = "The maximum possible water flow rate (m3/sec) through the coil. " +
                "This field is used when Coil Performance Input Method = UFactorTimesAreaAndDesignWaterFlowRate. " +
                "This field is autosizable.",

            };

        public static readonly IB_DataField RatedRatioForAirAndWaterConvection
            = new IB_DataField("RatedRatioForAirAndWaterConvection", "AirWaterRatio", dbType);

        
    }
    
    

}
