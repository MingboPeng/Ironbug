using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_WaterUseEquipmentDefinition : Ironbug_HVACWithParamComponent
    {
        
        /// Initializes a new instance of the Ironbug_SizingZone class.
        
        public Ironbug_WaterUseEquipmentDefinition()
          : base("Ironbug_WaterUseEquipmentDefinition", "WaterUseLoad",
              "Description",
              "Ironbug", "07:Curve & Load",
              typeof(HVAC.IB_WaterUseEquipmentDefinition_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("peakFlowRate", "flow", "peakFlowRate m3/s", GH_ParamAccess.item);

        }
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("WaterUseLoad", "load", "IB_WaterUseEquipmentDefinition", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double peakFlowRate = 0.000063;
            DA.GetData(0, ref peakFlowRate);
            var obj = new HVAC.IB_WaterUseEquipmentDefinition(peakFlowRate);

            obj.SetFieldValue(HVAC.IB_WaterUseEquipmentDefinition_FieldSet.Value.PeakFlowRate, peakFlowRate);
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.WaterUseLoad;

        public override Guid ComponentGuid => new Guid("{F8B38B50-1737-4A79-878F-F3B51312AE9A}");
    }
}