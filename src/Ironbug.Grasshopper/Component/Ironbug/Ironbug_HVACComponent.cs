using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.Grasshopper.Component
{
    public abstract class Ironbug_HVACComponent : GH_Component
    {
        private Ironbug_ObjParams SettingParams { get; set; }
        public Type DataFieldType { get; private set; }

        //public void SetDataFieldType(Type DataFieldType)
        //{
        //    this.DataFieldType = DataFieldType;
        //}

        private void Params_ParameterSourcesChanged(object sender, GH_ParamServerEventArgs e)
        {
            if (e.ParameterSide == GH_ParameterSide.Output || e.ParameterIndex != this.Params.Input.Count - 1)
            {
                return;
            }

            var source = e.Parameter.Sources;
            var sourceNum = source.Count;
            if (!source.Any())
            {
                if (this.SettingParams != null)
                {
                    this.SettingParams.CheckRecipients();
                }

                this.SettingParams = null;

                return;
            }

            var firstsSource = source.First() as IGH_Param;
            if (sourceNum == 1 && firstsSource != null)
            {
                this.SettingParams = (Ironbug_ObjParams)firstsSource.Attributes.GetTopLevel.DocObject;
                if (this.SettingParams != null)
                {
                    this.SettingParams.CheckRecipients();
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
