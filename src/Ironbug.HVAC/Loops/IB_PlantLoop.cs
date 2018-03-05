using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public class IB_PlantLoop:IB_ModelObject, IIB_ToOPSable
    {
        private List<IB_HVACObject> supplyComponents { get; set; } = new List<IB_HVACObject>();
        private List<IB_HVACObject> demandComponents { get; set; } = new List<IB_HVACObject>();

        private static PlantLoop InitMethod(Model model) => new PlantLoop(model);
        public IB_PlantLoop():base(InitMethod(new Model()))
        {
        }
        
        public void AddToSupply(IB_HVACObject HvacComponent)
        {
            this.supplyComponents.Add(HvacComponent);
        }

        public void AddToDemand(IB_HVACObject HvacComponent)
        {
            this.demandComponents.Add(HvacComponent);
        }
        
        
        public ModelObject ToOS(Model model)
        {
            var plant = base.ToOS(InitMethod, model).to_PlantLoop().get();

            //TODO: add IB_Loop to take care of branches matter
            // below this is for temporary testing purpose before supply branch is finished.
            //var boiler = new BoilerHotWater(model);
            //plant.addSupplyBranchForComponent(boiler);


            //Find the branch object first, and mark it. 
            //Reverce the objects order before the mark (supplyInletNode)
            //keep the order (supplyOutletNode);
            var objsBeforeBranch = this.GetObjsBeforeBranch(this.supplyComponents);
            var branchObj = (IB_PlantLoopBranches)this.supplyComponents.Find(_ => _ is IB_PlantLoopBranches);
            var objsAfterBranch = new List<IB_HVACObject>();


            //
            var spInletNode = plant.supplyInletNode();
            var spOutLetNode = plant.supplyOutletNode();

            foreach (var item in objsBeforeBranch)
            {
                item.AddToNode(spInletNode);

                //if (item is IB_LoopBranches) 
                //{
                //    plant.addSupplyBranchForComponent((HVACComponent)item.ToOS(model));
                //}
                //else
                //{
                   
                //    //plant.nod
                //    //item.AddToNode()
                //}
                
            }
            var branches = branchObj.Branches;
            branches.Reverse();
            foreach (var branch in branches)
            {
                plant.addSupplyBranchForComponent((HVACComponent)branch.First().ToOS(model));
                foreach (var item in branch.Skip(1))
                {
                    var node  = plant.supplyMixer().inletModelObjects().Last().to_Node().get();
                    item.AddToNode(node);
                }
            }

            foreach (var item in objsAfterBranch)
            {
                item.AddToNode(spOutLetNode);
            }


            foreach (var item in demandComponents)
            {
                plant.addDemandBranchForComponent((HVACComponent)item.ToOS(model));
            }

            
            return plant;
        }

        public override IB_ModelObject Duplicate()
        {
            //TODO: duplicate child objects
            return this.DuplicateIB_ModelObject(() => new IB_PlantLoop());
        }

        private IEnumerable<IB_HVACObject> GetObjsBeforeBranch(List<IB_HVACObject> SupplyOrDemandObjs)
        {
            int branchIndex = SupplyOrDemandObjs.FindIndex(_ => _ is IB_PlantLoopBranches);

            //TODO: check the branch index (<0, or 0)
            var beforeBranch = SupplyOrDemandObjs.Take(branchIndex);
            ////SupplyOrDemandObjs.RemoveRange(0, branchIndex);

            //var newList = new List<IB_HVACObject>();
            //newList.AddRange(beforeBranch.Reverse());
            //newList.AddRange(SupplyOrDemandObjs.Skip(branchIndex));

            return beforeBranch;

        }

        private IEnumerable<IB_HVACObject> GetObjsAfterBranch(List<IB_HVACObject> SupplyOrDemandObjs)
        {
            int branchIndex = SupplyOrDemandObjs.FindIndex(_ => _ is IB_PlantLoopBranches);

            return SupplyOrDemandObjs.Skip(branchIndex);

        }

    }
}
