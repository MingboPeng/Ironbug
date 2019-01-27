using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Ironbug.HVAC.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_OutputParams : GH_Component, IGH_VariableParameterComponent
    {
        protected override System.Drawing.Bitmap Icon => Properties.Resources.OutputVariable;

        public override Guid ComponentGuid => new Guid("03687964-1876-4593-B038-23905C85D5CC");
        public override GH_Exposure Exposure => GH_Exposure.secondary;

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
            this.Message = "Double click for more details!";
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
                            outputVariables.Add(new IB_OutputVariable(item.Name, IB_OutputVariable.TimeSteps.Hourly));
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
                var inputNames = this.Params.Input.Select(_ => _.Name).ToList();
                if (inputNames.Contains(outputV)) continue;
                //var paramFound = this.Params.Input.FirstOrDefault(_ => _.Name == outputV);
                //if (paramFound != null) continue;
                //Add new Param
                IGH_Param newParam = new Param_GenericObject();

                newParam.Name = outputV;
                newParam.NickName = outputV;
                newParam.Description = "TODO...";
                newParam.MutableNickName = false;
                newParam.Access = GH_ParamAccess.item;
                newParam.Optional = true;

                inputNames.Add(outputV);
                inputNames.Sort();
                var index = inputNames.IndexOf(outputV);
                Params.RegisterInputParam(newParam, index);
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

        public override void CreateAttributes()
        {
            var newAttri = new IB_SettingComponentAttributes(this);
            m_attributes = newAttri;
        }

        private bool isCleanInputs = false;

        internal void RespondToMouseDoubleClick()
        {
            isCleanInputs = !isCleanInputs;
            if (isCleanInputs)
            {
                this.RemoveUnused(this, EventArgs.Empty);
            }
            else
            {
                this.GetEPOutputVariables(this, EventArgs.Empty);
            }
        }
    }
}