using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Ironbug.HVAC.BaseClass;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_OAController : Ironbug_HVACComponentBase
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_OAController class.
        /// </summary>
        public Ironbug_OAController()
          : base("Ironbug_OAController", "OAController",
              "Description",
              "Ironbug", "06:Sizing&Controller",
              typeof(HVAC.IB_ControllerOutdoorAir_DataFieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("name", "name_", "name", GH_ParamAccess.item);
            pManager.AddGenericParameter("ControllerMechanicalVentilation", "MechVentCtrl_", "ControllerMechanicalVentilation", GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            
            pManager.AddGenericParameter("OutdoorAirSystemController", "OACtrl", "connect to OutdoorAirSystem", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_ControllerOutdoorAir();
            var name = string.Empty;
            var mechVentCtrl = (HVAC.IB_ControllerMechanicalVentilation)null;

            //collect settings
            DA.GetData(0, ref name);
            

            if (!string.IsNullOrWhiteSpace(name))
            {
                obj.SetFieldValue(HVAC.IB_ControllerOutdoorAir_DataFieldSet.Value.Name, name);
            }

            if (DA.GetData(1, ref mechVentCtrl))
            {
                obj.SetMechanicalVentilation(mechVentCtrl);
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
                return Properties.Resources.OACtrl;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("8c5f0941-421e-428b-9b26-0d17280448fe"); }
        }
    }
}