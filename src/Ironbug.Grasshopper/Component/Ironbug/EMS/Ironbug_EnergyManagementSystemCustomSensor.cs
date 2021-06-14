using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_EnergyManagementSystemCustomSensor : Ironbug_Component
    {
        public Ironbug_EnergyManagementSystemCustomSensor()
          : base("Ironbug_EnergyManagementSystemCustomSensor", "EMSCustomSensor",
              "Description",
              "Ironbug", "06:Sizing&Controller")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("_tagID", "_tagID", "Assign a tag id for this actuator that can be referenced in EMS program.", GH_ParamAccess.item);
            pManager.AddTextParameter("_keyName", "_keyName", "Object that to add sensor on", GH_ParamAccess.item);
            pManager.AddTextParameter("_outputVariable", "_outputVariable", "Output Variable", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("sensor", "sensor", "sensor", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_EnergyManagementSystemSensor();
            string tagID = null;
            DA.GetData(0, ref tagID);
            obj.SetTrackingID(tagID);

            string keyName = null;
            DA.GetData(1, ref keyName);
            obj.SetKeyName(keyName);

            string outputVariable = null;
            DA.GetData(2, ref outputVariable);
            obj.SetOutputVariableOrMeterName(outputVariable);

            DA.SetData(0, obj);

        }

        protected override System.Drawing.Bitmap Icon => null;

        
        public override Guid ComponentGuid => new Guid("8E693D67-342F-4A79-B8A4-BEEC4B273D4D");
    }
}