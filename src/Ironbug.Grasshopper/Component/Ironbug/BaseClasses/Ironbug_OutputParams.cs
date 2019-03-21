using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Ironbug.HVAC.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_OutputParams : Ironbug_Component, IGH_VariableParameterComponent
    {
        protected override System.Drawing.Bitmap Icon => Properties.Resources.OutputVariable;

        public override Guid ComponentGuid => new Guid("03687964-1876-4593-B038-23905C85D5CC");
        public override GH_Exposure Exposure => GH_Exposure.secondary;
        private IEnumerable<string> OutputVariables { get; set; } = new List<string>();

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
            pManager.AddGenericParameter("OutputVariables", "vars", "TODO...", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (this.Params.Input.Any())
            {
                this.Message = "Right click to retrieve!";
            }
            else
            {
                this.Message = "Double click to switch!";
            }
            
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
            if (side == GH_ParameterSide.Output) return false;
            return true;
        }

        public bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            if (side == GH_ParameterSide.Output) return false;
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

            menu.Items.RemoveAt(1); // remove Preview
            menu.Items.RemoveAt(2); // remove Bake

            var t = new ToolStripMenuItem("OutputVariables");
            this.GetEPOutputVariables(this, EventArgs.Empty);
            var allParams = this.OutputVariables.ToList();
            
            var inputParams = this.Params.Input.Select(_ => _.Name).ToList();
            allParams.Sort();
            foreach (var item in allParams)
            {
                var mitem = Menu_AppendItem(t.DropDown, item, OnClickParam, true, inputParams.Any(_ => _ == item));
            }
            if (allParams.Any()) {
                menu.Items.Add(t);
            }

            Menu_AppendSeparator(menu);
            Menu_AppendItem(menu, "Get EPOutputVariables", GetEPOutputVariables, true);
            Menu_AppendItem(menu, "RemoveUnused", RemoveUnused, true);
            Menu_AppendSeparator(menu);
        }

        private void OnClickParam(object sender, EventArgs e)
        {
            var clickedItem = sender as ToolStripMenuItem;
            if (clickedItem == null) return;
            
            //var name = clickedItem.Text;
            if (!clickedItem.Checked)
            {
                AddVariableToParam(clickedItem.Text);
            }
            else
            {
                RemoveParamFromName(clickedItem.Text);
            }

            this.Params.OnParametersChanged();
            this.OnDisplayExpired(true);
        }

        private void RemoveParamFromName(string text)
        {
            var inputParam = this.Params.Input.FirstOrDefault(_ => _.Name == text);
            this.Params.UnregisterInputParameter(inputParam);
        }

        public void GetEPOutputVariables(object sender, EventArgs e)
        {
            var recs = this.Params.Output[0].Recipients;
            if (recs.Count == 0) return;

            var rec = recs[0].Attributes.GetTopLevel.DocObject as Ironbug_HVACComponent;
            var obj = rec.IB_ModelObject;
            if (obj is null) return;
            
            if (obj is IB_ModelObject ibObj)
            {
                this.OutputVariables = ibObj.SimulationOutputVariables;
            }
        }

        private void AddVariablesToParams(IEnumerable<string> variablesTobeAdded)
        {
            foreach (var outputV in variablesTobeAdded)
            {
                AddVariableToParam(outputV);
            }
            this.Params.OnParametersChanged();
            this.OnDisplayExpired(true);
        }

        private void AddVariableToParam(string outputV)
        {
            //Don't add those already exist
            var inputNames = this.Params.Input.Select(_ => _.Name).ToList();
            if (inputNames.Contains(outputV)) return;
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
            this.OnDisplayExpired(true);
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

                this.AddVariablesToParams(this.OutputVariables);
            }
        }
    }
}