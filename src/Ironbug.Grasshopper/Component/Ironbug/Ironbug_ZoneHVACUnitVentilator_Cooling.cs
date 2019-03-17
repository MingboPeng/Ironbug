using System;
using Grasshopper.Kernel;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ZoneHVACUnitVentilator_Cooling : Ironbug_HVACComponent
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_ZoneHVACUnitHeater class.
        /// </summary>
        public Ironbug_ZoneHVACUnitVentilator_Cooling()
          : base("Ironbug_ZoneHVACUnitVentilator_Cooling", "UnitVentCooling",
              "Description",
              "Ironbug", "04:ZoneEquipments",
              typeof(IB_ZoneHVACUnitVentilator_DataFieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("CoolingCoil", "coilC_", "Cooling coil to provide cooling source. can be CoilHeatingWater, CoilHeatingElectirc, or CoilHeatingGas.", GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager.AddGenericParameter("Fan", "fan_", "Can be FanConstantVolume or FanVariableVolume.", GH_ParamAccess.item);
            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ZoneHVACUnitVentilator_Cooling", "UnitVent", "Connect to zone's equipment", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_ZoneHVACUnitVentilator_CoolingOnly();
            

            var fan = (IB_Fan)null;
            var coilC = (IB_CoilCoolingBasic)null;
            

            if (DA.GetData(0, ref coilC))
            {
                obj.SetCoolingCoil(coilC);
            }

            if (DA.GetData(1, ref fan))
            {
                obj.SetFan(fan);
            }


            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Properties.Resources.UnitVentC;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("F9A4E1F5-4C8D-4E17-8025-DBA7F329D479");
    }
}