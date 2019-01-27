using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ZoneHVACBaseboardConvectiveElectric : BaseClass.IB_ZoneEquipment
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ZoneHVACBaseboardConvectiveElectric();

        private static ZoneHVACBaseboardConvectiveElectric NewDefaultOpsObj(Model model) 
            => new ZoneHVACBaseboardConvectiveElectric(model);
        

        public IB_ZoneHVACBaseboardConvectiveElectric() : base(NewDefaultOpsObj(new Model()))
        {
        }

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_ZoneHVACBaseboardConvectiveElectric_DataFieldSet
        : IB_FieldSet<IB_ZoneHVACBaseboardConvectiveElectric_DataFieldSet, ZoneHVACBaseboardConvectiveElectric>
    {
        private IB_ZoneHVACBaseboardConvectiveElectric_DataFieldSet() { }

    }
}
