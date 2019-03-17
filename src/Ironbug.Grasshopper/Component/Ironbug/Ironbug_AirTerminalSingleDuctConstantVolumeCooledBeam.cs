using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_AirTerminalSingleDuctConstantVolumeCooledBeam : Ironbug_HVACComponent
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_AirTerminalSingleDuctVAVReheat class.
        /// </summary>
        public Ironbug_AirTerminalSingleDuctConstantVolumeCooledBeam()
          : base("Ironbug_AirTerminalChilledBeam", "ChilledBeam",
              "Description",
              "Ironbug", "03:AirTerminals",
              typeof(IB_AirTerminalSingleDuctConstantVolumeCooledBeam_DataFieldSet))
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("CoolingCoil", "coil_", "CoilCoolingCooledBeam only.", GH_ParamAccess.item);
            pManager[0].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("AirTerminalSingleDuctConstantVolumeCooledBeam", "AT", "connect to Zone", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new IB_AirTerminalSingleDuctConstantVolumeCooledBeam();

            var coil = (IB_CoilCoolingCooledBeam)null;
            
            if (DA.GetData(0, ref coil))
            {
                obj.SetCoolingCoil(coil);
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
                return Properties.Resources.ChilledBeam;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("82D7D027-0A37-4688-8158-0CDBC630316B"); }
        }
    }
}