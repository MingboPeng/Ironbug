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
        //private List<IB_HVACComponent> demandComponents { get; set; }
        private List<IB_ThermalZone> thermalZones { get; set; }

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
            this.thermalZones = new List<IB_ThermalZone>();
        }

        public void AddToSupplyEnd(IB_HVACComponent HvacComponent)
        {
            //TODO: check befor add
            this.supplyComponents.Add(HvacComponent);
            
        }

        public void AddToDemandBranch(IB_ThermalZone HvacComponent)
        {
            this.thermalZones.Add(HvacComponent);
        }

        public AirLoopHVAC ToOS( Model osModel)
        {
            this.osAirLoopHVAC = new AirLoopHVAC(osModel);
            var nd = this.osAirLoopHVAC.supplyOutletNode();
            //TODO: find a way to add set point manager
            //var stp = new SetpointManagerOutdoorAirReset(osModel);
            //stp.addToNode(nd);
            foreach (var item in this.supplyComponents)
            {
                item.AddToNode(nd);
            }

           
            foreach (var item in this.thermalZones)
            {
                var zone = (ThermalZone)item.ToOS(osModel);
                var airTerminal = (HVACComponent)item.AirTerminal.ToOS(osModel);
                this.osAirLoopHVAC.addBranchForZone(zone,airTerminal);
            }

            return this.osAirLoopHVAC;
            ////*********
            ////THIS IS CAUSING MEMORY PROBLEM
            //HVACComponent com = Component.GetHVACComponent();
            //var tCom = com.clone(osModel);
            //tCom.to_HVACComponent().get().addToNode(nd);
        }



    }


    




}
