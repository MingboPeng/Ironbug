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
        //public enum AttributeNames
        //{
        //    RatedInletWaterTemperature,
        //    RatedOutletWaterTemperature
        //}

        //Coil Heating Water 1, Name
        //autosize, U-Factor Times Area Value[kg * m ^ 2 / s ^ 3 * K] (Default: autosize)
        //Autosize, Maximum Water Flow Rate[m ^ 3 / s] (Default: Autosize)
        //, Performance Input Method
        //autosize, Rated Capacity[kg * m ^ 2 / s ^ 3] (Default: autosize)
        //82.2, Rated Inlet Water Temperature[C] (Default: 82.2)
        //16.6, Rated Inlet Air Temperature[C] (Default: 16.6)
        //71.1, Rated Outlet Water Temperature[C] (Default: 71.1)
        //32.2, Rated Outlet Air Temperature[C] (Default: 32.2)
        //0.5, Rated Ratio for Air and Water Convection(Default: 0.5)


        //Real obj to be saved in OS model
        private CoilHeatingWater osCoilHeatingWater { get; set; }

        //Ghost obj for place holder
        private CoilHeatingWater ghostCoilHeatingWater { get; set; }
        protected override HVACComponent ghostHVACComponent { get { return ghostCoilHeatingWater; } }

        //dealing with the ghost object
        public IB_CoilHeatingWater()
        {
            var model = new Model();
            this.ghostCoilHeatingWater = new CoilHeatingWater(model);
            this.CustomAttributes = new Dictionary<string, object>();
        }

        //dealing with the real object, only when it is ready to be added to os model
        public override bool AddToNode(ref Model model, Node node)
        {
            this.osCoilHeatingWater = new CoilHeatingWater(model);
            this.osCoilHeatingWater.SetCustomAttributes(this.CustomAttributes);
            return this.osCoilHeatingWater.addToNode(node);
        }

        //this method for internal use, needed to be protected. call SetAttribute() instead
        protected override void AddCustomAttribute(string AttributeName, object AttributeValue)
        {
            //adding attributes for real object to use later
            base.AddCustomAttribute(AttributeName, AttributeValue);
            //dueling the ghost object
            this.ghostCoilHeatingWater.SetCustomAttribute(AttributeName, AttributeValue);
        }

        
        public void SetAttribute(DataAttribute AttributeName, object AttributeValue)
        {
            this.AddCustomAttribute(AttributeName.FullName, AttributeValue);
        }

        public void SetAttributeByName(string AttributeName, object AttributeValue)
        {
            this.AddCustomAttribute(AttributeName, AttributeValue);
        }


    }

    public static class IB_CoilHeatingWater_Attributes
    {
        private static readonly CoilHeatingWater refObj = new CoilHeatingWater(new Model());
        

        //https://openstudio-sdk-documentation.s3.amazonaws.com/cpp/OpenStudio-2.4.0-doc/model/html/classopenstudio_1_1model_1_1_coil_heating_water.html

        public static readonly DataAttribute RatedInletWaterTemperature 
            = new DataAttribute("RatedInletWaterTemperature", "InWaterTemp", refObj);

        public static readonly DataAttribute RatedInletAirTemperature
            = new DataAttribute("RatedInletAirTemperature", "InAirTemp", refObj);

        public static readonly DataAttribute RatedOutletWaterTemperature 
            = new DataAttribute("RatedOutletWaterTemperature", "OutWaterTemp", refObj);

        public static readonly DataAttribute RatedOutletAirTemperature
            = new DataAttribute("RatedOutletAirTemperature", "OutAirTemp", refObj);

        public static readonly DataAttribute UFactorTimesAreaValue
            = new DataAttribute("UFactorTimesAreaValue", "UFactor", refObj);

        public static readonly DataAttribute MaximumWaterFlowRate
            = new DataAttribute("MaximumWaterFlowRate", "MaxFlow", refObj);

        public static readonly DataAttribute RatedRatioForAirAndWaterConvection
            = new DataAttribute("RatedRatioForAirAndWaterConvection", "AirWaterRatio", refObj);
        

        public static IEnumerable<DataAttribute> GetList()
        {
            return typeof(IB_CoilHeatingWater_Attributes).GetFields()
                            .Select(_ => (DataAttribute)_.GetValue(null));
        }

        public static DataAttribute GetAttributeByName(string name)
        {
            var field = typeof(IB_CoilHeatingWater_Attributes).GetField(name);
            return (DataAttribute)field.GetValue(null);
        }

        
    }

    public class DataAttribute
    {
        public string FullName { get; set; }
        public string PerfectName { get; set; }
        public string ShortName { get; set; }
        public Type Type { get; set; }
        //public string Unit { get; set; }

        public DataAttribute(string fullName, string shortName,  HVACComponent com)
        {
            this.FullName = fullName; //RatedInletWaterTemperature
            this.ShortName = shortName; //InWaterTemp

            this.PerfectName = CheckName(this.FullName); ////Rated Inlet Water Temperature
            var methodName = Char.ToLowerInvariant(this.FullName[0]) + this.FullName.Substring(1); //ratedInletWaterTemperature

            this.Type = com.GetType().GetMethod(methodName).ReturnType;
            
        }
        
        private static string CheckName(string LongName)
        {
            var r = new System.Text.RegularExpressions.Regex(@"(?<=[A-Z])(?=[A-Z][a-z]) |(?<=[^A-Z])(?=[A-Z]) |(?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);
            return r.Replace(LongName, " ");
        }

        public string Unit(bool IP = false)
        {
            return "ddd";
        }

    }

}
