using System;
using Grasshopper.Kernel;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ZoneHVACFourPipeFanCoil : Ironbug_HVACComponent
    {
        
        public Ironbug_ZoneHVACFourPipeFanCoil()
          : base("Ironbug_ZoneHVACFourPipeFanCoil", "4PipeFanCoil",
              "Description",
              "Ironbug", "04:ZoneEquipments",
              typeof(IB_ZoneHVACFourPipeFanCoil_DataFieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("HeatingCoil", "coilH_", "Heating coil to provide heating source. Must be CoilHeatingWater.", GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager.AddGenericParameter("CoolingCoil", "coilC_", "Cooling coil to provide cooling source. Must be CoilHeatingWater.", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddGenericParameter("Fan", "fan_", "Can be FanOnOff, FanConstantVolume or FanVariableVolume.", GH_ParamAccess.item);
            pManager[2].Optional = true;
        }
        
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ZoneHVACFourPipeFanCoil", "FCU", "Connect to zone's equipment", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_ZoneHVACFourPipeFanCoil();
            

            var fan = (IB_Fan)null;
            var coilH = (IB_CoilHeatingWater)null;
            var coilC = (IB_CoilCoolingWater)null;

            if (DA.GetData(0, ref coilH))
            {
                obj.SetHeatingCoil(coilH);
            }

            if (DA.GetData(1, ref coilC))
            {
                obj.SetCoolingCoil(coilC);
            }

            if (DA.GetData(2, ref fan))
            {
                obj.SetFan(fan);
            }


            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }
        
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources._4PipeFC;
            }
        }
        
        public override Guid ComponentGuid
        {
            get { return new Guid("A5704E42-164F-4BB1-8412-A44B66B6370D"); }
        }
    }
}