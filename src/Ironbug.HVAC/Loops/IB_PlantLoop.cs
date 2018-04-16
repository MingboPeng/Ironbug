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

        private static PlantLoop InitMethod(Model model) => new PlantLoop(model);
        public IB_PlantLoop() : base(InitMethod(new Model()))
        {
        }

        public void AddToSupply(IB_HVACObject HvacComponent)
        {
            if (CheckWithBranch(HvacComponent))
            {
                this.supplyComponents.Add(HvacComponent);
            }
            else
            {
                throw new Exception("Supply side can only have one branch group.");
            }
            
            
        }

        private bool CheckWithBranch(IB_HVACObject HvacComponent)
        {
            var isThereBranchesObj = this.supplyComponents.OfType<IB_PlantLoopBranches>().Any();
            var isBranchesObj = HvacComponent is IB_PlantLoopBranches;
            
            return !(isThereBranchesObj && isBranchesObj);
        }

        public void AddToDemand(IB_HVACObject HvacComponent)
        {

            if (CheckWithBranch(HvacComponent))
            {
                this.demandComponents.Add(HvacComponent);
            }
            else
            {
                throw new Exception("Demand side can only have one branch group.");
            }
        }


        public override ModelObject ToOS(Model model)
        {
            var plant = base.ToOS(InitMethod, model).to_PlantLoop().get();



            this.AddSupplyObjects(plant, this.supplyComponents);

            //TDDO: addDemandObjects
            this.AddDemandObjects(plant, this.demandComponents);
           


            return plant;
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

        public override IB_ModelObject Duplicate()
        {
            //TODO: duplicate child objects
            return this.DuplicateIBObj(() => new IB_PlantLoop());
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

