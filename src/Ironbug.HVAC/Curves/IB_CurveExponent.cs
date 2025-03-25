using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.HVAC.Curves
{
    public class IB_CurveExponent : IB_Curve, IIB_Curve2D
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CurveExponent();
        private static CurveExponent NewDefaultOpsObj(Model model)
            => new CurveExponent(model);
        

        public IB_CurveExponent():base(NewDefaultOpsObj)
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
            var fSet = HVAC.Curves.IB_CurveExponent_FieldSet.Value;

            att.TryGetValue<double>(fSet.Coefficient1Constant, out var c1);
            att.TryGetValue<double>(fSet.Coefficient2Constant, out var c2);
            att.TryGetValue<double>(fSet.Coefficient3Constant, out var c3);


            _coefficients = new List<double>()
            {
                0,
                c1, c2, c3,
            };
        }

        public void GetMinMax(out double minX, out double maxX)
        {
            if (this.GhostOSObject is CurveExponent c)
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
                c[2]* Math.Pow(x, c[3]),
            };

            return vs.Sum();
        }
    }

    public sealed class IB_CurveExponent_FieldSet
        : IB_FieldSet<IB_CurveExponent_FieldSet, CurveExponent>
    {
        private IB_CurveExponent_FieldSet() { }
        public IB_Field Coefficient1Constant { get; }
            = new IB_TopField("Coefficient1Constant", "C1");

        public IB_Field Coefficient2Constant { get; }
            = new IB_TopField("Coefficient2Constant", "C2");

        public IB_Field Coefficient3Constant { get; }
            = new IB_TopField("Coefficient3Constant", "C3");
    }
}
