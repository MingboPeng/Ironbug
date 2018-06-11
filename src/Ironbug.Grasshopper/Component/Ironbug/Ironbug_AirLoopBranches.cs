using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;
using Rhino.Geometry;
using Grasshopper;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_AirLoopBranches : GH_Component, IGH_VariableParameterComponent
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_AirLoopBranches class.
        /// </summary>
        public Ironbug_AirLoopBranches()
          : base("AirLoopBranches", "Branches",
              "Description",
              "Ironbug", "01:Loops")
        {
            Params.ParameterSourcesChanged += ParamSourcesChanged;
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Branch1", "B1", "A list of zones will be automatically converted to branches. One zone per branch", GH_ParamAccess.tree);
            pManager[0].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("AirLoopBranches", "Branches", "use this in air loop", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            
            var branches = this.CollectBranches();
            this.Message = this.CountBranches(branches);
            
            DA.SetDataTree(0, branches);
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
            param.Description = "A list of zones will be automatically converted to branches. One zone per branch";
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
                string nName = param.NickName;
                inputI ++;
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
                return Properties.Resources.Branches_Air;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("cdfeb7d7-63cc-4e0a-b77b-553026f30803"); }
        }


        private string CountBranches(DataTree<IB_AirLoopBranches> loops)
        {
            string messages = string.Empty;
            if (loops.DataCount == 0) return messages;

            var firstTree = loops.Branches.First();
            var airloopBranches = firstTree.First();
            var b = airloopBranches.Branches.Count;

            if (b>0) messages = $"{b} branches";
            if (loops.BranchCount>1) messages += "/tree";

            return messages;

        }

        private DataTree<IB_AirLoopBranches> CollectBranches()
        {
            GH_Structure<IGH_Goo> tree = new GH_Structure<IGH_Goo>();

            var allParams = this.Params.Input;
            foreach (var param in allParams)
            {
                if (param.SourceCount <= 0)
                {
                    continue;
                }
                
                if (!param.VolatileData.IsEmpty)
                {
                    var treeData = param.VolatileData as GH_Structure<IGH_Goo>;
                    tree.MergeStructure(treeData);
                }
                
            }

            var trees =  ConvertTreeItemsToTreeBranch(tree);

            return trees;

            DataTree<IB_AirLoopBranches> ConvertTreeItemsToTreeBranch(GH_Structure<IGH_Goo> treeItems)
            {
                DataTree<IB_AirLoopBranches> loops = new DataTree<IB_AirLoopBranches>();
                var ItemGroups = treeItems.Branches;
                int index = 0;
                foreach (var group in ItemGroups)
                {
                    var loop = new IB_AirLoopBranches();
                    foreach (var ghObj in group)
                    {
                        var item = (IB_HVACObject)((GH_ObjectWrapper)ghObj).Value;
                        if (item is IB_ThermalZone zone)
                        {
                            loop.Add(new List<IB_HVACObject>() { zone });
                        }
                        else
                        {
                            throw new Exception("Currently AirloopBranch only accepts Zone objects. If you want to add AirTerminals, please add it directly to zones!");
                        }

                    }
                    
                    loops.Add(loop, new GH_Path(index));
                    index++;
                }
                

                return loops;


            }
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