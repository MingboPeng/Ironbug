using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_EnergyManagementSystemActuator : Ironbug_Component
    {
        public Ironbug_EnergyManagementSystemActuator()
          : base("Ironbug_EMSActuator", "EMSActuator",
              "Description",
              "Ironbug", "06:Sizing&Controller")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
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
          
            string tagID = null;
            DA.GetData(0, ref tagID);
            

            string type = null;
            DA.GetData(1, ref type);

            string ctype = null;
            DA.GetData(2, ref ctype);

            var obj = new HVAC.IB_EnergyManagementSystemActuator();
            var fSet = HVAC.IB_EnergyManagementSystemActuator_FieldSet.Value;
            obj.AddCustomAttribute(fSet.ActuatedComponentType, type);
            obj.AddCustomAttribute(fSet.ActuatedComponentControlType, ctype);
            obj.SetTrackingID(tagID);

            //this.SetObjParamsTo(obj);
            DA.SetData(0, obj);

        }

        protected override System.Drawing.Bitmap Icon => null;

        
        public override Guid ComponentGuid => new Guid("933B351E-C1E3-48B6-82AF-74B0C29A1C53");
    }
}