using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_AirLoopHVAC : IB_Loop, IEquatable<IB_AirLoopHVAC>
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_AirLoopHVAC();
        [DataMember]
        public IB_SizingSystem SizingSystem { get; private set; } = new IB_SizingSystem();

        private static AirLoopHVAC NewDefaultOpsObj(Model model) => new AirLoopHVAC(model);

        public IB_AirLoopHVAC() : base(NewDefaultOpsObj)
        {
        }

        public void SetSizingSystem(IB_SizingSystem sizing)
        {
            this.SizingSystem = sizing;
            this.SizingSystem.ToOS(this.GhostOSModel, this.GhostOSObject as AirLoopHVAC);
        }


        public void AddToSupplySide(IB_HVACObject HvacComponent)
        {
            //TODO: check before add
            if (HvacComponent is IIB_AirLoopObject)
            {
                this.SupplyComponents.Add(HvacComponent);
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
                this.DemandComponents.Add(HvacComponent);
            }
            else
            {
                throw new Exception("Only airloop object or setpoint manager are allowed to add to airloop demand side!");
            }
            

        }

        public virtual List<IB_ThermalZone> GetThermalZones()
        {
            return this.DemandComponents
                .OfType<IB_AirLoopBranches>()
                .SelectMany(_ => _.Branches)
                .SelectMany(_ => _)
                .OfType<IB_ThermalZone>()
                ?.ToList();
        }


        public override ModelObject ToOS(Model model)
        {
            this.CheckSupplySide(this.SupplyComponents);
            
            var airLoopHVAC = base.OnNewOpsObj(NewDefaultOpsObj, model);
            this.SizingSystem.ToOS(model, airLoopHVAC);
            
            this.AddSupplyObjects(model, airLoopHVAC, this.SupplyComponents);
            this.AddDemandObjects(model, airLoopHVAC, this.DemandComponents);
            
            return airLoopHVAC;
        }

        private bool CheckSupplySide(IEnumerable<IB_HVACObject> Components)
        {
            var fanCount = Components.Count(_ => (_ is IB_Fan)|(_ is IB_AirLoopHVACUnitary));
            if (fanCount == 0)
            {
                throw new Exception("Airloop needs at least one fan!");
            }
            else
            {
                return true;
            }
            

        }
        private bool AddSupplyObjects(Model model, AirLoopHVAC AirLoopHVAC, IEnumerable<IB_HVACObject> Components)
        {
            var spnd = AirLoopHVAC.supplyOutletNode();
            var comps = Components.Where(_ => !(_ is IB_SetpointManager) && !(_ is IB_NodeProbe));

            foreach (var comp in comps)
            {
                var added = comp.AddToNode(model, spnd);
                if (!added)
                    throw new ArgumentException($"Warning: Failed to add {comp.GetType()} to air loop supply side!");

            }


            if (!this.AddSetPoints(model, AirLoopHVAC.supplyInletNode(), Components))
                throw new ArgumentException("Warning: Failed to add all set point managers to air loop supply side!");
            if (!this.AddNodeProbe(model, AirLoopHVAC.supplyInletNode(), Components))
                throw new ArgumentException("Warning: Failed to add all node probes to air loop supply side!");

            return true;
        }

        private bool AddDemandObjects(Model model, AirLoopHVAC AirLoopHVAC, IEnumerable<IB_HVACObject> Components)
        {
            var filteredObjs = Components.Where(_ => !(_ is IB_SetpointManager) && !(_ is IB_NodeProbe));
            (var objsBeforeBranch, var branchObj, var objsAfterBranch) = base.GetObjsBeforeAndAfterBranch(filteredObjs);

            //add objs before branch
            var dmInNd = AirLoopHVAC.demandInletNode();
            objsBeforeBranch.ToList().ForEach(_ => _.AddToNode(model, dmInNd));

            //add branch
            if (branchObj != null)
            {
                ((IB_AirLoopBranches)branchObj).ToOS_Demand(model, AirLoopHVAC);
            }

            //add objs after branch
            var dmOutNd = AirLoopHVAC.demandOutletNode();
            objsAfterBranch.ToList().ForEach(_ => _.AddToNode(model, dmOutNd));
            

            var addObjs = AirLoopHVAC.demandComponents().Where(_ => _.comment().Contains("TrackingID"));
            var allcopied = addObjs.Count() == filteredObjs.CountWithBranches();
            if (!allcopied)
                throw new ArgumentException("Failed to add all airloop demand components!");

            //TODO: might need to double check the set point order.
            if (!this.AddSetPoints(model, dmInNd, Components))
                throw new ArgumentException("Failed to add all set point managers to airloop demand side!");

            if (!this.AddNodeProbe(model, dmInNd, Components))
                throw new ArgumentException("Failed to add all node probes to airloop demand side!");

            return allcopied;
        }


        public override IB_ModelObject Duplicate()
        {
            var newObj = base.Duplicate() as IB_AirLoopHVAC;
            newObj.SizingSystem = this.SizingSystem.Duplicate() as IB_SizingSystem;

            return newObj;
        }

        public override bool Equals(object obj) => this.Equals(obj as IB_AirLoopHVAC);
        public bool Equals(IB_AirLoopHVAC other)
        {
            if (!base.Equals(other))
                return false;

            if (this.SizingSystem != other.SizingSystem)
                return false;

            return true;
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
