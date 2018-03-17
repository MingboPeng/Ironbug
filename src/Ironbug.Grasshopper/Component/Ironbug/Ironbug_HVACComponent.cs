using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.Grasshopper.Component
{
    public abstract class Ironbug_HVACComponent : GH_Component
    {
        private Ironbug_ObjParams settingParams { get; set; }
        public Type DataFieldType { get; private set; }

        //public void SetDataFieldType(Type DataFieldType)
        //{
        //    this.DataFieldType = DataFieldType;
        //}

        private void Params_ParameterSourcesChanged(object sender, GH_ParamServerEventArgs e)
        {
            if (this.RunCount < 1)  return;

            if (e.ParameterSide == GH_ParameterSide.Output || 
                e.Parameter.NickName != "params_")
            {
                return;
            }
            
            var source = e.Parameter.Sources;
            var sourceNum = source.Count;
            //removal case
            if (!source.Any())
            {
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
                settingParams = (Ironbug_ObjParams)firstsSource.Attributes.GetTopLevel.DocObject;
                if (settingParams != null)
                {
                    settingParams.CheckRecipients();
                }

            }

        }



        public Ironbug_HVACComponent(string name, string nickname, string description, string category, string subCategory, Type DataFieldType) 
            :base(name, nickname, description, category, subCategory)
        {
            this.DataFieldType = DataFieldType;
            Params.ParameterSourcesChanged += Params_ParameterSourcesChanged;
        }
        
    }
}
