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
   
    public abstract class Ironbug_DuplicatableHVACComponent : Ironbug_HVACComponent
    {
        public Ironbug_DuplicatableHVACComponent(string name, string nickname, string description, string category, string subCategory, Type DataFieldType)
            : base(name, nickname, description, category, subCategory, DataFieldType)
        {
            
        }

        protected bool HasDupParam { get=>_dupParm; }

        private bool _dupParm = false;
        protected IEnumerable<IB_ModelObject> SetObjDupParamsTo(IB_ModelObject IB_obj)
        {
            var objs = new List<IB_ModelObject>();
            var num = 1;
            var paramInput = this.Params.Input.FirstOrDefault(_ => _.Name == "DuplicateNumber_");
            if (paramInput is Param_Integer intP) // check if is null
            {
                if (paramInput.VolatileDataCount > 0)
                {
                    var branchIndex = Math.Min(this.RunCount, paramInput.VolatileData.PathCount);
                    var numO = intP.VolatileData.get_Branch(branchIndex - 1)[0];
                    if (numO is GH_Integer ghInt)
                    {
                        num = Math.Max(ghInt.Value, 1);
                    }
                }

            }

            if (num == 1)
            {
                return new List<IB_ModelObject>() { IB_obj };
            }
            else
            {

                for (int i = 0; i < num; i++)
                {
                    var newobj = IB_obj.Duplicate();
                    newobj.SetTrackingID();
                    objs.Add(newobj);
                }

                return objs;
            }

        }

        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            Menu_AppendItem(menu, "Duplicate Number Input", DupParamClicked, true, _dupParm)
                .ToolTipText = "Add or remove the DuplicateNumber input parameter.";
            Menu_AppendSeparator(menu);
            
            base.AppendAdditionalComponentMenuItems(menu);
        }
        private void DupParamClicked(object sender, EventArgs e)
        {

            this._dupParm = !_dupParm;

            if (_dupParm)
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
                _dupParm = reader.GetBoolean("_dupNum");
            }

            if (_dupParm)
            {
                AddDupParam();
            }
            return base.Read(reader);
        }
        public override bool Write(GH_IWriter writer)
        {
            writer.SetBoolean("_dupNum", _dupParm);
            return base.Write(writer);
        }

        public override void CreateAttributes()
        {
            var newAttri = new IB_DuplicatableComponentAttributes(this);
            m_attributes = newAttri;
        }

        
    }
    
}
