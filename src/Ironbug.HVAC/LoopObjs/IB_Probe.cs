using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_Probe: IB_HVACObject, IIB_AirLoopObject, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => ()=> new IB_Probe();
        
        private static Node NewDefaultOpsObj(Model model) => new Node(model);
        
        public IB_Probe() : base(NewDefaultOpsObj(new Model()))
        {
        }

        //This will never be called, check IB_Loop.AddNodeProbe();
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }

        //This will never be called, check IB_Loop.AddNodeProbe();
        public override bool AddToNode(Node node)
        {
            return true;
        }

        public override string ToString() => "Probe";
     
    }
    

    public sealed class IB_Probe_FieldSet
        : IB_FieldSet<IB_Probe_FieldSet, Node>
    {
        private IB_Probe_FieldSet() { }
        
    }
}
