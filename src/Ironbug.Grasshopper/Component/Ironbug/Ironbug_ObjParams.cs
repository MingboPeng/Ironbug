using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Ironbug.HVAC;
using Ironbug.Core;
using Rhino.Geometry;
using Ironbug.HVAC.BaseClass;
using GH_IO.Serialization;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ObjParams : GH_Component, IGH_VariableParameterComponent
    {
        //public Type lastDataFieldType { get; set; }
        private Type CurrentDataFieldType { get; set; }
        private List<IGH_Param> paramSet { get; set; }
        public Dictionary<IGH_DocumentObject,Type> DataFieldTypes { get; set; }

        private bool IsProSetting { get; set; } = false;

        private ICollection<IB_DataField> ProDataFieldList { get; set; }

        private IB_DataFieldSet DataFieldSet { get; set; }

        /// <summary>
        /// Initializes a new instance of the Ironbug_DataFields class.
        /// </summary>
        public Ironbug_ObjParams()
          : base("Ironbug_DataFields", "Nickname",
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

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var settingDatas = new Dictionary<IB_DataField, object>();
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
            Menu_AppendItem(menu, "ProSetting", ProSetting, true, this.IsProSetting);
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
                this.CurrentDataFieldType = typeof(IB_DataFieldSet).Assembly.GetType(typeName);
                this.DataFieldSet = GetDataFieldSet(CurrentDataFieldType);

            }
            return base.Read(reader);
        }

        public void CheckRecipients()
        {
            var outputs = this.Params.Output;
            if (!outputs.Any())
            {
                return;
            }
            var rec = outputs.Last().Recipients;
            if (rec.Count > 0)
            {
                this.DataFieldTypes = new Dictionary<IGH_DocumentObject, Type>();
                foreach (var item in rec)
                {
                    var targetHVACComponent = (Ironbug_HVACComponent)item.Attributes.GetTopLevel.DocObject;
                    
                    var dataFieldType = targetHVACComponent.DataFieldType;
                    this.DataFieldTypes.Add(targetHVACComponent, dataFieldType);
                }

                if (this.DataFieldTypes.Count > 1)
                {
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Different HVAC component has different setting parameters. You should use a new Ironbug_ObjParams for new HVAC component!");
                }

                var typeTobeShown = this.DataFieldTypes.First().Value;
                if (typeTobeShown != this.CurrentDataFieldType)
                {
                    AddParams(typeTobeShown);
                }
                
            }
        }

        private static IB_DataFieldSet GetDataFieldSet(Type type)
        {
            return Convert.ChangeType(Activator.CreateInstance(type, true), type) as IB_DataFieldSet;
        }

        

        public void AddParams(Type type)
        {
           
            this.CurrentDataFieldType = type;

            //remove all
            var inputParams = this.Params.Input;
            int paramCount = inputParams.Count;
            for (int i = 0; i < paramCount; i++)
            {
                this.Params.UnregisterInputParameter(inputParams[0]);
            }

            this.DataFieldSet = GetDataFieldSet(type);
            var dataFieldList = this.DataFieldSet.GetCustomizedDataFields();
            //including ProDataField and MasterDataField
            this.ProDataFieldList = dataFieldList.Where(_ => !(_ is IB_BasicDataField)).ToList();
            this.ProDataFieldList.Add(this.DataFieldSet.TheMasterDataField);

            //only show the basic setting first
            var dataFieldTobeAdded = dataFieldList.Where(_ => _ is IB_BasicDataField);
            if (!dataFieldTobeAdded.Any() || this.IsProSetting ==true)
            {
                dataFieldTobeAdded = dataFieldList;
            }

            var paramSet = new List<IGH_Param>();
            foreach (var item in dataFieldTobeAdded)
            {
                //TDDO: need to revisit this!!
                var description = item.Description;
                var iddObj = this.DataFieldSet.FirstOrDefault(_ => _.PerfectName == item.PerfectName);
                if (iddObj != null)
                {
                    description += iddObj.Description;
                }

                IGH_Param newParam = new Param_GenericObject();
                newParam.Name = item.FullName;
                newParam.NickName = item.ShortName;
                newParam.MutableNickName = false;
                newParam.Description = description;
                    
                newParam.Access = GH_ParamAccess.item;
                newParam.Optional = true;

                paramSet.Add(newParam);
                Params.RegisterInputParam(newParam);

            }

            this.paramSet = paramSet;

            this.Params.OnParametersChanged();
            this.ExpireSolution(true);
            
        }

        private Dictionary<IB_DataField, object> CollectSettingData()
        {
            if (CurrentDataFieldType ==null)
            {
                return null;
            }
            var dataFieldSet = this.DataFieldSet;
            var settingDatas = new Dictionary<IB_DataField,object>();

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
                        
                        if (dataField is IB_MasterDataField masterDataField)
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
                            //TODO: type of int??
                            if (dataField.DataType == typeof(double))
                            {
                                value = ((GH_Number)fristData).Value;
                            }
                            else
                            {
                                value = ((GH_String)fristData).Value;
                            }

                            settingDatas.TryAdd(dataField, value);
                        }

                        

                    }
                }




            }

            return settingDatas;

        }
        
        private void ProSetting(object sender, EventArgs e)
        {
            this.IsProSetting = !this.IsProSetting;
            

            if (this.IsProSetting)
            {

                this.AddProDataFields(this.ProDataFieldList);

            }
            else
            {
                this.RemoveProDataFields(this.ProDataFieldList);
            }
            //VariableParameterMaintenance();
            this.Params.OnParametersChanged();
            this.ExpireSolution(true);

        }


        private void AddProDataFields(IEnumerable<IB_DataField> DataFieldTobeAdded)
        {
            foreach (var item in DataFieldTobeAdded)
            {
                var description = item.DetailedDescription;
                description += item.Description;
                
                IGH_Param newParam = new Param_GenericObject();
                newParam.Name = item.FullName;
                newParam.NickName = item.ShortName;
                newParam.Description = description;
                newParam.MutableNickName = false;
                newParam.Access = GH_ParamAccess.item;
                newParam.Optional = true;
                Params.RegisterInputParam(newParam);

            }
        }


        private void RemoveProDataFields(IEnumerable<IB_DataField> DataFieldTobeAdded)
        {
            var inputParams = this.Params.Input;
            int paramTobeRemovedCount = DataFieldTobeAdded.Count();
            int operationIndex = inputParams.Count - paramTobeRemovedCount;
            for (int i = 0; i < paramTobeRemovedCount; i++)
            {
                this.Params.UnregisterInputParameter(inputParams[operationIndex]);
            }
            
        }

        public bool CanInsertParameter(GH_ParameterSide side, int index)
        {
            return false;
        }

        public bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            return false;
        }

        public IGH_Param CreateParameter(GH_ParameterSide side, int index)
        {
            throw new NotImplementedException();
        }

        public bool DestroyParameter(GH_ParameterSide side, int index)
        {
            throw new NotImplementedException();
        }

        public void VariableParameterMaintenance()
        {
            //throw new NotImplementedException();
        }
    }
}