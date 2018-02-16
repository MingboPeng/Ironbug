using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ControllerOutdoorAir : IB_ModelObject
    {
        //Real obj to be saved in OS model
        //private ControllerOutdoorAir osControllerOutdoorAir { get; set; }
        //private Model osModel { get; set; }

        public IB_ControllerOutdoorAir()
        {
            var model = new Model();
            this.ghostModelObject = new ControllerOutdoorAir(model);
            //check name
            this.SetAttribute(IB_ControllerOutdoorAir_DataFieldSet.Name, this.ghostModelObject.CheckName());
        }

        private static ControllerOutdoorAir InitMethod(Model model) => new ControllerOutdoorAir(model);
        public override ParentObject ToOS(Model model)
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

        

        



        public static IEnumerable<IB_DataField> GetList()
        {
            return GetList<IB_ControllerOutdoorAir_DataFieldSet>();
        }

        public static IB_DataField GetAttributeByName(string name)
        {
            return GetAttributeByName<IB_ControllerOutdoorAir_DataFieldSet>(name);
        }



    }
}
