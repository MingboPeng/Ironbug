using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.AvailabilityManager
{
    public class IB_AvailabilityManagerOptimumStart : IB_AvailabilityManager
    {
        private string _controlZoneName { get => this.Get(string.Empty); set => this.Set(value); }
        public static IB_AvailabilityManagerOptimumStart_FieldSet FieldSet => IB_AvailabilityManagerOptimumStart_FieldSet.Value;
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_AvailabilityManagerOptimumStart();

        private static AvailabilityManagerOptimumStart NewDefaultOpsObj(Model model) => new AvailabilityManagerOptimumStart(model);
        public IB_AvailabilityManagerOptimumStart() : base(NewDefaultOpsObj(new Model()))
        {
        } 

        public void SetControlZone(string controlZoneName)
        {
            if (string.IsNullOrEmpty(controlZoneName))
                throw new ArgumentException("Invalid control zone");
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
                    return false;

                return obj.setControlZone(zone);

            };

            IB_Utility.AddDelayFunc(func);


            return obj;
        }

        
    }

    public sealed class IB_AvailabilityManagerOptimumStart_FieldSet
        : IB_FieldSet<IB_AvailabilityManagerOptimumStart_FieldSet, AvailabilityManagerOptimumStart>
    {
        private IB_AvailabilityManagerOptimumStart_FieldSet() { }

    }


}
