﻿using System;
using Ironbug.HVAC.BaseClass;
using Newtonsoft.Json;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ZoneHVACBaseboardConvectiveWater : IB_ZoneEquipment
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ZoneHVACBaseboardConvectiveWater();

        private static ZoneHVACBaseboardConvectiveWater NewDefaultOpsObj(Model model) 
            => new ZoneHVACBaseboardConvectiveWater(model, model.alwaysOnDiscreteSchedule(), new CoilHeatingWaterBaseboard(model));

        private IB_CoilHeatingWaterBaseboard HeatingCoil => this.GetChild<IB_CoilHeatingWaterBaseboard>();

        [JsonConstructor]
        private IB_ZoneHVACBaseboardConvectiveWater(bool forDeserialization) : base(null)
        {
        }
        public IB_ZoneHVACBaseboardConvectiveWater() : base(NewDefaultOpsObj)
        {
            this.AddChild(new IB_CoilHeatingWaterBaseboard());
        }

        public void SetHeatingCoil(IB_CoilHeatingWaterBaseboard Coil)
        {
            this.SetChild(Coil);
        }

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(InitMethodWithChildren, model); 

            ZoneHVACBaseboardConvectiveWater InitMethodWithChildren(Model md) =>
                new ZoneHVACBaseboardConvectiveWater(md, md.alwaysOnDiscreteSchedule(), (StraightComponent)this.HeatingCoil.ToOS(md));
        }
    }

    public sealed class IB_ZoneHVACBaseboardConvectiveWater_FieldSet
        : IB_FieldSet<IB_ZoneHVACBaseboardConvectiveWater_FieldSet, ZoneHVACBaseboardConvectiveWater>
    {
        private IB_ZoneHVACBaseboardConvectiveWater_FieldSet() { }

    }
}
