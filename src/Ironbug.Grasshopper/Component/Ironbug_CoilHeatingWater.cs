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
        private Ironbug_ObjParams SettingParams { get; set; }
        public readonly Type DataFieldType = typeof(HVAC.IB_CoilHeatingWater_DataField);

        /// <summary>
        /// Initializes a new instance of the Ironbug_CoilHeatingWater class.
        /// </summary>
        public Ironbug_CoilHeatingWater()
          : base("Ironbug_CoilHeatingWater", "Nickname",
              "Description",
              "Ironbug", "01:LoopComponents")
        {
            Params.ParameterSourcesChanged += Params_ParameterSourcesChanged;
        }

        private void Params_ParameterSourcesChanged(object sender, GH_ParamServerEventArgs e)
        {
            if (e.ParameterSide == GH_ParameterSide.Output || e.ParameterIndex != 1)
            {
                return;
            }
            
            var source = e.Parameter.Sources;
            var recipientNum = source.Count;
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
            if (recipientNum == 1 && firstsSource != null)
            {
                this.SettingParams = (Ironbug_ObjParams)firstsSource.Attributes.GetTopLevel.DocObject;
                if (this.SettingParams!=null)
                {
                    this.SettingParams.CheckRecipients();
                }
                
                
            }
            else if (recipientNum == 0)
            {
                

            }

        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Hot water supply", "supply", "hot water supply source from hot water plant loop.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Parameters for Coil:Heating:Water", "params", "Detail settings for this Coil. Use Ironbug_ObjParams to set this.", GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager[1].Optional = true;
            //AddParams();
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
            var obj = new HVAC.IB_CoilHeatingWater();
            
            //CollectSettingData(ref coil);

            var settingParams = new Dictionary<HVAC.IB_DataField, object>();
            DA.GetData(1, ref settingParams);

            foreach (var item in settingParams)
            {
                try
                {
                    obj.SetAttribute(item.Key, item.Value);
                }
                catch (Exception)
                {

                    throw;
                }
            }


            DA.SetData(0, obj);
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



        //private void CollectSettingData(ref HVAC.IB_CoilHeatingWater Coil)
        //{
            
        //    foreach (var item in allInputParams)
        //    {
        //        if (item.SourceCount <= 0 || item.VolatileData.IsEmpty)
        //        {
        //            continue;
        //        }
        //        else
        //        {
        //            var values = new List<IGH_Goo>();
        //            values = item.VolatileData.AllData(true).ToList();

        //            if (!((values.First() == null) || String.IsNullOrWhiteSpace(values.First().ToString())))
        //            {
        //                var name = item.Name;
        //                var dataField = HVAC.IB_CoilHeatingWater_Attributes.GetAttributeByName(name);

        //                object value = null;
        //                if (dataField.Type == typeof(double))
        //                {
        //                    value = ((GH_Number)values.First()).Value;
        //                }
        //                else
        //                {
        //                    value = ((GH_String)values.First()).Value;
        //                }

        //                try
        //                {
        //                    Coil.SetAttribute(dataField, value);
        //                }
        //                catch (Exception)
        //                {

        //                    throw;
        //                }

        //            }
        //        }




        //    }

        //}




    }
    
}