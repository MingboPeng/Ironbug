using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_EnergyManagementSystemCurveVariable : Ironbug_Component
    {
        public Ironbug_EnergyManagementSystemCurveVariable()
          : base("IB_EMSCurveVariable", "EMSCurveVariable",
              "Description",
              "Ironbug", "06:Sizing&Controller")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("_name_", "_name_", "Name", GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager.AddTextParameter("_tagID", "_tagID", "Assign a tag id for this actuator that can be referenced in EMS program.", GH_ParamAccess.item);
            pManager.AddGenericParameter("_curve", "_curve", "Curve", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("curveVariable", "crvVariable", "CurveVariable", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_EnergyManagementSystemCurveVariable();
            string name = null;
            if(DA.GetData(0, ref name))
                obj.SetName(name);

            string tagId = null;
            if (DA.GetData(1, ref tagId))
                obj.SetTrackingID(tagId);

            HVAC.BaseClass.IB_Curve curve = null;
            DA.GetData(2, ref curve);
            obj.SetCurve(curve);

            DA.SetData(0, obj);

        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.EMS_Meter;


        public override Guid ComponentGuid => new Guid("F1DED304-27BD-4161-B2CE-2B26E88A0E24");
    }
}