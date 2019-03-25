using System;
using System.Collections.Generic;
using System.Linq;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_PlantLoopBranches : IB_LoopBranches, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_PlantLoopBranches();

        public void ToOS_Supply(Loop PlantLoop)
        {
            var branches = this.Branches;
            var plant = PlantLoop as PlantLoop;
            var model = PlantLoop.model();
            foreach (var branch in branches)
            {
                //add one branch
                plant.addSupplyBranchForComponent(branch.First().ToOS(model));
                //add the rest child in this branch
                var restChild = branch.Skip(1);
                foreach (var item in restChild)
                {
                    var node = plant.supplyMixer().inletModelObjects().Last().to_Node().get();
                    if (!item.AddToNode(node))
                        throw new ArgumentException($"Failed to add {item.GetType()} to {this.GetType()}!");
                    
                }
            }
        }

        public void ToOS_Demand(Loop PlantLoop)
        {
            var branches = this.Branches;
            var plant = PlantLoop as PlantLoop;
            var model = PlantLoop.model();
            foreach (var branch in branches)
            {
                //flatten the puppet structure 

                var items = branch;
                //add one branch
                var firstItem = items.First();
                plant.addDemandBranchForComponent(firstItem.ToOS(model));
                //add the rest child in this branch
                var restChild = items.Skip(1);
                //TDDO: double check the obj order here
                var node = plant.demandMixer().inletModelObjects().Last().to_Node().get();
                foreach (var item in restChild)
                {
                    
                    if (!item.AddToNode(node))
                        throw new ArgumentException($"Failed to add {item.GetType()} to {this.GetType()}!");
                }

    

            }
        }


        
    }

    


}
