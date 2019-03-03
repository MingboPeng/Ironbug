using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;
using Ironbug.HVAC.BaseClass;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_AirLoopHVAC : Ironbug_Component
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_AirLoopHVAC class.
        /// </summary>
        public Ironbug_AirLoopHVAC()
          : base("Ironbug_AirLoopHVAC", "AirLoop",
              EPDoc.AirLoopHVAC.Note,
              "Ironbug", "01:Loops")
        {
        }
        public override GH_Exposure Exposure => GH_Exposure.primary;

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("supply", "supply", "heating or cooling supply source", GH_ParamAccess.list);
            pManager.AddGenericParameter("demand", "demand", "zoneBranches or other HVAC components", GH_ParamAccess.list);
            pManager.AddGenericParameter("sizingSystem", "sizing", "HVAC components", GH_ParamAccess.item);
            
            pManager[0].Optional = true;
            pManager[1].Optional = true;
            pManager[2].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("AirLoopHVAC", "AirLoop", "toSaveOSM", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var supplyComs = new List<IB_HVACObject>();
            DA.GetDataList(0, supplyComs);

            var demandComs = new List<IB_HVACObject>();
            DA.GetDataList(1, demandComs);

            HVAC.IB_SizingSystem sizing = null;
           
            
            var airLoop = new HVAC.IB_AirLoopHVAC();
            if (DA.GetData(2, ref sizing))
            {
                airLoop.SetSizingSystem(sizing);
            }
            

            //TODO: need to check nulls
            foreach (var item in supplyComs)
            {
                airLoop.AddToSupplySide(item);
            }

            foreach (var item in demandComs)
            {
                airLoop.AddToDemandSide(item);
            }
            
            
            DA.SetData(0, airLoop);

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
                return Resources.AirLoop;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("a416631f-bdda-4e11-8a2c-658c38681201"); }
        }

        public override void CreateAttributes()
        {
            var newAttri = new IB_LoopComponentAttributes(this);
            m_attributes = newAttri;
        }
    }
}