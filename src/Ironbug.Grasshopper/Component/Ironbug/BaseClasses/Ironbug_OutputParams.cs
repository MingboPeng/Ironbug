using GH_IO.Serialization;
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
        private IB_OutputVariable.TimeSteps _outputFrequency = IB_OutputVariable.TimeSteps.Hourly; 
        protected override System.Drawing.Bitmap Icon => Properties.Resources.OutputVariable;

        public override Guid ComponentGuid => new Guid("03687964-1876-4593-B038-23905C85D5CC");
        public override GH_Exposure Exposure => GH_Exposure.secondary;
        private IEnumerable<string> OutputVariables { get; set; } = new List<string>();

        public Ironbug_OutputParams()
          : base("IB_OutputParams", "OutputParams",
              "Use this component to list all EnergyPlus output variables of a connected downstream HVAC component, and set selected variables to true to request the EnergyPlus to collect data during the simulation.",
              "Ironbug", "00:Ironbug")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("OutputVariables", "vars", "Add EnergyPlus output variables to HVAC component's parameter inputs.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (this.Params.Input.Any())
            {
                this.Message = $"Report: {this._outputFrequency}";
            }
            else
            {
                this.Message = "Right click to add!";
            }
            
           
            var settingDatas = new List<IB_OutputVariable>();
            settingDatas = CollectOutputVariable();
            var vars = new OutputVariables(settingDatas);
            DA.SetData(0, vars);

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
                            outputVariables.Add(new IB_OutputVariable(item.Name, this._outputFrequency));
                        }
                    }
                }
            }
            return outputVariables;
        }

        public bool CanInsertParameter(GH_ParameterSide side, int index)
        {
            return false ;
        }

        public bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            if (side == GH_ParameterSide.Output) return false;
            return true;
        }

        public IGH_Param CreateParameter(GH_ParameterSide side, int index)
        {
            return null;
        }

        public bool DestroyParameter(GH_ParameterSide side, int index)
        {
            return true;
            //throw new NotImplementedException();
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
            Menu_AppendItem(menu, "Hourly", ChangeReportRrequencyHourly, true, this._outputFrequency == IB_OutputVariable.TimeSteps.Hourly)
                    .ToolTipText = "Report selected variables hourly";
            Menu_AppendItem(menu, "Daily", ChangeReportRrequencyDaily, true, this._outputFrequency ==  IB_OutputVariable.TimeSteps.Daily)
                    .ToolTipText = "Report selected variables daily";
            Menu_AppendItem(menu, "Monthly", ChangeReportRrequencyMonthly, true, this._outputFrequency ==  IB_OutputVariable.TimeSteps.Monthly)
                    .ToolTipText = "Report selected variables monthly";
            Menu_AppendItem(menu, "RunPeriod", ChangeReportRrequencyAnnually, true, this._outputFrequency ==  IB_OutputVariable.TimeSteps.RunPeriod)
                    .ToolTipText = "Report selected variables for entire run period";
            Menu_AppendSeparator(menu);
        }

        private void ChangeReportRrequencyDaily(object sender, EventArgs e)
        {
            this._outputFrequency =  IB_OutputVariable.TimeSteps.Daily;
            this.ExpireSolution(true);
        }

        private void ChangeReportRrequencyAnnually(object sender, EventArgs e)
        {
            this._outputFrequency =  IB_OutputVariable.TimeSteps.RunPeriod;
            this.ExpireSolution(true);
        }

        private void ChangeReportRrequencyMonthly(object sender, EventArgs e)
        {
            this._outputFrequency = IB_OutputVariable.TimeSteps.Monthly;
            this.ExpireSolution(true);
        }

        private void ChangeReportRrequencyHourly(object sender, EventArgs e)
        {
            this._outputFrequency = IB_OutputVariable.TimeSteps.Hourly;
            this.ExpireSolution(true);
        }
        public override bool Read(GH_IReader reader)
        {
            if (reader.ItemExists("_outputFrequency"))
            {
                _outputFrequency = (IB_OutputVariable.TimeSteps)reader.GetInt32("_outputFrequency");
            }
            return base.Read(reader);
        }
        public override bool Write(GH_IWriter writer)
        {
            writer.SetInt32("_outputFrequency", (int)_outputFrequency);
            return base.Write(writer);
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
            if (rec is null) AddRuntimeMessage( GH_RuntimeMessageLevel.Error, $"{recs[0].Attributes.GetTopLevel.DocObject.Name} is not a valid Ironbug HVAC component.");
            var obj = rec.IB_ModelObject;
            if (obj is null) return;
            
            if (obj is IB_ModelObject ibObj)
            {
                this.OutputVariables = ibObj.SimulationOutputVariables;
            }
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

        
    }

    public class OutputVariables : List<IB_OutputVariable>
    {
        public OutputVariables(List<IB_OutputVariable> list): base(list)
        {
        }
        public override string ToString()
        {
            var names = this?.Select(_=>_.VariableName)?.ToList();
            if(names != null && names.Any())
            {
                return string.Join(Environment.NewLine, names);
            }
            else
            {
                return "No variable";
            }
        
        }
    }
}