using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_ZoneHVACTerminalUnitVariableRefrigerantFlow : Ironbug_HVACComponentBase
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_ZoneHVACTerminalUnitVariableRefrigerantFlow class.
        /// </summary>
        public Ironbug_ZoneHVACTerminalUnitVariableRefrigerantFlow()
          : base("Ironbug_ZoneHVACTerminalUnitVariableRefrigerantFlow", "VRFTerminal",
              "Description",
              "Ironbug", "04:ZoneEquipments",
              typeof(HVAC.IB_ZoneHVACTerminalUnitVariableRefrigerantFlow_DataFieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

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
            pManager.AddGenericParameter("ZoneHVACTerminalUnitVariableRefrigerantFlow", "VRFUnit_In", "Connect to Zone's equipment", GH_ParamAccess.item);
            pManager.AddGenericParameter("ZoneHVACTerminalUnitVariableRefrigerantFlow", "VRFUnit_Out", "Connect to VRF system", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_ZoneHVACTerminalUnitVariableRefrigerantFlow();
            

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
            DA.SetData(1, obj);
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
                return Properties.Resources.VRFUnit;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("1aa85a4b-f306-41ba-9723-5d78ecbec750"); }
        }
    }
}