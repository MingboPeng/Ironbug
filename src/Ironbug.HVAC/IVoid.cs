using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public interface IVoid<T> where T: OpenStudio.ModelObject
    {
        T Instance();
    }
}
