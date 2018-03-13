using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_OAController : Ironbug_HVACComponent
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_OAController class.
        /// </summary>
        public Ironbug_OAController()
          : base("Ironbug_OAController", "OAController",
              "Description",
              "Ironbug", "01:LoopComponents",
              typeof(HVAC.IB_ControllerOutdoorAir_DataFieldSet))
        {
        }
        
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("name", "name_", "name", GH_ParamAccess.item);
            pManager.AddGenericParameter("Parameters", "params_", "Detail settings for this object. Use Ironbug_ObjParams to set this.", GH_ParamAccess.item);
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

            
            //collect settings
            var settingParams = new Dictionary<HVAC.IB_DataField, object>();
            DA.GetData(0, ref name);
            DA.GetData(1, ref settingParams);

            obj.SetAttributes(settingParams);
            

            if (!string.IsNullOrWhiteSpace(name))
            {
                obj.SetAttribute(HVAC.IB_ControllerOutdoorAir_DataFieldSet.Name, name);
            }

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
                return null;
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