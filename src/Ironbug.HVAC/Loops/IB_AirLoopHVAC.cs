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

        private List<IB_HVACComponent> supplyComponents { get; set; }

        //real osAirLoopHVAC
        private AirLoopHVAC osAirLoopHVAC { get; set; }

        //ghost for preview 
        private AirLoopHVAC ghostAirLoopHVAC { get; set; }

        public IB_AirLoopHVAC()
        {
            //this.basePoint = new Point3d();
            //this.osModel = new Model();
            this.ghostAirLoopHVAC = new AirLoopHVAC(new Model());
            this.supplyComponents = new List<IB_HVACComponent>();
        }

        public void AddToSupplyEnd(IB_HVACComponent HvacComponent)
        {
            //TODO: check befor add
            this.supplyComponents.Add(HvacComponent);
            
        }

        public void AddToModel(ref Model osModel)
        {
            this.osAirLoopHVAC = new AirLoopHVAC(osModel);
            var nd = this.osAirLoopHVAC.supplyOutletNode();

            foreach (var item in supplyComponents)
            {
                item.AddToNode(ref osModel, nd);
            }
            

            ////*********
            ////THIS IS CAUSING MEMORY PROBLEM
            //HVACComponent com = Component.GetHVACComponent();
            //var tCom = com.clone(osModel);
            //tCom.to_HVACComponent().get().addToNode(nd);
        }



    }


    




}
