using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_EnergyManagementSystemInternalVariable : Ironbug_Component
    {
        public Ironbug_EnergyManagementSystemInternalVariable()
          : base("Ironbug_EMSInternalVariable", "EMSInternalVariable",
              "Description",
              "Ironbug", "06:Sizing&Controller")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("_obj", "_obj", "a Hvac object for getting internal variable", GH_ParamAccess.item);
            pManager.AddTextParameter("_tagID", "_tagID", "Assign a tag id for this actuator that can be referenced in EMS program.", GH_ParamAccess.item);
            pManager.AddTextParameter("_dataType", "_dataType", "Internal Data Type", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("InternalVariable", "intnVariable", "InternalVariable", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_EnergyManagementSystemInternalVariable();
            HVAC.BaseClass.IB_ModelObject hostObj = null;
            DA.GetData(0, ref hostObj);
            obj.SetHostObj(hostObj);

            string tagId = null;
            DA.GetData(1, ref tagId);
            obj.SetTrackingID(tagId);

            string dataType = null;
            DA.GetData(2, ref dataType);
            obj.SetInternalDataType(dataType);

            DA.SetData(0, obj);

        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.EMS_Meter;


        public override Guid ComponentGuid => new Guid("8A70D5E0-BF0D-4789-9A64-F61D579FF74C");
    }
}