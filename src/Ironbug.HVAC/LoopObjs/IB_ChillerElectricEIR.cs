using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ChillerElectricEIR : IB_HVACObject, IIB_DualLoopObj, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ChillerElectricEIR();

        private static ChillerElectricEIR NewDefaultOpsObj(Model model) => new ChillerElectricEIR(model);
        public IB_ChillerElectricEIR() : base(NewDefaultOpsObj(new Model()))
        {
            
        }

        public override HVACComponent ToOS(Model model)
        {
             return base.OnNewOpsObj(NewDefaultOpsObj, model);
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
