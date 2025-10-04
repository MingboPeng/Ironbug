using OpenStudio;
using System;
using System.Collections.Generic;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_ElecStorage : IB_ModelObject
    {
        public IB_ElecStorage(Func<OpenStudio.Model, ModelObject> ghostObjInit) : base(ghostObjInit)
        {
        }
        public abstract ElectricalStorage ToOS(Model model);

    }
}
