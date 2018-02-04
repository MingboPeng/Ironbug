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
        private Model osModel;

        //public Model OSModel
        //{
        //    get { return osModel; }
        //    set { osModel = value; }
        //}

        public AirLoopHVAC osAirLoopHVAC { get; set; }

        public IB_AirLoopHVAC()
        {
            //this.basePoint = new Point3d();
            this.osModel = new Model();
            this.osAirLoopHVAC = new AirLoopHVAC(osModel);
        }

        public void AddToSupplyEnd(IIB_HVACComponent Component)
        {
            
            var nd = this.osAirLoopHVAC.supplyOutletNode();

            //var coil = new OpenStudio.CoilHeatingWater(osModel);
            //coil.addToNode(nd);

            var com = Component as IB_CoilHeatingWater;
            com.AddToNode(ref osModel, nd);


            ////*********
            ////THIS IS CAUSING MEMORY PROBLEM
            //HVACComponent com = Component.GetHVACComponent();
            //var tCom = com.clone(osModel);
            //tCom.to_HVACComponent().get().addToNode(nd);

        }

        //TODO
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
