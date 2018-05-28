﻿using System;
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
                    loop.addBranchForZone(zone,airTerminal);
                }
            }
        }

        
    }


}