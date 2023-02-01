using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Ironbug.HVAC.BaseClass
{
    public class IB_FieldArgumentSet : List<IB_FieldArgument>, System.IEquatable<IB_FieldArgumentSet>
    {

        public IB_FieldArgumentSet()
        {
        }

       
        public void TryAdd(IB_FieldArgument arg)
        {
            var found = this.FirstOrDefault(_ => _.Field == arg.Field);
            if (found == null)
            {
                this.Add(arg);
            }
            else
            {
                found.Value = arg.Value;
            }

        }

        public void TryAdd(IB_Field field, object value)
        {
            var found = this.FirstOrDefault(_ => _.Field == field);
            if (found == null)
            {
                var arg = new IB_FieldArgument(field, value);
                this.Add(arg);
            }
            else
            {
                found.Value = value;
            }
           
        }

        public bool TryGetValue(IB_Field field, out object value)
        {
            var arg = this.FirstOrDefault(_ => _.Field == field);
            value = arg?.Value;
            return arg != null;
        }

        public bool TryGetValue<T>(IB_Field field, out T value)
        {
            var arg = this.FirstOrDefault(_ => _.Field == field);
            var valueObj = arg?.Value;
            if (valueObj is T v)
                value = v;
            else
                value = default(T);
            return arg != null;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as IB_FieldArgumentSet);
        }
        public bool Equals(IB_FieldArgumentSet other)
        {
            if (other is null)
                return this is null ? true : false;

            foreach (var item in this)
            {
                var found = other.FirstOrDefault(_ => _.Field == item.Field);
                if (item != found)
                    return false;
            }
            return true;
        }
        public static bool operator ==(IB_FieldArgumentSet x, IB_FieldArgumentSet y)
        {
            if (x is null)
                return y is null ? true : false;
            return x.Equals(y);
        }

        public static bool operator !=(IB_FieldArgumentSet x, IB_FieldArgumentSet y) => !(x == y);


    }

}
