using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Ironbug.HVAC.BaseClass;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_SetpointManagerOutdoorAirReset : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_SetpointManagerOutdoorAirReset class.
        /// </summary>
        public Ironbug_SetpointManagerOutdoorAirReset()
          : base("Ironbug_SetpointManagerOutdoorAirReset", "SpOAReset",
              "Description",
              "Ironbug", "05:SetpointManager")
        {
        }
        public override GH_Exposure Exposure => GH_Exposure.primary;
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("SetpointatOutdoorHighTemperature", "_SpOHTemp", "SetpointatOutdoorHighTemperature", GH_ParamAccess.item);
            pManager.AddNumberParameter("OutdoorHighTemperature", "_OHTemp", "OutdoorHighTemperature", GH_ParamAccess.item);
            pManager.AddNumberParameter("SetpointatOutdoorLowTemperature", "_SpOLTemp", "SetpointatOutdoorLowTemperature", GH_ParamAccess.item);
            pManager.AddNumberParameter("OutdoorLowTemperature", "_OLTemp", "OutdoorLowTemperature", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SetpointManagerOutdoorAirReset", "OAReset", "TODO:...", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_SetpointManagerOutdoorAirReset();

            double lowT = 0;
            double highT = 0;
            double lowOT = 0;
            double highOT = 0;
            
            DA.GetData(0, ref highT);
            DA.GetData(1, ref highOT);
            DA.GetData(2, ref lowT);
            DA.GetData(3, ref lowOT);
            


            var fieldSet = HVAC.IB_SetpointManagerOutdoorAirReset_DataFieldSet.Value;

            obj.SetAttribute(fieldSet.SetpointatOutdoorHighTemperature, highT);
            obj.SetAttribute(fieldSet.OutdoorHighTemperature, highOT);
            obj.SetAttribute(fieldSet.SetpointatOutdoorLowTemperature, lowT);
            obj.SetAttribute(fieldSet.OutdoorLowTemperature, lowOT);


            DA.SetData(0, obj);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Properties.Resources.SetpointOAReset;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("f251c255-bb89-4a16-8339-d7adbbdc474a"); }
        }
    }
}