using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_WaterHeaterSizing : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_WaterHeaterSizing();

        private static WaterHeaterSizing NewDefaultOpsObj(Model model) => new WaterHeaterMixed(model).waterHeaterSizing();

        public IB_WaterHeaterSizing():base(NewDefaultOpsObj)
        {
        }
        
        public ModelObject ToOS(Model model, WaterHeaterMixed waterHeater)
        {
            var sz = waterHeater.waterHeaterSizing();
            sz.SetCustomAttributes(model, this.CustomAttributes);
            return sz;
        }
        
        
    }

    public sealed class IB_WaterHeaterSizing_FieldSet 
        : IB_FieldSet<IB_WaterHeaterSizing_FieldSet, WaterHeaterSizing>
    {
        private IB_WaterHeaterSizing_FieldSet() { }
    }
}
