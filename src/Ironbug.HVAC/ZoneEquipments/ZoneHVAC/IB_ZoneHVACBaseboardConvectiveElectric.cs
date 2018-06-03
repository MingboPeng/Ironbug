using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ZoneHVACBaseboardConvectiveElectric : BaseClass.IB_ZoneEquipment
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ZoneHVACBaseboardConvectiveElectric();

        private static ZoneHVACBaseboardConvectiveElectric InitMethod(Model model) 
            => new ZoneHVACBaseboardConvectiveElectric(model);
        

        public IB_ZoneHVACBaseboardConvectiveElectric() : base(InitMethod(new Model()))
        {
        }

        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethod, model).to_ZoneHVACBaseboardConvectiveElectric().get();
        }
    }

    public sealed class IB_ZoneHVACBaseboardConvectiveElectric_DataFieldSet
        : IB_FieldSet<IB_ZoneHVACBaseboardConvectiveElectric_DataFieldSet, ZoneHVACBaseboardConvectiveElectric>
    {
        private IB_ZoneHVACBaseboardConvectiveElectric_DataFieldSet() { }

    }
}
