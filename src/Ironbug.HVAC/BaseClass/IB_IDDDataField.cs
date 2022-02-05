using OpenStudio;
using System;
using System.Collections.Generic;

namespace Ironbug.HVAC.BaseClass
{
    public class IB_IddField : IB_Field
    {
        private IB_IddField() : base(null, null) { }
        public IB_IddField(IB_Field IBField, IddField field)
            : base(IBField)
        {
            //var name = iddField.name();
            var prop = field.properties();
            (var validDataItems, var validDataStr) = GetValidData(field);
            

            var description = prop.note;
            description += GetDefaultFromIDD(prop);
            description += GetUnitsFromIDD(field);
            description += validDataStr;
            
            this.Description = description;
            //base.SetAcceptiableDataType(GetDataTypeFromIDD(field));
            base.SetValidData(validDataItems);
        }

        
        private static string GetDefaultFromIDD(IddFieldProperties properties)
        {
            var numDef = properties.numericDefault;
            var strDef = properties.stringDefault;


            var strTobeShown = 
                strDef.isNull() ? 
                numDef.isNull()? string.Empty: numDef.get().ToString() : 
                strDef.get();

            if (!string.IsNullOrWhiteSpace(strTobeShown))
            {
                return "\r\nDefault: " + strTobeShown;
            }
            else
            {
                return string.Empty;
            }
            
        }
        //private static Type GetDataTypeFromIDD(IddField field)
        //{
        //    var dataType = field.properties().type.valueDescription();


        //    //real, choice, alpha, integer ....
        //    if (dataType == "real")
        //    {
        //        return typeof(double);
        //    }
        //    else if (dataType == "alpha")
        //    {
        //        return typeof(string);
        //    }
        //    else if (dataType == "integer")
        //    {
        //        return typeof(int);
        //    }
        //    else
        //    {
        //        return typeof(object);
        //    }
            
        //}

        //public IB_IddField UpdateFromOpenStudioMethod(string getterName, Type type)
        //{
        //    this.GetterMethodName = getterName.ToLower()[0] + getterName.Substring(1);
        //    this.SetterMethodName = "set" + getterName;
        //    this.SetAcceptiableDataType(type);
            

        //    return this;
        //}

        private static string GetUnitsFromIDD(IddField field)
        {
            var unit = field.getUnits();
            
            var strTobeShown = unit.isNull() ? string.Empty : unit.get().standardString();
            var prettyStr = unit.isNull() ? string.Empty : unit.get().prettyString();
            

            if (!string.IsNullOrWhiteSpace(prettyStr))
            {
                return "\r\nUnit: " + prettyStr;
            }
            else
            {
                return strTobeShown;
            }
        }

        private static (IEnumerable<string> Items, string JoinedString) GetValidData(IddField field)
        {
            var strTobeShown = string.Empty;
            var items = new List<string>();
            var keys = field.keys();
            if (keys.Count ==0)
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


        
    }


}
