using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_LoopBranches : IB_HVACObject
    {
        public List<List<IB_HVACObject>> Branches { get; private set; } = new List<List<IB_HVACObject>>();

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

        public override IB_HVACObject Duplicate()
        {
            //var newBranches = new List<List<IB_HVACObject>>();
            var loopBranches = new IB_PlantLoopBranches();
            foreach (var branch in this.Branches)
            {
                var newBranch = new List<IB_HVACObject>();
                foreach (var item in branch)
                {
                    newBranch.Add(item.Duplicate());
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

        public override HVACComponent ToOS(Model model)
        {
            throw new NotImplementedException();
        }
        
    }
}