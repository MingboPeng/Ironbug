using System;
using System.Runtime.Serialization;
using Ironbug.HVAC.BaseClass;
using Newtonsoft.Json;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_SolarCollectorFlatPlateWater : IB_HVACObject, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SolarCollectorFlatPlateWater();

        private static SolarCollectorFlatPlateWater NewDefaultOpsObj(Model model) => new SolarCollectorFlatPlateWater(model);

        [DataMember]
        private string SurfaceID { get; set; }

        private IB_SolarCollectorPerformanceFlatPlate _scp => this.GetChild<IB_SolarCollectorPerformanceFlatPlate>(0);

        [JsonConstructor]
        private IB_SolarCollectorFlatPlateWater(bool forDeserialization) : base(null)
        {
        }

        public IB_SolarCollectorFlatPlateWater() : base(NewDefaultOpsObj)
        {
            this.AddChild(null);
        }

        public void SetSurface(string surfaceID)
        {
            SurfaceID = surfaceID;
        }

        public void SetSolarCollectorPerformance(IB_SolarCollectorPerformanceFlatPlate SolarCollectorPerformance)
        {
            this.SetChild(0, SolarCollectorPerformance);
        }


        public override HVACComponent ToOS(Model model)
        {
            if (string.IsNullOrEmpty(SurfaceID))
                throw new ArgumentException("Invalid shading surface ID");
        
            var oShade = model.getShadingSurfaceByName(SurfaceID);
            if (oShade == null || oShade.isNull())
                throw new ArgumentException($"Invalid shading surface ID: {SurfaceID}");
            if(!oShade.is_initialized())
                throw new ArgumentException($"Invalid shading surface ID: {SurfaceID}");

            var shd = oShade.get();
            var opsObj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            opsObj.setSurface(shd);

            if (this._scp != null)
            {
                var perf = opsObj.solarCollectorPerformance();
                _scp.ApplyAttributesToObj(model, perf);
            }

            return opsObj;
        }
    }


    public sealed class IB_SolarCollectorFlatPlateWater_FieldSet
        : IB_FieldSet<IB_SolarCollectorFlatPlateWater_FieldSet, SolarCollectorFlatPlateWater>
    {
        private IB_SolarCollectorFlatPlateWater_FieldSet() { }
    }
}
