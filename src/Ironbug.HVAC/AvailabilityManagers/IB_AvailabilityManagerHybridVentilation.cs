﻿using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.AvailabilityManager
{
    public class IB_AvailabilityManagerHybridVentilation : IB_AvailabilityManager
    {
        private string _controlZoneName { get => this.Get(string.Empty); set => this.Set(value); }
        public static IB_AvailabilityManagerHybridVentilation_FieldSet FieldSet => IB_AvailabilityManagerHybridVentilation_FieldSet.Value;
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_AvailabilityManagerHybridVentilation();

        private static AvailabilityManagerHybridVentilation NewDefaultOpsObj(Model model) => new AvailabilityManagerHybridVentilation(model);
        public IB_AvailabilityManagerHybridVentilation() : base(NewDefaultOpsObj)
        {
        } 

        public void SetControlZone(string controlZoneName)
        {
            if (string.IsNullOrEmpty(controlZoneName))
                return;
            _controlZoneName = controlZoneName;
        }
        

        public override OpenStudio.AvailabilityManager ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            // this will be executed after all loops (nodes) are saved
            Func<bool> func = () =>
            {
                var zone = model.GetThermalZone(_controlZoneName);
                if (zone == null)
                    throw new ArgumentException($"Invalid control zone ({_controlZoneName}) in {this.GetType().Name}");

                return obj.setControlledZone(zone);

            };

            IB_Utility.AddDelayFunc(func);

            return obj;
        }

    }

    public sealed class IB_AvailabilityManagerHybridVentilation_FieldSet
        : IB_FieldSet<IB_AvailabilityManagerHybridVentilation_FieldSet, AvailabilityManagerHybridVentilation>
    {
        private IB_AvailabilityManagerHybridVentilation_FieldSet() { }

    }


}
