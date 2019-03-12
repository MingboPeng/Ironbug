using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Ironbug.HVAC.BaseClass;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_PlantBranches : Ironbug_Component, IGH_VariableParameterComponent
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public Ironbug_PlantBranches()
          : base("PlantBranches", "PlantBranches",
               "Description",
              "Ironbug", "01:Loops")
        {
            Params.ParameterSourcesChanged += ParamSourcesChanged;
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Branch1", "B1", "Items to be added to a branch. Tree structured objects will be automatically converted to branches, instead of one branch.", GH_ParamAccess.tree);
            
            pManager[0].Optional = true;
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
            
            var branches = this.CollectBranches();
            this.Message = this.CountBranches(branches);

            DA.SetData(0, branches);



        }

        public bool CanInsertParameter(GH_ParameterSide side, int index)
        {
            return side == GH_ParameterSide.Input;
        }

        public bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            return side == GH_ParameterSide.Input;
        }

        public IGH_Param CreateParameter(GH_ParameterSide side, int index)
        {
            var param = new Param_GenericObject();
            param.NickName = String.Empty;
            param.Name = "Branch";
            param.Description = "Items to be added to a branch. Tree structured objects will be automatically converted to branches, instead of one branch.";
            param.Access = GH_ParamAccess.tree;
            param.Optional = true;
            return param;
        }

        public bool DestroyParameter(GH_ParameterSide side, int index)
        {
            return true;
        }

        public void VariableParameterMaintenance()
        {
            //basically just checking nick names 
            int inputI = 0;
            foreach (var param in Params.Input)
            {
                inputI++;
                param.NickName = $"B{ inputI }";
                
            }
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


        public List<List<IB_HVACObject>> MapToLoopBranches(GH_Structure<IGH_Goo> ghTrees)
        {
            var loopBranches = new List<List<IB_HVACObject>>();

            var ghBranches = ghTrees.Branches;
            
            var converter = new Converter<IGH_Goo, IB_HVACObject>((_) => (IB_HVACObject)((GH_ObjectWrapper)_).Value);

            if (ghBranches.Count > 0)
            {
                foreach (var ghBranch in ghBranches)
                {
                    loopBranches.Add(ghBranch.ConvertAll(converter));
                }

                
            }
            else
            {
                
                
            }



            return loopBranches;
        }

        //public List<List<IB_HVACObject>> CheckPuppets(List<List<IB_HVACObject>> Branches)
        //{
        //    var loopBranches = new List<List<IB_HVACObject>>();
        //    foreach (var branch in Branches)
        //    {
        //        if (branch.)
        //        {

        //        }
        //    }


        //    return loopBranches;
        //}

        private string CountBranches(HVAC.IB_PlantLoopBranches branches)
        {
            string messages = string.Empty;
            var b = branches.Branches.Count;

            if (b > 0)
            {
                messages = $"{b} branches";
            }

            return messages;

        }

        private HVAC.IB_PlantLoopBranches CollectBranches()
        {

            var branches = new HVAC.IB_PlantLoopBranches();

            var allParams = this.Params.Input;
            foreach (var param in allParams)
            {
                if (param.SourceCount <= 0)
                {
                    continue;
                }

                var tree = new List<List<IB_HVACObject>>();
                
                if (!param.VolatileData.IsEmpty)
                {
                    tree = MapToLoopBranches((GH_Structure<IGH_Goo>)param.VolatileData);
                }

                foreach (var branch in tree)
                {
                    branches.Add(branch);
                }
                

            }
            

            return branches;
        }

        private void ParamSourcesChanged(Object sender, GH_ParamServerEventArgs e)
        {

            //check input side only
            if (e.ParameterSide != GH_ParameterSide.Input) return;

            if (Params.Input.Last().Sources.Any())
            {
                IGH_Param newParam = CreateParameter(GH_ParameterSide.Input, Params.Input.Count);
                Params.RegisterInputParam(newParam, Params.Input.Count);
                VariableParameterMaintenance();
                Params.OnParametersChanged();
            }


        }

    }
}