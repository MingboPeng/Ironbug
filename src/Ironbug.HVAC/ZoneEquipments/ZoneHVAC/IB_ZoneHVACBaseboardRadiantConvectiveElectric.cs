using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ZoneHVACBaseboardRadiantConvectiveElectric : BaseClass.IB_ZoneEquipment
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ZoneHVACBaseboardRadiantConvectiveElectric();

        private static ZoneHVACBaseboardRadiantConvectiveElectric NewDefaultOpsObj(Model model) 
            => new ZoneHVACBaseboardRadiantConvectiveElectric(model);
        

        public IB_ZoneHVACBaseboardRadiantConvectiveElectric() : base(NewDefaultOpsObj(new Model()))
        {
        }

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_ZoneHVACBaseboardRadiantConvectiveElectric_DataFieldSet
        : IB_FieldSet<IB_ZoneHVACBaseboardRadiantConvectiveElectric_DataFieldSet, ZoneHVACBaseboardRadiantConvectiveElectric>
    {
        private IB_ZoneHVACBaseboardRadiantConvectiveElectric_DataFieldSet() { }

    }
}
