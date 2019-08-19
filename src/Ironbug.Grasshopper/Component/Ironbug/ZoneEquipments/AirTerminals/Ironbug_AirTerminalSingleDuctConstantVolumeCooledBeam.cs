using System;

using Grasshopper.Kernel;
using Ironbug.HVAC;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_AirTerminalSingleDuctConstantVolumeCooledBeam : Ironbug_HVACWithParamComponent
    {
        public Ironbug_AirTerminalSingleDuctConstantVolumeCooledBeam()
          : base("Ironbug_AirTerminalChilledBeam", "ChilledBeam",
              "Description",
              "Ironbug", "03:AirTerminals",
              typeof(IB_AirTerminalSingleDuctConstantVolumeCooledBeam_FieldSet))
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("CoolingCoil", "coil_", "CoilCoolingCooledBeam only.", GH_ParamAccess.item);
            pManager[0].Optional = true;
        }

        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("AirTerminalSingleDuctConstantVolumeCooledBeam", "AT", "connect to Zone", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new IB_AirTerminalSingleDuctConstantVolumeCooledBeam();

            var coil = (IB_CoilCoolingCooledBeam)null;
            
            if (DA.GetData(0, ref coil))
            {
                obj.SetCoolingCoil(coil);
            }


            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);

        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.ChilledBeam;

        public override Guid ComponentGuid => new Guid("82D7D027-0A37-4688-8158-0CDBC630316B");
    }
}