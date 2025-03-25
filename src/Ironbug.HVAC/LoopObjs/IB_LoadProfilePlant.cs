using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_LoadProfilePlant : IB_HVACObject, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_LoadProfilePlant();
        private static LoadProfilePlant NewDefaultOpsObj(Model model)
            => new LoadProfilePlant(model);
        

        public IB_LoadProfilePlant():base(NewDefaultOpsObj)
        {
        }
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
        
    }

    public sealed class IB_LoadProfilePlant_FieldSet
        : IB_FieldSet<IB_LoadProfilePlant_FieldSet, LoadProfilePlant>
    {
        private IB_LoadProfilePlant_FieldSet() { }
    }
}
