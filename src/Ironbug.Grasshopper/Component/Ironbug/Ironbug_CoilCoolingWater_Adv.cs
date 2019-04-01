using Grasshopper.Kernel;
using System;


namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilCoolingWater_Adv : Ironbug_HVACComponent
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_CoilCoolingWater class.
        /// </summary>
        public Ironbug_CoilCoolingWater_Adv()
          : base("Ironbug_CoilCoolingWater_Adv", "CoilClnWater_Adv", "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_CoilCoolingWater_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("ControllerWaterCoil", "_ctrl", "add a customized controller here", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("AirSide_CoilCoolingWater", "Coil", "Connect to air loop's supply side or other water cooled system.", GH_ParamAccess.item);
            pManager.AddGenericParameter("WaterSide_CoilCoolingWater", "ToWaterLoop", "Connect to chilled water loop's demand side via plantBranches", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            HVAC.IB_ControllerWaterCoil ctrl = null;
            DA.GetData(0, ref ctrl);
            var obj = new HVAC.IB_CoilCoolingWater(ctrl);
            
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
            DA.SetData(1, obj);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Properties.Resources.CoilCW_adv;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("371E6816-D142-4599-B691-5F649BD74119");
    }
}