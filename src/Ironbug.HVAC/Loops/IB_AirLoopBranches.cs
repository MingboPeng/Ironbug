using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_AirLoopBranches : IB_LoopBranches, IIB_AirLoopObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => ()=>new IB_AirLoopBranches();

        public void ToOS_Demand(Loop AirLoop)
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
                    if (loop.addBranchForZone(zone, airTerminal))
                        throw new ArgumentException($"Failed to add {item.GetType()} to {this.GetType()}!");
                }
            }
        }

        public override string ToString()
        {
            int c = Count();
            if (c <= 1)
            {
                return $"{this.Count()} zone branch";
            }
            return $"{this.Count()} zone branches";
        }


    }


}
