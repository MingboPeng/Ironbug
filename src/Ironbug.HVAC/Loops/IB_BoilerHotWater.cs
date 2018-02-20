using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_BoilerHotWater : IB_HVACComponent
    {
        private static BoilerHotWater InitMethod(Model model) => new BoilerHotWater(model);
        public IB_BoilerHotWater() : base(InitMethod(new Model()))
        {
            base.SetName("Boiler:Hotwater");
        }
        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((BoilerHotWater)this.ToOS(model)).addToNode(node);
        }

        public override IB_ModelObject Duplicate()
        {
            return base.DuplicateIB_ModelObject(() => new IB_BoilerHotWater());
        }

        public override ModelObject ToOS(Model model)
        {
            return base.ToOS(InitMethod, model);
        }
    }
}
