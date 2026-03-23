using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Linq;

namespace Ironbug.HVAC
{
    public class IB_PipeIndoor : IB_HVACObject, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_PipeIndoor();
        private static PipeIndoor NewDefaultOpsObj(Model model)
            => new PipeIndoor(model);

        private string _zoneName { get => this.Get(string.Empty); set => this.Set(value); }
        private string _construction { get => this.Get(string.Empty); set => this.Set(value); }

        public IB_PipeIndoor():base(NewDefaultOpsObj)
        {
        }

        public void SetAmbientTemperatureZone(string ambientTemperatureZoneName)
        {
            if (string.IsNullOrEmpty(ambientTemperatureZoneName))
                throw new ArgumentException("Invalid ambient temperature zone name");
            _zoneName = ambientTemperatureZoneName;
        }

        public void SetContstruction(string construction)
        {
            if (string.IsNullOrEmpty(construction))
                throw new ArgumentException("Invalid construction");
            _construction = construction;
        }

        public override HVACComponent ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            if (!string.IsNullOrEmpty(_construction))
            {
                var cons = model.getConstructionsByName(_construction, true);
                if (cons.Any())
                {
                    obj.setConstruction(cons.First());
                }
            }

            if (!string.IsNullOrEmpty(_zoneName))
            {
                // this will be executed after all loops (nodes) are saved
                Func<bool> func = () =>
                {
                    var zone = model.GetThermalZone(_zoneName);
                    if (zone == null)
                        throw new ArgumentException($"Invalid ambient temperature zone ({_zoneName}) in {this.GetType().Name}");

                    return obj.setAmbientTemperatureZone(zone);

                };

                IB_Utility.AddDelayFunc(func);
            }
            return obj;
        }
        
    }

    public sealed class IB_PipeIndoor_FieldSet
        : IB_FieldSet<IB_PipeIndoor_FieldSet, PipeIndoor>
    {
        private IB_PipeIndoor_FieldSet() { }
    }
}
