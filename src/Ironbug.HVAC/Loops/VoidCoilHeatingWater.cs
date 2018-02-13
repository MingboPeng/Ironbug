using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class VoidCoilHeatingWater : CoilHeatingWater, IVoid<VoidCoilHeatingWater>
    {
        public VoidCoilHeatingWater(Model model) : base(model)
        {
        }
        
        public VoidCoilHeatingWater(Model model, Schedule availableSchedule) : base(model, availableSchedule)
        {
        }

        public Dictionary<string, object> CustomAttributes { get; private set; }

        //private VoidCoilHeatingWater instance { get; }
        public  VoidCoilHeatingWater Instance()
        {
            return new VoidCoilHeatingWater(new Model());
        }
        
        public CoilHeatingWater ToReal(ref Model model)
        {
            var obj = new CoilHeatingWater(model);
            obj.SetCustomAttributes(this.CustomAttributes);
            //aa.addToNode();
            return new CoilHeatingWater(model);
        }
        



    }
}
