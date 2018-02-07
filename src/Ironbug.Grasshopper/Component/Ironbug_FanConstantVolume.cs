using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_FanConstantVolume : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_FanConstantVolume class.
        /// </summary>
        public Ironbug_FanConstantVolume()
          : base("Ironbug_FanConstantVolume", "Nickname",
              "Description",
              "Ironbug", "01:LoopComponents")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            AddParams();
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

            CollectSettingData(ref obj);

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
            get { return new Guid("f517230e-27e9-4fd0-bfbc-31f0596d35c4"); }
        }


        public void AddParams()
        {
            var settingList = HVAC.IB_FanConstantVolume_Attributes.GetList();

            foreach (var item in settingList)
            {
                IGH_Param newParam = new Param_GenericObject();
                newParam.Name = item.FullName;
                newParam.NickName = item.ShortName;
                newParam.Access = GH_ParamAccess.item;
                newParam.Optional = true;
                Params.RegisterInputParam(newParam);

            }

        }


        private void CollectSettingData(ref HVAC.IB_FanConstantVolume Coil)
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
                        var dataField = HVAC.IB_FanConstantVolume_Attributes.GetAttributeByName(name);
                        
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
}