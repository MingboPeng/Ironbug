using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenStudio;

namespace Ironbug.HVAC
{
    public abstract class IB_LoopBranches : IB_HVACObject, IIB_DualLoopObject
    {
        public List<List<IB_HVACObject>> Branches { get; set; } = new List<List<IB_HVACObject>>();
        public IB_LoopBranches() : base(new Node(new Model()))
        {

        }

        public void Add(List<IB_HVACObject> HVACObjects)
        {
            this.Branches.Add(HVACObjects);
        }

        public override string ToString()
        {
            return "LoopBranches";
        }

    }

    public class IB_PlantLoopBranches : IB_LoopBranches
    {
        public override bool AddToNode(Node node)
        {
            //TODO: add Branches to node.
            var model = node.model();
            return ((PumpVariableSpeed)this.ToOS(model)).addToNode(node);
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

            //TODO: duplicate List<List< IB_HVACObject >> Branches
            
            //throw new NotImplementedException();
        }

        public override ModelObject ToOS(Model model)
        {
            //TODO: add branches objects to the target model.
            throw new NotImplementedException();
        }
    }


}
