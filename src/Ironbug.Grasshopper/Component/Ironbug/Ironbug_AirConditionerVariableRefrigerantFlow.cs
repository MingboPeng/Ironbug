using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_AirConditionerVariableRefrigerantFlow : Ironbug_HVACComponent
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_ZoneHVACTerminalUnitVariableRefrigerantFlow class.
        /// </summary>
        public Ironbug_AirConditionerVariableRefrigerantFlow()
          : base("Ironbug_AirConditionerVariableRefrigerantFlow", "VRFSystem",
              "Description",
              "Ironbug", "01:Loops",
              typeof(HVAC.IB_AirConditionerVariableRefrigerantFlow_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("terminals", "_terminals", "VRF terminals.", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("AirConditionerVariableRefrigerantFlow", "VRFSystem", "VRFSystem", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            var obj = new HVAC.IB_AirConditionerVariableRefrigerantFlow();
            var terminals = new List<HVAC.IB_ZoneHVACTerminalUnitVariableRefrigerantFlow>();
            DA.GetDataList(0, terminals);
            foreach (var term in terminals)
            {
                obj.AddTerminal(term);
            }

            this.SetObjParamsTo(obj);
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
                return Properties.Resources.VRF;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("20B79F9A-1E3F-4A26-9D5B-5D6C988C15E4"); }
        }
    }
}