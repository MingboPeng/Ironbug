using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_EnergyManagementSystemCurveVariable : IB_EnergyManagementSystemVariable
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_EnergyManagementSystemCurveVariable();

        private static EnergyManagementSystemCurveOrTableIndexVariable NewDefaultOpsObj(Model model) => new EnergyManagementSystemCurveOrTableIndexVariable(model);
        public IB_EnergyManagementSystemCurveVariable() : base(NewDefaultOpsObj)
        {
        }

        public string Name { get => Get<string>(); set { Set(value); this.GhostOSObject.setName(value); } }
        public IB_Curve Curve { get => Get<IB_Curve>(); set => Set(value); }
    
        public override ModelObject ToOS(Model model)
        {
            var curve = Curve.GetOsmObjInModel(model) as Curve;
            if (curve == null)
                curve = Curve.ToOS(model);
            var obj = new EnergyManagementSystemCurveOrTableIndexVariable(model, curve);
            if (!string.IsNullOrEmpty(Name))
                obj.setName(Name);
            return obj;
        }

    }

    
}
