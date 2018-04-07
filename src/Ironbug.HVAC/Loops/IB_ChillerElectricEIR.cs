using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ChillerElectricEIR : IB_HVACObject, IIB_PlantLoopObjects
    {
        private static ChillerElectricEIR InitMethod(Model model) => new ChillerElectricEIR(model);
        public IB_ChillerElectricEIR() : base(InitMethod(new Model()))
        {
        }
        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((ChillerElectricEIR)this.ToOS(model)).addToNode(node);
        }

        public override IB_ModelObject Duplicate()
        {
            return base.DuplicateIBObj(() => new IB_ChillerElectricEIR());
        }

        public override ModelObject ToOS(Model model)
        {
            return base.ToOS(InitMethod, model).to_ChillerElectricEIR().get();
        }
    }

    public sealed class IB_ChillerElectricEIR_DataFieldSet
        : IB_DataFieldSet<IB_ChillerElectricEIR_DataFieldSet, ChillerElectricEIR>
    {
        private IB_ChillerElectricEIR_DataFieldSet() { }

        public IB_DataField Name { get; }
            = new IB_BasicDataField("Name", "Name");
        public IB_DataField ReferenceCapacity { get; }
            = new IB_BasicDataField("ReferenceCapacity", "Capacity");
        
        public IB_DataField ReferenceCOP { get; }
            = new IB_BasicDataField("ReferenceCOP", "COP");
        
        public IB_DataField ReferenceLeavingChilledWaterTemperature { get; }
            = new IB_BasicDataField("ReferenceLeavingChilledWaterTemperature", "LeavingT");
    }
}
