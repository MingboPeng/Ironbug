using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ControllerOutdoorAir : IB_ModelObject
    {
        private static ControllerOutdoorAir InitMethod(Model model) => new ControllerOutdoorAir(model);
        public IB_ControllerOutdoorAir() : base(InitMethod(new Model()))
        {
        }
        
        public override IB_ModelObject Duplicate()
        {
            return this.DuplicateIB_ModelObject(() => new IB_ControllerOutdoorAir());
        }

        public override ModelObject ToOS(Model model)
        {
            return (ControllerOutdoorAir)this.ToOS(InitMethod, model);
        }
    }

    public class IB_ControllerOutdoorAir_DataFieldSet : IB_DataFieldSet
    {
        //private static readonly CoilHeatingWater refObj = new CoilHeatingWater(new Model());
        //public new IddObject IddObject = new CoilHeatingWater(new Model()).iddObject();
        protected override IddObject RefIddObject => new ControllerOutdoorAir(new Model()).iddObject();
        private Type DataFieldSetType = typeof(IB_ControllerOutdoorAir_DataFieldSet);


        //https://openstudio-sdk-documentation.s3.amazonaws.com/cpp/OpenStudio-2.4.0-doc/model/html/classopenstudio_1_1model_1_1_controller_outdoor_air.html

        //Following list items are fields that I want to have picked for GH user to edit
        public static readonly IB_DataField Name = new IB_DataField("Name", "Name", strType, true);

        public static readonly IB_DataField MinimumOutdoorAirFlowRate
            = new IB_DataField("MinimumOutdoorAirFlowRate", "MinOAFlowRate", dbType, BasicSetting: true);

        public static readonly IB_DataField EconomizerControlType
            = new IB_DataField("EconomizerControlType", "Economizer", strType, BasicSetting: true);


        public static readonly IB_DataField HighHumidityControl
            = new IB_DataField("HighHumidityControl", "HiHumidityCtrl", boType);

        

        
        

    }
}
