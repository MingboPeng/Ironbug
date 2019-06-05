using System;

using Grasshopper.Kernel;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_AirTerminalSingleDuctParallelPIUReheat : Ironbug_HVACComponent
    {
        public Ironbug_AirTerminalSingleDuctParallelPIUReheat()
          : base("Ironbug_AirTerminalSingleDuctParallelPIUReheat", "ParallelPIUReheat",
              "Description",
              "Ironbug", "03:AirTerminals",
              typeof(IB_AirTerminalSingleDuctParallelPIUReheat_FieldSet))
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
            pManager.AddGenericParameter("AirTerminalSingleDuctParallelPIUReheat", "PFP", "TODO:...", GH_ParamAccess.item);
        }

        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_AirTerminalSingleDuctParallelPIUReheat();
            

            var fan = (IB_FanConstantVolume)null;
            var coil = (IB_CoilHeatingBasic)null;

            if (DA.GetData(0, ref coil))
            {
                obj.SetReheatCoil(coil);
            }

            if (DA.GetData(1, ref fan))
            {
                obj.SetFan(fan);
            }

            

            var objs = this.SetObjParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.PFPBox;

        public override Guid ComponentGuid => new Guid("3204e32a-94f2-4696-9d80-d8702d2948cf");
    }
}