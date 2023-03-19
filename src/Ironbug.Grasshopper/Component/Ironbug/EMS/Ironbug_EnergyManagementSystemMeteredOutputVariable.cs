using System;
using Grasshopper.Kernel;
using Ironbug.HVAC;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_EnergyManagementSystemMeteredOutputVariable : Ironbug_HVACWithParamComponent
    {
        public Ironbug_EnergyManagementSystemMeteredOutputVariable()
          : base("IB_EMSMeteredOutputVariable", "EMSMeteredVariable",
              "Description",
              "Ironbug", "06:Sizing & Controller",
              typeof(IB_EnergyManagementSystemMeteredOutputVariable_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("_EMSProgram", "_EMSProgram", "EMS program", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("meteredVariable", "meteredVariable", "Metered output variable", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IB_EnergyManagementSystemProgram p = null;
            DA.GetData(0, ref p);
            var obj = new HVAC.IB_EnergyManagementSystemMeteredOutputVariable(p);
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);

        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.EMS_Meter;


        public override Guid ComponentGuid => new Guid("A2B4912D-A358-46B0-B9C2-E70007695E92");
    }
}