﻿using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_CoilCoolingCooledBeam : IB_CoilCoolingBasic, IIB_DualLoopObj, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilCoolingCooledBeam();

        private static CoilCoolingCooledBeam NewDefaultOpsObj(Model model) => new CoilCoolingCooledBeam(model);

        public IB_CoilCoolingCooledBeam() : base(NewDefaultOpsObj(new Model()))
        {
        }
        

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }

        //protected override ModelObject NewOpsObj(Model model)
        //{
        //    return base.OnNewOpsObj(NewDefaultOpsObj, model).to_CoilCoolingCooledBeam().get();
        //}
    }
    public sealed class IB_CoilCoolingCooledBeam_DataFieldSet
        : IB_FieldSet<IB_CoilCoolingCooledBeam_DataFieldSet, CoilCoolingCooledBeam>
    {
        private IB_CoilCoolingCooledBeam_DataFieldSet() { }
        
    }

}
