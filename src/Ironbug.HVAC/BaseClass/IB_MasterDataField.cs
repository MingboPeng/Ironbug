
using System;
using System.Collections.Generic;
using System.Linq;
using Ironbug.Core;

namespace Ironbug.HVAC.BaseClass
{
    public class IB_MasterDataField:IB_DataField
    {
        private IDictionary<string, IB_DataField> settings;
        public IB_MasterDataField(string Description, IDictionary<string, IB_DataField> Settings):base("AllAvailableSettings", "MasterSettings", typeof(string), false)
        {
            base.Description = Description;
            this.settings = Settings;
        }

        public Dictionary<IB_DataField, object> CheckUserInputs(IEnumerable<string> UserInputs)
        {
            var dic = new Dictionary<IB_DataField, object>();

            foreach (var item in UserInputs)
            {
                if (!item.Contains(','))
                {
                    throw new System.Exception(item + " is missing \",\" (comma), please use comma to seperate the parameter name and its value!");
                }

                var strs = item.Split(',');
                var settingName = strs[0].Trim();
                var settingValue = strs[1].Trim();

                if (!this.settings.ContainsKey(settingName.ToUpper()))
                {
                    throw new System.Exception(settingName + " is not available, please double check the spelling!");
                }

                if (string.IsNullOrWhiteSpace(settingValue))
                {
                    throw new System.Exception(settingName + " has no value, and it cannot be empty!");
                }

                var dataField = this.settings[settingName.ToUpper()];

                var value = Convert.ChangeType(settingValue, dataField.DataType);
                

                dic.TryAdd(dataField, value);
                

            }

            return dic;
        }
        
    }
    
}
