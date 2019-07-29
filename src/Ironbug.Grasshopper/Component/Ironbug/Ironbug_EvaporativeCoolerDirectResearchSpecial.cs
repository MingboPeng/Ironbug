using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_EvaporativeCoolerIndirectResearchSpecial : Ironbug_HVACComponent
    {
        
        
        /// Initializes a new instance of the Ironbug_BoilerHotWater class.
        
        public Ironbug_EvaporativeCoolerIndirectResearchSpecial()
          : base("Ironbug_EvaporativeCoolerIndirectResearchSpecial", "EvapCoolerIndir",
              "Description",
              "Ironbug", "02:LoopComponents", 
              typeof(HVAC.IB_EvaporativeCoolerIndirectResearchSpecial_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("EvaporativeCoolerIndirectResearchSpecial", "EvapCoolerIndir", "EvaporativeCoolerIndirectResearchSpecial", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_EvaporativeCoolerIndirectResearchSpecial();

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.EvapCoolerIndir;

        public override Guid ComponentGuid => new Guid("{CFFB97C1-10DA-4323-8438-CD246762813E}");
    }
}