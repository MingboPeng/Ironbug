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
                plant.addSupplyBranchForComponent((HVACComponent)branch.First().ToOS(model));
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
            var branches = this.CheckPuppetsInBranches(this).Branches;
            var plant = PlantLoop as PlantLoop;
            var model = PlantLoop.model();
            foreach (var branch in branches)
            {
                //flatten the puppet structure 

                var items = branch.SelectMany(_ => _.GetPuppetsOrSelf()).Select(_ => _ as IB_HVACObject);
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

                ////TODO: testing puppets only
                //var items = branch.SelectMany(_ => _.GetPuppetsOrSelf());
                //foreach (var item in items)
                //{
                //    var IB_obj = item as IB_HVACObject;
                //    plant.addDemandBranchForComponent((HVACComponent)IB_obj.ToOS(model));
                //}


            }
        }

        public IB_PlantLoopBranches CheckPuppetsInBranches(IB_LoopBranches IB_Branches)
        {
            
            var branches = IB_Branches.Branches;
            var newBranches = new IB_PlantLoopBranches();
            foreach (var branch in branches)
            {
                //check branch if there is puppets,
                //if yes, duplicate this branch to match number of puppets
                var optionalPuppetHost = branch.Where(_ => _.IsPuppetHost()).FirstOrDefault();
                if (optionalPuppetHost != null)
                {
                    var puppets = branch.SelectMany(_ => _.GetPuppetsOrSelf()).Select(_ => _ as IB_HVACObject);

                    foreach (var puppet in puppets)
                    {
                        var newBranch = new List<IB_HVACObject>();

                        //TODO: use branch.FindIndex()

                        foreach (var item in branch)
                        {
                            //check if it is puppet host,
                            //if yes, replace it by its puppet
                            if (item.IsPuppetHost())
                            {
                                newBranch.Add(puppet);
                            }
                            else
                            {
                                var dupItem = item.Duplicate() as IB_HVACObject; //if is not puppet, duplicate this
                                newBranch.Add(dupItem);
                            }
                        }

                        newBranches.Add(newBranch);

                    }

                }
                else
                {
                    newBranches.Add(branch);
                }


            }

            return newBranches;
               
        }
        
    }

    


}
