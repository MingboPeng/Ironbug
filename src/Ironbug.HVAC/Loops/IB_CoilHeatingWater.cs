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
        
        //Real obj to be saved in OS model
        private CoilHeatingWater osCoilHeatingWater { get; set; }
        private Model osModel { get; set; }

        //Ghost obj for place holder
        //private CoilHeatingWater ghostHVACComponent { get; set; }
        //protected override HVACComponent ghostHVACComponent { get { return ghostCoilHeatingWater; } }

        //dealing with the ghost object
        public IB_CoilHeatingWater()
        {
            this.ghostHVACComponent = new CoilHeatingWater(new Model());
        }

        //dealing with the real object, use only when it is ready to be added to os model
        public override bool AddToNode(ref Model model, Node node)
        {
            this.osModel = model;
            this.osCoilHeatingWater = this.osCoilHeatingWater ?? new CoilHeatingWater(model);
            this.osCoilHeatingWater.SetCustomAttributes(this.CustomAttributes);
            return this.osCoilHeatingWater.addToNode(node);
        }

        public override HVACComponent plantDemand()
        {
            this.osCoilHeatingWater = this.osCoilHeatingWater ?? new CoilHeatingWater(osModel);
            return osCoilHeatingWater;
        }

        ////this method for internal use, needed to be protected. call SetAttribute() instead
        //protected override void AddCustomAttribute(string AttributeName, object AttributeValue)
        //{
        //    //adding attributes for real object to use later
        //    base.AddCustomAttribute(AttributeName, AttributeValue);
        //    //dealing the ghost object
        //    this.ghostHVACComponent.SetCustomAttribute(AttributeName, AttributeValue);
        //}



    }

    public class IB_CoilHeatingWater_Attributes: IB_DataFieldSet
    {
        //private static readonly CoilHeatingWater refObj = new CoilHeatingWater(new Model());


        //https://openstudio-sdk-documentation.s3.amazonaws.com/cpp/OpenStudio-2.4.0-doc/model/html/classopenstudio_1_1model_1_1_coil_heating_water.html

        
        public static readonly IB_DataField RatedInletWaterTemperature 
            = new IB_DataField("RatedInletWaterTemperature", "InWaterTemp", dbType);

        public static readonly IB_DataField RatedInletAirTemperature
            = new IB_DataField("RatedInletAirTemperature", "InAirTemp", dbType);

        public static readonly IB_DataField RatedOutletWaterTemperature 
            = new IB_DataField("RatedOutletWaterTemperature", "OutWaterTemp", dbType);

        public static readonly IB_DataField RatedOutletAirTemperature
            = new IB_DataField("RatedOutletAirTemperature", "OutAirTemp", dbType);

        public static readonly IB_DataField UFactorTimesAreaValue
            = new IB_DataField("UFactorTimesAreaValue", "UFactor", dbType);

        public static readonly IB_DataField MaximumWaterFlowRate
            = new IB_DataField("MaximumWaterFlowRate", "MaxFlow", dbType);

        public static readonly IB_DataField RatedRatioForAirAndWaterConvection
            = new IB_DataField("RatedRatioForAirAndWaterConvection", "AirWaterRatio", dbType);


        public static IEnumerable<IB_DataField> GetList()
        {
            return GetList<IB_CoilHeatingWater_Attributes>();
        }

        public static IB_DataField GetAttributeByName(string name)
        {
            return GetAttributeByName<IB_CoilHeatingWater_Attributes>(name);
        }

        

    }
    
    

}
