using System;
using Grasshopper.Kernel;
using Ironbug.HVAC;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ZoneHVACBaseboardRadiantConvectiveElectric : Ironbug_HVACComponent
    {
        
        /// Initializes a new instance of the Ironbug_ZoneHVACUnitHeater class.
        
        public Ironbug_ZoneHVACBaseboardRadiantConvectiveElectric()
          : base("Ironbug_ZoneHVACBaseboardRadiantConvectiveElectric", "BaseboardElecRadC",
              "Description",
              "Ironbug", "04:ZoneEquipments",
              typeof(IB_ZoneHVACBaseboardRadiantConvectiveElectric_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;
        
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ZoneHVACBaseboardRadiantConvectiveElectric", "Baseboard", "Connect to zone's equipment", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_ZoneHVACBaseboardRadiantConvectiveElectric();
            
            
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon => Properties.Resources.BaseboardERC;

        public override Guid ComponentGuid => new Guid("7B15131A-BE82-4BFB-AEDD-F04CFDAE7A41");
    }
}