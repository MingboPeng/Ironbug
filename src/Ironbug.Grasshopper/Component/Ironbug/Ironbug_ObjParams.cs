using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Ironbug.Core;
using Ironbug.HVAC.BaseClass;
using GH_IO.Serialization;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ObjParams : GH_Component, IGH_VariableParameterComponent
    {
        //public Type lastDataFieldType { get; set; }
        private Type CurrentDataFieldType { get; set; }
        //private List<IGH_Param> paramSet { get; set; }
        private Dictionary<IGH_DocumentObject,Type> DataFieldTypes { get; set; }

        private bool IsProSetting { get; set; } = false;
        private bool CanUseProSetting { get; set; } = true;
        private bool IsMasterSetting { get; set; } = false;

        private ICollection<IB_Field> profieldList { get; set; }

        private ICollection<IB_Field> masterFieldList { get; set; }

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
        
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("HVACObjParams", "ObjParams", "HVACObjParams", GH_ParamAccess.item);
        }

        protected override void BeforeSolveInstance()
        {
            var num = this.Params.Output.Count;
            base.BeforeSolveInstance(); 
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var settingDatas = new Dictionary<IB_Field, object>();
            settingDatas = CollectSettingData();
            DA.SetData(0, settingDatas);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Properties.Resources.ObjParams;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("c01b9512-5d83-4f5c-9116-ce897b94b2f2"); }
        }

        

        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            Menu_AppendItem(menu, "ProSettings", ProSetting, this.CanUseProSetting, this.IsProSetting);
            Menu_AppendItem(menu, "MasterSettings", MasterSetting, true, this.IsMasterSetting);
            Menu_AppendItem(menu, "RemoveUnused", RemoveUnused, true);
            Menu_AppendSeparator(menu);
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
            if (reader.ItemExists("DataFieldSetType"))
            {
                var typeName = reader.GetString("DataFieldSetType");
                this.CurrentDataFieldType = typeof(IB_FieldSet).Assembly.GetType(typeName);
                this.FieldSet = GetFieldSet(CurrentDataFieldType);

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
                    var targetHVACComponent = (Ironbug_HVACComponentBase)item.Attributes.GetTopLevel.DocObject;
                    
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
                    this.IsProSetting = false;
                    this.IsMasterSetting = false;
                    AddParams(typeTobeShown);
                }
                
            }
        }

        private static IB_FieldSet GetFieldSet(Type type)
        {
            return Convert.ChangeType(Activator.CreateInstance(type, true), type) as IB_FieldSet;
        }

        

        private void AddParams(Type type)
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
            //including ProField and MasterField
            //MasterField is ProField
            this.profieldList = fieldList.Where(_ => (_ is IB_ProField)).ToList();

            this.masterFieldList = fieldList.Where(_ => !((_ is IB_ProField)||(_ is IB_BasicField) || (_ is IB_TopField))).ToList();

            //this.ProDataFieldList.Add(this.FieldSet.TheMasterDataField);

            //only show the basic setting first
            var fieldTobeAdded = fieldList.Where(_ => _ is IB_BasicField);
            if (!fieldTobeAdded.Any())
            {
                fieldTobeAdded = profieldList;
            }
            if (!fieldTobeAdded.Any())
            {
                this.CanUseProSetting = false;
                this.IsMasterSetting = true;
                fieldTobeAdded = masterFieldList;
            }


            this.AddFieldsToParams(fieldTobeAdded);

            this.Params.OnParametersChanged();
            this.OnAttributesChanged();

        }

        private Dictionary<IB_Field, object> CollectSettingData()
        {
            if (CurrentDataFieldType ==null)
            {
                return null;
            }
            var dataFieldSet = this.FieldSet;
            var settingDatas = new Dictionary<IB_Field,object>();

            var allInputParams = this.Params.Input;
            foreach (var item in allInputParams)
            {
                if (item.SourceCount <= 0 || item.VolatileData.IsEmpty)
                {
                    continue;
                }
                else
                {
                    var fristData = item.VolatileData.AllData(true).ToList().First();

                    if (!((fristData == null) || String.IsNullOrWhiteSpace(fristData.ToString())))
                    {
                        
                        var dataField = dataFieldSet.FirstOrDefault(_ => _.FULLNAME == item.Name.ToUpper());
                        
                        //TODO: will remove master field at some point
                        if (dataField is IB_MasterField masterDataField)
                        {
                            var userInputs = item.VolatileData.AllData(true).Select(_ => ((GH_String)_).Value);
                            var masterDic = masterDataField.CheckUserInputs(userInputs, dataFieldSet);

                            foreach (var masterItem in masterDic)
                            {
                                settingDatas.TryAdd(masterItem.Key, masterItem.Value);
                            }

                        }
                        else //IB_BasicDataField or IB_ProDataField
                        {
                            object value = null;
                            fristData.CastTo(out value);
                            
                            settingDatas.TryAdd(dataField, value);
                        }

                        

                    }
                }




            }

            return settingDatas;

        }
        
        private void ProSetting(object sender, EventArgs e)
        {
            if (this.profieldList == null) return;

            this.IsProSetting = !this.IsProSetting;
            

            if (this.IsProSetting)
            {

                this.AddFieldsToParams(this.profieldList);

            }
            else
            {
                this.RemoveFields(this.profieldList);
            }
            //VariableParameterMaintenance();
            this.Params.OnParametersChanged();
            this.ExpireSolution(true);

        }

        private void MasterSetting(object sender, EventArgs e)
        {
            if (this.masterFieldList == null) return;

            this.IsMasterSetting = !this.IsMasterSetting;
            
            if (this.IsMasterSetting)
            {
                this.AddFieldsToParams(this.masterFieldList);
            }
            else
            {
                this.RemoveFields(this.masterFieldList);
            }
            //VariableParameterMaintenance();
            this.Params.OnParametersChanged();
            this.ExpireSolution(true);
        }

        private List<IGH_Param> AddFieldsToParams(IEnumerable<IB_Field> fieldTobeAdded)
        {
            var paramList = new List<IGH_Param>();

            foreach (var field in fieldTobeAdded)
            {
                //Don't add those already exist
                var paramFound = this.Params.Input.FirstOrDefault(_ => _.Name.ToUpper() == field.FULLNAME);
                if (paramFound != null) continue;
                //Add new Param
                IGH_Param newParam = new Param_GenericObject();
                if (field.DataType == typeof(string)) newParam = new Param_String();
                if (field.DataType == typeof(double)) newParam = new Param_Number();
                if (field.DataType == typeof(bool)) newParam = new Param_Boolean();

                newParam.Name = field.FullName;
                newParam.NickName = field.NickName;
                newParam.Description = $"{field.DetailedDescription}\r\n{field.Description}";
                newParam.MutableNickName = false;
                newParam.Access = GH_ParamAccess.item;
                newParam.Optional = true;

                paramList.Add(newParam);
                Params.RegisterInputParam(newParam);
            }
            return paramList;
            
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

            foreach (var item in tobeRemoved)
            {

                this.Params.UnregisterInputParameter(item);
                this.IsMasterSetting = false;
                this.IsProSetting = false;
            }
            this.Params.OnParametersChanged();
            this.ExpireSolution(true);
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
    }
}