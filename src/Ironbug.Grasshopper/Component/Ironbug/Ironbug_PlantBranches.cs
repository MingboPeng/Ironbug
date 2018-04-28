using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Ironbug.HVAC.BaseClass;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_PlantBranches : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public Ironbug_PlantBranches()
          : base("PlantBranches", "Branches",
               "Description",
              "Ironbug", "01:Loops")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Branch1", "B1", "Items to be added to a branch. Tree structured objects will be automatically converted to branches, instead of one branch.", GH_ParamAccess.tree);
            pManager.AddGenericParameter("Branch2", "B2", "...", GH_ParamAccess.tree);
            pManager[1].Optional = true;
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
            var tree1 = new GH_Structure<IGH_Goo>();
            var tree2 = new GH_Structure<IGH_Goo>();

            DA.GetDataTree(0, out tree1);
            DA.GetDataTree(1, out tree2);



            var loopBranches = this.mapToLoopBranches(tree1);//just test the first tree for now.
            //loopBranches.a

            //DA.GetDataList(0, branch1);
            //DA.GetDataList(1, branch2);

            
            //branches.Add(branch1);
            //branches.Add(branch2);

            DA.SetData(0, loopBranches);



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
                return Properties.Resources.Branches_Plant;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("2d545ece-6191-4b87-980b-42b76efd9d0c"); }
        }


        public IB_PlantLoopBranches mapToLoopBranches(GH_Structure<IGH_Goo> ghTrees)
        {
            var loopBranches = new IB_PlantLoopBranches();

            var ghBranches = ghTrees.Branches;
            
            var converter = new Converter<IGH_Goo, IB_HVACObject>((_) => (IB_HVACObject)((GH_ObjectWrapper)_).Value);

            if (ghBranches.Count > 0)
            {
                foreach (var ghBranch in ghBranches)
                {
                    //var loopBranch = new List<IB_HVACObject>();
                    
                    //var loopBranch = ghBranch.ConvertAll(converter);
                    loopBranches.Add(ghBranch.ConvertAll(converter));
                }

                
            }
            else
            {
                
                
            }



            return loopBranches;
        }

    }
}