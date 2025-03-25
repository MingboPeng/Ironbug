using System;
using System.Collections.Generic;
using System.Linq;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_SwimmingPoolIndoor : IB_HVACObject
    {
        private string _surfaceID { get => this.Get(string.Empty); set => this.Set(value); }
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SwimmingPoolIndoor();

        private static SwimmingPoolIndoor NewDefaultOpsObj(Model model) => new SwimmingPoolIndoor(model, new Surface(new Point3dVector(new List<Point3d>() { new Point3d(0,0,0), new Point3d(0,1,0), new Point3d(1,0,0)}), model));
        public IB_SwimmingPoolIndoor() : base(NewDefaultOpsObj)
        {
        }

        public void SetWaterSufaceID(string waterSuface)
        {
            if (string.IsNullOrEmpty(waterSuface))
                throw new ArgumentException("Invalid water surface ID");
            _surfaceID = waterSuface;
        }

        public override HVACComponent ToOS(Model model)
        {
            var newObj = this.OnNewOpsObj(NewDefaultOpsObj, model);
            //var srfs = model.getSurfaces();
            //var nn = srfs.Select(_=>_.nameString(true)).ToList();
            var srf = model.getSurfaceByName(_surfaceID);
            if (srf == null || !srf.is_initialized())
                throw new ArgumentException("Failed to find water surface for SwimmingPoolIndoor");

            newObj.setSurface(srf.get());
            return newObj;
        }
    }

    public sealed class IB_SwimmingPoolIndoor_FieldSet
        : IB_FieldSet<IB_SwimmingPoolIndoor_FieldSet, SwimmingPoolIndoor>
    {
        private IB_SwimmingPoolIndoor_FieldSet() {}
        
    }
}
