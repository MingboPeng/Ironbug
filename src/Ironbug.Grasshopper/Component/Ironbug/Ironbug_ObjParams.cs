using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Ironbug.HVAC;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ObjParams : GH_Component
    {
        //public Type lastDataFieldType { get; set; }
        private Type CurrentDataFieldType { get; set; }
        public Dictionary<IGH_DocumentObject,Type> DataFieldTypes { get; set; }

        private bool IsProSetting { get; set; }
        private IEnumerable<HVAC.IB_DataField> dataFieldList { get; set; }

        /// <summary>
        /// Initializes a new instance of the Ironbug_DataFields class.
        /// </summary>
        public Ironbug_ObjParams()
          : base("Ironbug_DataFields", "Nickname",
              "Description",
              "Ironbug", "00:Ironbug")
        {
            this.IsProSetting = false;
            
            
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
            var settingDatas = new Dictionary<HVAC.IB_DataField, object>();
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
                return null;
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

                var typeTodeShown = this.DataFieldTypes.First().Value;
                if (typeTodeShown != this.CurrentDataFieldType)
                {
                    AddParams(typeTodeShown);
                }
                
            }
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

            
            var method = type.GetMethod("GetList");
            this.dataFieldList = (IEnumerable<HVAC.IB_DataField>)(method.Invoke(Activator.CreateInstance(type),null));

            //only show the basic setting first
            var dataFieldTobeAdded = dataFieldList.Where(_ => _.IsBasicSetting == true);
            if (!dataFieldTobeAdded.Any() || this.IsProSetting ==true)
            {
                dataFieldTobeAdded = dataFieldList;
            }

            foreach (var item in dataFieldTobeAdded)
            {
                IGH_Param newParam = new Param_GenericObject();
                newParam.Name = item.FullName;
                newParam.NickName = item.ShortName;
                newParam.MutableNickName = false;
                newParam.Description = item.Description;
                if (item.ValidData.Any())
                {
                    newParam.Description += "\n\nAcceptable values:" + string.Join(",", item.ValidData);
                }
                
                newParam.Access = GH_ParamAccess.item;
                newParam.Optional = true;
                Params.RegisterInputParam(newParam);

            }
            this.Params.OnParametersChanged();
            this.ExpireSolution(true);
            //this.ExpireSolution(true);
            
        }

        private Dictionary<HVAC.IB_DataField, object> CollectSettingData()
        {

            var settingDatas = new Dictionary<HVAC.IB_DataField,object>();

            var allInputParams = this.Params.Input;
            foreach (var item in allInputParams)
            {
                if (item.SourceCount <= 0 || item.VolatileData.IsEmpty)
                {
                    continue;
                }
                else
                {
                    var values = new List<IGH_Goo>();
                    values = item.VolatileData.AllData(true).ToList();

                    if (!((values.First() == null) || String.IsNullOrWhiteSpace(values.First().ToString())))
                    {
                        var name = item.Name;

                        object[] arg = new object[] { name };
                        var method = this.CurrentDataFieldType.GetMethod("GetAttributeByName");

                        var dataField = method.Invoke(this.CurrentDataFieldType, arg) as HVAC.IB_DataField;
                        
                        //((HVAC.IB_DataFieldSet).GetAttributeByName();

                        object value = null;
                        if (dataField.DataType == typeof(double))
                        {
                            value = ((GH_Number)values.First()).Value;
                        }
                        else
                        {
                            value = ((GH_String)values.First()).Value;
                        }
                        settingDatas.Add(dataField,value);
                        //try
                        //{

                        //    Coil.SetAttribute(dataField, value);
                        //}
                        //catch (Exception)
                        //{

                        //    throw;
                        //}

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

                this.AddProDataFields(this.dataFieldList.Where(_ => _.IsBasicSetting == false));

            }
            else
            {
                this.RemoveProDataFields(this.dataFieldList.Where(_ => _.IsBasicSetting == false));
            }
            //VariableParameterMaintenance();
            this.Params.OnParametersChanged();
            this.ExpireSolution(true);

        }


        private void AddProDataFields(IEnumerable<HVAC.IB_DataField> DataFieldTobeAdded)
        {
            foreach (var item in DataFieldTobeAdded)
            {
                IGH_Param newParam = new Param_GenericObject();
                newParam.Name = item.FullName;
                newParam.NickName = item.ShortName;
                newParam.Description = item.Description + " \n"+string.Join(",",item.ValidData);
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
    }
}