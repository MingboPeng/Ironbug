﻿using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_CoolingTowerSingleSpeed : IB_HVACObject, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoolingTowerSingleSpeed();

        private static CoolingTowerSingleSpeed InitMethod(Model model) => new CoolingTowerSingleSpeed(model);
        public IB_CoolingTowerSingleSpeed() : base(InitMethod(new Model()))
        {
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((CoolingTowerSingleSpeed)this.ToOS(model)).addToNode(node);
        }

        //public override IB_ModelObject Duplicate()
        //{
        //    return base.DuplicateIBObj(() => new IB_CoolingTowerSingleSpeed());
        //}

        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethod, model).to_CoolingTowerSingleSpeed().get();
        }
    }

    public sealed class IB_CoolingTowerSingleSpeed_DataFields
        : IB_DataFieldSet<IB_CoolingTowerSingleSpeed_DataFields, CoolingTowerSingleSpeed>
    {

        private IB_CoolingTowerSingleSpeed_DataFields() { }

        public IB_DataField NominalCapacity { get; }
            = new IB_BasicDataField("NominalCapacity", "Capacity");


    }
}
