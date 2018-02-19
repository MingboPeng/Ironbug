using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingWater: IB_HVACComponent
    {
        //Coil Heating Water 1, Name
        //autosize,             U-Factor Times Area Value[kg * m ^ 2 / s ^ 3 * K] (Default: autosize)
        //Autosize,             Maximum Water Flow Rate[m ^ 3 / s] (Default: Autosize)
        //,                     Performance Input Method
        //autosize,             Rated Capacity[kg * m ^ 2 / s ^ 3] (Default: autosize)
        //82.2,                 Rated Inlet Water Temperature[C] (Default: 82.2)
        //16.6,                 Rated Inlet Air Temperature[C] (Default: 16.6)
        //71.1,                 Rated Outlet Water Temperature[C] (Default: 71.1)
        //32.2,                 Rated Outlet Air Temperature[C] (Default: 32.2)
        //0.5,                  Rated Ratio for Air and Water Convection(Default: 0.5)
        
        private static CoilHeatingWater InitMethod(Model model) => new CoilHeatingWater(model);
        
        //dealing with the ghost object
        public IB_CoilHeatingWater() : base(InitMethod(new Model()))
        {
            //check name
            //TODO: there is a method incoke problem when call this method right after GH started
            base.SetName("Coil:Heating:Water");
        }

        public override IB_ModelObject Duplicate()
        {
            return base.Duplicate(() => new IB_CoilHeatingWater());
        }
        
        //dealing with the real object, use only when it is ready to be added to os model
        public override bool AddToNode(Model model, Node node)
        {
            
            return ((CoilHeatingWater)this.ToOS(model)).addToNode(node);
            
            //this.osCoilHeatingWater.setn
        }


        public override ParentObject ToOS(Model model)
        {
            return base.ToOS(InitMethod, model).to_CoilHeatingWater().get();
        }

        
    }

    public class IB_CoilHeatingWater_DataFieldSet: IB_DataFieldSet
    {
        //private static readonly CoilHeatingWater refObj = new CoilHeatingWater(new Model());
        //public new IddObject IddObject = new CoilHeatingWater(new Model()).iddObject();
        protected override IddObject RefIddObject => new CoilHeatingWater(new Model()).iddObject();
        

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

        

        public static IEnumerable<IB_DataField> GetList()
        {
            return GetList<IB_CoilHeatingWater_DataFieldSet>();
        }

        public static IB_DataField GetAttributeByName(string name)
        {
            return GetAttributeByName<IB_CoilHeatingWater_DataFieldSet>(name);
        }

        

    }
    
    

}
