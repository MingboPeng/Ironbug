using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_SetpointManagerSystemNodeResetTemperature : IB_SetpointManager
    {
        private string _nodeID { get => this.Get(string.Empty); set => this.Set(value); }
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SetpointManagerSystemNodeResetTemperature();

        private static SetpointManagerSystemNodeResetTemperature NewDefaultOpsObj(Model model) => new SetpointManagerSystemNodeResetTemperature(model);


        public IB_SetpointManagerSystemNodeResetTemperature() : base(NewDefaultOpsObj(new Model()))
        { 
        }


        public void SetSensorNode(string probeTrackingID)
        {
            if (string.IsNullOrEmpty(probeTrackingID))
                throw new ArgumentException("Invalid probe tracking ID");
            _nodeID = probeTrackingID;
        }

        public override HVACComponent ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);

            // this will be executed after all loops (nodes) are saved
            Func<bool> func = () =>
            {
                var node = model.GetNodeByTrackingID(_nodeID);
                if (node == null)
                    throw new ArgumentException($"Invalid sensor node ({_nodeID}) in {this.GetType().Name}");

                return obj.setReferenceNode(node);

            };

            IB_Utility.AddDelayFunc(func);
            return obj;
        }
    }

    public sealed class IB_SetpointManagerSystemNodeResetTemperature_FieldSet
        : IB_FieldSet<IB_SetpointManagerSystemNodeResetTemperature_FieldSet, SetpointManagerSystemNodeResetTemperature>
    {
        private IB_SetpointManagerSystemNodeResetTemperature_FieldSet() { }

    }
}
