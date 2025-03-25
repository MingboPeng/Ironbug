using OpenStudio;
using System;
using System.Collections.Generic;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_Curve: IB_ModelObject
    {
        // runtime cached data
        protected List<double> _coefficients;
        public IB_Curve(Func<OpenStudio.Model, ModelObject> ghostObjInit) : base(ghostObjInit)
        {
        }

        public abstract Curve ToOS(Model model);
    }
}
