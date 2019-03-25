using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_SizingPlant : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SizingPlant();

        private static SizingPlant NewDefaultOpsObj(Model model) => new SizingPlant(model, new PlantLoop(model));

        public IB_SizingPlant() : base(NewDefaultOpsObj(new Model()))
        {
        }
        

        public ModelObject ToOS(PlantLoop loop)
        {
            var sz = loop.sizingPlant();
            sz.SetCustomAttributes(this.CustomAttributes);
            return sz;
            //create a new sizingPlant to target plant loop
            //var targetModel = loop.model();
            //return base.OnNewOpsObj((Model model) => new SizingPlant(model, loop), targetModel);
        }
        
    }

    public sealed class IB_SizingPlant_FieldSet
        : IB_FieldSet<IB_SizingPlant_FieldSet, SizingPlant>
    {
        private IB_SizingPlant_FieldSet()
        {
        }

        public IB_Field LoopType { get; }
            = new IB_BasicField("LoopType", "type");

        public IB_Field DesignLoopExitTemperature { get; }
            = new IB_BasicField("DesignLoopExitTemperature", "exitT");

        public IB_Field LoopDesignTemperatureDifference { get; }
            = new IB_BasicField("LoopDesignTemperatureDifference", "deltaT");
    }
}