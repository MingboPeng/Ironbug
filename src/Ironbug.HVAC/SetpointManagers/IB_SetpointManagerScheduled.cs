using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public class IB_SetpointManagerScheduled : IB_SetpointManager
    {
        private double temperature = 12.7778; //55F

        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SetpointManagerScheduled(this.temperature);

        private  SetpointManagerScheduled NewDefaultOpsObj(Model model) 
            => new SetpointManagerScheduled(model, new ScheduleRuleset(model, this.temperature));
        private static SetpointManagerScheduled NewDefaultOpsObj(Model model, double temp) 
            => new SetpointManagerScheduled(model, new ScheduleRuleset(model, temp));

        public IB_SetpointManagerScheduled(double temperature) : base(NewDefaultOpsObj(new Model(), temperature))
        {
            this.temperature = temperature;

        }
        public override bool AddToNode(Node node)
        {
            var model = node.model();

            return ((SetpointManagerScheduled)this.ToOS(model)).addToNode(node);
        }
        
        protected override ModelObject NewOpsObj(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model).to_SetpointManagerScheduled().get();
        }
    }
}
