using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ZoneHVACBaseboardRadiantConvectiveElectric : BaseClass.IB_ZoneEquipment
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ZoneHVACBaseboardRadiantConvectiveElectric();

        private static ZoneHVACBaseboardRadiantConvectiveElectric InitMethod(Model model) 
            => new ZoneHVACBaseboardRadiantConvectiveElectric(model);
        

        public IB_ZoneHVACBaseboardRadiantConvectiveElectric() : base(InitMethod(new Model()))
        {
        }

        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethod, model).to_ZoneHVACBaseboardRadiantConvectiveElectric().get();
        }
    }

    public sealed class IB_ZoneHVACBaseboardRadiantConvectiveElectric_DataFieldSet
        : IB_DataFieldSet<IB_ZoneHVACBaseboardRadiantConvectiveElectric_DataFieldSet, ZoneHVACBaseboardRadiantConvectiveElectric>
    {
        private IB_ZoneHVACBaseboardRadiantConvectiveElectric_DataFieldSet() { }

    }
}
