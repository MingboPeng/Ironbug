using GH_IO.Serialization;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Ironbug.Core;
using Ironbug.HVAC.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ObjParams : Ironbug_Component, IGH_VariableParameterComponent
    {
        public override GH_Exposure Exposure => GH_Exposure.secondary;

        protected override System.Drawing.Bitmap Icon => Properties.Resources.ObjParams;

        public override Guid ComponentGuid => new Guid("c01b9512-5d83-4f5c-9116-ce897b94b2f2");
        
        private Type CurrentDataFieldType { get; set; }

        private Dictionary<IGH_DocumentObject, Type> DataFieldTypes { get; set; }

        private bool IsBasicSetting { get; set; } = true;
        private bool IsThereBasicSetting { get; set; } = true;
        private bool IsMasterSetting { get; set; } = false;

        private ICollection<IB_Field> basicfieldList { get; set; } = new List<IB_Field>();

        private ICollection<IB_Field> masterFieldList { get; set; } = new List<IB_Field>();

        private IB_FieldSet FieldSet { get; set; }

        /// <summary>
        /// Initializes a new instance of the Ironbug_DataFields class.
        /// </summary>
        public Ironbug_ObjParams()
          : base("Ironbug_ObjParams", "ObjParams",
              "Description",
              "Ironbug", "00:Ironbug")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            //pManager.AddGenericParameter("////", "////", "All inputs vary based on the connected HVAC component", GH_ParamAccess.item);
            //pManager[0].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("HVACObjParams", "params", "HVACObjParams", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (this.Params.Input.Any())
            {
                this.Message = "Double click to switch!";
            }
            else
            {
                this.Message = null;
            }

            var settingDatas = new Dictionary<IB_Field, object>();

            var inputCount = this.Params.Input.Count;
            for (int i = 0; i < inputCount; i++)
            {

                GH_ObjectWrapper ghObj = null;
                if (DA.GetData(i, ref ghObj))
                {
                    ghObj.CastTo(out object value);
                   
                    //value.CastTo(out value);
                    var fieldName = this.Params.Input[i].Name;
                    var dataField = this.FieldSet.FirstOrDefault(_ => _.FULLNAME == fieldName.ToUpper());
                
                    if (dataField.ValidData.Any() && (dataField.DataType != typeof(bool)))
                    {
                        var valueStr = value.ToString();
                        if (!dataField.ValidData.Contains(valueStr))
                        {
                            throw new ArgumentException($"Input \"{valueStr}\" is not a valid option, please double check the typo!");
                        }
                    }

                    settingDatas.TryAdd(dataField, value);
                }
            }

            if (settingDatas.Any())
            {
                DA.SetData(0, settingDatas);
            }
    
        }

        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {

            menu.Items.RemoveAt(1); // remove Preview
            menu.Items.RemoveAt(2); // remove Bake

            var t = new ToolStripMenuItem("Parameters");
            var allParams = this.basicfieldList.ToList();
            allParams.AddRange(this.masterFieldList);
            var inputParams = this.Params.Input.Select(_ => _.Name);
            foreach (var item in allParams)
            {
                var mitem = Menu_AppendItem(t.DropDown, item.FullName, OnClickParam, true, inputParams.Any(_ => _ == item.FullName));
                mitem.Tag = item;
            }
            menu.Items.Add(t);

            Menu_AppendSeparator(menu);

            Menu_AppendItem(menu, "BasicSettings", BasicSetting, this.IsThereBasicSetting, this.IsBasicSetting);
            Menu_AppendItem(menu, "AllSettings", MasterSetting, true, this.IsMasterSetting);
            Menu_AppendItem(menu, "RemoveUnused", RemoveUnused, true);
            Menu_AppendSeparator(menu);

        }

        private void OnClickParam(object sender, EventArgs e)
        {
            var clickedItem = sender as ToolStripMenuItem;
            if (clickedItem == null) return;

            var field = clickedItem.Tag as IB_Field;
            if (field == null) return;
            //var name = clickedItem.Text;
            if (!clickedItem.Checked)
            {
                AddFieldToParam(field);
            }
            else
            {
                RemoveParamFromField(field);
            }

            this.Params.OnParametersChanged();
            this.OnDisplayExpired(true);


        }

        public override bool Write(GH_IWriter writer)
        {
            if (this.CurrentDataFieldType != null)
            {
                writer.SetString("DataFieldSetType", this.CurrentDataFieldType.ToString());
            }

            return base.Write(writer);
        }

        public override bool Read(GH_IReader reader)
        {
            if (reader.ItemExists("DataFieldSetType") )
            {
                var typeName = reader.GetString("DataFieldSetType");
                this.CurrentDataFieldType = typeof(IB_FieldSet).Assembly.GetType(typeName);
                this.FieldSet = GetFieldSet(CurrentDataFieldType);
                this.basicfieldList = FieldSet.Where(_ => _ is IB_BasicField).ToList();
                this.masterFieldList = FieldSet.Where(_ => !((_ is IB_BasicField) || (_ is IB_TopField))).ToList();
            }
            return base.Read(reader);
        }

        public void CheckRecipients()
        {
            //var outputs = this.Params.Output;
            //if (!outputs.Any())
            //{
            //    return;
            //}
            var recipients = this.Params.Output.Last().Recipients;
            if (recipients.Count > 0)
            {
                //TODO: Do I need IGH_DocumentObject?
                this.DataFieldTypes = new Dictionary<IGH_DocumentObject, Type>();
                foreach (var item in recipients)
                {
                    var targetHVACComponent = (Ironbug_HVACComponent)item.Attributes.GetTopLevel.DocObject;

                    var dataFieldType = targetHVACComponent.DataFieldType;
                    this.DataFieldTypes.Add(targetHVACComponent, dataFieldType);
                }

                if (this.DataFieldTypes.Values.Distinct().Count() > 1)
                {
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Different HVAC component has different setting parameters. You should use a new Ironbug_ObjParams for new HVAC component!");
                }

                var typeTobeShown = this.DataFieldTypes.First().Value;
                if (typeTobeShown != this.CurrentDataFieldType)
                {
                    this.IsBasicSetting = false;
                    this.IsMasterSetting = false;
                    AddParamsByType(typeTobeShown);

                    this.Message = "Double click for more details!";
                }
            }
        }

        private static IB_FieldSet GetFieldSet(Type type)
        {
            return Convert.ChangeType(Activator.CreateInstance(type, true), type) as IB_FieldSet;
        }

        private void AddParamsByType(Type type)
        {
            this.CurrentDataFieldType = type;

            //remove all
            var inputParams = this.Params.Input;
            int paramCount = inputParams.Count;
            for (int i = 0; i < paramCount; i++)
            {
                this.Params.UnregisterInputParameter(inputParams[0]);
            }

            this.FieldSet = GetFieldSet(type);
            //var fieldList = this.FieldSet.GetSelfPreperties();
            var fieldList = this.FieldSet;

            this.basicfieldList = fieldList.Where(_ => _ is IB_BasicField).ToList();
            this.masterFieldList = fieldList.Where(_ => !((_ is IB_BasicField) || (_ is IB_TopField))).ToList();

            //only show the basic setting first
            var fieldTobeAdded = basicfieldList;
            this.IsBasicSetting = true;
            if (!fieldTobeAdded.Any())
            {
                this.IsThereBasicSetting = false;
                this.IsBasicSetting = false;
                this.IsMasterSetting = true;
                fieldTobeAdded = masterFieldList;
            }

            this.AddFieldsToParams(fieldTobeAdded);

            this.Params.OnParametersChanged();
            this.OnDisplayExpired(true);
        }

        
        private void BasicSetting(object sender, EventArgs e)
        {
            if (this.basicfieldList == null) return;

            this.IsBasicSetting = !this.IsBasicSetting;

            if (this.IsBasicSetting)
            {
                this.AddFieldsToParams(this.basicfieldList);
            }
            else
            {
                this.RemoveFields(this.basicfieldList);
            }
            //VariableParameterMaintenance();
            this.Params.OnParametersChanged();
            //this.ExpireSolution(true);

            this.OnDisplayExpired(true);
        }

        private void MasterSetting(object sender, EventArgs e)
        {
            if (this.masterFieldList == null) return;

            this.IsMasterSetting = !this.IsMasterSetting;

            if (this.IsMasterSetting)
            {
                this.IsBasicSetting = true;
                this.AddFieldsToParams(this.basicfieldList);
                this.AddFieldsToParams(this.masterFieldList);
            }
            else
            {
                this.RemoveFields(this.masterFieldList);
            }
            //VariableParameterMaintenance();
            this.Params.OnParametersChanged();
            //this.ExpireSolution(true);

            this.OnDisplayExpired(true);
        }

        private void AddFieldsToParams(IEnumerable<IB_Field> fieldTobeAdded)
        {
            foreach (var field in fieldTobeAdded)
            {
                AddFieldToParam(field);
            }
        }

        private void AddFieldToParam(IB_Field fieldTobeAdded)
        {
            var field = fieldTobeAdded;
            //Don't add those already exist
            var inputNames = this.Params.Input.Select(_ => _.Name.ToUpper()).ToList();
            if (inputNames.Contains(field.FULLNAME)) return;


            //Add new Param
            IGH_Param newParam = new Param_GenericObject();
            if (field.DataType == typeof(string)) newParam = new Param_String();
            if (field.DataType == typeof(double)) newParam = new Param_Number();
            if (field.DataType == typeof(bool)) newParam = new Param_Boolean();

            newParam.Name = field.FullName;
            newParam.NickName = field.NickName;
            newParam.Description = field.Description;
            newParam.MutableNickName = false;
            newParam.Access = GH_ParamAccess.item;
            newParam.Optional = true;
            
            inputNames.Add(field.FULLNAME);
            inputNames.Sort();
            var index = inputNames.IndexOf(field.FULLNAME);
            Params.RegisterInputParam(newParam, index);

            
        }

        private void RemoveParamFromField(IB_Field field)
        {
            var inputParam = this.Params.Input.FirstOrDefault(_ => _.Name == field.FullName);
            this.Params.UnregisterInputParameter(inputParam);
            this.IsMasterSetting = false;
            this.IsBasicSetting = false;

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
            //Do not remove all inputs if there is no connected input source.
            if (inputParams.Count == tobeRemoved.Count) {
                this.MasterSetting(this, EventArgs.Empty);
                return;
            }

            foreach (var item in tobeRemoved)
            {
                this.Params.UnregisterInputParameter(item);
                this.IsMasterSetting = false;
                this.IsBasicSetting = false;
            }
            this.Params.OnParametersChanged();
            this.OnDisplayExpired(true);
        }

        private void RemoveFields(IEnumerable<IB_Field> fieldsTobeRemoved)
        {
            var inputParams = this.Params.Input;
            var tobeRemoved = fieldsTobeRemoved;
            foreach (var item in tobeRemoved)
            {
                var paramTobeRemoved = inputParams.FirstOrDefault(_ => _.Name.ToUpper() == item.FULLNAME);
                if (paramTobeRemoved == null) continue;

                this.Params.UnregisterInputParameter(paramTobeRemoved);
            }
        }

        public bool CanInsertParameter(GH_ParameterSide side, int index)
        {
            return false;
        }

        public bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            if (this.IsMasterSetting && side == GH_ParameterSide.Input)
            {
                return true;
            }
            return false;
        }

        public IGH_Param CreateParameter(GH_ParameterSide side, int index)
        {
            throw new NotImplementedException();
        }

        public bool DestroyParameter(GH_ParameterSide side, int index)
        {
            return true;
        }

        public void VariableParameterMaintenance()
        {
            //throw new NotImplementedException();
        }

        public override void CreateAttributes()
        {
            var newAttri = new IB_SettingComponentAttributes(this);
            m_attributes = newAttri;
        }
        

        internal void RespondToMouseDoubleClick()
        {
            if (!IsMasterSetting)
            {
                this.MasterSetting(this, EventArgs.Empty);
            }
            else if (!IsBasicSetting)
            {
                this.BasicSetting(this, EventArgs.Empty);
            }
            else
            {
                this.RemoveUnused(this, EventArgs.Empty);
            }
            
           
        }

    }
}