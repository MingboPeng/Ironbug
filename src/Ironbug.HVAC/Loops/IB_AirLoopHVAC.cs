using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_AirLoopHVAC : IB_Loop
    {

        private IList<IB_HVACObject> supplyComponents { get; set; }= new List<IB_HVACObject>();
        private IList<IB_HVACObject> demandComponents { get; set; } = new List<IB_HVACObject>();
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
            if (HvacComponent is IIB_AirLoopObject || HvacComponent is IB_SetpointManager)
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
            
            if (HvacComponent is IIB_AirLoopObject || HvacComponent is IB_SetpointManager)
            {
                this.demandComponents.Add(HvacComponent);
            }
            else
            {
                throw new Exception("Only airloop object or setpoint manager are allowed to add to airloop demand side!");
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

        private bool CheckSupplySide(IEnumerable<IB_HVACObject> Components)
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
        private bool AddSupplyObjects(AirLoopHVAC AirLoopHVAC, IEnumerable<IB_HVACObject> Components)
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

        private bool AddDemandObjects(AirLoopHVAC AirLoopHVAC, IEnumerable<IB_HVACObject> Components)
        {
            var filteredObjs = Components.Where(_ => !(_ is IB_SetpointManager));
            (var objsBeforeBranch, var branchObj, var objsAfterBranch) = base.GetObjsBeforeAndAfterBranch(filteredObjs);


            var dmInNd = AirLoopHVAC.demandInletNode();
            objsBeforeBranch.ToList().ForEach(_ => _.AddToNode(dmInNd));


            if (branchObj != null)
            {
                ((IB_AirLoopBranches)branchObj).ToOS_Demand(AirLoopHVAC);
            }

            var dmOutNd = AirLoopHVAC.demandOutletNode();
            objsAfterBranch.ToList().ForEach(_ => _.AddToNode(dmOutNd));
            

            var addObjs = AirLoopHVAC.demandComponents().Where(_ => _.comment().Contains("TrackingID"));
            var allcopied = addObjs.Count() == filteredObjs.CountWithBranches();

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
