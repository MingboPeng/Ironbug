﻿using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilCoolingFourPipeBeam : Ironbug_DuplicableHVACWithParamComponent
    {
        
        /// Initializes a new instance of the Ironbug_CoilHeatingWater class.
        
        public Ironbug_CoilCoolingFourPipeBeam()
          : base("IB_CoilCoolingFourPipeBeam", "Coil_Cln4PipBeam",
              "Description",
              "Ironbug", "04:ZoneEquipments",
              typeof(HVAC.IB_CoilCoolingFourPipeBeam_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quinary | GH_Exposure.obscure;

        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilCoolingFourPipeBeam", "CoilC", "Connect to chilled beam", GH_ParamAccess.item);
            pManager.AddGenericParameter("WaterSide_Coil", "ToWaterLoop", "Connect to chilled water loop's demand side via plantBranches", GH_ParamAccess.item);
        }

        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_CoilCoolingFourPipeBeam();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
            DA.SetDataList(1, objs);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.Coil_Cooling4PB;


        public override Guid ComponentGuid => new Guid("{A71EB4F1-EB32-4CC1-A729-C3E4A6DB3616}");

    }
    
}