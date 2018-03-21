using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ThermalZone : Ironbug_HVACComponent
    {
        //private Ironbug_ObjParams SettingParams { get; set; }
        ////this is used as a reference in Ironbug_ObjParams.
        //public readonly Type DataFieldType = typeof(HVAC.IB_ThermalZone_DataFieldSet); 

        /// <summary>
        /// Initializes a new instance of the Ironbug_ThermalZone class.
        /// </summary>
        public Ironbug_ThermalZone()
          : base("Ironbug_ThermalZone", "ThermalZone",
              "Description",
              "Ironbug", "01:LoopComponents",
              typeof(IB_ThermalZone_DataFieldSet))
        {
        }
        
        
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("HoneybeeZone", "HBZone", "HBZone", GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager.AddGenericParameter("AirTerminal", "AriTerminal_", "AriTerminal", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddGenericParameter("ZoneEquipments", "Equipments_", "ZoneEquipments", GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager.AddGenericParameter("SizingZone", "Sizing_", "Zone sizing", GH_ParamAccess.item);
            pManager[3].Optional = true;
            pManager.AddGenericParameter("SettingParam", "params_", "SettingParam", GH_ParamAccess.item);
            pManager[4].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("OpenStudio ThermalZone", "OSZone", "connect to airloop's demand side", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var zone = new IB_ThermalZone();


            IB_AirTerminal airTerminal = null;

            if (DA.GetData(1, ref airTerminal))
            {
                zone.SetAirTerminal(airTerminal);
            }
            else
            {
                
                zone.SetAirTerminal(new IB_AirTerminalSingleDuctUncontrolled());
            }

            var sizing = new IB_SizingZone();
            if (DA.GetData(3, ref sizing))
            {
                zone.SetSizingZone(sizing);
            }
            //TODO: add ZoneEquipments

            //Collect setting params
            var settingParams = new Dictionary<IB_DataField, object>();
            DA.GetData("SettingParam", ref settingParams);
            zone.SetAttributes(settingParams);

            DA.SetData(0, zone);
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
                return Resources.ThermalZone;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("8aa3ced0-54bb-4cc3-b53b-9b63dbe714a0"); }
        }
    }
}