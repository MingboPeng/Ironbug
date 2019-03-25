using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilHeatingWater_Adv : Ironbug_HVACComponent
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_CoilHeatingWater class.
        /// </summary>
        public Ironbug_CoilHeatingWater_Adv()
          : base("Ironbug_CoilHeatingWater_Adv", "CoilHtnWater_Adv",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_CoilHeatingWater_FieldSet))
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
            pManager.AddGenericParameter("AirSide_CoilHeatingWater", "Coil", "connect to air loop's supply side or other water heated system.", GH_ParamAccess.item);
            pManager.AddGenericParameter("WaterSide_CoilHeatingWater", "ToWaterLoop", "connect to hot water loop's demand side via plantBranches", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            HVAC.IB_ControllerWaterCoil ctrl = null;
            DA.GetData(0, ref ctrl);
            var obj = new HVAC.IB_CoilHeatingWater(ctrl);
            

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
            DA.SetData(1, obj);
        }



        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Properties.Resources.CoilHW_adv;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("0DE0F2D8-6A20-4A47-8C4B-BC149BA7E116");

    }
    
}