using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public class IB_SizingPlant: IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SizingPlant();

        private static SizingPlant InitMethod(Model model) => new SizingPlant(model, new PlantLoop(model));
        

        public IB_SizingPlant() : base(InitMethod(new Model()))
        {
        }
        public override IB_ModelObject Duplicate()
        {
            return base.DuplicateIBObj(IB_InitSelf);
        }

        public ModelObject ToOS(PlantLoop loop)
        {
            //create a new sizingPlant to target plant loop
            var targetModel = loop.model();
            return base.OnInitOpsObj((Model model) => new SizingPlant(model, loop), targetModel);
        }
        //this is replaced by above method
        protected override ModelObject InitOpsObj(Model model)
        {
            throw new NotImplementedException();
        }

    }

    public sealed class IB_SizingPlant_DataFieldSet
        : IB_FieldSet<IB_SizingPlant_DataFieldSet, SizingPlant>
    {

        private IB_SizingPlant_DataFieldSet() { }
        
        public IB_Field LoopType { get; }
            = new IB_BasicField("LoopType", "type");

        public IB_Field DesignLoopExitTemperature { get; }
            = new IB_BasicField("DesignLoopExitTemperature", "exitT");

        public IB_Field LoopDesignTemperatureDifference { get; }
            = new IB_BasicField("LoopDesignTemperatureDifference", "deltaT");


    }

}

