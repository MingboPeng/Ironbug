using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Ironbug.HVAC.BaseClass;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_PlantBranches : Ironbug_Component, IGH_VariableParameterComponent
    {
        private bool mapBranchToLoop = false;
        public Ironbug_PlantBranches()
          : base("PlantBranches", "PlantBranches",
               "Description",
              "Ironbug", "01:Loops")
        {
            Params.ParameterSourcesChanged += ParamSourcesChanged;
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;
        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Branch1", "B1", "Items to be added to a branch. Tree structured objects will be automatically converted to branches, instead of one branch.", GH_ParamAccess.tree);
            pManager[0].WireDisplay = GH_ParamWireDisplay.faint;
            pManager[0].DataMapping = GH_DataMapping.Graft;
            pManager[0].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PlantLoopBranches", "Branches", "use this in plantloop", GH_ParamAccess.tree);
        }

        
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
            param.Description = "Items to be added to a branch. Tree structured objects will be automatically converted to branches, instead of one branch.";
            param.Access = GH_ParamAccess.tree;
            param.WireDisplay = GH_ParamWireDisplay.faint;
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

        protected override System.Drawing.Bitmap Icon => Properties.Resources.Branches_Plant;

        public override Guid ComponentGuid => new Guid("2d545ece-6191-4b87-980b-42b76efd9d0c");


        

        private string CountBranches(DataTree<HVAC.IB_PlantLoopBranches> TreeLoops)
        {
            string messages = string.Empty;
            if (TreeLoops.DataCount == 0) return messages;

            var totalB = 0;

            foreach (var loop in TreeLoops.Branches)
            {
                totalB += loop.First().Branches.Count;
            }

            if (totalB > 0) messages = totalB==1? $"{totalB} branch": $"{totalB} branches";
            if (TreeLoops.BranchCount > 1) messages += $"/{TreeLoops.BranchCount} Loops";

            return messages;

        }

        private DataTree<HVAC.IB_PlantLoopBranches> CollectBranches()
        {
            DataTree<HVAC.IB_PlantLoopBranches> treeLoops = new DataTree<HVAC.IB_PlantLoopBranches>();

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
                    MapBranchToBranch(ref treeLoops, (GH_Structure<IGH_Goo>)param.VolatileData);
                    
                }

            }

            return treeLoops;

            void MapBranchToBranch(ref DataTree<HVAC.IB_PlantLoopBranches> loops, GH_Structure< IGH_Goo> ghTrees)
            {
                //DataTree<HVAC.IB_PlantLoopBranches> loops = new DataTree<HVAC.IB_PlantLoopBranches>();
                //var tempLoop = new HVAC.IB_PlantLoopBranches();

                var ghBranches = ghTrees.Branches;
                var converter = new Converter<IGH_Goo, IB_HVACObject>((_) => (IB_HVACObject)((GH_ObjectWrapper)_).Value);
                int index = loops.BranchCount;
                foreach (var ghBranch in ghBranches)
                {
                    var branchItems = ghBranch.ConvertAll(converter);
                    if (branchItems.Any(_ => _ is HVAC.IB_Probe))
                    {
                        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Probe cannot be added in PlantBranch yet! Stay tuned!");
                    }
                    else if (this.mapBranchToLoop)
                    {
                        var loop = new HVAC.IB_PlantLoopBranches();
                        foreach (var item in branchItems)
                        {
                            loop.Add(new List<IB_HVACObject>() { item });
                        }
                        loops.Add(loop, new GH_Path(index));
                        index++;
                    }
                    else
                    {
                        if (loops.BranchCount ==0)
                        {
                            loops.Add(new HVAC.IB_PlantLoopBranches());
                        }
                        loops.Branch(0)[0].Add(branchItems);
                        //tempLoop.Add(branchItems);
                    }
                }

                //if (!this.mapBranchToLoop)
                //{
                //    loops.Add(tempLoop, new GH_Path(index));
                //}



                //return loops;
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

        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
        {
            writer.SetBoolean("mapBranchToLoop", mapBranchToLoop);
            return base.Write(writer);
        }
        public override bool Read(GH_IO.Serialization.GH_IReader reader)
        {
            if (reader.ItemExists("mapBranchToLoop"))
            {
                mapBranchToLoop = reader.GetBoolean("mapBranchToLoop");
            }
            return base.Read(reader);
        }

        protected override void AppendAdditionalComponentMenuItems(System.Windows.Forms.ToolStripDropDown menu)
        {

            Menu_AppendItem(menu, "Map branch to Loop", ChangeMapping, true, this.mapBranchToLoop)
                .ToolTipText = "This will create a loop per input branch, items in each branch will be add to plant loop branches individually.";
            Menu_AppendSeparator(menu);

            base.AppendAdditionalComponentMenuItems(menu);
        }

        private void ChangeMapping(object sender, EventArgs e)
        {
            this.mapBranchToLoop = !this.mapBranchToLoop;
            this.ExpireSolution(true);
        }
    }
}