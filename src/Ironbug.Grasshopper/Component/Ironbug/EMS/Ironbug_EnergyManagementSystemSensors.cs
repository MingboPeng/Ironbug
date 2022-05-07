
//using Grasshopper.Kernel;
//using Grasshopper.Kernel.Parameters;
//using Ironbug.HVAC.BaseClass;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Windows.Forms;

//namespace Ironbug.Grasshopper.Component
//{
//    public class Ironbug_EnergyManagementSystemSensors : Ironbug_Component, IGH_VariableParameterComponent
//    {
//        protected override System.Drawing.Bitmap Icon => null;

//        public override Guid ComponentGuid => new Guid("E5A01F19-74F9-43BC-80F9-D4C4DCEF06C8");
//        private IEnumerable<string> OutputVariables { get; set; } = new List<string>();

//        public Ironbug_EnergyManagementSystemSensors()
//          : base("IB_EnergyManagementSystemSensors", "EmsSensors",
//              "Description",
//              "Ironbug", "06:Sizing&Controller")
//        {
//        }
//        public override GH_Exposure Exposure => GH_Exposure.tertiary;
//        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
//        {
//        }

//        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
//        {
//            pManager.AddGenericParameter("sensors", "sensors", "Connect to Ironbug component's _param input to get each object's available sensors", GH_ParamAccess.item);
//        }

//        protected override void SolveInstance(IGH_DataAccess DA)
//        {
//            this.Message = "Right click to add!";

//            var settingDatas = CollectOutputVariable();
//            DA.SetData(0, settingDatas);

//        }

//        private List<HVAC.IB_EnergyManagementSystemSensor> CollectOutputVariable()
//        {
//            var outputVariables = new List<HVAC.IB_EnergyManagementSystemSensor>();
//            var allInputParams = this.Params.Input;
//            foreach (var item in allInputParams)
//            {
//                if (item.SourceCount <= 0 || item.VolatileData.IsEmpty)
//                {
//                    continue;
//                }
//                else
//                {
//                    var fristData = item.VolatileData.AllData(true).ToList().First();

//                    if (!((fristData == null) || String.IsNullOrWhiteSpace(fristData.ToString())))
//                    {
//                        string value;
//                        fristData.CastTo(out value);
//                        if (!string.IsNullOrEmpty(value))
//                        {
//                            var sensor = new HVAC.IB_EnergyManagementSystemSensor();
//                            var f = HVAC.IB_EnergyManagementSystemSensor_FieldSet.Value;
//                            sensor.AddCustomAttribute(f.OutputVariableOrMeterName, item.Name);
//                            sensor.SetTrackingID(value);
//                            outputVariables.Add(sensor);
//                        }
//                    }
//                }
//            }
//            return outputVariables;
//        }

//        public bool CanInsertParameter(GH_ParameterSide side, int index)
//        {
//            return false ;
//        }

//        public bool CanRemoveParameter(GH_ParameterSide side, int index)
//        {
//            if (side == GH_ParameterSide.Output) return false;
//            return true;
//        }

//        public IGH_Param CreateParameter(GH_ParameterSide side, int index)
//        {
//            return null;
//        }

//        public bool DestroyParameter(GH_ParameterSide side, int index)
//        {
//            throw new NotImplementedException();
//        }

//        public void VariableParameterMaintenance()
//        {
//            //throw new NotImplementedException();
//        }

//        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
//        {

//            menu.Items.RemoveAt(1); // remove Preview
//            menu.Items.RemoveAt(2); // remove Bake

//            var t = new ToolStripMenuItem("EMS Sensors");
//            this.GetEPOutputVariables(this, EventArgs.Empty);
//            var allParams = this.OutputVariables.ToList();
            
//            var inputParams = this.Params.Input.Select(_ => _.Name).ToList();
//            allParams.Sort();
//            foreach (var item in allParams)
//            {
//                var mitem = Menu_AppendItem(t.DropDown, item, OnClickParam, true, inputParams.Any(_ => _ == item));
//            }
//            if (allParams.Any()) {
//                menu.Items.Add(t);
//            }
            
        
//        }

      
//        private void OnClickParam(object sender, EventArgs e)
//        {
//            var clickedItem = sender as ToolStripMenuItem;
//            if (clickedItem == null) return;
            
//            //var name = clickedItem.Text;
//            if (!clickedItem.Checked)
//            {
//                AddVariableToParam(clickedItem.Text);
//            }
//            else
//            {
//                RemoveParamFromName(clickedItem.Text);
//            }

//            this.Params.OnParametersChanged();
//            this.OnDisplayExpired(true);
//        }

//        private void RemoveParamFromName(string text)
//        {
//            var inputParam = this.Params.Input.FirstOrDefault(_ => _.Name == text);
//            this.Params.UnregisterInputParameter(inputParam);
//        }

//        public void GetEPOutputVariables(object sender, EventArgs e)
//        {
//            var recs = this.Params.Output[0].Recipients;
//            if (recs.Count == 0) return;

//            var rec = recs[0].Attributes.GetTopLevel.DocObject as Ironbug_HVACComponent;
//            if (rec is null) AddRuntimeMessage( GH_RuntimeMessageLevel.Error, $"{recs[0].Attributes.GetTopLevel.DocObject.Name} is not a valid Ironbug HVAC component.");
//            var obj = rec.IB_ModelObject;
//            if (obj is null) return;
            
//            if (obj is IB_ModelObject ibObj)
//            {
//                this.OutputVariables = ibObj.SimulationOutputVariables;
//            }
//        }



//        private void AddVariableToParam(string outputV)
//        {
//            //Don't add those already exist
//            var inputNames = this.Params.Input.Select(_ => _.Name).ToList();
//            if (inputNames.Contains(outputV)) return;

//            var newParam = new Param_String();

//            newParam.Name = outputV;
//            newParam.NickName = outputV;
//            newParam.Description = "Assign a tag id for this actuator that can be referenced in EMS program. Note, you are responsible to keep the tag id unique across the entire model";
//            newParam.MutableNickName = false;
//            newParam.Access = GH_ParamAccess.item;
//            newParam.Optional = false;

//            inputNames.Add(outputV);
//            inputNames.Sort();
//            var index = inputNames.IndexOf(outputV);
//            Params.RegisterInputParam(newParam, index);
//        }

        
//    }
//}