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
        private static DistrictHeating InitMethod(Model model) => new DistrictHeating(model);
        public IB_DistrictHeating() : base(InitMethod(new Model()))
        {
        }
        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((DistrictHeating)this.ToOS(model)).addToNode(node);
        }

        public override IB_ModelObject Duplicate()
        {
            return base.DuplicateIBObj(() => new IB_DistrictHeating());
        }

        public override ModelObject ToOS(Model model)
        {
            return base.ToOS(InitMethod, model).to_DistrictHeating().get();
        }
    }

    public sealed class IB_DistrictHeating_DataFieldSet
        : IB_DataFieldSet<IB_DistrictHeating_DataFieldSet, DistrictHeating>
    {
        private IB_DistrictHeating_DataFieldSet() { }
        
        public IB_DataField Name { get; }
            = new IB_BasicDataField("Name", "Name");
        public IB_DataField NominalCapacity { get; }
            = new IB_BasicDataField("NominalCapacity", "Capacity");

    }
}
