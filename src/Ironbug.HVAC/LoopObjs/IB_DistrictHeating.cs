using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public class IB_DistrictHeating : IB_HVACObject, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_DistrictHeating();

        private static DistrictHeating NewDefaultOpsObj(Model model) => new DistrictHeating(model);
        public IB_DistrictHeating() : base(NewDefaultOpsObj(new Model()))
        {
        }

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_DistrictHeating_DataFieldSet
        : IB_FieldSet<IB_DistrictHeating_DataFieldSet, DistrictHeating>
    {
        private IB_DistrictHeating_DataFieldSet() { }
        
        public IB_Field Name { get; }
            = new IB_BasicField("Name", "Name");
        public IB_Field NominalCapacity { get; }
            = new IB_BasicField("NominalCapacity", "Capacity");

    }
}
