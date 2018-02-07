using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilHeatingWater : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_CoilHeatingWater class.
        /// </summary>
        public Ironbug_CoilHeatingWater()
          : base("Ironbug_CoilHeatingWater", "Nickname",
              "Description",
              "Ironbug", "01:LoopComponents")
        {
            
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Hot water supply", "supply", "hot water supply source from hot water plant loop.", GH_ParamAccess.item);
            pManager[0].Optional = true;
            AddParams();
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilHeatingWater", "CoilHW", "connect to airloop's supply side", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var coil = new HVAC.IB_CoilHeatingWater();
            
            CollectSettingData(ref coil);

            DA.SetData(0, coil);
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
            get { return new Guid("4f849460-bb38-441c-9387-95c5be5830e7"); }
        }
        
        public void AddParams()
        {
            var settingList = HVAC.IB_CoilHeatingWater_Attributes.GetList();
            
            foreach (var item in settingList)
            {
                IGH_Param newParam = new Param_GenericObject();
                newParam.Name = item.FullName;
                newParam.NickName = item.ShortName;
                newParam.Access = GH_ParamAccess.item;
                newParam.Optional = true;
                Params.RegisterInputParam(newParam);
                
            }

            //this.ExpireSolution(true);


        }
        

        private void CollectSettingData(ref HVAC.IB_CoilHeatingWater Coil)
        {

            var FlyResults = new List<string>();
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
                        var dataField = HVAC.IB_CoilHeatingWater_Attributes.GetAttributeByName(name);

                        object value = null;
                        if (dataField.Type == typeof(double))
                        {
                            value = ((GH_Number)values.First()).Value;
                        }
                        else
                        {
                            value = ((GH_String)values.First()).Value;
                        }

                        try
                        {
                            Coil.SetAttribute(dataField, value);
                        }
                        catch (Exception)
                        {

                            throw;
                        }

                    }
                }




            }
                
        }



    }


    public static class GH_InputParamManager_Ext
    {
        public static void AddParamsByAttr(this GH_Component paramManager)
        {

        }
    }
}