using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_EnergyManagementSystemActuator2 : Ironbug_Component
    {
        public Ironbug_EnergyManagementSystemActuator2()
          : base("IB_EMSActuator2", "EMSActuator2",
              "Description",
              "Ironbug", "06:Sizing&Controller")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("_obj", "_obj", "a Hvac object for setting actuator", GH_ParamAccess.item);
            pManager.AddTextParameter("_tagID", "_tagID", "Assign a tag id for this actuator that can be referenced in EMS program.", GH_ParamAccess.item);
            pManager.AddTextParameter("_type", "_type", "Actuated Component Type", GH_ParamAccess.item);
            pManager.AddTextParameter("_controlType", "_ctrlType", "Actuated Component Control Type", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("actuator", "actuator", "actuator", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
         
            HVAC.BaseClass.IB_ModelObject hostObj = null;
            DA.GetData(0, ref hostObj);

            string tagID = null;
            DA.GetData(1, ref tagID);
         
            string type = null;
            DA.GetData(2, ref type);

            string ctype = null;
            DA.GetData(3, ref ctype);

            var obj = new HVAC.IB_EnergyManagementSystemActuator(hostObj);
            var fSet = HVAC.IB_EnergyManagementSystemActuator_FieldSet.Value;
            obj.AddCustomAttribute(fSet.ActuatedComponentType, type);
            obj.AddCustomAttribute(fSet.ActuatedComponentControlType, ctype);
            obj.SetTrackingID(tagID);

            //this.SetObjParamsTo(obj);
            DA.SetData(0, obj);

        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.EMS_Actuator;


        public override Guid ComponentGuid => new Guid("8F6CD814-4993-4F1C-ACEF-E2AD4E1E4D28");
    }
}