using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ToJson : Ironbug_Component
    {
        public Ironbug_ToJson()
          : base("Ironbug_ToJson", "ToJson",
              "Use this component to measure variables like temperature, flow rate, etc, in the loop.\nPlace this between loopObjects.",
              "Ironbug", "HVAC")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.septenary | GH_Exposure.hidden;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("HVACSystem", "HVAC", "A HVAC system from Ironbug_HVACSystem", GH_ParamAccess.item);
        }


        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Probe", "Probe", "TODO....", GH_ParamAccess.item);
        }

        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
          
            HVAC.IB_HVACSystem sys = null;
            DA.GetData(0, ref sys);

            var json = sys.ToJson();
            var sys2 = HVAC.IB_HVACSystem.FromJson(json);
            if (sys != sys2)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Failed to convert to json");
                return;
            }

            DA.SetData(0, json);

        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("FDE5126C-9BF1-4790-A30A-6D93FDA89DBE");

    }

   
}