﻿using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_EnergyManagementSystemConstructionVariable : Ironbug_Component
    {
        public Ironbug_EnergyManagementSystemConstructionVariable()
          : base("Ironbug_EnergyManagementSystemConstructionVariable", "EMSConstructionVariable",
              "Description",
              "Ironbug", "06:Sizing&Controller")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("_name_", "_name_", "Name", GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager.AddGenericParameter("_ConstructionID", "_ConstructionID", "Construction ID", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ConstructionVariable", "constrVariable", "ConstructionVariable", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_EnergyManagementSystemConstructionIndexVariable();
            string name = null;
            if(DA.GetData(0, ref name))
                obj.SetName(name);

            string Construction = null;
            DA.GetData(1, ref Construction);
            obj.SetConstructionID(Construction);

            DA.SetData(0, obj);

        }

        protected override System.Drawing.Bitmap Icon => null;

        
        public override Guid ComponentGuid => new Guid("8A70D5E0-BF0D-4789-9A64-F61D579FF74C");
    }
}