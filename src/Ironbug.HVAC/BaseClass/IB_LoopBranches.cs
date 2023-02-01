using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_LoopBranches : IB_HVACObject
    {
        public List<List<IB_HVACObject>> Branches 
        { 
            get => TryGetList<List<IB_HVACObject>>(); 
            private set => Set(value); 
        } 

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

        public override List<string> ToStrings()
        {
            return new List<string>() { this.ToString() };
        }

        //public override IB_ModelObject Duplicate()
        //{
        //    //var newBranches = new List<List<IB_HVACObject>>();
        //    var loopBranches = new IB_PlantLoopBranches();
        //    foreach (var branch in this.Branches)
        //    {
        //        var newBranch = new List<IB_HVACObject>();
        //        foreach (var item in branch)
        //        {
        //            newBranch.Add(item.Duplicate() as IB_HVACObject);
        //        }
        //        loopBranches.Add(newBranch);
        //    }

        //    return loopBranches;
        //}

        public int Count()
        {
            //var count = 0;
            var probeNotCounted = this.Branches.SelectMany(_ => _).Where(_=>!(_ is IB_Probe)).Where(_ => !(_ is IB_SetpointManager));
            //this.Branches.ForEach(_ => count += _.Count);
            return probeNotCounted.Count();
        }

        public override HVACComponent ToOS(Model model)
        {
            throw new NotImplementedException();
        }

    }
}