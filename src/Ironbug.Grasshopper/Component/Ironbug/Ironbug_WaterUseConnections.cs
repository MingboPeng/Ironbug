using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Ironbug.HVAC.BaseClass;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_WaterUseConnections : Ironbug_HVACComponent
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_ZoneEquipmentGroup class.
        /// </summary>
        public Ironbug_WaterUseConnections()
          : base("Ironbug_WaterUseConnections", "WaterUseConnections",
              "Description",
               "Ironbug", "02:LoopComponents",
               typeof(HVAC.IB_WaterUseConnections_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("WaterUseEquipment", "Equips_", "A list of WaterUseEquipments.", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("WaterUseConnections", "waterUse", "Connect to demand side of plant loop.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var eqps = new List<HVAC.IB_WaterUseEquipment>();
            DA.GetDataList(0, eqps);

            var obj = new HVAC.IB_WaterUseConnections();
            foreach (var item in eqps)
            {
                obj.AddWaterUseEquipment(item);
            }

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);

        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Properties.Resources.WaterUseConnect;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("018E550E-19DA-4F1C-BF4F-080C9DDAC8DF");
    }


}