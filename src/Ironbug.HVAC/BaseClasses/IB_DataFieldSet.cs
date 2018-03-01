using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ironbug.HVAC
{
    public abstract class IB_DataFieldSet
    {
        protected static readonly Type dbType = typeof(double);
        protected static readonly Type intType = typeof(int);
        protected static readonly Type strType = typeof(string);
        protected static readonly Type boType = typeof(bool);

        

        protected abstract OpenStudio.IddObject RefIddObject { get; }
        protected abstract Type ParentType { get; }

        public IEnumerable<IB_DataField> GetList()
        {
            return this.GetType().GetFields()
                            .Select(_ => (IB_DataField)_.GetValue(null));
        }

        public IB_DataField GetAttributeByName(string name)
        {
            var field = this.GetType().GetField(name);
            return (IB_DataField)field.GetValue(null);
        }

        private IEnumerable<string> GetAllAvailableSettings()
        {
            var methods = this.ParentType
                .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(_ => (
                    _.Name.StartsWith("set") &&
                    _.GetParameters().Count() == 1 &&
                    (
                        _.GetParameters().First().ParameterType == typeof(string) ||
                        _.GetParameters().First().ParameterType == typeof(double)
                    )
                    )
                ).Select(_ => _.Name.Substring(3))
                .OrderBy(_ => _);

            return methods;
        }

        public IB_DataField AvailableSettingNames()
        {
            var names = this.GetAllAvailableSettings();
            var stringList = string.Join("\\n", names);
            var df = new IB_DataField("All", "all", strType);
            df.Description = stringList;
            return df;

        }



    }
}
