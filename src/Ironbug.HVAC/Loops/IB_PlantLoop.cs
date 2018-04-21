using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public class IB_PlantLoop : IB_Loop
    {
        private List<IB_HVACObject> supplyComponents { get; set; } = new List<IB_HVACObject>();
        private List<IB_HVACObject> demandComponents { get; set; } = new List<IB_HVACObject>();

        private IB_SizingPlant IB_SizingPlant { get; set; } = new IB_SizingPlant();

        private static PlantLoop InitMethod(Model model) => new PlantLoop(model);
        public IB_PlantLoop() : base(InitMethod(new Model()))
        {
        }
        public void SetSizingPlant(IB_SizingPlant sizing)
        {
            this.IB_SizingPlant = sizing;
        }

        public void AddToSupply(IB_HVACObject HvacComponent)
        {

            
            if (HvacComponent is IB_PlantLoopBranches)
            {
                CheckWithBranch(this.supplyComponents);
            }

            this.supplyComponents.Add(HvacComponent);
        }
        

        public void AddToDemand(IB_HVACObject HvacComponent)
        {
            if (HvacComponent is IB_PlantLoopBranches)
            {
                CheckWithBranch(this.demandComponents);
            }

            this.demandComponents.Add(HvacComponent);
            
        }


        public override ModelObject ToOS(Model model)
        {
            var plant = base.ToOS(InitMethod, model).to_PlantLoop().get();

            IB_SizingPlant.ToOS(plant);

            this.AddSupplyObjects(plant, this.supplyComponents);
            
            this.AddDemandObjects(plant, this.demandComponents);
            
            return plant;
        }

        public override IB_ModelObject Duplicate()
        {

            var newObj = (IB_PlantLoop)this.DuplicateIBObj(() => new IB_PlantLoop());

            this.supplyComponents.ForEach(d =>
                newObj.AddToSupply((IB_HVACObject)d.Duplicate())
                );

            this.demandComponents.ForEach(d =>
                newObj.AddToDemand((IB_HVACObject)d.Duplicate())
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
            objsBeforeBranch.ToList().ForEach(_ => _.AddToNode(spInletNode));

            if (branchObj != null)
            {
                ((IB_PlantLoopBranches)branchObj).ToOS_Supply(plant);
            }
            

            var spOutLetNode = plant.supplyOutletNode();
            objsAfterBranch.ToList().ForEach(_ => _.AddToNode(spOutLetNode));

            
            var addedObjs = plant.supplyComponents().Where(_ => _.comment().Contains("TrackingID"));
            var allcopied = addedObjs.Count() == filteredObjs.CountWithBranches();
            
            //TODO: might need to double check the set point order.
            allcopied &= this.AddSetPoints(spInletNode, Components);

            if (!allcopied)
            {
                throw new Exception("Failed to add plant loop's supply components!");
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
            var allcopied = addedObjs.Count() == Components.CountWithBranches();


            //TODO: might need to double check the setpoint order.
            allcopied &= this.AddSetPoints(deInletNode, Components);

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
        : IB_DataFieldSet<IB_PlantLoop_DataFieldSet, PlantLoop>
    {
        private IB_PlantLoop_DataFieldSet() { }

        public IB_DataField FluidType { get; }
            = new IB_BasicDataField("FluidType", "Fluid")
            {
                DetailedDescription = "Water, Steam, etc. "
            };

    }


    public static class Extensions
    {
        public static int CountWithBranches(this IEnumerable<IB_HVACObject> enumerable)
        {
            var count = 0;
            foreach (var item in enumerable)
            {
                if (item is IB_PlantLoopBranches)
                {
                    count += ((IB_PlantLoopBranches)item).Count();
                }
                else if (item is IB_AirLoopBranches)
                {
                    count += ((IB_AirLoopBranches)item).Count()*2; // because added air terminal with each zone
                }
                else
                {
                    count++;
                }
            }

            return count;
        }
    }
}

