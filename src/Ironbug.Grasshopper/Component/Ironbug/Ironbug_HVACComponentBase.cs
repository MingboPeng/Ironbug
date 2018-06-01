using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;
using Grasshopper.Kernel.Parameters;
using Ironbug.HVAC.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.Grasshopper.Component
{
    public abstract class Ironbug_HVACComponentBase : GH_Component
    {
        private Ironbug_ObjParams settingParams { get; set; }
        public Type DataFieldType { get; private set; }
        

        private void Params_ParameterSourcesChanged(object sender, GH_ParamServerEventArgs e)
        {
            if (this.RunCount < 1)  return;

            //e.ParameterSide == GH_ParameterSide.Output // would this will ever happen??
            if (e.ParameterSide == GH_ParameterSide.Input && 
                e.Parameter.NickName == "params_")
            {
                ParamSettingChanged();
            }

            void ParamSettingChanged()
            {
                var source = e.Parameter.Sources;
                var sourceNum = source.Count;
                //removal case
                if (!source.Any())
                {
                    //settingParams?.CheckRecipients(); //This is a clean version
                    if (settingParams != null)
                    {
                        //remove all inputParams
                        settingParams.CheckRecipients();
                    }

                    settingParams = null;

                    return;
                }

                //adding case
                var firstsSource = source.First() as IGH_Param;
                if (sourceNum == 1 && firstsSource != null)
                {
                    //link to a new ObjParams
                    settingParams = (Ironbug_ObjParams)firstsSource.Attributes.GetTopLevel.DocObject;
                    if (settingParams != null)
                    {
                        settingParams.CheckRecipients();
                    }

                }
            }
            

        }

        public string PuppetableStateMsg { get; set; } 
        protected void PuppetStateChanged(object sender, PuppetEventArg e)
        {
            if (e.State is IB_PuppetableState_Host state)
            {
                this.PuppetableStateMsg = state.ToString();
            }
            else
            {
                this.PuppetableStateMsg = string.Empty;
            }
            this.TellPuppetReceivers();
            this.Attributes.ExpireLayout();
            this.Attributes.PerformLayout();
        }

        //loop branches and vrf system are puppet receivers
        private void TellPuppetReceivers()
        {
            var puppetReceivers = this.Params.Output.SelectMany(_ => _.Recipients).Where(CheckIfReceiver);
            foreach (var reciever in puppetReceivers)
            {
                reciever.ExpireSolution(true);
            }

            //local function
            bool CheckIfReceiver(IGH_Param gh_Param)
            {
                var owner = gh_Param.Attributes.GetTopLevel.DocObject;
                
                if (owner is Ironbug_AirLoopBranches || owner is Ironbug_PlantBranches || owner is Ironbug_AirConditionerVariableRefrigerantFlow)
                {
                    return true;
                }
                //in case of user uses any gh_param, instated of connect to puppet receiver directly.
                else if (owner is IGH_Param)
                {
                    return true;
                }
                else 
                {
                    return false;
                }
            }

        }

        protected override void BeforeSolveInstance()
        {
            this.PuppetableStateMsg = string.Empty;
            base.BeforeSolveInstance();
        }

        public Ironbug_HVACComponentBase(string name, string nickname, string description, string category, string subCategory, Type DataFieldType) 
            :base(name, nickname, description, category, subCategory)
        {
            this.DataFieldType = DataFieldType;
            var paramInput = CreateParamInput();
            Params.RegisterInputParam(paramInput);
            Params.ParameterSourcesChanged += Params_ParameterSourcesChanged;
        }
        
        private static IGH_Param CreateParamInput()
        {
            IGH_Param newParam = new Param_GenericObject();
            newParam.Name = "Parameters";
            newParam.NickName = "params_";
            newParam.Description = "Detail settings for this object. Use Ironbug_ObjParams to set this.";
            newParam.MutableNickName = false;
            newParam.Access = GH_ParamAccess.item;
            newParam.Optional = true;

            return newParam;
        }

        protected void SetObjParamsTo(IB_ModelObject IB_obj)
        {
            var paramInput = this.Params.Input.Last();
            var attrsDic = paramInput.VolatileData.AllData(true).ToList().FirstOrDefault();

            var attris = (Dictionary<IB_DataField, object>) null;
            attrsDic?.CastTo(out attris);

            if (attris is null) return;
            if (attris.Count == 0) return;

            IB_obj.SetAttributes(attris);
            
        }
        
        
        public override void CreateAttributes()
        {
            var newAttri = new IB_ComponentAttributes(this);
            m_attributes = newAttri;
            
        }

    }
    
}
