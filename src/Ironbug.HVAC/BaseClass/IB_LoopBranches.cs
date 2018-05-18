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
        

        protected override ModelObject InitOpsObj(Model model) => null;


    }

    //public class IB_LoopBranch: IB_HVACObject
    //{
    //    public IList<IB_HVACObject> BranchItems { get; set; } = new List<IB_HVACObject>();

    //    protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_LoopBranch();

    //    public override bool AddToNode(Node node)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    protected override ModelObject InitOpsObj(Model model)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}


}
