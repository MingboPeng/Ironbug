using System;

using Grasshopper.Kernel;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_AirTerminalSingleDuctVAVReheat : Ironbug_HVACComponent
    {
        public Ironbug_AirTerminalSingleDuctVAVReheat()
          : base("Ironbug_AirTerminalSingleDuctVAVReheat", "VAVReheat",
              "Description",
              "Ironbug", "03:AirTerminals",
              typeof(IB_AirTerminalSingleDuctVAVReheat_FieldSet))
        {
        }

        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("HeatingCoil", "coil_", "Heating coil to provide reheat source. can be CoilHeatingWater, CoilHeatingElectirc, or CoilHeatingGas.", GH_ParamAccess.item);
            pManager[0].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("AirTerminalSingleDuctVAVReheat", "AT", "connect to Zone", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new IB_AirTerminalSingleDuctVAVReheat();
            

            var coil = (IB_CoilHeatingBasic)null;
            
            if (DA.GetData(0, ref coil))
            {
                obj.SetReheatCoil(coil);
            }


            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);

        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.VAVBox;

        public override Guid ComponentGuid => new Guid("aaf86609-f508-4fb2-9ed4-a8323e9549bd");
    }
}