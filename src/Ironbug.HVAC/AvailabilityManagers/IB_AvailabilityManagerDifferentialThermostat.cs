using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.AvailabilityManager
{
    public class IB_AvailabilityManagerDifferentialThermostat : IB_AvailabilityManager
    {
        private string _nodeCID { get => this.Get(string.Empty); set => this.Set(value); }
        private string _nodeHID { get => this.Get(string.Empty); set => this.Set(value); }
        public static IB_AvailabilityManagerDifferentialThermostat_FieldSet FieldSet => IB_AvailabilityManagerDifferentialThermostat_FieldSet.Value;
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_AvailabilityManagerDifferentialThermostat();

        private static AvailabilityManagerDifferentialThermostat NewDefaultOpsObj(Model model) => new AvailabilityManagerDifferentialThermostat(model);
        public IB_AvailabilityManagerDifferentialThermostat() : base(NewDefaultOpsObj)
        {
        }

        public void SetSensorNode(string coldProbeTrackingID, string hotProbeTrackingID)
        {
            if (string.IsNullOrEmpty(coldProbeTrackingID) || string.IsNullOrEmpty(hotProbeTrackingID))
                throw new ArgumentException("Invalid probe tracking ID");
            _nodeCID = coldProbeTrackingID;
            _nodeHID = hotProbeTrackingID;
        }

        public override OpenStudio.AvailabilityManager ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);

            // this will be executed after all loops (nodes) are saved
            Func<bool> func = () =>
            {
                var nodeC = model.GetNodeByTrackingID(_nodeCID);
                var nodeH = model.GetNodeByTrackingID(_nodeHID);
                if (nodeC == null || nodeH == null)
                    throw new ArgumentException($"Invalid sensor node ({_nodeCID} or {_nodeHID}) in {this.GetType().Name}");

                return obj.setColdNode(nodeC) && obj.setHotNode(nodeH);

            };

            IB_Utility.AddDelayFunc(func);

            return obj;
        }


    }

    public sealed class IB_AvailabilityManagerDifferentialThermostat_FieldSet
        : IB_FieldSet<IB_AvailabilityManagerDifferentialThermostat_FieldSet, AvailabilityManagerDifferentialThermostat>
    {
        private IB_AvailabilityManagerDifferentialThermostat_FieldSet() { }

    }


}
