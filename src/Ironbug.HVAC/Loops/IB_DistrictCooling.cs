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
        private static DistrictCooling InitMethod(Model model) => new DistrictCooling(model);
        public IB_DistrictCooling() : base(InitMethod(new Model()))
        {
        }
        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((DistrictCooling)this.ToOS(model)).addToNode(node);
        }

        public override IB_ModelObject Duplicate()
        {
            return base.DuplicateIBObj(() => new IB_DistrictCooling());
        }

        public override ModelObject ToOS(Model model)
        {
            return base.ToOS(InitMethod, model).to_DistrictCooling().get();
        }
    }
    public sealed class IB_DistrictCooling_DataFieldSet
        : IB_DataFieldSet<IB_DistrictCooling_DataFieldSet, DistrictCooling>
    {
        private IB_DistrictCooling_DataFieldSet() { }
        public IB_DataField Name { get; }
            = new IB_BasicDataField("Name", "Name");
        public IB_DataField NominalCapacity { get; }
            = new IB_BasicDataField("NominalCapacity", "Capacity");
    }

}
