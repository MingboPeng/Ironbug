using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_HVACScenario : Ironbug_Component
    {
        public Ironbug_HVACScenario()
          : base("IB_HVACScenario", "IB_HVACScenario",
              "Group multiple HVAC systems as a HVAC design option (aka. HVACScenario)",
              "Ironbug", "HVAC")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "A human readable name for this HVAC scenario. By default, the ID will be used for name.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Systems", "Systems", "HVAC systems from IB_HVACSystem", GH_ParamAccess.list);

            pManager[0].Optional = true;
            pManager[1].DataMapping = GH_DataMapping.Flatten;
        }
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("HVACScenario", "HVACScenario", "A HVAC Scenario that contains a group of HVAC systems", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var id = this.InstanceGuid.ToString().Substring(0, 6);
            this.Message = $"ID: {id}";

            var systems = new List<HVAC.IB_HVACSystem>();
            var name = string.Empty;

            DA.GetData(0, ref name);
            DA.GetDataList(1, systems);

            var hvac = new HVAC.IB_HVACScenario(id, name, systems);
            DA.SetData(0, hvac);
        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("DE766640-4BA1-44BE-AE66-138B2D97E4CD");


    }
}