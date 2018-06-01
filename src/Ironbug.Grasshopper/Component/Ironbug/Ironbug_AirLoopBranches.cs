using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;
using Rhino.Geometry;

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
            pManager.AddGenericParameter("Branch1", "B1", "A list of zones will be automatically converted to branches. One zone per branch", GH_ParamAccess.list);
            pManager[0].Optional = true;
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
            param.Description = "A list of zones will be automatically converted to branches. One zone per branch";
            param.Access = GH_ParamAccess.list;
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


        private string CountBranches(HVAC.IB_AirLoopBranches branches)
        {
            string messages = string.Empty;
            var b = branches.Branches.Count;

            if (b>0)
            {
                messages = $"{b} branches";
            }

            return messages;

        }

        private IB_AirLoopBranches CollectBranches()
        {
            
            var branches = new IB_AirLoopBranches();
            
            var allParams = this.Params.Input;
            foreach (var param in allParams)
            {
                if (param.SourceCount <= 0)
                {
                    continue;
                }
                
                //TODO: need to check what if the tree structure.
                if (!param.VolatileData.IsEmpty)
                {
                    var branch = param
                        .VolatileData
                        .AllData(true)
                        .Select((_) =>  (IB_HVACObject)((GH_ObjectWrapper)_).Value)
                        .ToList();
                    
                    foreach (var item in branch)
                    {
                        if (item is IB_ThermalZone zone)
                        {
                            branches.Add(new List<IB_HVACObject>() { zone });
                        }
                        else
                        {
                            throw new Exception("Currently AirloopBranch only accepts Zone objects. If you want to add AirTerminals, please add it directly to zones!");
                        }
                        
                    }


                    
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