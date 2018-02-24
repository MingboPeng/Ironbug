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
        


    }
}
