using System;
using System.Collections.Generic;
using System.Linq;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_EnergyManagementSystemCurveVariable : IB_EnergyManagementSystemVariable
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_EnergyManagementSystemCurveVariable();

        private static EnergyManagementSystemCurveOrTableIndexVariable NewDefaultOpsObj(Model model) => new EnergyManagementSystemCurveOrTableIndexVariable(model);
        public IB_EnergyManagementSystemCurveVariable() : base(NewDefaultOpsObj(new Model()))
        {
        }
        private string _name { get; set; }
        private IB_Curve _curve { get; set; }
        public void SetName(string name)
        {
            this._name = name;
            var p = this.GhostOSObject;
            p.setName(name);
        }

        public void SetCurve(IB_Curve curve)
        {
            this._curve = curve;
        }
        public override IB_ModelObject Duplicate()
        {
            var dup = base.Duplicate() as IB_EnergyManagementSystemCurveVariable;
            dup._name = this._name;
            dup._curve = this._curve;
            return dup;
        }
        public override ModelObject ToOS(Model model)
        {
            var curve = _curve.GetOsmObjInModel(model) as Curve;
            if (curve == null)
                curve = _curve.ToOS(model);
            var obj = new EnergyManagementSystemCurveOrTableIndexVariable(model, curve);
            if (!string.IsNullOrEmpty(_name))
                obj.setName(_name);
            return obj;
        }

    }

    
}
