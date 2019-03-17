using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Ironbug.HVAC.BaseClass;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ControllerMechanicalVentilation : Ironbug_HVACComponent
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_OAController class.
        /// </summary>
        public Ironbug_ControllerMechanicalVentilation()
          : base("Ironbug_ControllerMechanicalVentilation", "MechVentController",
              "Description",
              "Ironbug", "06:Sizing&Controller",
              typeof(HVAC.IB_ControllerMechanicalVentilation_DataFieldSet))
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
            
            pManager.AddGenericParameter("ControllerMechanicalVentilation", "Ctrl", "connect to OAController", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_ControllerMechanicalVentilation();

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);

        }
        
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.MechVentCtrl;
            }
        }
        
        public override Guid ComponentGuid
        {
            get { return new Guid("51B8F4A2-A0D8-49F6-8FD9-4F6D97711965"); }
        }
    }
}