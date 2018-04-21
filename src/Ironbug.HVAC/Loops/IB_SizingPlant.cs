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
        private static SizingPlant InitMethod(Model model) => new SizingPlant(model, new PlantLoop(model));

        public override IB_ModelObject Duplicate()
        {
            return base.DuplicateIBObj(() => new IB_SizingPlant());
        }

        public ModelObject ToOS(PlantLoop loop)
        {
            //create a new sizingPlant to target plant loop
            var targetModel = loop.model();
            return base.ToOS((Model model) => new SizingPlant(model, loop), targetModel);
        }

        public IB_SizingPlant() : base(InitMethod(new Model()))
        {
        }

        
    }

    public sealed class IB_SizingPlant_DataFieldSet
        : IB_DataFieldSet<IB_SizingPlant_DataFieldSet, SizingPlant>
    {

        private IB_SizingPlant_DataFieldSet() { }

        public IB_DataField LoopType { get; }
            = new IB_BasicDataField("LoopType", "Type");

        
    }

}

