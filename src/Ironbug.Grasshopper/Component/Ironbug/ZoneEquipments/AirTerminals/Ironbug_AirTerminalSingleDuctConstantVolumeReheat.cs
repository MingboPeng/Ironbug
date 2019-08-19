using System;

using Grasshopper.Kernel;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_AirTerminalSingleDuctConstantVolumeReheat : Ironbug_HVACWithParamComponent
    {
        public Ironbug_AirTerminalSingleDuctConstantVolumeReheat()
          : base("Ironbug_AirTerminalSingleDuctConstantVolumeReheat", "DiffuserReheat",
              "Description",
              "Ironbug", "03:AirTerminals",
              typeof(IB_AirTerminalSingleDuctConstantVolumeReheat_FieldSet))
        {
        }

        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("HeatingCoil", "coil_", "Heating coil to provide reheat source. can be CoilHeatingWater, CoilHeatingElectirc, or CoilHeatingGas.", GH_ParamAccess.item);
            pManager[0].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("AirTerminalSingleDuctConstantVolumeReheat", "AT", "connect to Zone", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new IB_AirTerminalSingleDuctConstantVolumeReheat();
            

            var coil = (IB_CoilHeatingBasic)null;
            
            if (DA.GetData(0, ref coil))
            {
                obj.SetReheatCoil(coil);
            }


            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);

        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.AirTerminalUncontrolledReheat;

        public override Guid ComponentGuid => new Guid("{85D5AA98-9B44-4A34-A21A-733F44B0A97D}");
    }
}