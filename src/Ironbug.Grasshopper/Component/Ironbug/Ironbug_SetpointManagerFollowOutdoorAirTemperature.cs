using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_SetpointManagerFollowOutdoorAirTemperature : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_SetpointManagerOutdoorAirReset class.
        /// </summary>
        public Ironbug_SetpointManagerFollowOutdoorAirTemperature()
          : base("Ironbug_SetpointManagerFollowOutdoorAirTemperature", "SpFollowOATemp",
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
            pManager.AddTextParameter("ControlVariable", "_CtrlVar_", "Default:Temperature", GH_ParamAccess.item);
            pManager.AddTextParameter("ReferenceTemperatureType", "_RefType_", "Default:OutdoorAirWetBulb", GH_ParamAccess.item);
            pManager.AddNumberParameter("MaximumSetpointTemperature", "_maxT_", "Default:80C", GH_ParamAccess.item);
            pManager.AddNumberParameter("MinimumSetpointTemperature", "_minT_", "Default:5C", GH_ParamAccess.item);
            pManager.AddNumberParameter("OffsetTemperatureDifference", "_diff_", "Default:0", GH_ParamAccess.item);

            pManager[0].Optional = true;
            pManager[1].Optional = true;
            pManager[2].Optional = true;
            pManager[3].Optional = true;
            pManager[4].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SetpointManagerFollowOutdoorAirTemperature", "SpFollowOATemp", "TODO:...", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_SetpointManagerFollowOutdoorAirTemperature();

            string ctrlVar = "Temperature";
            string refType = "OutdoorAirWetBulb";
            double maxT = 80;
            double minT = 5;
            double diff = 0;
            
            DA.GetData(0, ref ctrlVar);
            DA.GetData(1, ref refType);
            DA.GetData(2, ref maxT);
            DA.GetData(3, ref minT);
            DA.GetData(4, ref diff);


            var fieldSet = HVAC.IB_SetpointManagerFollowOutdoorAirTemperature_DataFieldSet.Value;

            obj.SetFieldValue(fieldSet.ControlVariable, ctrlVar);
            obj.SetFieldValue(fieldSet.ReferenceTemperatureType, refType);
            obj.SetFieldValue(fieldSet.MaximumSetpointTemperature, maxT);
            obj.SetFieldValue(fieldSet.MinimumSetpointTemperature, minT);
            obj.SetFieldValue(fieldSet.OffsetTemperatureDifference, diff);

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
                return Properties.Resources.SetPointFlowOA;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("FF3EEF96-60FF-4D6E-B29D-204D16AF6DBD"); }
        }
    }
}