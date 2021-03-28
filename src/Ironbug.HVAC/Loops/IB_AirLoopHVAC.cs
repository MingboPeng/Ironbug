using System;
using System.Collections.Generic;
using System.Linq;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_AirLoopHVAC : IB_Loop
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_AirLoopHVAC();

        private List<IB_HVACObject> supplyComponents { get; set; }= new List<IB_HVACObject>();
        private List<IB_HVACObject> demandComponents { get; set; } = new List<IB_HVACObject>();

        private IB_SizingSystem _SizingSystem { get; set; } = new IB_SizingSystem();

        private static AirLoopHVAC NewDefaultOpsObj(Model model) => new AirLoopHVAC(model);

        public IB_AirLoopHVAC() : base(NewDefaultOpsObj(new Model()))
        {
            //this.basePoint = new Point3d();
            //this.osModel = new Model();
            //this.ghostAirLoopHVAC = new AirLoopHVAC(new Model());
        }

        public void SetSizingSystem(IB_SizingSystem sizing)
        {
            this._SizingSystem = sizing;
            this._SizingSystem.ToOS(this.GhostOSObject as AirLoopHVAC);
        }


        public void AddToSupplySide(IB_HVACObject HvacComponent)
        {
            //TODO: check before add
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
                throw new Exception("Only airloop object or setpoint manager are allowed to add to airloop demand side!");
            }
            

        }

        public override IB_ModelObject Duplicate()
        {
            var newObj = this.Duplicate(() => new IB_AirLoopHVAC());

            this.supplyComponents.ForEach(d =>
                newObj.AddToSupplySide(d.Duplicate() as IB_HVACObject)
                );

            this.demandComponents.ForEach(d =>
                newObj.AddToDemandSide(d.Duplicate() as IB_HVACObject)
                );

            newObj.SetSizingSystem(this._SizingSystem.Duplicate() as IB_SizingSystem);

            return newObj;
        }

        public override ModelObject ToOS(Model model)
        {
            this.CheckSupplySide(this.supplyComponents);
            
            var airLoopHVAC = base.OnNewOpsObj(NewDefaultOpsObj, model);
            this._SizingSystem.ToOS(airLoopHVAC);
            
            this.AddSupplyObjects(airLoopHVAC, this.supplyComponents);
            this.AddDemandObjects(airLoopHVAC, this.demandComponents);
            
            return airLoopHVAC;
        }

        private bool CheckSupplySide(IEnumerable<IB_HVACObject> Components)
        {
            var fanCount = Components.Count(_ => (_ is IB_Fan)| (_ is IB_AirLoopHVACUnitarySystem));
            if (fanCount == 0)
            {
                throw new Exception("Airloop needs at least one fan!");
            }
            else
            {
                return true;
            }
            

        }
        private bool AddSupplyObjects(AirLoopHVAC AirLoopHVAC, IEnumerable<IB_HVACObject> Components)
        {
            var spnd = AirLoopHVAC.supplyOutletNode();
            var comps = Components.Where(_ => !(_ is IB_SetpointManager) && !(_ is IB_Probe));

            var allcopied = true;
            foreach (var comp in comps)
            {
                allcopied &= comp.AddToNode(spnd);
            }
            if (!allcopied)
                throw new ArgumentException("Warning: Failed to add all air loop supply components!");


            if (!this.AddSetPoints(AirLoopHVAC.supplyInletNode(), Components))
                throw new ArgumentException("Warning: Failed to add all set point managers to air loop supply side!");
            if (!this.AddNodeProbe(AirLoopHVAC.supplyInletNode(), Components))
                throw new ArgumentException("Warning: Failed to add all node probes to air loop supply side!");

            return allcopied;
        }

        private bool AddDemandObjects(AirLoopHVAC AirLoopHVAC, IEnumerable<IB_HVACObject> Components)
        {
            var filteredObjs = Components.Where(_ => !(_ is IB_SetpointManager) && !(_ is IB_Probe));
            (var objsBeforeBranch, var branchObj, var objsAfterBranch) = base.GetObjsBeforeAndAfterBranch(filteredObjs);

            //add objs before branch
            var dmInNd = AirLoopHVAC.demandInletNode();
            objsBeforeBranch.ToList().ForEach(_ => _.AddToNode(dmInNd));

            //add branch
            if (branchObj != null)
            {
                ((IB_AirLoopBranches)branchObj).ToOS_Demand(AirLoopHVAC);
            }

            //add objs after branch
            var dmOutNd = AirLoopHVAC.demandOutletNode();
            objsAfterBranch.ToList().ForEach(_ => _.AddToNode(dmOutNd));
            

            var addObjs = AirLoopHVAC.demandComponents().Where(_ => _.comment().Contains("TrackingID"));
            var allcopied = addObjs.Count() == filteredObjs.CountWithBranches();
            if (!allcopied)
                throw new ArgumentException("Failed to add all airloop demand components!");

            //TODO: might need to double check the set point order.
            if (!this.AddSetPoints(dmInNd, Components))
                throw new ArgumentException("Failed to add all set point managers to airloop demand side!");

            if (!this.AddNodeProbe(dmInNd, Components))
                throw new ArgumentException("Failed to add all node probes to airloop demand side!");

            return allcopied;
        }

        

        
    }

    public sealed class IB_AirLoopHVAC_FieldSet
      : IB_FieldSet<IB_AirLoopHVAC_FieldSet, AirLoopHVAC>
    {
        private IB_AirLoopHVAC_FieldSet() { }
        public IB_Field NightCycleControlType { get; }
            = new IB_BasicField("NightCycleControlType", "NightCycle") { DetailedDescription = "Valid inputs are: StayOff, CycleOnAny, and CycleOnAnyZoneFansOnly."};

        

    }






}
