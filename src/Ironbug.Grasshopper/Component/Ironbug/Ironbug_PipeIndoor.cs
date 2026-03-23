using System;
using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_PipeIndoor : Ironbug_DuplicableHVACWithParamComponent
    {
        public Ironbug_PipeIndoor()
          : base("IB_PipeIndoor", "PipeIndoor",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_PipeIndoor_FieldSet))
        {
            
        }

        public override GH_Exposure Exposure => GH_Exposure.septenary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {

            pManager.AddTextParameter("construction", "const_name_", "A name of a ‘wall’ construction object that gives a layer-by-layer description of the pipe wall and its insulation. The construction object follows standard conventions, describing material properties for each layer beginning with the outermost insulation layer and ending with the pipe wall layer.", GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager.AddGenericParameter("AmbientTemperatureZone", "ambientZone_", "If Zone is specified as the environment type, this field is used to specify the name of the zone in which the pipe is located. The zone temperature is used to calculate the heat transfer rate from the pipe.", GH_ParamAccess.item);
            pManager[1].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PipeIndoor", "Pipe", "PipeIndoor", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_PipeIndoor();

            HVAC.BaseClass.IB_ThermalZone zone = null;
            if (DA.GetData(1, ref zone)) obj.SetAmbientTemperatureZone(zone.GetRoomName());
        
            string construction = null;
            if (DA.GetData(0, ref construction) && !string.IsNullOrEmpty(construction))
                obj.SetContstruction(construction);


            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => Resources.PipeIndoor;

        public override Guid ComponentGuid => new Guid("B5024874-E6B2-4E61-BFC0-6824F0661336");


    }

   
}