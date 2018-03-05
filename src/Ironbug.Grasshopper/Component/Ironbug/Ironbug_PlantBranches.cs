using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_PlantBranches : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public Ironbug_PlantBranches()
          : base("PlantBranches", "Branches",
               "Description",
              "Ironbug", "02:Loops")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Branch1", "B1", "heating or cooling supply source", GH_ParamAccess.list);
            pManager.AddGenericParameter("Branch2", "B2", "zoneBranches or other HVAC components", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PlantLoopBranches", "Branches", "use this in plantloop", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var branch1 = new List<HVAC.IB_HVACObject>();
            var branch2 = new List<HVAC.IB_HVACObject>();


            DA.GetDataList(0, branch1);
            DA.GetDataList(1, branch2);

            var branches = new HVAC.IB_PlantLoopBranches();
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
            get { return new Guid("2d545ece-6191-4b87-980b-42b76efd9d0c"); }
        }
    }
}