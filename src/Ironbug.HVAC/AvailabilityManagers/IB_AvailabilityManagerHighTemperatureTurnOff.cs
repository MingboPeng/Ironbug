﻿using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.AvailabilityManager
{
    public class IB_AvailabilityManagerHighTemperatureTurnOff : IB_AvailabilityManager
    {
        private string _nodeID { get => this.Get(string.Empty); set => this.Set(value); }
        public static IB_AvailabilityManagerHighTemperatureTurnOff_FieldSet FieldSet => IB_AvailabilityManagerHighTemperatureTurnOff_FieldSet.Value;
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_AvailabilityManagerHighTemperatureTurnOff();

        private static AvailabilityManagerHighTemperatureTurnOff NewDefaultOpsObj(Model model) => new AvailabilityManagerHighTemperatureTurnOff(model);
        public IB_AvailabilityManagerHighTemperatureTurnOff() : base(NewDefaultOpsObj)
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
                    throw new ArgumentException($"Invalid sensor node ({_nodeID}) in {this.GetType().Name}");

                return obj.setSensorNode(node);
                
            };

            IB_Utility.AddDelayFunc(func);

            return obj;
        }

       
    }

    public sealed class IB_AvailabilityManagerHighTemperatureTurnOff_FieldSet
        : IB_FieldSet<IB_AvailabilityManagerHighTemperatureTurnOff_FieldSet, AvailabilityManagerHighTemperatureTurnOff>
    {
        private IB_AvailabilityManagerHighTemperatureTurnOff_FieldSet() { }

    }


}
