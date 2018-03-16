using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_DataFieldSet
    {
        protected static readonly Type dbType = typeof(double);
        protected static readonly Type intType = typeof(int);
        protected static readonly Type strType = typeof(string);
        protected static readonly Type boType = typeof(bool);

        

        protected abstract OpenStudio.IddObject RefIddObject { get; }
        protected abstract Type ParentType { get; }

        public readonly IB_MasterDataField AllAvailableSettings;

        public IB_DataFieldSet()
        {
            this.AllAvailableSettings = AllAvailableSettingNames();
        }

        public IEnumerable<IB_DataField> GetList()
        {
            return this.GetType().GetFields()
                            .Select(_ => (IB_DataField)_.GetValue(this));
        }

        public IB_DataField GetAttributeByName(string name)
        {
            var field = this.GetType().GetField(name);
            return (IB_DataField)field.GetValue(this);
        }

        private IEnumerable<IB_DataField> GetAllAvailableSettings()
        {
            var masterSettings = this.ParentType
                .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(_ => (
                    _.Name.StartsWith("set") &&
                    _.GetParameters().Count() == 1 &&
                    (
                        _.GetParameters().First().ParameterType == typeof(string) ||
                        _.GetParameters().First().ParameterType == typeof(double)
                    )
                    )
                ).Select(_ => new IB_DataField(_.Name.Substring(3),"NoShortName",_.GetParameters().First().ParameterType))
                .OrderBy(_ => _.FullName);

            return masterSettings;
        }

        public IB_MasterDataField AllAvailableSettingNames()
        {

            var masterSettings = this.GetAllAvailableSettings();
            var description = "This gives you an option that if you are looking for a setting that is not listed above,"+
                "please feel free to pick any setting from following items, "+
                "but please double check the EnergyPlus Input References to ensure you know what you are doing.\r\n\r\n";
            description += "TDDO: show an example to explain how to use this!\r\n\r\n";
            description += string.Join("\r\n", masterSettings.Select(_=>_.FullName));

            //TODO: there must be a better way to do this.
            var masterDataFieldMap = new Dictionary<string, IB_DataField>();
            foreach (var item in masterSettings)
            {
                masterDataFieldMap.Add(item.FullName.ToUpper(), item);
            }

            var df = new IB_MasterDataField(description, masterDataFieldMap);
            return df;

        }



    }
}
