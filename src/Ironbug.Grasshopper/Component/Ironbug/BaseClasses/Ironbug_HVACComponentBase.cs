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
    public abstract class Ironbug_HVACComponentBase : GH_Component
    {
        //private Ironbug_ObjParams settingParams { get; set; }
        public Type DataFieldType { get; private set; }
        //public IB_FieldSet DataField { get; private set; }

        public IB_ModelObject IB_ModelObject  => iB_ModelObject;
        private IB_ModelObject iB_ModelObject;

        private void Params_ParameterSourcesChanged(object sender, GH_ParamServerEventArgs e)
        {
            var pName = e.Parameter.NickName;
            var isParam = pName == "params_" || pName == "Parameters_";
            if (e.ParameterSide == GH_ParameterSide.Input && isParam )
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
                        //settingParams = objParams;
                    }
                    else if (docObj is Ironbug_OutputParams outputParams)
                    {
                        //outputParams.GetEPOutputVariables(this, EventArgs.Empty);
                    }
                    else
                    {
                        this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "params_ only accepts Ironbug_ObjParams or Ironbug_OutputParams!");
                    }
                }
            }
            

        }

        public string PuppetableStateMsg { get; set; } 
        //protected void PuppetStateChanged(object sender, PuppetEventArg e)
        //{
        //    if (e.State is IB_PuppetableState_Host state)
        //    {
        //        this.PuppetableStateMsg = state.ToString();
        //    }
        //    else
        //    {
        //        this.PuppetableStateMsg = string.Empty;
        //    }
        //    this.TellPuppetReceivers();
        //    this.Attributes.ExpireLayout();
        //    this.Attributes.PerformLayout();
        //}

        //loop branches and vrf system are puppet receivers
        //private void TellPuppetReceivers()
        //{
        //    var puppetReceivers = this.Params.Output.SelectMany(_ => _.Recipients).Where(CheckIfReceiver);
        //    foreach (var reciever in puppetReceivers)
        //    {
        //        var recompute = this.OnPingDocument().SolutionHistory.Count == 0;
        //        reciever.ExpireSolution(recompute);

        //    }

        //    //local function
        //    bool CheckIfReceiver(IGH_Param gh_Param)
        //    {
        //        var owner = gh_Param.Attributes.GetTopLevel.DocObject;
                
        //        if (owner is Ironbug_AirLoopBranches || 
        //            owner is Ironbug_PlantBranches || 
        //            owner is Ironbug_AirConditionerVariableRefrigerantFlow)
        //        {
        //            return true;
        //        }
        //        //in case of user uses any gh_param, instated of connect to puppet receiver directly.
        //        else if (owner is IGH_Param)
        //        {
        //            return true;
        //        }
        //        else 
        //        {
        //            return false;
        //        }
        //    }

        //}

        protected override void BeforeSolveInstance()
        {
            this.PuppetableStateMsg = string.Empty;
            base.BeforeSolveInstance();
        }

        protected override void AfterSolveInstance()
        {
            if (this.iB_ModelObject is null)
            {
                var data = this.Params.Output.Last().VolatileData.AllData(true).FirstOrDefault() as GH_ObjectWrapper;
                this.iB_ModelObject = data?.Value as IB_ModelObject;
            }
            
            base.AfterSolveInstance();  
        }
        
        private static string FindComDescription(string UsersDescription, Type DataFieldType)
        {
            return UsersDescription == "Description"? (Activator.CreateInstance(DataFieldType, true) as IB_FieldSet).OwnerEpNote : UsersDescription;
        }

        public Ironbug_HVACComponentBase(string name, string nickname, string description, string category, string subCategory, Type DataFieldType) 
            :base(name, nickname, FindComDescription(description, DataFieldType), category, subCategory)
        {
            this.DataFieldType = DataFieldType;
            var paramInput = CreateParamInput();
            Params.RegisterInputParam(paramInput);
            Params.ParameterSourcesChanged += Params_ParameterSourcesChanged;
        }
        
        private static IGH_Param CreateParamInput()
        {
            IGH_Param newParam = new Param_GenericObject();
            newParam.Name = "Parameters_";
            newParam.NickName = "params_";
            newParam.Description = "Detail settings for this HVAC object. Use Ironbug_ObjParams to set input parameters, or use Ironbug_OutputParams to set output variables.";
            newParam.MutableNickName = false;
            newParam.Access = GH_ParamAccess.list;
            newParam.Optional = true;

            return newParam;
        }

        protected void SetObjParamsTo(IB_ModelObject IB_obj)
        {
            var paramInput = this.Params.Input.Last();
            var objParams = paramInput.VolatileData.AllData(true).ToList();
            var inputP = (Dictionary<IB_Field, object>) null;
            var outputP = (List<IB_OutputVariable>)null;

            foreach (var ghitem in objParams)
            {
                var item = ghitem as GH_ObjectWrapper;
                

                if (item.Value is Dictionary<IB_Field, object> inputParams)
                {
                    if (inputParams.Count == 0) continue;
                    if (inputP is null)
                    {
                        inputP = inputParams;
                    }
                }
                else if(item.Value is List<IB_OutputVariable> outputParams)
                {
                    if (outputParams.Count == 0) continue;
                    if (outputP is null)
                    {
                        outputP = outputParams;
                    }

                }
                
                
            }
            

            IB_obj.SetFieldValues(inputP);
            IB_obj.AddOutputVariables(outputP);
            
        }


        public override void CreateAttributes()
        {
            var newAttri = new IB_ComponentAttributes(this);
            m_attributes = newAttri;
            
        }

        
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            
            Menu_AppendItem(menu, "IP-Unit", ChangeUnit, true , IB_ModelObject.IPUnit)
                .ToolTipText = "This will set all HVAC components with IP unit system";
            Menu_AppendSeparator(menu);
        }

        private void ChangeUnit(object sender, EventArgs e)
        {
            IB_ModelObject.IPUnit = !IB_ModelObject.IPUnit;
            //TODO: maybe need recompute all??
            //Only Panel
            //But is it necessary, the unit is only for representation
            this.ExpireSolution(true);
        }
    }
    
}
