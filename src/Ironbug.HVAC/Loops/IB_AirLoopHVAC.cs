using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_AirLoopHVAC : BaseClasses.IB_Loop
    {

        private List<IB_HVACObject> supplyComponents { get; set; }= new List<IB_HVACObject>();
        private List<IB_HVACObject> demandComponents { get; set; } = new List<IB_HVACObject>();
        //private List<IB_ThermalZone> thermalZones { get; set; } = new List<IB_ThermalZone>();

        //real osAirLoopHVAC
        //private AirLoopHVAC osAirLoopHVAC { get; set; }

        
        private static AirLoopHVAC InitMethod(Model model) => new AirLoopHVAC(model);
        //ghost for preview 
        //private AirLoopHVAC ghostAirLoopHVAC { get; set; }

        public IB_AirLoopHVAC() : base(InitMethod(new Model()))
        {
            //this.basePoint = new Point3d();
            //this.osModel = new Model();
            //this.ghostAirLoopHVAC = new AirLoopHVAC(new Model());
        }

        public void AddToSupplySide(IB_HVACObject HvacComponent)
        {
            //TODO: check befor add
            if (HvacComponent is IIB_AirLoopObject)
            {
                this.supplyComponents.Add(HvacComponent);
            }
            else
            {
                throw new Exception("Only airloop object is allowed to add to airloop!");
            }
        }

        public void AddToDemandSide(IB_HVACObject HvacComponent)
        {
            
            if (HvacComponent is IIB_AirLoopObject)
            {
                this.demandComponents.Add(HvacComponent);
            }
            else
            {
                throw new Exception("Only airloop object is allowed to add to airloop!");
            }
            

        }

        public override IB_ModelObject Duplicate()
        {
            //TODO: duplicate child objects
            return this.DuplicateIB_ModelObject(() => new IB_AirLoopHVAC());
        }

        public override ModelObject ToOS( Model osModel)
        {
            this.CheckSupplySide(this.supplyComponents);

            var airLoopHVAC = base.ToOS(InitMethod, osModel).to_AirLoopHVAC().get();

            this.AddSupplyObjects(airLoopHVAC, this.supplyComponents);
            this.AddDemandObjects(airLoopHVAC, this.demandComponents);
            
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
            var objsBeforeBranch = base.GetObjsBeforeBranch(Components);
            var branchObj = (IB_AirLoopBranches)Components.Find(_ => _ is IB_AirLoopBranches);
            var objsAfterBranch = base.GetObjsAfterBranch(Components);


            var dmInNd = AirLoopHVAC.demandInletNode();
            var comps = objsBeforeBranch.Where(_ => !(_ is IB_SetpointManager)).Where(_ => !(_ is IB_AirLoopBranches));
            comps.ToList().ForEach(_ => _.AddToNode(dmInNd));

            branchObj.ToOS_Demand(AirLoopHVAC);

            var dmOutNd = AirLoopHVAC.demandOutletNode();
            comps = objsAfterBranch.Where(_ => !(_ is IB_SetpointManager)).Where(_ => !(_ is IB_AirLoopBranches));
            comps.ToList().ForEach(_ => _.AddToNode(dmOutNd));
            

            var addedObjs = AirLoopHVAC.demandComponents().Where(_ => _.comment().Contains("TrackingID"));
            var allcopied = addedObjs.Count() == Components.CountIncludesBranches()*2;// because added air terminal with each zone

            //TODO: might need to double check the setpoint order.
            allcopied &= this.AddSetPoints(AirLoopHVAC, Components);
            
            if (!allcopied)
            {
                throw new Exception("Failed to add airloop demand components!");
            }

            return allcopied;
        }

        

        
    }
    







}
