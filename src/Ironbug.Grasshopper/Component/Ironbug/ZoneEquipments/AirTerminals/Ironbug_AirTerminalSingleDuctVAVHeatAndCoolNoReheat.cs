﻿using System;

using Grasshopper.Kernel;
using Ironbug.HVAC;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_AirTerminalSingleDuctVAVHeatAndCoolNoReheat : Ironbug_DuplicableHVACWithParamComponent
    {
        public Ironbug_AirTerminalSingleDuctVAVHeatAndCoolNoReheat()
          : base("IB_AirTerminalSingleDuctVAVHeatAndCoolNoReheat", "VAVHtnClnNoReheat",
              "Description",
              "Ironbug", "03:AirTerminals",
              typeof(IB_AirTerminalSingleDuctVAVHeatAndCoolNoReheat_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary | GH_Exposure.obscure;
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("AirTerminalSingleDuctVAVHeatAndCoolNoReheat", "AT", "connect to Zone", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new IB_AirTerminalSingleDuctVAVHeatAndCoolNoReheat();
            
            
            this.SetObjParamsTo(obj);

            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);

        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.VAVBoxHtnClnNoReheat;

        public override Guid ComponentGuid => new Guid("{1C5FCFFB-76FA-42A1-9FB7-408FFF50800E}");
    }
}