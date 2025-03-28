﻿using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Ironbug.HVAC;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilCoolingDXMultiSpeed : Ironbug_HVACWithParamComponent
    {
        
        
        public Ironbug_CoilCoolingDXMultiSpeed()
          : base("IB_CoilCoolingDXMultiSpeed", "CoilClnDXM",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_CoilCoolingDXMultiSpeed_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilCoolingDXMultiSpeedStageData", "Stages", "A list of IB_CoilCoolingDXMultiSpeedStageData", GH_ParamAccess.list);
        }

        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilCoolingDXMultiSpeed", "Coil", "CoilCoolingDXMultiSpeed", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            var stages = new List<IB_CoilCoolingDXMultiSpeedStageData>();
            if (!DA.GetDataList(0, stages))
                return;

            var obj = new HVAC.IB_CoilCoolingDXMultiSpeed();
            obj.SetStages(stages);

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);

        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.CoilCDXM;


        public override Guid ComponentGuid => new Guid("43C457EB-5B33-49F2-9BC6-96FDCD9CF80A");
    }
}