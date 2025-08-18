using System;
using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ElectricLoadCenterDistribution : Ironbug_HVACWithParamComponent
    {
        public Ironbug_ElectricLoadCenterDistribution()
          : base("IB_ElectricLoadCenterDistribution", "ElectricLoadCenterDistribution",
              "Description",
              "Ironbug", "08:ElectricLoadCenter",
              typeof(HVAC.IB_ElectricLoadCenterDistribution_FieldSet))
        {
            
        }

        //public override GH_Exposure Exposure => GH_Exposure.septenary;

        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Distribution", "Distribution", "Distribution", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_ElectricLoadCenterDistribution();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("37129A27-30E5-4091-8C0A-491DFAB84AF7");


    }

   
}