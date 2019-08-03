using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.HVAC
{
    public class IB_ExistPlantLoop : IB_PlantLoop, IIB_ExistingLoop
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ExistPlantLoop(ExistingObj);

        private IB_PlantLoopBranches _demandBranchGroup { get; set; } = new IB_PlantLoopBranches();


        public IB_ExistingObj ExistingObj { get; private set; }

        public IB_ExistPlantLoop(IB_ExistingObj ExistingAirloop) : base()
        {
            this.ExistingObj = ExistingAirloop;
        }

        public void AddBranches(IB_PlantLoopBranches BranchesTobeAppended)
        {
            this._demandBranchGroup = BranchesTobeAppended;
        }


        public override IB_ModelObject Duplicate()
        {
            var newObj = this.Duplicate(() => new IB_ExistPlantLoop(ExistingObj));

            newObj.AddBranches((IB_PlantLoopBranches)this._demandBranchGroup.Duplicate());

            return newObj;
        }
 

        public override ModelObject ToOS(Model model)
        {

            var name = ExistingObj.Name;
            var optLp = model.getPlantLoopByName(name);
            if (optLp.isNull()) throw new ArgumentException($"Cannot find [{name}]!");

            var plant = optLp.get();

            var branches = this._demandBranchGroup.Branches;
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
            
            return plant;
        }

        public override string ToString()
        {
            return string.Format("{0} demand branches to be added in this loop", this._demandBranchGroup.Branches.Count);
        }

        public override List<string> ToStrings()
        {
            return new List<string>() { this.ToString() };
        }
    }
}