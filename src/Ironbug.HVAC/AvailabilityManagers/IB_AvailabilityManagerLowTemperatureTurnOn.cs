using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.AvailabilityManager
{
    public class IB_AvailabilityManagerLowTemperatureTurnOn : IB_AvailabilityManager
    {
        private string _nodeID { get => this.Get(string.Empty); set => this.Set(value); }
        public static IB_AvailabilityManagerLowTemperatureTurnOn_FieldSet FieldSet => IB_AvailabilityManagerLowTemperatureTurnOn_FieldSet.Value;
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_AvailabilityManagerLowTemperatureTurnOn();

        private static AvailabilityManagerLowTemperatureTurnOn NewDefaultOpsObj(Model model) => new AvailabilityManagerLowTemperatureTurnOn(model);
        public IB_AvailabilityManagerLowTemperatureTurnOn() : base(NewDefaultOpsObj(new Model()))
        {
        } 

        public void SetSensorNode(string probeTrackingID)
        {
            if (string.IsNullOrEmpty(probeTrackingID))
                throw new ArgumentException("Invalid probe tracking ID");
            _nodeID = probeTrackingID;
        }

        public override OpenStudio.AvailabilityManager ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);

            // this will be executed after all loops (nodes) are saved
            Func<bool> func = () =>
            {
                var node = model.GetNodeByTrackingID(_nodeID);
                if (node == null)
                    return false;

                return obj.setSensorNode(node);
                
            };

            IB_Utility.DelayAddSensorNode(func);

            return obj;
        }

       
    }

    public sealed class IB_AvailabilityManagerLowTemperatureTurnOn_FieldSet
        : IB_FieldSet<IB_AvailabilityManagerLowTemperatureTurnOn_FieldSet, AvailabilityManagerLowTemperatureTurnOn>
    {
        private IB_AvailabilityManagerLowTemperatureTurnOn_FieldSet() { }

    }


}
