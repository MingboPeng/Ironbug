using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public class IB_SetpointManagerWarmest : IB_SetpointManager
    {
        private static SetpointManagerWarmest InitMethod(Model model) => new SetpointManagerWarmest(model);
        public IB_SetpointManagerWarmest() : base(InitMethod(new Model()))
        {
            
        }
        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((SetpointManagerWarmest)this.ToOS(model)).addToNode(node);
        }

        public override IB_ModelObject Duplicate()
        {
            return base.DuplicateIBObj(() => new IB_SetpointManagerWarmest());
        }

        public override ModelObject ToOS(Model model)
        {
            return base.ToOS(InitMethod, model).to_SetpointManagerWarmest().get();
        }
    }


}
