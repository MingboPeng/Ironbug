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

        private List<IB_HVACObject> supplyComponents { get; set; }= new List<IB_HVACObject>();
        private List<IB_HVACObject> demandComponents { get; set; } = new List<IB_HVACObject>();
        private List<IB_ThermalZone> thermalZones { get; set; } = new List<IB_ThermalZone>();

        //real osAirLoopHVAC
        //private AirLoopHVAC osAirLoopHVAC { get; set; }

        //ghost for preview 
        private AirLoopHVAC ghostAirLoopHVAC { get; set; }

        public IB_AirLoopHVAC()
        {
            //this.basePoint = new Point3d();
            //this.osModel = new Model();
            this.ghostAirLoopHVAC = new AirLoopHVAC(new Model());
        }

        public void AddToSupplySide(IB_HVACObject HvacComponent)
        {
            //TODO: check befor add
            this.supplyComponents.Add(HvacComponent);
            
        }

        public void AddToDemandSide(IB_HVACObject HvacComponent)
        {
            //TODO: check befor add
            this.demandComponents.Add(HvacComponent);

        }

        public void AddToDemandBranch(IB_ThermalZone HvacComponent)
        {
            this.thermalZones.Add(HvacComponent);
        }

        public AirLoopHVAC ToOS( Model osModel)
        {
            CheckSupplySide(this.supplyComponents);

            var airLoopHVAC = new AirLoopHVAC(osModel);

            this.AddSupplyObjects(airLoopHVAC, this.supplyComponents);
            this.AddDemandObjects(airLoopHVAC, this.demandComponents);

            foreach (var item in this.thermalZones)
            {
                var zone = (ThermalZone)item.ToOS(osModel);
                var airTerminal = (HVACComponent)item.AirTerminal.ToOS(osModel);
                airLoopHVAC.addBranchForZone(zone,airTerminal);
            }

            return airLoopHVAC;
        }

        private bool CheckSupplySide(List<IB_HVACObject> Components)
        {
            var fanCount = Components.Count(_ => _ is IB_Fan);
            if (fanCount == 0)
            {
                throw new Exception("Airloop need at least one fan!");
            }
            else
            {
                return true;
            }
            

        }
        private bool AddSupplyObjects(AirLoopHVAC AirLoopHVAC, List<IB_HVACObject> Components)
        {
            var spnd = AirLoopHVAC.supplyOutletNode();
            var comps = Components.Where(_ => !(_ is IB_SetpointManager));
            comps.ToList()
                .ForEach(_ => _.AddToNode(spnd));

            var allcopied = AirLoopHVAC.supplyComponents()
                .Where(_=>!_.IsNode())
                .Count() == comps.Count();

            allcopied &= this.AddSetPoints(AirLoopHVAC, Components);


            if (!allcopied)
            {
                throw new Exception("Failed to add airloop supply components!");
            }

            return allcopied;
        }

        private bool AddDemandObjects(AirLoopHVAC AirLoopHVAC, List<IB_HVACObject> Components)
        {
            
            //var spnd = AirLoopHVAC.supplyOutletNode();
            //var comps = Components.Where(_ => !(_ is IB_SetpointManager));
            //comps.ToList()
            //    .ForEach(_ => _.AddToNode(spnd));

            //var allcopied = AirLoopHVAC.supplyComponents()
            //    .Where(_ => !_.IsNode())
            //    .Count() == comps.Count();

            //allcopied &= this.AddSetPoints(AirLoopHVAC, Components);


            //if (!allcopied)
            //{
            //    throw new Exception("Failed to add airloop supply components!");
            //}

            return true;
        }

        private bool AddSetPoints(AirLoopHVAC AirLoopHVAC, List<IB_HVACObject> Components)
        {
            //var setPtAtIndex = new Dictionary<int, IB_SetpointManager>();

            //var trackingIDAndComp = new Dictionary<string, ModelObject>();
            
            
            var allTrackingIDs = AirLoopHVAC.components().Select(_ => _.comment()).ToList();
            

            var setPts = Components.Where(_ => _ is IB_SetpointManager);

            //TODO: check if there is only one component and it is setpoint.


            foreach (var item in setPts)
            {
                var setPt = (IB_SetpointManager)item;
                var atIndex = Components.IndexOf(item);

                OptionalNode nodeWithSetPt = null;

                if (atIndex == 0 )
                {
                    //Find the component after setpoint
                    var comaAfterSetPt = Components[atIndex + 1].GetTrackingID();
                    var comaAfterSetPt_Index = allTrackingIDs.IndexOf(comaAfterSetPt);

                    //Find the node for setPoint
                    var node_Index = comaAfterSetPt_Index - 1;
                    nodeWithSetPt = AirLoopHVAC.components().ElementAt(node_Index).to_Node();
                }
                else if(atIndex >0)
                {
                    //Find the component before setpoint
                    var comBeforeSetPt = Components[atIndex - 1].GetTrackingID();
                    var combeforeSetPt_Index = allTrackingIDs.IndexOf(comBeforeSetPt);

                    //Find the node for setPoint
                    var node_Index = combeforeSetPt_Index + 1;
                    nodeWithSetPt = AirLoopHVAC.components().ElementAt(node_Index).to_Node();
                }
                
                
                //Add to the node
                if (nodeWithSetPt.is_initialized())
                {
                    item.AddToNode(nodeWithSetPt.get());
                }

                
                
            }

            var allcopied = AirLoopHVAC.SetPointManagers().Count() == setPts.Count();

            if (!allcopied)
            {
                throw new Exception("Failed to add set point managers!");
            }

            return allcopied;
            
            

        }



    }
    







}
