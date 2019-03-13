using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_HeaderedPumpsConstantSpeed : Ironbug_HVACComponent
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_PumpConstantSpeed class.
        /// </summary>
        public Ironbug_HeaderedPumpsConstantSpeed()
          : base("Ironbug_HeaderedPumpsConstantSpeed", "PumpConst_Headered",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_PumpConstantSpeed_DataFields))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quinary;

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PumpConst_Headered", "Pump", "connect to plantloop's supply side", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_HeaderedPumpsConstantSpeed();
            
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Properties.Resources.PumpC_Headered;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("E9857ABA-0E0E-4921-A525-8EC777E4D553");
    }
}