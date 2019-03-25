using System;
using Grasshopper.Kernel;
using Ironbug.HVAC.Curves;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_WaterHeaterMixed : Ironbug_HVACComponent
    {
        public Ironbug_WaterHeaterMixed()
          : base("Ironbug_WaterHeaterMixed", "WaterHeaterMixed",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_WaterHeaterMixed_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;
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
            pManager.AddGenericParameter("WaterHeaterMixed", "WaterHeater", "Connect to hot water loop's supply side.", GH_ParamAccess.item);
            pManager.AddGenericParameter("WaterHeaterMixed_DeamanSide", "ToDemandSide", "Connect to other plant loop's demand side.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_WaterHeaterMixed();
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
            DA.SetData(1, obj);
        }
        
        protected override System.Drawing.Bitmap Icon => Properties.Resources.WaterHeaterMix;
        
        public override Guid ComponentGuid => new Guid("A202ED17-8359-4A2A-A0DD-D17515DCC5CF");
    }
}