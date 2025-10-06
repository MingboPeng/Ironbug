using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;
using Ironbug.HVAC;
using System;
using System.Collections.Generic;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ElectricLoadCenter : Ironbug_Component
    {
        public Ironbug_ElectricLoadCenter()
          : base("IB_ElectricLoadCenter", "ELC MainPanel",
              "The main panel of the ElectricLoadCenter",
              "Ironbug", "08:ElectricLoadCenter"
              )
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;


        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("_subPanels", "_subPanels", "A list of ElectricLoadCenters", GH_ParamAccess.list);
            pManager.AddGenericParameter("powerInTransformer", "transformerIn", "An optional input for a transformer for transferring electricity from the grid to a building (as distribution transformers) when applicable", GH_ParamAccess.item);
            pManager.AddGenericParameter("powerOutTransformer", "transformerOut", "An optional input for a transformer for transferring electricity from onsite generators to the grid when applicable", GH_ParamAccess.item);

            pManager[0].DataMapping = GH_DataMapping.Flatten;
            pManager[1].Optional = true;
            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ElectricLoadCenter", "ELC", "ElectricLoadCenter main panel", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_ElectricLoadCenter();


            var subPanels = new List<HVAC.IB_ElectricLoadCenterDistribution>();
            IB_ElectricLoadCenterTransformer inTransformer = null;
            IB_ElectricLoadCenterTransformer outTransformer = null;

            DA.GetDataList(0, subPanels);
            DA.GetData(1, ref inTransformer);
            DA.GetData(2, ref outTransformer);

            obj.SetSubPanels(subPanels);
            obj.SetPowerInTransformer(inTransformer);
            obj.SetPowerOutTransformer(outTransformer);
            
            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => Resources.MainPanel;

        public override Guid ComponentGuid => new Guid("EA1FF6A4-A434-4DF0-8885-8C1699D92C02");


    }

   
}