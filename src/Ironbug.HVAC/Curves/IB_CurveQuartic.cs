using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.HVAC.Curves
{
    public class IB_CurveQuartic : IB_Curve, IIB_Curve2D
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CurveQuartic();
        private static CurveQuartic NewDefaultOpsObj(Model model)
            => new CurveQuartic(model);
        

        public IB_CurveQuartic():base(NewDefaultOpsObj)
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
            var fSet = HVAC.Curves.IB_CurveQuartic_FieldSet.Value;

            att.TryGetValue<double>(fSet.Coefficient1Constant, out var c1);
            att.TryGetValue<double>(fSet.Coefficient2x, out var c2);
            att.TryGetValue<double>(fSet.Coefficient3xPOW2, out var c3);
            att.TryGetValue<double>(fSet.Coefficient4xPOW3, out var c4);
            att.TryGetValue<double>(fSet.Coefficient5xPOW4, out var c5);


            _coefficients = new List<double>()
            {
                0,
                c1, c2, c3, c4,
                c5,
            };
        }


        public void GetMinMax(out double minX, out double maxX)
        {
            if (this.GhostOSObject is CurveQuartic c)
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
                c[3]* Math.Pow(x, 2),
                c[4]* Math.Pow(x, 3),
                c[5]* Math.Pow(x, 4)
            };

            return vs.Sum();
        }
    }

    public sealed class IB_CurveQuartic_FieldSet
        : IB_FieldSet<IB_CurveQuartic_FieldSet, CurveQuartic>
    {
        private IB_CurveQuartic_FieldSet() { }
        public IB_Field Coefficient1Constant { get; }
            = new IB_TopField("Coefficient1Constant", "C1");

        public IB_Field Coefficient2x { get; }
            = new IB_TopField("Coefficient2x", "C2");

        public IB_Field Coefficient3xPOW2 { get; }
            = new IB_TopField("Coefficient3xPOW2", "C3");

        public IB_Field Coefficient4xPOW3 { get; }
            = new IB_TopField("Coefficient4xPOW3", "xPOW3");

        public IB_Field Coefficient5xPOW4 { get; }
            = new IB_TopField("Coefficient5xPOW4", "xPOW4");

    }
}
