using System;

using Grasshopper.Kernel;
using Ironbug.HVAC;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_AirTerminalSingleDuctConstantVolumeFourPipeInduction : Ironbug_HVACComponent
    {
        public Ironbug_AirTerminalSingleDuctConstantVolumeFourPipeInduction()
          : base("Ironbug_AirTerminalSingleDuctConstantVolumeFourPipeInduction", "4PipeInduction",
              "Description",
              "Ironbug", "03:AirTerminals",
              typeof(IB_AirTerminalSingleDuctConstantVolumeFourPipeInduction_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("CoolingCoil", "coilC_", "Optional input, but use CoilCoolingWater only. By default, this 4PipeInduction will become a heating only 2PipeInduction, if its cooling coil is left empty.", GH_ParamAccess.item);
            pManager.AddGenericParameter("HeatingCoil", "coilH_", "CoilHeatingWater only.", GH_ParamAccess.item);

            pManager[0].Optional = true;
        }

        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("AirTerminalSingleDuctConstantVolumeFourPipeInduction", "AT", "connect to Zone", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new IB_AirTerminalSingleDuctConstantVolumeFourPipeInduction();

            var coilC = (IB_CoilCoolingWater)null;
            var coilH = (IB_CoilHeatingWater)null;

            if (DA.GetData(0, ref coilC)) obj.SetCoolingCoil(coilC);
            if (DA.GetData(1, ref coilH)) obj.SetHeatingCoil(coilH);

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);

        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.FourPipeInduction;

        public override Guid ComponentGuid => new Guid("{F75B8370-76C8-4532-9865-5E693860745C}");
    }
}