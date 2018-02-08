using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ObjParams : GH_Component
    {
        public Type ObjParamsType { get; set; }
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
            pManager.AddGenericParameter("HVACObj", "HVACObj", "HVACObj", GH_ParamAccess.item);
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


        public void AddParams(Type type)
        {
            //HVAC.IB_DataFieldSet
            if (this.ObjParamsType == type)
            {
                return;
            }
            this.ObjParamsType = type;
            var method = type.GetMethod("GetList");
            var dataFieldList = (IEnumerable<HVAC.IB_DataField>)(method.Invoke(Attributes, null));
            
            //remove all
            var inputParams = this.Params.Input;
            foreach (var item in inputParams)
            {
                this.Params.UnregisterInputParameter(item);
            }

            foreach (var item in dataFieldList)
            {
                IGH_Param newParam = new Param_GenericObject();
                newParam.Name = item.FullName;
                newParam.NickName = item.ShortName;
                newParam.MutableNickName = false;
                newParam.Access = GH_ParamAccess.item;
                newParam.Optional = true;
                Params.RegisterInputParam(newParam);

            }
            this.ExpireSolution(false);
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
                        var method = this.ObjParamsType.GetMethod("GetAttributeByName");

                        var dataField = method.Invoke(this.ObjParamsType, arg) as HVAC.IB_DataField;
                        
                        //((HVAC.IB_DataFieldSet).GetAttributeByName();

                        object value = null;
                        if (dataField.Type == typeof(double))
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

    }
}