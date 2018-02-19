using System;
using System.Collections.Generic;
using System.Linq;
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

        protected static IEnumerable<IB_DataField> GetList<T>() where T : IB_DataFieldSet
        {
            return typeof(T).GetFields()
                            .Select(_ => (IB_DataField)_.GetValue(null));
        }

        protected static IB_DataField GetAttributeByName<T>(string name) where T : IB_DataFieldSet
        {
            var field = typeof(T).GetField(name);
            return (IB_DataField)field.GetValue(null);
        }
        
    }
}
