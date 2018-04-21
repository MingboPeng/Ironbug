using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenStudio;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_LoopBranches : IB_HVACObject
    {
        public List<List<IB_HVACObject>> Branches { get; set; } = new List<List<IB_HVACObject>>();
        public IB_LoopBranches() : base(new Node(new Model()))
        {

        }

        public void Add(List<IB_HVACObject> HVACObjects)
        {
            if (!HVACObjects.Any())
            {
                return;
            }
            this.Branches.Add(HVACObjects);
        }

        public override string ToString()
        {
            return "LoopBranches";
        }

        public override IB_ModelObject Duplicate()
        {
            //var newBranches = new List<List<IB_HVACObject>>();
            var loopBranches = new IB_PlantLoopBranches();
            foreach (var branch in this.Branches)
            {
                var newBranch = new List<IB_HVACObject>();
                foreach (var item in branch)
                {
                    newBranch.Add((IB_HVACObject)item.Duplicate());
                }
                loopBranches.Add(newBranch);
            }

            return loopBranches;
            
           
        }

        public int Count()
        {
            var count = 0;
            this.Branches.ForEach(_ => count += _.Count);
            return count;
        }

        public override bool AddToNode(Node node)
        {
            throw new NotImplementedException();
        }

        public override ModelObject ToOS(Model model)
        {
            throw new NotImplementedException();
        }
        public virtual void ToOS_Supply(Loop Loop)
        {
            throw new NotImplementedException();
        }

        public virtual void ToOS_Demand(Loop Loop)
        {
            throw new NotImplementedException();
        }


    }

    public class IB_PlantLoopBranches : IB_LoopBranches,IIB_PlantLoopObjects
    {

        public override void ToOS_Supply(Loop PlantLoop)
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
                    item.AddToNode(node);
                }
            }
        }
        public override void ToOS_Demand(Loop PlantLoop)
        {
            var branches = this.Branches;
            var plant = PlantLoop as PlantLoop;
            var model = PlantLoop.model();
            foreach (var branch in branches)
            {
                //add one branch
                plant.addDemandBranchForComponent((HVACComponent)branch.First().ToOS(model));
                //add the rest child in this branch
                var restChild = branch.Skip(1);
                foreach (var item in restChild)
                {
                    //TDDO: double check the obj order here
                    var node = plant.demandMixer().inletModelObjects().Last().to_Node().get();
                    item.AddToNode(node);
                }
            }
        }
    }

    public class IB_AirLoopBranches : IB_LoopBranches, IIB_AirLoopObject
    {

        public override void ToOS_Demand(Loop AirLoop)
        {
            var branches = this.Branches;
            var loop = AirLoop as AirLoopHVAC;
            var model = AirLoop.model();
            foreach (var branch in branches)
            {
                foreach (var item in branch)
                {
                    var thermalZone = (IB_ThermalZone)item;
                    var zone = (ThermalZone)item.ToOS(model);
                    var airTerminal = (HVACComponent)thermalZone.AirTerminal.ToOS(model);
                    loop.addBranchForZone(zone,airTerminal);
                }
            }
        }

        
    }


}
