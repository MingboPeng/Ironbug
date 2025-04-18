﻿using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Ironbug.HVAC
{
    public class IB_PlantLoop : IB_Loop, IEquatable<IB_PlantLoop>
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_PlantLoop();

        [DataMember]
        public IB_SizingPlant SizingPlant { get; private set; } = new IB_SizingPlant();
        [DataMember]
        public IB_PlantEquipmentOperationSchemeBase OperationScheme { get; private set; }

        private static PlantLoop NewDefaultOpsObj(Model model) => new PlantLoop(model);
        public IB_PlantLoop() : base(NewDefaultOpsObj)
        {
        }
        public void SetSizingPlant(IB_SizingPlant sizing)
        {
            this.SizingPlant = sizing;
            //var obj = this.GhostOSObject as PlantLoop;
            //this.IB_SizingPlant.ToOS(this.GhostOSObject as PlantLoop);
            
        }
        public void SetOperationScheme(IB_PlantEquipmentOperationSchemeBase schema)
        {
            this.OperationScheme = schema;
        }

        public void AddToSupply(IB_HVACObject HvacComponent)
        {
            if (!(HvacComponent is IIB_PlantLoopObjects))
                throw new ArgumentException($"{HvacComponent.GetType()} is not a plant loop object.\nOnly plant loop object is allowed to add to plantloop!");

            if (HvacComponent is IB_PlantLoopBranches)
            {
                CheckWithBranch(this.SupplyComponents);
            }

            this.SupplyComponents.Add(HvacComponent);
        }
        

        public void AddToDemand(IB_HVACObject HvacComponent)
        {
            if (!(HvacComponent is IIB_PlantLoopObjects))
                throw new ArgumentException($"{HvacComponent.GetType()} is not a plant loop object.\nOnly plant loop object is allowed to add to plantloop!");

            if (HvacComponent is IB_PlantLoopBranches)
            {
                CheckWithBranch(this.DemandComponents);
            }

            this.DemandComponents.Add(HvacComponent);
            
        }

        public override ModelObject ToOS(Model model)
        {
            var plant = base.OnNewOpsObj(NewDefaultOpsObj, model).to_PlantLoop().get();

            SizingPlant.ToOS(model, plant);

            this.AddSupplyObjects(model, plant, this.SupplyComponents);
            this.AddDemandObjects(model, plant, this.DemandComponents);

            this.OperationScheme?.ToOS(model, plant);
            return plant;
        }

        //protected override ModelObject NewOpsObj(Model model)
        //{
        //    var plant = base.OnNewOpsObj(NewDefaultOpsObj, model).to_PlantLoop().get();

        //    IB_SizingPlant.ToOS(plant);

        //    this.AddSupplyObjects(plant, this.supplyComponents);
            
        //    this.AddDemandObjects(plant, this.demandComponents);
            
        //    return plant;
        //}

        public override IB_ModelObject Duplicate()
        {

            var newObj = base.Duplicate() as IB_PlantLoop;

            newObj.SetSizingPlant((IB_SizingPlant)this.SizingPlant.Duplicate());
            newObj.OperationScheme = this.OperationScheme?.Duplicate() as IB_PlantEquipmentOperationSchemeBase;
            return newObj;
        }


        private bool AddSupplyObjects(Model model, PlantLoop plant, List<IB_HVACObject> Components)
        {

            //Find the branch object first, and mark it. 
            //Revere the objects order before the mark (supplyInletNode)
            //keep the order (supplyOutletNode);
            var filteredObjs = Components.Where(_ => !(_ is IB_SetpointManager) && !(_ is IB_NodeProbe));
            (var objsBeforeBranch, var branchObj, var objsAfterBranch) = base.GetObjsBeforeAndAfterBranch(filteredObjs);
            
            var spInletNode = plant.supplyInletNode();
            //objsBeforeBranch.ToList().ForEach(_ => _.AddToNode(spInletNode));
            objsBeforeBranch.ToList().ForEach(_ => 
            {
                if (!_.AddToNode(model, spInletNode))
                {
                    throw new ArgumentException($"Failed to add {_.GetType()} to {this.GetType()}!");
                }
            });

            if (branchObj != null)
            {
                ((IB_PlantLoopBranches)branchObj).ToOS_Supply(model, plant);
            }
            
            var spOutLetNode = plant.supplyOutletNode();
            //objsAfterBranch.ToList().ForEach(_ => _.AddToNode(spOutLetNode));
            objsAfterBranch.ToList().ForEach(_ => 
            {
                if (!_.AddToNode(model, spOutLetNode))
                {
                    throw new ArgumentException($"Failed to add {_.GetType()} to {this.GetType()}!");
                }
            });

            var addedObjs = plant.supplyComponents().Where(_ => _.comment().Contains("TrackingID"));
            var allcopied = addedObjs.Count() == filteredObjs.CountWithBranches();
            
            //TODO: might need to double check the set point order.
            allcopied &= this.AddSetPoints(model, spInletNode, Components);
            allcopied &= this.AddNodeProbe(model, spInletNode, Components);

            if (!allcopied)
            {
                throw new ArgumentException("Failed to add plant loop's supply components!");
            }

            return allcopied;
        }

        

        private bool AddDemandObjects(Model model, PlantLoop plant, List<IB_HVACObject> Components)
        {

            //Find the branch object first, and mark it. 
            //Reverse the objects order before the mark (supplyInletNode)
            //keep the order (supplyOutletNode);
            var filteredObjs = Components.Where(_ => !(_ is IB_SetpointManager) && !(_ is IB_NodeProbe));
            (var objsBeforeBranch, var branchObj, var objsAfterBranch) = base.GetObjsBeforeAndAfterBranch(Components);
            //var branchObj = (IB_PlantLoopBranches)Components.Find(_ => _ is IB_PlantLoopBranches);


            //
            var deInletNode = plant.demandInletNode();
            objsBeforeBranch.ToList().ForEach(_ => _.AddToNode(model, deInletNode));

            if (branchObj != null)
            {
                ((IB_PlantLoopBranches)branchObj).ToOS_Demand(model, plant);
            }

            var deOutLetNode = plant.demandOutletNode();
            objsAfterBranch.ToList().ForEach(_ => _.AddToNode(model, deOutLetNode));

            
            var addedObjs = plant.demandComponents().Where(_ => _.comment().Contains("TrackingID"));

            //var allcopied = addedObjs.Count() == Components.CountWithBranches();


            //TODO: might need to double check the setpoint order.
            var allcopied = this.AddSetPoints(model, deInletNode, Components);
            allcopied &= this.AddNodeProbe(model, deInletNode, Components);

            if (!allcopied)
            {
                throw new Exception("Failed to add plant loop's demand components!");
            }

            return allcopied;
        }

        private bool CheckWithBranch(IEnumerable<IB_HVACObject> HvacComponents)
        {
            var nranchesObjCount = HvacComponents.OfType<IB_PlantLoopBranches>().Count();
            var isThereAlreadyOne = nranchesObjCount >= 1;
            if (isThereAlreadyOne)
            {
                throw new Exception("Each side of the loop can only have one branch group.");
            }

            return !isThereAlreadyOne;
        }

        public override bool Equals(object obj)=> this.Equals(obj as IB_PlantLoop);
        public bool Equals(IB_PlantLoop other)
        {
            if (!base.Equals(other))
                return false;

            if (this.SizingPlant != other.SizingPlant)
                return false;
            if (this.OperationScheme != other.OperationScheme)
                return false;
            return true;
        }
    }


    public sealed class IB_PlantLoop_FieldSet
        : IB_FieldSet<IB_PlantLoop_FieldSet, PlantLoop>
    {
        private IB_PlantLoop_FieldSet() { }

        public IB_Field Name { get; }
            = new IB_BasicField("Name", "Name");

        public IB_Field FluidType { get; }
            = new IB_BasicField("FluidType", "Fluid")
            {
                DetailedDescription = "Water, Steam, etc. "
            };

    }



}

