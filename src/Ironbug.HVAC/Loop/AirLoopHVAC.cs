using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_AirLoopHVAC
    {
        //base point to draw the loop.
        public Point3d basePoint { get; set; }
        public Model model { get; set; }
        private AirLoopHVAC osAirLoopHVAC { get; set; }

        public IB_AirLoopHVAC()
        {
            //this.basePoint = new Point3d();
            this.model = new Model();
            this.osAirLoopHVAC = new AirLoopHVAC(model);
        }

        public void AddToSupply(HVACComponent component)
        {
            
            var nd = this.osAirLoopHVAC.supplyOutletNode();
            component.clone(this.model).to_HVACComponent().get().addToNode(nd);

        }

        public void AddToSupply(IB_CoilHeatingWater coil)
        {
            this.AddToSupply(coil.osCoilHeatingWater);
        }



    }

    public class IB_CoilHeatingWater{

        public CoilHeatingWater osCoilHeatingWater { get; set; }


        public IB_CoilHeatingWater()
        {
            var model = new Model();
            this.osCoilHeatingWater = new CoilHeatingWater(model);
        }


    }




}
