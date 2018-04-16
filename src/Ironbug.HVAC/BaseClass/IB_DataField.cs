using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC.BaseClass
{
    public class IB_DataField : IEqualityComparer<IB_DataField>
    {
        public string FULLNAME => FullName.ToUpper();
        public string FullName { get; private set; }
        public string PerfectName { get; private set; }
        public string ShortName { get; set; }
        public string GetterMethodName { get; protected set; }
        public string SetterMethodName { get; protected set; } 
        public Type DataType { get; private set; }
        public bool IsHidden { get; set; }

        public IEnumerable<string> ValidData { get; private set; } = new List<string>();

        //Description comes with EnergyPlus IDD file
        public string Description { get; protected set; }

        ////Description added manually
        public string DetailedDescription { get; set; }

        //public string Unit { get; set; }

        //public IB_DataField(string FullName, string ShortName, Type DataType)
        //    : this(FullName, ShortName, DataType, new List<object>())
        //{
        //}

        public IB_DataField(string FullName, string ShortName)
        {
            
            this.FullName = CheckInputFullName(FullName);//RatedInletWaterTemperature
            this.ShortName = ShortName; //InWaterTemp
            
            this.PerfectName = MakePerfectName(this.FullName); ////Rated Inlet Water Temperature

            //this.GetterMethodName = Char.ToLowerInvariant(this.FullName[0]) + this.FullName.Substring(1); //ratedInletWaterTemperature
            this.SetterMethodName = "set" + this.FullName;

            //this.Type = com.GetType().GetMethod(methodName).ReturnType;
            //this.DataType = DataType;
            //this.IsBasicSetting = BasicSetting;
            //this.ValidData = ValidData;
        }

        //protected IB_DataField SetDescription(string Description)
        //{
        //    this.Description = Description;
        //    return this;
        //}
        private static string CheckInputFullName(string fullName)
        {
            var spacedName = MakePerfectName(fullName);
            var cleanFullName = new CultureInfo("en-us", false).TextInfo.ToTitleCase(spacedName); //ToTitleCase
            cleanFullName = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9]").Replace(cleanFullName, string.Empty);

            return cleanFullName;
        }

        protected IB_DataField SetAcceptiableDataType(Type DataType)
        {
            this.DataType = DataType;
            return this;
        }

        protected IB_DataField SetValidData(IEnumerable<string> ValidData)
        {
            this.ValidData = ValidData;
            return this;
        }

        private static string MakePerfectName(string LongName)
        {
            var r = new System.Text.RegularExpressions.Regex(@"(?<=[A-Z])(?=[A-Z][a-z]) |(?<=[^A-Z])(?=[A-Z]) |(?<=[A-Za-z])(?=[^A-Za-z])", System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace);
            return r.Replace(LongName, " ");
        }

        //public string Unit(bool IP = false)
        //{
        //    return "ddd";
        //}

        public bool Equals(IB_DataField x, IB_DataField y)
        {
            return x.FullName == y.FullName && x.DataType == y.DataType;
        }

        public int GetHashCode(IB_DataField obj)
        {
            return obj.FullName.GetHashCode()*47 +  obj.DataType.GetHashCode()*47;
        }

        public override string ToString()
        {
            return this.PerfectName;
        }
    }

    /// <summary>
    /// This class is for date field that is going to be hidden on genetic obj parameter setting,
    /// and mandatory for setting the hvac object up at the first place.
    /// 
    /// </summary>
    public class IB_MandatoryDataField : IB_DataField
    {

        public IB_MandatoryDataField(string FullName, string ShortName)
            : base(FullName, ShortName)
        {

        }
    }

    public class IB_BasicDataField : IB_DataField
    {
        
        public IB_BasicDataField(string FullName, string ShortName) 
            :base(FullName, ShortName)
        {
            
        }
    }

    public class IB_ProDataField : IB_DataField
    {
        public IB_ProDataField(string FullName, string ShortName)
            : base(FullName, ShortName)
        {

        }
    }


}
