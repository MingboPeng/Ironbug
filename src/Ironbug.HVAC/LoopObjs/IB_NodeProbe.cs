using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_NodeProbe: IB_HVACObject, IIB_AirLoopObject, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => ()=> new IB_NodeProbe();
        
        private static Node NewDefaultOpsObj(Model model) => new Node(model);
        
        public IB_NodeProbe() : base(NewDefaultOpsObj(new Model()))
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

        public override string ToString() => "NodeProbe";
     
    }
    

    public sealed class IB_Probe_FieldSet
        : IB_FieldSet<IB_Probe_FieldSet, Node>
    {
        private IB_Probe_FieldSet() { }
        
    }
}
