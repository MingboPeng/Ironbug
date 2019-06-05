using System;

using Grasshopper.Kernel;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_AirTerminalSingleDuctSeriesPIUReheat : Ironbug_HVACComponent
    {
        
        /// Initializes a new instance of the Ironbug_AirTerminalSingleDuctSeriesPIUReheat class.
        
        public Ironbug_AirTerminalSingleDuctSeriesPIUReheat()
          : base("Ironbug_AirTerminalSingleDuctSeriesPIUReheat", "SeriesPIUReheat",
              "Description",
              "Ironbug", "03:AirTerminals",
              typeof(IB_AirTerminalSingleDuctSeriesPIUReheat_FieldSet))
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("HeatingCoil", "coil_", "Heating coil to provide reheat source. can be CoilHeatingWater, CoilHeatingElectirc, or CoilHeatingGas.", GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager.AddGenericParameter("Fan", "fan_", "Can be FanConstantVolume or FanVariableVolume.", GH_ParamAccess.item);
            pManager[1].Optional = true;
        }

        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("AirTerminalSingleDuctSeriesPIUReheat", "SFP", "TODO:...", GH_ParamAccess.item);
        }

        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_AirTerminalSingleDuctSeriesPIUReheat();
            

            var fan = (IB_Fan)null;
            var coil = (IB_CoilHeatingBasic)null;

            if (DA.GetData(0, ref coil))
            {
                obj.SetReheatCoil(coil);
            }

            if (DA.GetData(1, ref fan))
            {
                obj.SetFan((IB_Fan)fan);
            }


            var objs = this.SetObjParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.SFPBox;

        public override Guid ComponentGuid => new Guid("95ec31ae-9cd0-4c5d-abc8-d13e1b9bec83");
    }
}