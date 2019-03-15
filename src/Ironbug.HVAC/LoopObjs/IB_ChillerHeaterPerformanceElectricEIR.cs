
using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ChillerHeaterPerformanceElectricEIR: IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ChillerHeaterPerformanceElectricEIR();

        private static ChillerHeaterPerformanceElectricEIR NewDefaultOpsObj(Model model) => new ChillerHeaterPerformanceElectricEIR(model);
        
        public IB_ChillerHeaterPerformanceElectricEIR() : base(NewDefaultOpsObj(new Model()))
        {
        }

        public ModelObject ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_ChillerHeaterPerformanceElectricEIR_DataFields
        : IB_FieldSet<IB_ChillerHeaterPerformanceElectricEIR_DataFields, ChillerHeaterPerformanceElectricEIR>
    {
        private IB_ChillerHeaterPerformanceElectricEIR_DataFields() { }

    }


}