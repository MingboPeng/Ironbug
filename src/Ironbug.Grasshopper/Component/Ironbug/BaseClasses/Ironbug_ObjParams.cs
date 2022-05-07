﻿using GH_IO.Serialization;
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

        //private bool IsIPUnit = false;

        
        /// Initializes a new instance of the Ironbug_FieldSet class.
        
        public Ironbug_ObjParams()
          : base("IB_ObjParams", "ObjParams",
              "Description",
              "Ironbug", "00:Ironbug")
        {
            this.MutableNickName = false;
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            //pManager.AddGenericParameter("////", "////", "All inputs vary based on the connected HVAC component", GH_ParamAccess.item);
            //pManager[0].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("HVACObjParams", "P", "HVACObjParams", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            ChangeNameWithUnitSystem();
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

                IGH_Goo ghObj = null;
                if (DA.GetData(i, ref ghObj))
                {
                    ghObj.CastTo(out object value) ;
                    //ghObj.CastTo(out double aa);
                   
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

                  

                    if (IB_ModelObject.IPUnit && !string.IsNullOrEmpty( dataField.UnitSI))
                    {
                        value = dataField.ConvertToSI((double)value);
                    }

                    settingDatas.TryAdd(dataField, value);
                }
            }
            DA.SetData(0, settingDatas);
           
    
        }

        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            
            //menu.Items.RemoveAt(1); // remove Preview
            //menu.Items.RemoveAt(2); // remove Bake

            var t = new ToolStripMenuItem("Parameters");
            var allParams = this.basicfieldList.ToList();
            allParams.AddRange(this.masterFieldList);
            if (!allParams.Any()) return;

            var inputParams = this.Params.Input.Select(_ => _.Name).ToList();
            var sortedParams = allParams.OrderBy(_=>_.FullName);
            foreach (var item in sortedParams)
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

            base.AppendAdditionalComponentMenuItems(menu);
        }

        private void ChangeUnitSystem(object sender, EventArgs e)
        {
            IB_ModelObject.IPUnit = !IB_ModelObject.IPUnit;

            this.ExpireSolution(true);
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
            //writer.SetBoolean("IsIPUnit", this.IsIPUnit);
            return base.Write(writer);
        }

        public override bool Read(GH_IReader reader)
        {
            if (reader.ItemExists("DataFieldSetType") )
            {
                var typeName = reader.GetString("DataFieldSetType");
                this.CurrentDataFieldType = typeof(IB_FieldSet).Assembly.GetType(typeName);
                if (this.CurrentDataFieldType == null)
                {
                    var typeNames = typeName.Split('_');
                    if (typeNames.Last() != "FieldSet")
                    {
                        var newTypeName = string.Join("_", typeNames.Take(2))+"_FieldSet";
                        this.CurrentDataFieldType = typeof(IB_FieldSet).Assembly.GetType(newTypeName);
                    }
                    
                }
                this.FieldSet = GetFieldSet(CurrentDataFieldType);
                this.basicfieldList = FieldSet.Where(_ => _ is IB_BasicField).ToList();
                this.masterFieldList = FieldSet.Where(_ => !((_ is IB_BasicField) || (_ is IB_TopField))).ToList();
            }

            //if (reader.ItemExists("IsIPUnit"))
            //{
            //    this.IsIPUnit = reader.GetBoolean("IsIPUnit");
            //}
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

        private void ChangeNameWithUnitSystem()
        {
            if (IB_ModelObject.IPUnit)
            {
                this.Name = "IB_ObjParams [IP]";
                this.NickName = "ObjParams [IP]";
            }
            else
            {
                this.Name = "IB_ObjParams";
                this.NickName = "ObjParams";
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
            var outp = this.Params.Output.Last();
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
            if (field.DataType == typeof(int)) newParam = new Param_Integer();
            if (field.DataType == typeof(string)) newParam = new Param_String();
            if (field.DataType == typeof(double)) newParam = new Param_Number();
            if (field.DataType == typeof(bool)) newParam = new Param_Boolean();
            

            newParam.Name = field.FullName;
            newParam.NickName = field.NickName;
            var description = string.Join(Environment.NewLine,new string[] { field.DetailedDescription, field.Description });
            newParam.Description = $"Data type: {field.DataType.Name}\n\n{description}";
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
            if (side == GH_ParameterSide.Input)
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