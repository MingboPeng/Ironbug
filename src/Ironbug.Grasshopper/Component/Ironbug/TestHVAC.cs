using System;
using System.Collections.Generic;
using Ironbug.HVAC;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Grasshopper.Kernel.Parameters;

namespace Ironbug.Grasshopper.Component
{
    public class TestHVAC : GH_Component,IGH_VariableParameterComponent
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public TestHVAC()
          : base("testHVAC", "testHVAC",
              "Description",
              "Mingbo_Dev", "test")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("demand", "demand", "zoneMixer or other HVAC components", GH_ParamAccess.item);
            //pManager.AddTextParameter("name", "name", "name", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("modifedCoil", "modifedCoil", "connect to OutdoorAirSystem", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IIB_ModelObject ib_obj =null;
            DA.GetData(0, ref ib_obj);


            //var supplyComs = new HVAC.IB_CoilHeatingWater();
            //DA.GetData(0, ref supplyComs);

            //string name = "new name";
            //DA.GetData(1, ref name);

            //var supplyComs2 = supplyComs.Duplicate();
            //supplyComs2.SetAttribute(HVAC.IB_CoilHeatingWater_DataFieldSet.Name, name);

            var state = ib_obj.CurrentState;
            

            DA.SetData(0, state);


        }

        public bool CanInsertParameter(GH_ParameterSide side, int index)
        {
            return true;
        }

        public bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            return true;
        }

        public IGH_Param CreateParameter(GH_ParameterSide side, int index)
        {
            return new Param_GenericObject();
        }

        public bool DestroyParameter(GH_ParameterSide side, int index)
        {
            throw new NotImplementedException();
        }

        public void VariableParameterMaintenance()
        {
            //throw new NotImplementedException();
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
            get { return new Guid("7be1ded0-2430-4d34-9c9a-9cb44d23548a"); }
        }
    }
}