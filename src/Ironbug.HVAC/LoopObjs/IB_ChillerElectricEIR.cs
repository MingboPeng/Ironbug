using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ChillerElectricEIR : IB_HVACObject, IIB_DualLoopObj, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ChillerElectricEIR();

        private static ChillerElectricEIR InitMethod(Model model) => new ChillerElectricEIR(model);
        public IB_ChillerElectricEIR() : base(InitMethod(new Model()))
        {
            
        }
        public override bool AddToNode(Node node)
        {
            var model = node.model();

            return ((ChillerElectricEIR)this.ToOS(model)).addToNode(node);
        }
        
        protected override ModelObject InitOpsObj(Model model)
        {
            ChillerElectricEIR postProcess(ModelObject _) => _.to_ChillerElectricEIR().get();
            return base.OnInitOpsObj(InitMethod, model, postProcess);
        }
    }

    public sealed class IB_ChillerElectricEIR_DataFieldSet
        : IB_FieldSet<IB_ChillerElectricEIR_DataFieldSet, ChillerElectricEIR>
    {
        private IB_ChillerElectricEIR_DataFieldSet() { }

        public IB_Field Name { get; }
            = new IB_BasicField("Name", "Name");
        public IB_Field ReferenceCapacity { get; }
            = new IB_BasicField("ReferenceCapacity", "Capacity");
        
        public IB_Field ReferenceCOP { get; }
            = new IB_BasicField("ReferenceCOP", "COP");
        
        public IB_Field ReferenceLeavingChilledWaterTemperature { get; }
            = new IB_BasicField("ReferenceLeavingChilledWaterTemperature", "LeavingT");
    }
}
