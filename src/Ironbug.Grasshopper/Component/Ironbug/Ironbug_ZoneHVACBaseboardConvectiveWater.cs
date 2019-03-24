using System;
using Grasshopper.Kernel;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ZoneHVACBaseboardConvectiveWater : Ironbug_HVACComponent
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_ZoneHVACUnitHeater class.
        /// </summary>
        public Ironbug_ZoneHVACBaseboardConvectiveWater()
          : base("Ironbug_ZoneHVACBaseboardConvectiveWater", "BaseboardWaterC",
              "Description",
              "Ironbug", "04:ZoneEquipments",
              typeof(IB_ZoneHVACBaseboardConvectiveWater_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilHeatingWaterBaseboard", "coil_", "Heating coil to provide heating source. Only CoilHeatingWaterBaseboard is accepted.", GH_ParamAccess.item);
            pManager[0].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ZoneHVACBaseboardConvectiveWater", "Baseboard", "Connect to zone's equipment", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_ZoneHVACBaseboardConvectiveWater();
            
            
            var coilH = (IB_CoilHeatingWaterBaseboard)null;

            if (DA.GetData(0, ref coilH))
            {
                obj.SetHeatingCoil(coilH);
            }
            
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Properties.Resources.BaseboardWC;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("0849B166-9E6A-4BD2-A743-6BAF790F931F"); }
        }
    }
}