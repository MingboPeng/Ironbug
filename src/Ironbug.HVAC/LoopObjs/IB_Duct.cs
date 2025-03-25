using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_Duct : IB_HVACObject, IIB_AirLoopObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_Duct();
        private static Duct NewDefaultOpsObj(Model model)
            => new Duct(model);
        

        public IB_Duct():base(NewDefaultOpsObj)
        {
        }
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
        
    }

    public sealed class IB_Duct_FieldSet
        : IB_FieldSet<IB_Duct_FieldSet, Duct>
    {
        private IB_Duct_FieldSet() { }
        public IB_Field Name { get; }
            = new IB_BasicField("Name", "Name");

    }
}
