using Grasshopper.Kernel;
using System;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_SetpointManagerOutdoorAirPretreat : Ironbug_HVACComponent
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_SetpointManagerOutdoorAirReset class.
        /// </summary>
        public Ironbug_SetpointManagerOutdoorAirPretreat()
          : base("Ironbug_SetpointManagerOutdoorAirPretreat", "SPM_OAPretreat",
              "Description",
              "Ironbug", "05:SetpointManager",
              typeof(HVAC.IB_SetpointManagerOutdoorAirPretreat_FieldSet))
        {
        }
        public override GH_Exposure Exposure => GH_Exposure.primary;
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SetpointManagerOutdoorAirPretreat", "SPM", "TODO:...", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_SetpointManagerOutdoorAirPretreat();
            
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Properties.Resources.SetPointOARetreat;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("D2A9417D-0A5D-4F08-8EBC-5F0F05B36495");
    }
}