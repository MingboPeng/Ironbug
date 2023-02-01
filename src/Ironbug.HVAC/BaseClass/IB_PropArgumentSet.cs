using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Ironbug.HVAC.BaseClass
{
    [DataContract]
    public class IB_PropArgumentSet : Dictionary<string, object>, System.IEquatable<IB_PropArgumentSet>
    {
      
        public IB_PropArgumentSet Duplicate()
        {
            var dup = new IB_PropArgumentSet();
            foreach (var item in this)
            {
                var pv = item.Value;
                if (pv is IB_ModelObject mo)
                    pv = mo.Duplicate();
                dup.Add(item.Key, pv);
            }
            return dup;
        }
        public bool Equals(IB_PropArgumentSet other)
        {
            return this.SequenceEqual(other);
        }
    }

}
