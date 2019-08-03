using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_EvaporativeCoolerDirectResearchSpecial : Ironbug_DuplicableHVACWithParamComponent
    {


        /// Initializes a new instance of the Ironbug_BoilerHotWater class.

        public Ironbug_EvaporativeCoolerDirectResearchSpecial()
          : base("Ironbug_EvaporativeCoolerDirectResearchSpecial", "EvapCoolerDir",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_EvaporativeCoolerDirectResearchSpecial_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("EvaporativeCoolerDirectResearchSpecial", "EvapCoolerDir", "EvaporativeCoolerDirectResearchSpecial", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_EvaporativeCoolerDirectResearchSpecial();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.EvapCoolerDir;

        public override Guid ComponentGuid => new Guid("{789D92F0-DDA0-46B8-9938-211F0645350F}");
    }
}