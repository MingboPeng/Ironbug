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
    public abstract class Ironbug_LoopObjectComponent : Ironbug_HVACComponent
    {
        public Ironbug_LoopObjectComponent(string name, string nickname, string description, string category, string subCategory, Type DataFieldType)
            : base(name, nickname, description, category, subCategory, DataFieldType)
        {
        }

        protected IEnumerable<IB_ModelObject> SetObjDupParamsTo(IB_ModelObject IB_obj)
        {
            var objs = new List<IB_ModelObject>();
            var num = 1;
            var paramInput = this.Params.Input.FirstOrDefault(_ => _.Name == "DuplicateNumber_");
            if (paramInput is Param_Integer intP)
            {

                var numO = intP.VolatileData.AllData(true).FirstOrDefault();
                if (numO is GH_Integer ghInt)
                {
                    num = Math.Max(ghInt.Value, 1);
                }
            }

            for (int i = 0; i < num; i++)
            {
                var newobj = IB_obj.Duplicate();
                newobj.SetTrackingID();
                objs.Add(newobj);
            }

            return objs;
        }

        private bool _dupNum;
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            Menu_AppendItem(menu, "Duplicate Number", AddDupParam, true, this._dupNum)
                .ToolTipText = "This will set all HVAC components with IP unit system";
            Menu_AppendSeparator(menu);

            base.AppendAdditionalComponentMenuItems(menu);
        }
        private void AddDupParam(object sender, EventArgs e)
        {
            this._dupNum = !_dupNum;
            //Don't add if already exist
            var lastP = this.Params.Input.Last();
            if (lastP.Name == "DuplicateNumber_")
            {
                Params.UnregisterInputParameter(lastP);
            }
            else
            {
                //Add new Param
                IGH_Param newParam = new Param_Integer();

                newParam.Name = "DuplicateNumber_";
                newParam.NickName = "n_";
                newParam.Description = $"Number of duplicates";
                newParam.MutableNickName = false;
                newParam.Access = GH_ParamAccess.item;
                newParam.Optional = true;

                Params.RegisterInputParam(newParam);
            }


            this.Params.OnParametersChanged();
            this.OnDisplayExpired(true);
        }

        public override bool Read(GH_IReader reader)
        {
            if (reader.ItemExists("_dupNum"))
            {
                _dupNum = reader.GetBoolean("_dupNum");
            }

            return base.Read(reader);
        }
        public override bool Write(GH_IWriter writer)
        {
            writer.SetBoolean("_dupNum", _dupNum);
            return base.Write(writer);
        }

        public override void CreateAttributes()
        {
            var newAttri = new IB_DuplicatableComponentAttributes(this);
            m_attributes = newAttri;
        }
    }
    
}
