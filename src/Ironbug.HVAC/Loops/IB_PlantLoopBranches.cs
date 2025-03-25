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

        public void ToOS_Supply(Model model, Loop plantLoop)
        {
            var branches = this.Branches;
            var plant = plantLoop as PlantLoop;

            foreach (var branch in branches)
            {
                //add one branch
                plant.addSupplyBranchForComponent(branch.First().ToOS(model));
                //add the rest child in this branch
                var restChild = branch.Skip(1);
                foreach (var item in restChild)
                {
                    var node = plant.supplyMixer().inletModelObjects().Last().to_Node().get();
                    if (!item.AddToNode(model, node))
                        throw new ArgumentException($"Failed to add {item.GetType()} to {this.GetType()}!");
                    
                }
            }
        }

        public void ToOS_Demand(Model model, Loop PlantLoop)
        {
            var branches = this.Branches;
            var plant = PlantLoop as PlantLoop;
            foreach (var branch in branches)
            {
                //flatten the puppet structure 
                var items = branch;
                //add one branch
                plant.addDemandBranchForComponent(items.First().ToOS(model));
                //add the rest child in this branch
                var restChild = items.Skip(1);
                if (restChild.Any())
                {
                    //TDDO: double check the obj order here
                    var node = plant.demandMixer().inletModelObjects().Last().to_Node().get();
                    foreach (var item in restChild)
                    {
                        if (!item.AddToNode(model, node))
                            throw new ArgumentException($"Failed to add {item.GetType()} to {this.GetType()}!");
                    }
                }
                
            }
        }

        //private static bool AddCheckSpecialObjects(IB_HVACObject obj, Model model, PlantLoop plant, Node node)
        //{
        //    if (plant == null && node == null)
        //        throw new ArgumentException($"Plantloop and node cannot be null at the same time!");

        //    // for water-coolded chiller, it has to be added to condenser plant's demand side first
        //    if (obj is IB_ChillerElectricEIR chiller)
        //    {
        //        // create openstudio object without attributes
        //        var c = chiller.ToOS(model, false);
        //        // add to plantloop
        //        if (plant != null)
        //            plant.addDemandBranchForComponent(c);
        //        else
        //            c.addToNode(node);
        //        // assign attributes
        //        obj.ApplyAttributesToObj(c);
        //        return true;
        //    }


        //    // regular objects
        //    var done = false;
        //    if (plant != null)
        //    {
        //        plant.addDemandBranchForComponent(obj.ToOS(model));
        //        done = true;
        //    }
        //    else
        //    {
        //        done = obj.AddToNode(node);
        //    }

        //    return done;
        //}


        
    }

    


}
