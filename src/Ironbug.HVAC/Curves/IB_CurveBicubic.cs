using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.HVAC.Curves
{
    public class IB_CurveBicubic: IB_Curve, IIB_Curve3D
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CurveBicubic();
        private static CurveBicubic NewDefaultOpsObj(Model model)
            => new CurveBicubic(model);
        

        public IB_CurveBicubic():base(NewDefaultOpsObj(new Model()))
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
            var fSet = HVAC.Curves.IB_CurveBicubic_FieldSet.Value;

            att.TryGetValue<double>(fSet.Coefficient1Constant, out var c1);
            att.TryGetValue<double>(fSet.Coefficient2x, out var c2);
            att.TryGetValue<double>(fSet.Coefficient3xPOW2, out var c3);
            att.TryGetValue<double>(fSet.Coefficient4y, out var c4);
            att.TryGetValue<double>(fSet.Coefficient5yPOW2, out var c5);
            att.TryGetValue<double>(fSet.Coefficient6xTIMESY, out var c6);
            att.TryGetValue<double>(fSet.Coefficient7xPOW3, out var c7);
            att.TryGetValue<double>(fSet.Coefficient8yPOW3, out var c8);
            att.TryGetValue<double>(fSet.Coefficient9xPOW2TIMESY, out var c9);
            att.TryGetValue<double>(fSet.Coefficient10xTIMESYPOW2, out var c10);


            _coefficients = new List<double>()
            {   
                0,
                c1, c2, c3, c4,
                c5, c6, c7, c8,
                c9, c10

            };
        }

        public void GetMinMax(out double minX,out double maxX, out double minY, out double maxY)
        {
            if (this.GhostOSObject is CurveBicubic c)
            {
                minX = c.minimumValueofx();
                maxX = c.maximumValueofx();
                minY = c.minimumValueofy();
                maxY = c.maximumValueofy();
            }
            else
            {
                throw new ArgumentException($"Invalid curve type {this.GhostOSObject.GetType().Name}");
            }
        }

        public double Compute(double x, double y)
        {
            GetCoefficients();
            var c = _coefficients;
                
            var vs = new List<double>
            {
                c[1],
                c[2]*x,
                c[3]* Math.Pow(x, 2),
                c[4]*y,
                c[5]* Math.Pow(y, 2),
                c[6]*x*y,
                c[7]* Math.Pow(x, 3),
                c[8]* Math.Pow(y, 3),
                c[9] * Math.Pow(x, 2) * y,
                c[10] * x * Math.Pow(y, 2),
            };
       
            return vs.Sum();
        }
    }

    public sealed class IB_CurveBicubic_FieldSet
        : IB_FieldSet<IB_CurveBicubic_FieldSet, CurveBicubic>
    {
        private IB_CurveBicubic_FieldSet() { }
        public IB_Field Coefficient1Constant { get; }
            = new IB_TopField("Coefficient1Constant", "C1");

        public IB_Field Coefficient2x { get; }
            = new IB_TopField("Coefficient2x", "C2");

        public IB_Field Coefficient3xPOW2 { get; }
            = new IB_TopField("Coefficient3xPOW2", "C3");

        public IB_Field Coefficient4y { get; }
            = new IB_TopField("Coefficient4y", "C4");

        public IB_Field Coefficient5yPOW2 { get; }
            = new IB_TopField("Coefficient5yPOW2", "C5");

        public IB_Field Coefficient6xTIMESY { get; }
            = new IB_TopField("Coefficient6xTIMESY", "C6");

        public IB_Field Coefficient7xPOW3 { get; }
            = new IB_TopField("Coefficient7xPOW3", "C7");

        public IB_Field Coefficient8yPOW3 { get; }
            = new IB_TopField("Coefficient8yPOW3", "C8");

        public IB_Field Coefficient9xPOW2TIMESY { get; }
            = new IB_TopField("Coefficient9xPOW2TIMESY", "C9");

        public IB_Field Coefficient10xTIMESYPOW2 { get; }
            = new IB_TopField("Coefficient10xTIMESYPOW2", "C10");
    }
}
