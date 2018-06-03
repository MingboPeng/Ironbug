
using Ironbug.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.HVAC.BaseClass
{
    public class IB_MasterField:IB_Field
    {
        //private IB_DataFieldSet _dataFieldSet;
        public IB_MasterField(string Description)
            :base("AllAvailableSettings", "MasterSettings")
        {
            base.Description = Description;
            //this._dataFieldSet = dataFieldSet;
            //base.SetAcceptiableDataType(typeof(string));
        }
        
        public Dictionary<IB_Field, object> CheckUserInputs(IEnumerable<string> userInputs, IB_FieldSet dataFieldSet)
        {
            var dic = new Dictionary<IB_Field, object>();

            foreach (var item in userInputs)
            {
                if (!item.Contains(','))
                {
                    throw new Exception(item + " is missing \",\" (comma), please use comma to separate the parameter name and its value!");
                }

                var strs = item.Split(',');
                var settingName = strs[0].Trim();
                var settingValue = strs[1].Trim();

                //check data field name
                if (!dataFieldSet.Contains(settingName))
                {
                    throw new Exception(settingName + " is not available, please double check the spelling!");
                }

                var dataField = dataFieldSet.GetByName(settingName);

                //check value
                if (string.IsNullOrWhiteSpace(settingValue))
                {
                    throw new Exception(settingName + " has no value, and it cannot be empty!");
                }

                //check value type 
                
                //var dataField = this._dataFieldSet[settingName.ToUpper()];

                var value = Convert.ChangeType(settingValue, dataField.DataType);
                
                //TODO: need to double check if double added, as dataFiled is a reference type
                dic.TryAdd(dataField, value);
                
            }

            return dic;
        }
        
    }
    
}
