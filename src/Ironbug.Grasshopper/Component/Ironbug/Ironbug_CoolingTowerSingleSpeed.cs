using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoolingTowerSingleSpeed : Ironbug_HVACComponent
    {
        
        /// <summary>
        /// Initializes a new instance of the Ironbug_BoilerHotWater class.
        /// </summary>
        public Ironbug_CoolingTowerSingleSpeed()
          : base("Ironbug_CoolingTowerSingleSpeed", "CoolingTower1",
              "Description",
              "Ironbug", "02:LoopComponents", 
              typeof(HVAC.IB_CoolingTowerSingleSpeed_DataFields))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.senary;

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
            pManager.AddGenericParameter("CoolingTowerSingleSpeed", "ClnTower1", "CoolingTowerSingleSpeed", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_CoolingTowerSingleSpeed();

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
                return Properties.Resources.CoolingTower1;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("40943146-68A0-4AFA-9CE9-8FC74931A8F0"); }
        }
    }
}