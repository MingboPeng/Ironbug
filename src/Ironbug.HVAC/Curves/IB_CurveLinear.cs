using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.HVAC.Curves
{
    public class IB_CurveLinear : IB_Curve, IIB_Curve2D
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CurveLinear();
        private static CurveLinear NewDefaultOpsObj(Model model)
            => new CurveLinear(model);
        

        public IB_CurveLinear():base(NewDefaultOpsObj)
        {
        }
        public override Curve ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }

        private void GetCoefficients()
        {
            if (_coefficients != null)
                return;

            var att = this.CustomAttributes;
            var fSet = HVAC.Curves.IB_CurveLinear_FieldSet.Value;

            att.TryGetValue<double>(fSet.Coefficient1Constant, out var c1);
            att.TryGetValue<double>(fSet.Coefficient2x, out var c2);


            _coefficients = new List<double>()
            {
                0,
                c1, c2,
            };
        }

        public void GetMinMax(out double minX, out double maxX)
        {
            if (this.GhostOSObject is CurveLinear c)
            {
                minX = c.minimumValueofx();
                maxX = c.maximumValueofx();
            }
            else
            {
                throw new ArgumentException($"Invalid curve type {this.GhostOSObject.GetType().Name}");
            }
        }

        public double Compute(double x)
        {
            GetCoefficients();
            var c = _coefficients;

            var vs = new List<double>
            {
                c[1],
                c[2]*x,
            };

            return vs.Sum();
        }
    }

    public sealed class IB_CurveLinear_FieldSet
        : IB_FieldSet<IB_CurveLinear_FieldSet, CurveLinear>
    {
        private IB_CurveLinear_FieldSet() { }
        public IB_Field Coefficient1Constant { get; }
            = new IB_TopField("Coefficient1Constant", "C1");

        public IB_Field Coefficient2x { get; }
            = new IB_TopField("Coefficient2x", "C2");
    }
}
