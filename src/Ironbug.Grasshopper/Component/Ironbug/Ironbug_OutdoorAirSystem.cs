﻿using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_OutdoorAirSystem : Ironbug_Component
    {
        
        /// Initializes a new instance of the Ironbug_OutdoorAirSystem class.
        
        public Ironbug_OutdoorAirSystem()
          : base("IB_OutdoorAirSystem", "OASystem",
              EPDoc.AirLoopHVACOutdoorAirSystem.Note,
              "Ironbug", "02:LoopComponents")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;
        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            
            pManager.AddGenericParameter("ExhaustStream", "ExStream_", "add objs to outdoor air exhaust stream", GH_ParamAccess.list);
            pManager[0].Optional = true;
            pManager.AddGenericParameter("HeatRecovery", "HeatRecovery_", "HeatRecovery", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddGenericParameter("IntakeStream", "OAStream_", "add objs to outdoor air intake stream", GH_ParamAccess.list);
            pManager[2].Optional = true;

            pManager.AddGenericParameter("Controller", "Controller_", "Controller for OutdoorAirSystem", GH_ParamAccess.item);
            pManager[3].Optional = true;
        }

        
        
        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("OutdoorAirSystem", "OA", "OutdoorAirSystem", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_OutdoorAirSystem();

            var hx = new HVAC.IB_HeatExchangerAirToAirSensibleAndLatent();
            if (DA.GetData(1, ref hx))
            {
                obj.SetHeatExchanger(hx);
            }

            var controller = new HVAC.IB_ControllerOutdoorAir();
            if (DA.GetData(3, ref controller))
            {
                obj.SetController(controller);
            }

            
            var exObjs = new List<HVAC.BaseClass.IB_HVACObject>();
            if (DA.GetDataList(0, exObjs))
            {
                foreach (var item in exObjs)
                {
                    obj.AddToReliefStream(item);
                }

            }
            var oaObjs = new List<HVAC.BaseClass.IB_HVACObject>();
            if (DA.GetDataList(2,oaObjs))
            {
                foreach (var item in oaObjs)
                {
                    obj.AddToOAStream(item);
                }
               
            }



            DA.SetData(0, obj);
        }


        protected override System.Drawing.Bitmap Icon => Properties.Resources.OutdoorAir;

        public override Guid ComponentGuid => new Guid("648436b4-3ce6-4db2-a1a5-91d9a2999e9f");
    }
}