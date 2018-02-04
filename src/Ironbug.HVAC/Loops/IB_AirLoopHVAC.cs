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
        private Model osModel { get; set; }
        public AirLoopHVAC osAirLoopHVAC { get; set; }

        public IB_AirLoopHVAC()
        {
            //this.basePoint = new Point3d();
            this.osModel = new Model();
            this.osAirLoopHVAC = new AirLoopHVAC(osModel);
        }

        public void AddToSupplyEnd(IIB_HVACComponent Component)
        {
            HVACComponent com = Component.GetHVACComponent();
            var nd = this.osAirLoopHVAC.supplyOutletNode();
            com.clone(this.osModel).to_HVACComponent().get().addToNode(nd);

        }

        public void AddToModel(Model Model)
        {
            //return Model;
        }

        //public void AddToSupply(IB_CoilHeatingWater coil)
        //{
        //    this.AddToSupply(coil.osCoilHeatingWater);
        //}
        

    }


    




}
