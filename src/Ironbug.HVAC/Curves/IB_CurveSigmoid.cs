using Ironbug.HVAC.BaseClass;
using Newtonsoft.Json.Linq;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.HVAC.Curves
{
    public class IB_CurveSigmoid : IB_Curve, IIB_Curve2D
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CurveSigmoid();
        private static CurveSigmoid NewDefaultOpsObj(Model model)
            => new CurveSigmoid(model);
        

        public IB_CurveSigmoid():base(NewDefaultOpsObj(new Model()))
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
            var fSet = HVAC.Curves.IB_CurveSigmoid_FieldSet.Value;

            att.TryGetValue<double>(fSet.Coefficient1C1, out var c1);
            att.TryGetValue<double>(fSet.Coefficient2C2, out var c2);
            att.TryGetValue<double>(fSet.Coefficient3C3, out var c3);
            att.TryGetValue<double>(fSet.Coefficient4C4, out var c4);
            att.TryGetValue<double>(fSet.Coefficient5C5, out var c5);


            _coefficients = new List<double>()
            {
                0,
                c1, c2, c3, c4,
                c5,
            };
        }

        public void GetMinMax(out double minX, out double maxX)
        {
            if (this.GhostOSObject is CurveSigmoid c)
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

            var ep = (c[3] - x) / c[4];
            var d = Math.Pow( 1 + Math.Exp(ep), c[5]);
            var v = c[1] + c[2] / d;

            return v;
        }
    }

    public sealed class IB_CurveSigmoid_FieldSet
        : IB_FieldSet<IB_CurveSigmoid_FieldSet, CurveSigmoid>
    {
        private IB_CurveSigmoid_FieldSet() { }

        public IB_Field Coefficient1C1 { get; }
            = new IB_TopField("Coefficient1C1", "C1");

        public IB_Field Coefficient2C2 { get; }
            = new IB_TopField("Coefficient2C2", "C2");

        public IB_Field Coefficient3C3{ get; }
            = new IB_TopField("Coefficient3C3", "C3");

        public IB_Field Coefficient4C4 { get; }
            = new IB_TopField("Coefficient4C4", "C4");

        public IB_Field Coefficient5C5 { get; }
            = new IB_TopField("Coefficient5C5", "C5");
    }
}
