using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingWater: IIB_HVACComponent
    {

        public CoilHeatingWater osCoilHeatingWater { get; set; }


        public IB_CoilHeatingWater()
        {
            var model = new Model();
            this.osCoilHeatingWater = new CoilHeatingWater(model);
        }

        public HVACComponent GetHVACComponent()
        {
            return this.osCoilHeatingWater;
        }
    }
}
