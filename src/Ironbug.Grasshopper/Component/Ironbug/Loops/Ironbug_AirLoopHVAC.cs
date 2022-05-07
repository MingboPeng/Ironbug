using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;
using Ironbug.HVAC.BaseClass;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_AirLoopHVAC : Ironbug_HVACWithParamComponent
    {
        public Ironbug_AirLoopHVAC()
          : base("IB_AirLoopHVAC", "AirLoop",
              "Description",
              "Ironbug", "01:Loops",
              typeof(HVAC.IB_AirLoopHVAC_FieldSet))
        {
        }
        public override GH_Exposure Exposure => GH_Exposure.primary;

        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("_supply", "_supply", "heating or cooling supply source", GH_ParamAccess.list);
            pManager.AddGenericParameter("_demand", "_demand", "zoneBranches or other HVAC components", GH_ParamAccess.list);
            pManager.AddGenericParameter("sizingSystem", "sizing", "HVAC components", GH_ParamAccess.item);
            
            //pManager[0].Optional = true;
            //pManager[1].Optional = true;
            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("airLoopHVAC", "airLoop", "To HVACsystem", GH_ParamAccess.item);
        }

        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var supplyComs = new List<IB_HVACObject>();
            DA.GetDataList(0, supplyComs);

            var demandComs = new List<IB_HVACObject>();
            DA.GetDataList(1, demandComs);

            //check if there is a dual loop object that cannot be duplicated.
            var supplyPathCount = this.Params.Input[0].VolatileData.PathCount;
            var demandPathCount = this.Params.Input[1].VolatileData.PathCount;
            var dualLoopObjs = supplyComs.Where(_ => _ is HVAC.IIB_DualLoopObj);
            if (supplyPathCount < demandPathCount & dualLoopObjs.Any())
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"Dual loop object [{dualLoopObjs.First().GetType().Name}] in supply side cannot be auto-duplicated. \nIt is because the data structure of supply side doesn't match demand side's. \nPlease use Ironbug_duplicate to duplicate {demandPathCount} objects, and match demand input's data structure.");
                return;
            }


            HVAC.IB_SizingSystem sizing = null;
           
            var airLoop = new HVAC.IB_AirLoopHVAC();
            if (DA.GetData(2, ref sizing))
            {
                airLoop.SetSizingSystem(sizing);
            }

            

            //TODO: need to check nulls
            foreach (var item in supplyComs)
            {
                airLoop.AddToSupplySide(item);
            }

            foreach (var item in demandComs)
            {
                airLoop.AddToDemandSide(item);
            }


            this.SetObjParamsTo(airLoop);
            DA.SetData(0, airLoop);

            this.Message = this.RunCount == 1 ? $"{this.RunCount} Loop" : $"{this.RunCount} Loops";
        }


        protected override System.Drawing.Bitmap Icon => Resources.AirLoop;

        public override Guid ComponentGuid => new Guid("a416631f-bdda-4e11-8a2c-658c38681201");

        public override void CreateAttributes()
        {
            var newAttri = new IB_LoopComponentAttributes(this);
            m_attributes = newAttri;
        }
    }
}