using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_FromJson : Ironbug_Component
    {
        public Ironbug_FromJson()
          : base("IB_FromJson", "FromJson",
              "Load a HVAC system from Json strings",
              "Ironbug", "HVAC")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Json", "Json", "Json string of a HVAC system", GH_ParamAccess.item);
        }


        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Hvac", "Hvac", "IB_HVACSystem", GH_ParamAccess.item);
        }

        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string json = null;
            DA.GetData(0, ref json);

            HVAC.IB_HVACSystem sys = HVAC.IB_HVACSystem.FromJson(json);
            DA.SetData(0, sys);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.FromJson;

        public override Guid ComponentGuid => new Guid("90AB41C5-F652-4B17-8B02-BA8CF8739821");

    }

   
}