using System;

using Grasshopper.Kernel;
using Ironbug.HVAC;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_AirTerminalSingleDuctConstantVolumeFourPipeBeam : Ironbug_HVACComponent
    {
        public Ironbug_AirTerminalSingleDuctConstantVolumeFourPipeBeam()
          : base("Ironbug_AirTerminalSingleDuctConstantVolumeFourPipeBeam", "4PipeBeam",
              "Description",
              "Ironbug", "03:AirTerminals",
              typeof(IB_AirTerminalSingleDuctConstantVolumeFourPipeBeam_FieldSet))
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("CoolingCoil", "coilC_", "CoilCoolingFourPipeBeam only.", GH_ParamAccess.item);
            pManager.AddGenericParameter("HeatingCoil", "coilH_", "CoilHeatingFourPipeBeam only.", GH_ParamAccess.item);

        }

        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("AirTerminalSingleDuctConstantVolumeFourPipeBeam", "AT", "connect to Zone", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new IB_AirTerminalSingleDuctConstantVolumeFourPipeBeam();

            var coilC = (IB_CoilCoolingFourPipeBeam)null;
            var coilH = (IB_CoilHeatingFourPipeBeam)null;

            if (DA.GetData(0, ref coilC)) obj.SetCoolingCoil(coilC);
            if (DA.GetData(1, ref coilH)) obj.SetHeatingCoil(coilH);

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);

        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.FourPipeBeam;

        public override Guid ComponentGuid => new Guid("{6214EE92-ECCC-4247-A3F5-486FE90ABECD}");
    }
}