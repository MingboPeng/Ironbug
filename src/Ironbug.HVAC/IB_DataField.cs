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
        public Type Type { get; set; }
        //public string Unit { get; set; }

        public IB_DataField(string fullName, string shortName, Type typeobj)
        {
            this.FullName = fullName; //RatedInletWaterTemperature
            this.ShortName = shortName; //InWaterTemp

            this.PerfectName = CheckName(this.FullName); ////Rated Inlet Water Temperature
            var methodName = Char.ToLowerInvariant(this.FullName[0]) + this.FullName.Substring(1); //ratedInletWaterTemperature

            //this.Type = com.GetType().GetMethod(methodName).ReturnType;
            this.Type = typeobj;

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
