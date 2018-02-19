using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Ironbug.Grasshopper.Properties;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_FanConstantVolume : GH_Component
    {
        private Ironbug_ObjParams SettingParams { get; set; }
        public readonly Type DataFieldType = typeof(HVAC.IB_FanConstantVolume_DataFields);

        /// <summary>
        /// Initializes a new instance of the Ironbug_FanConstantVolume class.
        /// </summary>
        public Ironbug_FanConstantVolume()
          : base("Ironbug_FanConstantVolume", "Nickname",
              "Description",
              "Ironbug", "01:LoopComponents")
        {
            Params.ParameterSourcesChanged += Params_ParameterSourcesChanged;
        }

        private void Params_ParameterSourcesChanged(object sender, GH_ParamServerEventArgs e)
        {
            if (e.ParameterSide == GH_ParameterSide.Output || e.ParameterIndex != this.Params.Input.Count-1)
            {
                return;
            }

            var source = e.Parameter.Sources;
            var sourceNum = source.Count;
            if (!source.Any())
            {
                if (this.SettingParams!= null)
                {
                    this.SettingParams.CheckRecipients();
                }

                this.SettingParams = null;
                
                return;
            }

            var firstsSource = source.First() as IGH_Param;
            if (sourceNum == 1 && firstsSource != null)
            {
                this.SettingParams = (Ironbug_ObjParams)firstsSource.Attributes.GetTopLevel.DocObject;
                if (this.SettingParams != null)
                {
                    //this.SettingParams.AddParams(DataFieldType);
                    this.SettingParams.CheckRecipients();
                }

            }

        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Parameters for Fan:ConstantVolume", "params_", "Detail settings for this fan. Use Ironbug_ObjParams to set this.", GH_ParamAccess.item);
            pManager[0].Optional = true;
            //AddParams();
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("FanConstantVolume", "Fan", "connect to airloop's supply side", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_FanConstantVolume();

            var settingParams = new Dictionary<HVAC.IB_DataField, object>();
            DA.GetData(0, ref settingParams);

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
                return Resources.FanC;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("f517230e-27e9-4fd0-bfbc-31f0596d35c4"); }
        }

        
    }

   
}