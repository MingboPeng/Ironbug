using System;

using Grasshopper.Kernel;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_AirTerminalSingleDuctVAVHeatAndCoolReheat : Ironbug_HVACWithParamComponent
    {
        public Ironbug_AirTerminalSingleDuctVAVHeatAndCoolReheat()
          : base("Ironbug_AirTerminalSingleDuctVAVHeatAndCoolReheat", "VAVHtnClnReheat",
              "Description",
              "Ironbug", "03:AirTerminals",
              typeof(IB_AirTerminalSingleDuctVAVHeatAndCoolReheat_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("HeatingCoil", "coil_", "Heating coil to provide reheat source. can be CoilHeatingWater, CoilHeatingElectirc, or CoilHeatingGas.", GH_ParamAccess.item);
            pManager[0].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("AirTerminalSingleDuctVAVHeatAndCoolReheat", "AT", "connect to Zone", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new IB_AirTerminalSingleDuctVAVHeatAndCoolReheat();
            

            var coil = (IB_CoilHeatingBasic)null;
            
            if (DA.GetData(0, ref coil))
            {
                obj.SetReheatCoil(coil);
            }


            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);

        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.VAVHtnClnBox;

        public override Guid ComponentGuid => new Guid("{2855C5D9-7D62-4C7E-9EC1-EA76DE57A2F2}");
    }
}