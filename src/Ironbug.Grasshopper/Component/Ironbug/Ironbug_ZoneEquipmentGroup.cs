using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Ironbug.HVAC.BaseClass;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ZoneEquipmentGroup : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_ZoneEquipmentGroup class.
        /// </summary>
        public Ironbug_ZoneEquipmentGroup()
          : base("Ironbug_ZoneEquipmentGroup", "ZoneEquipmentGroup",
              "Description",
              "Ironbug", "00:Ironbug")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("ZoneEquipments", "Equipments_", "A list of zone equipments that will be grouped.", GH_ParamAccess.list);
            pManager[0].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ZoneEquipmentGroup", "EquipGroup", "A list of zone equipment groups that will be added to each zones.", GH_ParamAccess.item);
            pManager[0].DataMapping = GH_DataMapping.Flatten;
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var zoneEqps = new List<IB_ZoneEquipment>();
            DA.GetDataList(0, zoneEqps);

            var group = new HVAC.IB_ZoneEquipmentGroup(zoneEqps);
            DA.SetData(0, group);

        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => null;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("73fb35a3-4bc5-441a-b922-f0a90f0b6d5b");
    }


}