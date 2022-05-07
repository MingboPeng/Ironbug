using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilHeatingDesuperheater : Ironbug_HVACWithParamComponent
    {
        
        public Ironbug_CoilHeatingDesuperheater()
          : base("IB_CoilHeatingDesuperheater", "Desuperheater",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_CoilHeatingDesuperheater_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary | GH_Exposure.obscure;
        
        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("HeatingSource", "HeatingSource", "Heating source, can be CoilCoolingDXSingleSpeed or CoilCoolingDXTwoSpeed", GH_ParamAccess.item);
        }

        
        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilHeatingDesuperheater", "Desuperheater", "A desuperheater with a heating source (CoilCoolingDXSingleSpeed or CoilCoolingDXTwoSpeed).", GH_ParamAccess.item);
        }

        
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            var obj = (HVAC.IB_CoilHeatingDesuperheater)null;
            var heatingSource = (HVAC.BaseClass.IB_CoilDX)null;
            if (DA.GetData(0, ref heatingSource))
            {
                obj = new HVAC.IB_CoilHeatingDesuperheater(heatingSource);

            }

            var v0 = typeof(OpenStudio.Model).Assembly.GetName().Version;
            var isOldVersion = v0.CompareTo(new Version("2.8.0")) < 0;
            if (isOldVersion)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, $"This desuperheater is not supported until OpenStudio 2.8.\nYou have OpenStudio {v0}.");
            }
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.DesuperHeater;


        public override Guid ComponentGuid => new Guid("44EA1323-99CE-4D03-A7EC-8A58A7652906");
    }
}