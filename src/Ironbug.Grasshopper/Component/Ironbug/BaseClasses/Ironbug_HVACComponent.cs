using GH_IO.Serialization;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Ironbug.HVAC.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Ironbug.Grasshopper.Component
{
    public abstract class Ironbug_HVACComponent : Ironbug_Component
    {
        public Type DataFieldType { get; private set; }

        public IB_ModelObject IB_ModelObject => iB_ModelObject;
        private IB_ModelObject iB_ModelObject;

        private bool _hasObjPrarmInput = false;
        private bool _hasDupParamInput = false;

        private bool _dupParmOn = false;

        //This constructor will create a HVACComponent without param input and duplication input.
        public Ironbug_HVACComponent(string name, string nickname, string description, string category, string subCategory, Type DataFieldType)
         : base(name, nickname, FindComDescription(description, DataFieldType), category, subCategory)
        {
            this.DataFieldType = DataFieldType;
        }

        //This constructor will create a HVACComponent with optional param input
        public Ironbug_HVACComponent(string name, string nickname, string description, string category, string subCategory, Type DataFieldType, bool hasParam)
           : this(name, nickname, description, category, subCategory, DataFieldType)
        {
            this._hasObjPrarmInput = hasParam;
            if (this._hasObjPrarmInput)
            {
                var paramInput = AddParamInput();
                Params.ParameterSourcesChanged += Params_ParameterSourcesChanged;
            }

        }

        //This constructor will create a HVACComponent with optional param input and duplication input.
        public Ironbug_HVACComponent(string name, string nickname, string description, string category, string subCategory, Type DataFieldType, bool hasParam, bool hasDup)
           : this(name, nickname, description, category, subCategory, DataFieldType, hasParam)
        {
            this._hasDupParamInput = hasDup;

        }

        private void Params_ParameterSourcesChanged(object sender, GH_ParamServerEventArgs e)
        {
            var pName = e.Parameter.NickName;
            var isParam = pName == "params_" || pName.StartsWith("Parameters");
            if (e.ParameterSide == GH_ParameterSide.Input && isParam)
            {
                ParamSettingChanged(e);
            }

            void ParamSettingChanged(GH_ParamServerEventArgs args)
            {
                var sources = args.Parameter.Sources;

                //adding case
                foreach (var item in sources)
                {
                    var docObj = item.Attributes.GetTopLevel.DocObject;
                    if (docObj is Ironbug_ObjParams objParams)
                    {
                        objParams.CheckRecipients();
                    }
                    else if (docObj is Ironbug_OutputParams outputParams)
                    {
                        //outputParams.GetEPOutputVariables(this, EventArgs.Empty);
                    }
                }
            }


        }

        private static string FindComDescription(string UsersDescription, Type DataFieldType)
        {
            var thereIsNoDescription = UsersDescription == "Description";
            var description = "There is no component description available now! \nPlease stay tuned or contribute :>\n\nSource code: https://github.com/MingboPeng/Ironbug";
            //var epDoc = string.Empty;
            if (thereIsNoDescription)
            {
                try
                {
                    var epdoc = (Activator.CreateInstance(DataFieldType, true) as IB_FieldSet)?.OwnerEpNote;
                    if (!string.IsNullOrEmpty(epdoc))
                    {
                        description = epdoc;
                    }
                }
                catch (Exception)
                {
                    //throw new ArgumentException($"{e.InnerException}");
                }
            }
            else
            {
                description = UsersDescription;
            }
            return description;
        }

        private IGH_Param AddParamInput()
        {
            IGH_Param param = new Param_GenericObject();
            param.Name = "Parameters_";
            param.NickName = "params_";
            param.Description = "Detail settings for this HVAC object. Use Ironbug_ObjParams to set input parameters, or use Ironbug_OutputParams to set output variables.";
            param.MutableNickName = false;
            param.Access = GH_ParamAccess.list;
            param.Optional = true;
            param.WireDisplay = GH_ParamWireDisplay.faint;

            Params.RegisterInputParam(param);
            return param;
        }

        protected void SetObjParamsTo(IB_ModelObject IB_obj)
        {
            //save it for outputParams
            this.iB_ModelObject = IB_obj;
            var paramInput = this.Params.Input.First(_ => _.Name.StartsWith("Parameters"));
            //catch the data when it is in branch
            if (this.Phase != GH_SolutionPhase.Computing) return;
            if (paramInput.VolatileDataCount == 0) return;
            var branchIndex = Math.Min(this.RunCount, paramInput.VolatileData.PathCount);
            var objParams = paramInput.VolatileData.get_Branch(branchIndex - 1);
            var inputP = (Dictionary<IB_Field, object>)null;
            var outputP = (List<IB_OutputVariable>)null;
            var emsSensors = (List<HVAC.IB_EnergyManagementSystemSensor>)null;
            var emsActuators = (List<HVAC.IB_EnergyManagementSystemActuator>)null;
            var emsInVariables = (List<HVAC.IB_EnergyManagementSystemInternalVariable>)null;
            var paramSource = (List<string>)null;

            foreach (var ghitem in objParams)
            {
                if (ghitem == null) continue;
                var item = ghitem as GH_ObjectWrapper;
                if (item == null)
                    throw new ArgumentException("params_ only accepts Ironbug_ObjParams or Ironbug_OutputParams!");

                if (item.Value is Dictionary<IB_Field, object> inputParams)
                {
                    if (inputParams.Count == 0) continue;
                    inputP = inputP ?? inputParams;
                }
                else if (item.Value is List<IB_OutputVariable> outputParams)
                {
                    if (outputParams.Count == 0) continue;
                    outputP = outputP ?? outputParams;
                }
                else if (item.Value is List<HVAC.IB_EnergyManagementSystemSensor> sensors)
                {
                    if (sensors.Count == 0) continue;
                    emsSensors = emsSensors ?? sensors;
                }
                else if (item.Value is List<HVAC.IB_EnergyManagementSystemActuator> actuators)
                {
                    if (actuators.Count == 0) continue;
                    emsActuators = emsActuators ?? actuators;
                }
                else if (item.Value is List<HVAC.IB_EnergyManagementSystemInternalVariable> inVars)
                {
                    if (inVars.Count == 0) continue;
                    emsInVariables = emsInVariables ?? inVars;
                }
                else if (item.Value is RefObject refObj)
                {

                    if (paramSource is null)
                    {
                        paramSource = new List<string>();
                        paramSource.Add(refObj.OsString);
                        paramSource.AddRange(refObj.ChildrenString);

                    }
                }
                else
                {
                    throw new ArgumentException("params_ only accepts Ironbug_ObjParams or Ironbug_OutputParams!");
                }


            }

            IB_obj.SetRefObject(paramSource);
            IB_obj.SetFieldValues(inputP);
            IB_obj.AddOutputVariables(outputP);
            IB_obj.AddEMSActuators(emsActuators);
            IB_obj.AddEMSSensors(emsSensors);
            IB_obj.AddEMSInternalVariables(emsInVariables);

        }

        protected IEnumerable<IB_ModelObject> SetObjDupParamsTo(IB_ModelObject IB_obj)
        {
            var objs = new List<IB_ModelObject>();
            var num = 0;
            var paramInput = this.Params.Input.FirstOrDefault(_ => _.Name == "DuplicateNumber_");
            if (paramInput is Param_Integer intP) // check if is null
            {
                //user has no input value.
                if (paramInput.VolatileDataCount == 0)
                {
                    return new List<IB_ModelObject>() { IB_obj };
                }

                //user has an input value here, can be 0, 1, 2 or more 
                var branchIndex = Math.Min(this.RunCount, paramInput.VolatileData.PathCount);
                var numO = intP.VolatileData.get_Branch(branchIndex - 1)[0];
                if (numO is GH_Integer ghInt)
                {
                    num = Math.Max(ghInt.Value, 0);
                }


                objs = IB_obj.Duplicate(num, renewIDs: true);

                return objs;

            }
            else
            {
                //There is no duplicate input param, so just return the existing IB_object
                return new List<IB_ModelObject>() { IB_obj };
            }

            

        }

        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            if (this._hasDupParamInput)
            {
                Menu_AppendItem(menu, "Duplicate Number Input", DupParamClicked, true, _dupParmOn)
                    .ToolTipText = "Add or remove the DuplicateNumber input parameter.";
                Menu_AppendSeparator(menu);
            }

            base.AppendAdditionalComponentMenuItems(menu);
        }

        private void DupParamClicked(object sender, EventArgs e)
        {

            this._dupParmOn = !_dupParmOn;

            if (_dupParmOn)
            {
                this.AddDupParam();
            }
            else
            {
                this.RemoveDupParam();
            }


            this.Params.OnParametersChanged();
            //this.OnDisplayExpired(true);
            this.ExpireSolution(true);
        }
        private void RemoveDupParam()
        {
            var lastP = this.Params.Input.Last();
            if (lastP.Name == "DuplicateNumber_")
            {
                Params.UnregisterInputParameter(lastP);
            }
        }

        private void AddDupParam()
        {

            //this.RecordUndoEvent("AddDupParam", new GH_ExpressionUndoAction(this,""));
            //Add new Param
            IGH_Param newParam = new Param_Integer();

            newParam.Name = "DuplicateNumber_";
            newParam.NickName = "n_";
            newParam.Description = $"Number of duplicates";
            newParam.MutableNickName = false;
            newParam.Access = GH_ParamAccess.list;
            newParam.Optional = true;
            newParam.WireDisplay = GH_ParamWireDisplay.faint;
            Params.RegisterInputParam(newParam);

        }
        public override bool Read(GH_IReader reader)
        {
            if (reader.ItemExists("_dupNum"))
            {
                _dupParmOn = reader.GetBoolean("_dupNum");
            }
            if (_dupParmOn)
            {
                AddDupParam();
            }
            return base.Read(reader);
        }
        public override bool Write(GH_IWriter writer)
        {
            writer.SetBoolean("_dupNum", _dupParmOn);
            return base.Write(writer);
        }

    }

    public abstract class Ironbug_HVACWithParamComponent : Ironbug_HVACComponent
    {
      
        public Ironbug_HVACWithParamComponent(string name, string nickname, string description, string category, string subCategory, Type DataFieldType) 
            :base(name, nickname, description, category, subCategory,DataFieldType, 
                 hasParam:true)
        {
        }
        
    }
    public abstract class Ironbug_DuplicableHVACComponent : Ironbug_HVACComponent
    {
        public Ironbug_DuplicableHVACComponent(string name, string nickname, string description, string category, string subCategory, Type DataFieldType)
            : base(name, nickname, description, category, subCategory, DataFieldType,
                  hasParam: false,
                  hasDup: true)
        {
        }
        public override void CreateAttributes()
        {
            m_attributes = new IB_DuplicableComponentAttributes(this);
        }

    }

    public abstract class Ironbug_DuplicableHVACWithParamComponent : Ironbug_HVACComponent
    {
        public Ironbug_DuplicableHVACWithParamComponent(string name, string nickname, string description, string category, string subCategory, Type DataFieldType)
            : base(name, nickname, description, category, subCategory, DataFieldType, 
                  hasParam: true, 
                  hasDup:true)
        {
        }
        public override void CreateAttributes()
        {
            m_attributes = new IB_DuplicableComponentAttributes(this);
        }
    }

}
