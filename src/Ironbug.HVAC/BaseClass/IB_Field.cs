using OpenStudio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Ironbug.HVAC.BaseClass
{
    [DataContract]
    public class IB_Field : IEquatable<IB_Field>
    {
        public string FULLNAME => FullName.ToUpper();
        [DataMember]
        public string FullName { get; private set; }
        public string PerfectName { get; private set; }
        public string NickName { get; set; }
        //public string GetterMethodName { get; protected set; }
        //public string SetterMethodName { get; protected set; }
      
        public MethodInfo SetterMethod { get;  set; } = null;
        [DataMember]
        public string DataTypeName { get; set; } = typeof(string).FullName;
    
        /// <summary>
        /// Setter method's parameter type used in OpenStudio
        /// </summary>
        public Type DataType => Type.GetType(DataTypeName);
        //public bool IsHidden { get; set; }

        public IEnumerable<string> ValidData { get; private set; } = new List<string>();

        //Description comes with EnergyPlus IDD file
        public string Description { get; protected set; }

        ////Description added manually
        public string DetailedDescription { get; set; }

        public string UnitSI { get; set; }
        public string UnitIP { get; set; }
        protected IB_Field() { }
        internal IB_Field(MethodInfo opsSetterMethod)
            : this(opsSetterMethod.Name.Substring(3), string.Empty, opsSetterMethod.GetParameters().First().ParameterType)
        {
            //TODO: check Curve type
            this.SetterMethod = opsSetterMethod;
        }

        protected IB_Field(IB_Field otherField)
            : this(otherField.FullName, otherField.NickName, otherField.DataType)
        {
            this.SetterMethod = otherField.SetterMethod;
            this.Description = otherField.Description;
        }
        public IB_Field(string fullName, string nickName, Type valueType = default)
        {

            this.FullName = CheckInputFullName(fullName);//RatedInletWaterTemperature
            this.NickName = string.IsNullOrEmpty(nickName) ? FullName : nickName; //InWaterTemp
            this.PerfectName = MakePerfectName(this.FullName); ////Rated Inlet Water Temperature
            this.DataTypeName = valueType == default ? typeof(string).FullName : valueType.FullName;

            //var assembly = valueType.Assembly.GetName().Name;
            if (this.DataTypeName.StartsWith("OpenStudio."))
            {
                this.DataTypeName = $"{this.DataTypeName}, OpenStudio";
            }

        }
        public void AddDescriptionFromEpNote(string EpNote)
        {
            var dec = string.Empty;

            if (!string.IsNullOrEmpty(EpNote))
            {
                dec = EpNote;
                dec += Environment.NewLine;
                dec += Description;
                dec += Environment.NewLine;
                dec += Environment.NewLine;
                dec += "Above content copyright © 1996-2020 EnergyPlus, all contributors. All rights reserved. EnergyPlus is a trademark of the US Department of Energy.";
                
            }
            else
            {
                dec = "There is no documentation available";
            }

            this.Description = dec;
        }

        public IB_Field UpdateFromIddField(IddField field)
        {
            var prop = field.properties();
            (var validDataItems, var validDataStr) = GetValidData(field);


            //var description = prop.note;
            var description = GetUnitsFromIDD(field);
            description += prop.autosizable ? "\nSet this parameter to -9999 for autosizing." : "";
            description += GetDefaultFromIDD(prop);
            description += validDataStr;

            this.Description = string.IsNullOrEmpty(description)? DetailedDescription: description;

            this.SetValidData(validDataItems);

            return this;

            (IEnumerable<string> Items, string JoinedString) GetValidData(IddField f)
            {
                var strTobeShown = string.Empty;
                var items = new List<string>();
                var keys = f.keys();
                if (keys.Count == 0)
                {
                    return (items, strTobeShown);
                }

                foreach (var item in keys)
                {
                    //TODO: check letter cases, or item.__str__
                    var keyValue = item.name();
                    strTobeShown += "\r\n    -" + keyValue;
                    items.Add(keyValue);

                }

                return (items, "\r\nValid Options:" + strTobeShown);


            }
            
            string GetDefaultFromIDD(IddFieldProperties properties)
            {
                var numDef = properties.numericDefault;
                var strDef = properties.stringDefault;

                
                var strTobeShown = strDef.isNull() ? 
                    numDef.isNull() ? 
                    string.Empty : numDef.get().ToString() : 
                    strDef.get();

          
                if (!string.IsNullOrWhiteSpace(strTobeShown))
                {
                    var unit = string.IsNullOrWhiteSpace(this.UnitSI) ? string.Empty : $" {this.UnitSI}";
                    strTobeShown = strTobeShown.ToLower().StartsWith("autosize") ? strTobeShown : $"{strTobeShown} {unit}";
                    return $"\r\nDefault: {strTobeShown}";
                }
                else
                {
                    return string.Empty;
                }

            }

            string GetUnitsFromIDD(IddField f)
            {
                if (!f.IsRealType())
                {
                    return string.Empty;
                }
                var unit = f.getUnits();
                var unitIP = f.getUnits(true);
                if (unit.isNull())
                {
                    return string.Empty;
                }

                var strTobeShown = unit.get().standardString();
                var prettyStr =unit.get().prettyString();

                var strTobeShownIP = unitIP.get().standardString();
                var prettyStrIP = unitIP.get().prettyString();

                this.UnitSI = string.IsNullOrEmpty(prettyStr) ? strTobeShown : prettyStr;
                this.UnitIP = string.IsNullOrEmpty(prettyStrIP) ? strTobeShownIP : prettyStrIP;
                this.UnitIP = string.IsNullOrEmpty(UnitIP) ? UnitSI : UnitIP;

                if (string.IsNullOrEmpty(UnitSI))
                {
                    return string.Empty;
                }
                else if (UnitSI == UnitIP)
                {
                    return $"\r\nUnit: {UnitSI}";
                }
                else
                {
                    return $"\r\nUnit: {UnitSI} [IP: {UnitIP}]";
                }
                
            }
        }


      
        
        private static string CheckInputFullName(string fullName)
        {
            var spacedName = MakePerfectName(fullName);
            var cleanFullName = new CultureInfo("en-us", false).TextInfo.ToTitleCase(spacedName); //ToTitleCase
            cleanFullName = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9]").Replace(cleanFullName, string.Empty);

            return cleanFullName;
        }

        //protected IB_Field SetAcceptiableDataType(Type DataType)
        //{
        //    this.DataType = DataType;
        //    return this;
        //}

        protected IB_Field SetValidData(IEnumerable<string> ValidData)
        {
            this.ValidData = ValidData;
            return this;
        }

        private static string MakePerfectName(string LongName)
        {
            var r = new System.Text.RegularExpressions.Regex(@"(?<=[A-Z])(?=[A-Z][a-z]) |(?<=[^A-Z])(?=[A-Z]) |(?<=[A-Za-z])(?=[^A-Za-z])", System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace);
            return r.Replace(LongName, " ");
        }

        public double ConvertToSI(double ValueIP)
        {
            if ((this.UnitIP == this.UnitSI)|| string.IsNullOrEmpty(this.UnitSI))
            {
                return ValueIP;
            }
            var valueOp = OpenStudioUtilitiesUnits.convert(ValueIP, this.UnitIP, this.UnitSI);
            if (valueOp.is_initialized())
            {
                return valueOp.get();
            }
            else
            {
                throw new ArgumentException($"Failed to convert {ValueIP} from {this.UnitIP} to {UnitSI}");
            }
            
        }

        public override bool Equals(object obj) => this.Equals(obj as IB_Field);
        public bool Equals(IB_Field other)
        {
            if (other == null)
                return false;
            return this.FullName == other.FullName && this.DataType == other.DataType;
        }
    
        public static bool operator ==(IB_Field x, IB_Field y)
        {
            if (x is null)
                return y is null ? true : false;
            return x.Equals(y);
        }

        public static bool operator !=(IB_Field x, IB_Field y) => !(x == y);

        public int GetHashCode(IB_Field obj)
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
    /// and optional for setting the hvac object up at the first place.
    /// 
    /// </summary>
    public class IB_TopField : IB_Field
    {
        private IB_TopField() : base() { }
        public IB_TopField(string FullName, string ShortName)
            : base(FullName, ShortName)
        {

        }
    }

    public class IB_BasicField : IB_Field
    {
        private IB_BasicField() : base() { }
        public IB_BasicField(string FullName, string ShortName) 
            :base(FullName, ShortName)
        {
            
        }
    }
    
    public sealed class IB_Field_Comment
    {
        private static readonly Lazy<IB_Field> lazy =
            new Lazy<IB_Field>(() => new IB_Field("Comment", "Comment"));

        public static IB_Field Instance => lazy.Value;

        private IB_Field_Comment()
        {
        }
    }

    public sealed class IB_Field_Name
    {
        private static readonly Lazy<IB_Field> lazy =
            new Lazy<IB_Field>(() => new IB_Field("Name", "Name"));

        public static IB_Field Instance => lazy.Value;

        private IB_Field_Name()
        {
        }
    }

}
