using OpenStudio;
using System;
using System.Collections.Generic;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_ElecInverter : IB_ModelObject
    {
        public IB_ElecInverter(Func<OpenStudio.Model, ModelObject> ghostObjInit) : base(ghostObjInit)
        {
        }
        public abstract Inverter ToOS(Model model);

    }
}
