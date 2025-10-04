using OpenStudio;
using System;
using System.Collections.Generic;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_Generator: IB_ModelObject
    {
        public IB_Generator(Func<OpenStudio.Model, ModelObject> ghostObjInit) : base(ghostObjInit)
        {
        }
        public abstract Generator ToOS(Model model);

    }
}
