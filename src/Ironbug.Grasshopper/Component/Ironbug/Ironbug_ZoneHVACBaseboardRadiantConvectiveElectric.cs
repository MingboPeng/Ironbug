using System;
using Grasshopper.Kernel;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ZoneHVACBaseboardRadiantConvectiveElectric : Ironbug_HVACComponentBase
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_ZoneHVACUnitHeater class.
        /// </summary>
        public Ironbug_ZoneHVACBaseboardRadiantConvectiveElectric()
          : base("Ironbug_ZoneHVACBaseboardRadiantConvectiveElectric", "BaseboardERC",
              "Description",
              "Ironbug", "04:ZoneEquipments",
              typeof(IB_ZoneHVACBaseboardRadiantConvectiveElectric_DataFieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ZoneHVACBaseboardRadiantConvectiveElectric", "BaseboardERC", "Connect to zone's equipment", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_ZoneHVACBaseboardRadiantConvectiveElectric();
            obj.PuppetEventHandler += PuppetStateChanged;
            
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
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("7B15131A-BE82-4BFB-AEDD-F04CFDAE7A41"); }
        }
    }
}