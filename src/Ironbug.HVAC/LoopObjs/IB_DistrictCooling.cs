using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public class IB_DistrictCooling: IB_HVACObject, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_DistrictCooling();

        private static DistrictCooling NewDefaultOpsObj(Model model) => new DistrictCooling(model);
        public IB_DistrictCooling() : base(NewDefaultOpsObj(new Model()))
        {
        }

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }
    public sealed class IB_DistrictCooling_FieldSet
        : IB_FieldSet<IB_DistrictCooling_FieldSet, DistrictCooling>
    {
        private IB_DistrictCooling_FieldSet() { }
        public IB_Field Name { get; }
            = new IB_BasicField("Name", "Name");
        public IB_Field NominalCapacity { get; }
            = new IB_BasicField("NominalCapacity", "Capacity");
    }

}
