using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.HVAC
{
    public class IB_PlantLoop : IB_Loop
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_PlantLoop();

        private List<IB_HVACObject> supplyComponents { get; set; } = new List<IB_HVACObject>();
        private List<IB_HVACObject> demandComponents { get; set; } = new List<IB_HVACObject>();

        private IB_SizingPlant IB_SizingPlant { get; set; } = new IB_SizingPlant();
        
        private static PlantLoop NewDefaultOpsObj(Model model) => new PlantLoop(model);
        public IB_PlantLoop() : base(NewDefaultOpsObj(new Model()))
        {
        }
        public void SetSizingPlant(IB_SizingPlant sizing)
        {
            this.IB_SizingPlant = sizing;
        }

        public void AddToSupply(IB_HVACObject HvacComponent)
        {
            if (!(HvacComponent is IIB_PlantLoopObjects))
                throw new ArgumentException($"{HvacComponent.GetType()} is not a plant loop object.\nOnly plant loop object is allowed to add to plantloop!");

            if (HvacComponent is IB_PlantLoopBranches)
            {
                CheckWithBranch(this.supplyComponents);
            }

            this.supplyComponents.Add(HvacComponent);
        }
        

        public void AddToDemand(IB_HVACObject HvacComponent)
        {
            if (!(HvacComponent is IIB_PlantLoopObjects))
                throw new ArgumentException($"{HvacComponent.GetType()} is not a plant loop object.\nOnly plant loop object is allowed to add to plantloop!");

            if (HvacComponent is IB_PlantLoopBranches)
            {
                CheckWithBranch(this.demandComponents);
            }

            this.demandComponents.Add(HvacComponent);
            
        }

        public override ModelObject ToOS(Model model)
        {
            var plant = base.OnNewOpsObj(NewDefaultOpsObj, model).to_PlantLoop().get();

            IB_SizingPlant.ToOS(plant);

            this.AddSupplyObjects(plant, this.supplyComponents);

            this.AddDemandObjects(plant, this.demandComponents);

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

        public new IB_PlantLoop Duplicate()
        {

            var newObj = this.DuplicateIBObj(() => new IB_PlantLoop());

            this.supplyComponents.ForEach(d =>
                newObj.AddToSupply(d.Duplicate())
                );

            this.demandComponents.ForEach(d =>
                newObj.AddToDemand(d.Duplicate())
                );

            newObj.SetSizingPlant((IB_SizingPlant)this.IB_SizingPlant.Duplicate());

            return newObj;
        }


        private bool AddSupplyObjects(PlantLoop plant, List<IB_HVACObject> Components)
        {

            //Find the branch object first, and mark it. 
            //Reverce the objects order before the mark (supplyInletNode)
            //keep the order (supplyOutletNode);
            var filteredObjs = Components.Where(_ => !(_ is IB_SetpointManager));
            (var objsBeforeBranch, var branchObj, var objsAfterBranch) = base.GetObjsBeforeAndAfterBranch(filteredObjs);
            
            var spInletNode = plant.supplyInletNode();
            //objsBeforeBranch.ToList().ForEach(_ => _.AddToNode(spInletNode));
            objsBeforeBranch.ToList().ForEach(_ => 
            {
                if (!_.AddToNode(spInletNode))
                {
                    throw new ArgumentException($"Failed to add {_.GetType()} to {this.GetType()}!");
                }
            });

            if (branchObj != null)
            {
                ((IB_PlantLoopBranches)branchObj).ToOS_Supply(plant);
            }
            
            var spOutLetNode = plant.supplyOutletNode();
            //objsAfterBranch.ToList().ForEach(_ => _.AddToNode(spOutLetNode));
            objsAfterBranch.ToList().ForEach(_ => 
            {
                if (!_.AddToNode(spOutLetNode))
                {
                    throw new ArgumentException($"Failed to add {_.GetType()} to {this.GetType()}!");
                }
            });

            var addedObjs = plant.supplyComponents().Where(_ => _.comment().Contains("TrackingID"));
            var allcopied = addedObjs.Count() == filteredObjs.CountWithBranches();
            
            //TODO: might need to double check the set point order.
            allcopied &= this.AddSetPoints(spInletNode, Components);

            if (!allcopied)
            {
                throw new ArgumentException("Failed to add plant loop's supply components!");
            }

            return allcopied;
        }

        

        private bool AddDemandObjects(PlantLoop plant, List<IB_HVACObject> Components)
        {

            //Find the branch object first, and mark it. 
            //Reverce the objects order before the mark (supplyInletNode)
            //keep the order (supplyOutletNode);
            var filteredObjs = Components.Where(_ => !(_ is IB_SetpointManager));
            (var objsBeforeBranch, var branchObj, var objsAfterBranch) = base.GetObjsBeforeAndAfterBranch(Components);
            //var branchObj = (IB_PlantLoopBranches)Components.Find(_ => _ is IB_PlantLoopBranches);


            //
            var deInletNode = plant.demandInletNode();
            objsBeforeBranch.ToList().ForEach(_ => _.AddToNode(deInletNode));

            if (branchObj != null)
            {
                ((IB_PlantLoopBranches)branchObj).ToOS_Demand(plant);
            }

            var deOutLetNode = plant.demandOutletNode();
            objsAfterBranch.ToList().ForEach(_ => _.AddToNode(deOutLetNode));

            
            var addedObjs = plant.demandComponents().Where(_ => _.comment().Contains("TrackingID"));

            //var allcopied = addedObjs.Count() == Components.CountWithBranches();


            //TODO: might need to double check the setpoint order.
            var allcopied = this.AddSetPoints(deInletNode, Components);

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


    }


    public sealed class IB_PlantLoop_DataFieldSet
        : IB_FieldSet<IB_PlantLoop_DataFieldSet, PlantLoop>
    {
        private IB_PlantLoop_DataFieldSet() { }

        public IB_Field Name { get; }
            = new IB_BasicField("Name", "Name");

        public IB_Field FluidType { get; }
            = new IB_BasicField("FluidType", "Fluid")
            {
                DetailedDescription = "Water, Steam, etc. "
            };

    }



}

