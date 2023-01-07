using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.HVAC.Curves
{
    public class IB_CurveBiquadratic : IB_Curve, IIB_Curve3D
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CurveBiquadratic();
        private static CurveBiquadratic NewDefaultOpsObj(Model model)
            => new CurveBiquadratic(model);
        

        public IB_CurveBiquadratic():base(NewDefaultOpsObj(new Model())) { }

        public override Curve ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }

        private List<double> _coefficients;
        private void GetCoefficients()
        {
            if (_coefficients != null)
                return;

            var att = this.CustomAttributes;
            var fSet = HVAC.Curves.IB_CurveBiquadratic_FieldSet.Value;

            att.TryGetValue<double>(fSet.Coefficient1Constant, out var c1);
            att.TryGetValue<double>(fSet.Coefficient2x, out var c2);
            att.TryGetValue<double>(fSet.Coefficient3xPOW2, out var c3);
            att.TryGetValue<double>(fSet.Coefficient4y, out var c4);
            att.TryGetValue<double>(fSet.Coefficient5yPOW2, out var c5);
            att.TryGetValue<double>(fSet.Coefficient6xTIMESY, out var c6);


            _coefficients = new List<double>()
            {
                0,
                c1, c2, c3, c4,
                c5, c6, 
            };
        }


        public void GetMinMax(out double minX, out double maxX, out double minY, out double maxY)
        {
            if (this.GhostOSObject is CurveBiquadratic c)
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
                c[3]*x*x,
                c[4]*y,
                c[5]*y*y,
                c[6]*x*y
            };

            return vs.Sum();
        }
    }

    public sealed class IB_CurveBiquadratic_FieldSet
        : IB_FieldSet<IB_CurveBiquadratic_FieldSet, CurveBiquadratic>
    {
        private IB_CurveBiquadratic_FieldSet() { }

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


        public IB_Field MinimumValueofx { get; }
            = new IB_BasicField("MinimumValueofx", "minX");

        public IB_Field MaximumValueofx { get; }
            = new IB_BasicField("MaximumValueofx", "maxX");

        public IB_Field MinimumValueofy { get; }
            = new IB_BasicField("MinimumValueofy", "minY");

        public IB_Field MaximumValueofy { get; }
            = new IB_BasicField("MaximumValueofy", "maxY");
    }
}
