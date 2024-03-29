﻿using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Ironbug.HVAC;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilHeatingDXMultiSpeed : Ironbug_HVACWithParamComponent
    {
        public Ironbug_CoilHeatingDXMultiSpeed()
          : base("IB_CoilHeatingDXMultiSpeed", "CoilHtn_DXM",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_CoilHeatingDXMultiSpeed_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary | GH_Exposure.obscure;
        
        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilHeatingDXMultiSpeedStageData", "Stages", "A list of IB_CoilHeatingDXMultiSpeedStageData", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilHeatingDXMultiSpeed", "Coil", "CoilHeatingDXMultiSpeed", GH_ParamAccess.item);
        }

        
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            var stages = new List<IB_CoilHeatingDXMultiSpeedStageData>();
            if (!DA.GetDataList(0, stages))
                return;

            var obj = new HVAC.IB_CoilHeatingDXMultiSpeed();
            obj.SetStages(stages);

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }


        protected override System.Drawing.Bitmap Icon => Properties.Resources.CoilHDXM;

        public override Guid ComponentGuid => new Guid("C378771E-CCBB-4672-AD2B-1B3F48159AED");
    }
}