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

        private static DistrictHeating InitMethod(Model model) => new DistrictHeating(model);
        public IB_DistrictHeating() : base(InitMethod(new Model()))
        {
        }
        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((DistrictHeating)this.ToOS(model)).addToNode(node);
        }
        

        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethod, model).to_DistrictHeating().get();
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
