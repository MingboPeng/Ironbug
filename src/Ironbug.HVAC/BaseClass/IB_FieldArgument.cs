using System;
using System.Runtime.Serialization;

namespace Ironbug.HVAC.BaseClass
{
    [DataContract]
    public class IB_FieldArgument: IEquatable<IB_FieldArgument>
    {
        [DataMember]
        public IB_Field Field { get; internal set; }
       
        private object _value;
        [DataMember]
        public object Value {
            get 
            {
                return _value;
            }
            internal set
            {
                _value = value.GetType().IsSubclassOf(typeof(IB_ModelObject)) ? value: Convert.ChangeType(value, Field.DataType);
            }
        }
        private IB_FieldArgument() { }
        public IB_FieldArgument(IB_Field field, object value)
        {
            this.Field = field;
            this.Value = value;
        }
        public override string ToString()
        {
            return this.Value.ToString();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as IB_FieldArgument);
        }

        public bool Equals(IB_FieldArgument other)
        {
            if (other is null)
                return this is null ? true : false;
            if (!this.Field.Equals(other.Field))
                return false;

            if (this.Value.GetType().IsSubclassOf(typeof(IB_ModelObject)))
            {
                return this.Value.Equals(other.Value);
            }
            else
            {
                return this.Value.ToString().Equals(other.Value.ToString());
            }

        }
        public static bool operator == (IB_FieldArgument x, IB_FieldArgument y) {
            if (x is null)
                return y is null ? true : false;
            return x.Equals(y);
        }

        public static bool operator !=(IB_FieldArgument x, IB_FieldArgument y) => !(x == y);
    }
}
