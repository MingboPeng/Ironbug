using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Ironbug.HVAC.BaseClass;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_AirLoopBranches : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_AirLoopBranches class.
        /// </summary>
        public Ironbug_AirLoopBranches()
          : base("AirLoopBranches", "Branches",
              "Description",
              "Ironbug", "02:Loops")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Branch1", "B1", "...", GH_ParamAccess.list);
            //pManager[0].Optional = true;
            pManager.AddGenericParameter("Branch2", "B2", "...", GH_ParamAccess.list);
            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("AirLoopBranches", "Branches", "use this in air loop", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var branch1 = new List<IB_HVACObject>();
            var branch2 = new List<IB_HVACObject>();


            DA.GetDataList(0, branch1);
            DA.GetDataList(1, branch2);

            var branches = new IB_AirLoopBranches();
            branches.Add(branch1);
            branches.Add(branch2);

            DA.SetData(0, branches);
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
                return Properties.Resources.Branches;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("cdfeb7d7-63cc-4e0a-b77b-553026f30803"); }
        }
    }
}