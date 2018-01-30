using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.Ladybug
{ 
    public abstract class IronbugBase
    {
        protected dynamic RawObj { get; set; }
        public object getRawObj()
        {
            return this.RawObj;
        }
    }
}
