using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_DistrictHeatingSteam : IB_HVACObject, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_DistrictHeatingSteam();

        private static DistrictHeatingSteam NewDefaultOpsObj(Model model) => new DistrictHeatingSteam(model);
        public IB_DistrictHeatingSteam() : base(NewDefaultOpsObj(new Model()))
        {
        }

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_DistrictHeatingSteam_FieldSet
    : IB_FieldSet<IB_DistrictHeatingSteam_FieldSet, DistrictHeatingSteam>
    {
        private IB_DistrictHeatingSteam_FieldSet() { }

    }
}
