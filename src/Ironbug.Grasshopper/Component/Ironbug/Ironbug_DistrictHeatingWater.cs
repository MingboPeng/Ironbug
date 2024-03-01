﻿using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_DistrictHeatingWater : Ironbug_DuplicableHVACWithParamComponent
    {
        
        public Ironbug_DistrictHeatingWater()
          : base("IB_DistrictHeatingWater", "DistrictHeatingWater",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_DistrictHeatingWater_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;
        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("DistrictHeating", "DistHeating", "DistrictHeating for plant loop's supply.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_DistrictHeatingWater();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }


        protected override System.Drawing.Bitmap Icon => Properties.Resources.DistricHeating;


        public override Guid ComponentGuid => new Guid("393db8da-b414-4e96-844d-a1a5ec9d6d51");
    }
}