using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_EnergyManagementSystemActuator3 : Ironbug_Component
    {
        public Ironbug_EnergyManagementSystemActuator3()
          : base("Ironbug_EMSActuator3", "EMSActuator3",
              "Description",
              "Ironbug", "06:Sizing&Controller")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("_objName", "_objName", "A unique name for searching OpenStudio/EnergyPlus object for setting the actuator", GH_ParamAccess.item);
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
         
            string hostObjName = null;
            DA.GetData(0, ref hostObjName);

            string tagID = null;
            DA.GetData(1, ref tagID);
         
            string type = null;
            DA.GetData(2, ref type);

            string ctype = null;
            DA.GetData(3, ref ctype);

            var obj = new HVAC.IB_EnergyManagementSystemActuator(hostObjName);
            var fSet = HVAC.IB_EnergyManagementSystemActuator_FieldSet.Value;
            obj.AddCustomAttribute(fSet.ActuatedComponentType, type);
            obj.AddCustomAttribute(fSet.ActuatedComponentControlType, ctype);
            obj.SetTrackingID(tagID);

            //this.SetObjParamsTo(obj);
            DA.SetData(0, obj);

        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.EMS_Actuator;


        public override Guid ComponentGuid => new Guid("7BE13F9D-BD47-4380-9F8A-BD4AC4BCC5D1");
    }
}