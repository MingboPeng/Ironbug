using System;
using System.Collections.Generic;
using Ironbug.HVAC;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Grasshopper.Kernel.Parameters;
using System.Windows.Forms;
using System.Linq;
using Ironbug.HVAC.BaseClass;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_OutputParams : GH_Component, IGH_VariableParameterComponent
    {
        public Ironbug_OutputParams()
          : base("Ironbug_OutputParams", "OutputParams",
              "Description",
              "Ironbug", "00:Ironbug")
        {
        }
        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            //pManager.AddGenericParameter("demand", "demand", "zoneMixer or other HVAC components", GH_ParamAccess.item);
            //pManager.AddTextParameter("name", "name", "name", GH_ParamAccess.item);
        }
        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("OutputParams", "OutputParams", "TODO...", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var settingDatas = new List<IB_OutputVariable>();
            settingDatas = CollectOutputVariable();
            DA.SetData(0, settingDatas);
            
        }

        private List<IB_OutputVariable> CollectOutputVariable()
        {
            var outputVariables = new List<IB_OutputVariable>();
            var allInputParams = this.Params.Input;
            foreach (var item in allInputParams)
            {
                if (item.SourceCount <= 0 || item.VolatileData.IsEmpty)
                {
                    continue;
                }
                else
                {
                    var fristData = item.VolatileData.AllData(true).ToList().First();

                    if (!((fristData == null) || String.IsNullOrWhiteSpace(fristData.ToString())))
                    {
                        
                        bool value;
                        fristData.CastTo(out value);
                        if (value)
                        {
                            outputVariables.Add(new IB_OutputVariable(item.Name, IB_OutputVariable.OutputVariableTimeStep.Monthly));
                        }
                        

                    }
                }
                

            }
            return outputVariables;
        }

        public bool CanInsertParameter(GH_ParameterSide side, int index)
        {
            return true;
        }

        public bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            return true;
        }

        public IGH_Param CreateParameter(GH_ParameterSide side, int index)
        {
            return new Param_GenericObject();
        }

        public bool DestroyParameter(GH_ParameterSide side, int index)
        {
            throw new NotImplementedException();
        }

        public void VariableParameterMaintenance()
        {
            //throw new NotImplementedException();
        }
        
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            Menu_AppendItem(menu, "EPOutputVariables", GetEPOutputVariables, true);
            Menu_AppendItem(menu, "RemoveUnused", RemoveUnused, true);
            Menu_AppendSeparator(menu);
        }

        public void GetEPOutputVariables(object sender, EventArgs e)
        {
            var recs = this.Params.Output[0].Recipients;
            if (recs.Count == 0) return;

            var rec = recs[0].Attributes.GetTopLevel.DocObject as Ironbug_HVACComponentBase;
            //var obj = rec.Params.Output.Last().VolatileData.AllData(true).First();
            var obj = rec.IB_ModelObject;
            if (obj is null) return;

            //object value = null;
            //obj.CastTo(out value);
            if (obj is IB_ModelObject ibObj)
            {
                var outvariables = ibObj.SimulationOutputVariables;
                this.AddVariablesToParams(outvariables);
            }

        }

        private void AddVariablesToParams(IEnumerable<string> variablesTobeAdded)
        {
            foreach (var outputV in variablesTobeAdded)
            {
                //Don't add those already exist
                var paramFound = this.Params.Input.FirstOrDefault(_ => _.Name == outputV);
                if (paramFound != null) continue;
                //Add new Param
                IGH_Param newParam = new Param_GenericObject();

                newParam.Name = outputV;
                newParam.NickName = outputV;
                newParam.Description = "TODO...";
                newParam.MutableNickName = false;
                newParam.Access = GH_ParamAccess.item;
                newParam.Optional = true;
                
                Params.RegisterInputParam(newParam);
            }
            this.Params.OnParametersChanged();
            this.ExpireSolution(true);

        }

        private void RemoveUnused(object sender, EventArgs e)
        {
            var inputParams = this.Params.Input;
            var tobeRemoved = new List<IGH_Param>();
            foreach (var item in inputParams)
            {
                if (item.SourceCount > 0) continue;
                tobeRemoved.Add(item);
            }

            foreach (var item in tobeRemoved)
            {
                this.Params.UnregisterInputParameter(item);
            }
            this.Params.OnParametersChanged();
            this.ExpireSolution(true);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.OutputVariable;
        
        public override Guid ComponentGuid => new Guid("03687964-1876-4593-B038-23905C85D5CC");
    }
}