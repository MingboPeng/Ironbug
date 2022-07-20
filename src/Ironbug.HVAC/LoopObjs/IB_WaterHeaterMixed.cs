﻿using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_WaterHeaterMixed : IB_HVACObject, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_WaterHeaterMixed();

        private static WaterHeaterMixed NewDefaultOpsObj(Model model) => new WaterHeaterMixed(model);
        public IB_WaterHeaterMixed() : base(NewDefaultOpsObj(new Model()))
        {
        }
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_WaterHeaterMixed_FieldSet
        : IB_FieldSet<IB_WaterHeaterMixed_FieldSet, WaterHeaterMixed>
    {
        private IB_WaterHeaterMixed_FieldSet() { }
    }
}
