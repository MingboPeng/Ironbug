using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoolingTowerVariableSpeed : Ironbug_HVACComponentBase
    {
        
        /// <summary>
        /// Initializes a new instance of the Ironbug_BoilerHotWater class.
        /// </summary>
        public Ironbug_CoolingTowerVariableSpeed()
          : base("Ironbug_CoolingTowerVariableSpeed", "ClnTowerV",
              "Description",
              "Ironbug", "02:LoopComponents", 
              typeof(HVAC.IB_CoolingTowerVariableSpeed_DataFields))
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
            pManager.AddGenericParameter("CoolingTowerVariableSpeed", "ClnTowerV", "CoolingTowerVariableSpeed", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_CoolingTowerVariableSpeed();

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
                return Properties.Resources.CoolingTowerV;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("D56FCFB6-02A8-4E39-804A-5B0DE9ED6E56"); }
        }
    }
}