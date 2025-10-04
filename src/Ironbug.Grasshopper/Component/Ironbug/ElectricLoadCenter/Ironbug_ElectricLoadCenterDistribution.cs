using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ElectricLoadCenterDistribution : Ironbug_HVACWithParamComponent
    {
        public Ironbug_ElectricLoadCenterDistribution()
          : base("IB_ElectricLoadCenterDistribution", "SubPanel",
              "Description",
              "Ironbug", "08:ElectricLoadCenter",
              typeof(HVAC.IB_ElectricLoadCenterDistribution_FieldSet))
        {

        }

        //public override GH_Exposure Exposure => GH_Exposure.septenary;


        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("_generators", "_generators", "A list of IB_Generator", GH_ParamAccess.list);
            pManager.AddGenericParameter("_inverter", "_inverter", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("_transformer", "_transformer", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("_electricalStorage", "_electricalStorage", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("_storageConverter", "_storageConverter", "", GH_ParamAccess.item);

            pManager[0].Optional = true;
            pManager[0].DataMapping = GH_DataMapping.Flatten;
            pManager[1].Optional = true;
            pManager[2].Optional = true;
            pManager[3].Optional = true;
            pManager[4].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("subPanel", "subPanel", "A distribution sub-panel of the ElectricLoadCenter main panel", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_ElectricLoadCenterDistribution();

            var generators = new List<HVAC.BaseClass.IB_Generator>();
            IB_ElecInverter inverter = null;
            IB_ElectricLoadCenterTransformer transformer = null;
            IB_ElecStorage storage = null;
            IB_ElectricLoadCenterStorageConverter converter = null;

            DA.GetDataList(0, generators);
            DA.GetData(1, ref inverter);
            DA.GetData(2, ref transformer);
            DA.GetData(3, ref storage);
            DA.GetData(4, ref converter);

            if (generators != null && generators.Any()) 
                obj.SetGenerators(generators);
            if (inverter != null)
                obj.SetInverter(inverter);
            if (transformer != null)
                obj.SetTransformer(transformer);
            if (storage != null)
                obj.SetElectricalStorage(storage);
            if (converter != null)
                obj.SetStorageConverter(converter);


            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("37129A27-30E5-4091-8C0A-491DFAB84AF7");


    }


}