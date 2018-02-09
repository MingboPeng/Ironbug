using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public class IB_DataField
    {
        public string FullName { get; set; }
        public string PerfectName { get; set; }
        public string ShortName { get; set; }
        public string GetterMethodName { get; set; }
        public string SetterMethodName { get; set; }
        public Type DataType { get; set; }
        public bool ProSetting { get; set; }
        //public string Unit { get; set; }

        public IB_DataField(string FullName, string ShortName, Type DataType)
            : this(FullName, ShortName, DataType, true)
        {
        }

        public IB_DataField(string FullName, string ShortName, Type DataType, bool ProSetting)
        {
            this.FullName = FullName; //RatedInletWaterTemperature
            this.ShortName = ShortName; //InWaterTemp

            this.PerfectName = CheckName(this.FullName); ////Rated Inlet Water Temperature
            this.GetterMethodName = Char.ToLowerInvariant(this.FullName[0]) + this.FullName.Substring(1); //ratedInletWaterTemperature
            this.SetterMethodName = "set" + this.FullName;

            //this.Type = com.GetType().GetMethod(methodName).ReturnType;
            this.DataType = DataType;
            this.ProSetting = ProSetting;
        }

        private static string CheckName(string LongName)
        {
            var r = new System.Text.RegularExpressions.Regex(@"(?<=[A-Z])(?=[A-Z][a-z]) |(?<=[^A-Z])(?=[A-Z]) |(?<=[A-Za-z])(?=[^A-Za-z])", System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace);
            return r.Replace(LongName, " ");
        }

        public string Unit(bool IP = false)
        {
            return "ddd";
        }

    }
    
}
