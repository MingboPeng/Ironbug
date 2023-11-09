using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.AvailabilityManager
{
    public class IB_AvailabilityManagerLowTemperatureTurnOff : IB_AvailabilityManager
    {
        private string _nodeID { get => this.Get(string.Empty); set => this.Set(value); }
        public static IB_AvailabilityManagerLowTemperatureTurnOff_FieldSet FieldSet => IB_AvailabilityManagerLowTemperatureTurnOff_FieldSet.Value;
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_AvailabilityManagerLowTemperatureTurnOff();

        private static AvailabilityManagerLowTemperatureTurnOff NewDefaultOpsObj(Model model) => new AvailabilityManagerLowTemperatureTurnOff(model);
        public IB_AvailabilityManagerLowTemperatureTurnOff() : base(NewDefaultOpsObj(new Model()))
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

            IB_Utility.AddDelayFunc(func);

            return obj;
        }

       
    }

    public sealed class IB_AvailabilityManagerLowTemperatureTurnOff_FieldSet
        : IB_FieldSet<IB_AvailabilityManagerLowTemperatureTurnOff_FieldSet, AvailabilityManagerLowTemperatureTurnOff>
    {
        private IB_AvailabilityManagerLowTemperatureTurnOff_FieldSet() { }

    }


}
