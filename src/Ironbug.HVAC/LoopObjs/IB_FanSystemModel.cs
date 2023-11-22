using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.HVAC
{
    public class IB_FanSystemModel : IB_Fan
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_FanSystemModel();
        
        private static FanSystemModel NewDefaultOpsObj(Model model) => new FanSystemModel(model);

        //odd: double flowFraction
        //even: double electricPowerFraction
        public List<double> Speeds
        {
            get => this.TryGetList<double>();
            private set => this.Set(value);
        }

        public IB_FanSystemModel():base(NewDefaultOpsObj(new Model()))
        {
        }

        public void SetSpeeds(List<double> speeds)
        {
            this.Speeds = speeds;
        }

        /// <summary>
        /// flowFraction and electricPowerFraction pair and separated by ","
        /// </summary>
        /// <param name="speeds"></param>
        public void SetSpeeds(List<string> speeds)
        {
            var sp = new List<double>();
            foreach (string s in speeds)
            {
                var p = s.Split(',').Select(n => double.Parse(n));
                if (p == null || p.Count() != 2)
                    throw new ArgumentException($"Invalid speed pair: {s}");
                if (p.Any(_=>_>1 || _<0))
                    throw new ArgumentException($"Invalid fraction value (0-1): {s}");
                sp.AddRange(p);

            }
            this.Speeds = sp;
        }

        public override HVACComponent ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            if (Speeds!= null && Speeds.Any())
            {
                var flows = Speeds.Where((_, i) => i % 2 != 0);
                var powers = Speeds.Where((_, i) => i % 2 == 0);
                var speeds = flows.Zip(powers, (f, p) => new FanSystemModelSpeed(f, p));
                foreach (var speed in speeds)
                {
                    obj.addSpeed(speed);
                }
            }
            return obj;
        }
        
        
    }



    public sealed class IB_FanSystemModel_FieldSet
        : IB_FieldSet<IB_FanSystemModel_FieldSet, FanSystemModel>
    {
        private IB_FanSystemModel_FieldSet() {}
        
        
    }
}
