using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using System;
using System.Collections.Generic;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_Duplicate : GH_Component
    {
        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override System.Drawing.Bitmap Icon => Properties.Resources.Duplicate;

        public override Guid ComponentGuid => new Guid("cb1cb9d6-e29e-4d26-9133-82b47c0e6d8d");

        /// <summary>
        /// Initializes a new instance of the Ironbug_CreatePuppets class.
        /// </summary>
        public Ironbug_Duplicate()
          : base("Ironbug_Duplicate", "Duplicate",
              "Description",
              "Ironbug", "00:Ironbug")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Reference", "ref", "a reference obj for creating puppets", GH_ParamAccess.item);
            pManager.AddNumberParameter("Amount", "n", "number of puppets to be created", GH_ParamAccess.item, 2);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Objects", "objs", "Objects", GH_ParamAccess.list);
            pManager[0].DataMapping = GH_DataMapping.Flatten;
            pManager.AddGenericParameter("-", "-", "-", GH_ParamAccess.list);
            pManager[1].DataMapping = GH_DataMapping.Flatten;
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            HVAC.BaseClass.IB_ModelObject obj = null;
            double amount = 2;
            DA.GetData(0, ref obj);
            DA.GetData(1, ref amount);

            if (obj == null) return;
            
            var dupObjs = new List<HVAC.BaseClass.IB_ModelObject>();

            for (int i = 0; i < amount; i++)
            {
                HVAC.BaseClass.IB_ModelObject dupObj = null;
                if (obj is HVAC.BaseClass.IB_HVACObject hvacObj)
                {
                    dupObj = hvacObj.Duplicate();
                }
                else
                {
                    dupObj = obj.Duplicate();
                }

                dupObj.SetTrackingID();
                dupObjs.Add(dupObj);
            }

            DA.SetDataList(0, dupObjs);


            

            var refComponent = this.Params.Input[0].Sources[0].Attributes.GetTopLevel.DocObject;
            if (!(refComponent is Ironbug_HVACComponent)) return;

            var component = refComponent as Ironbug_HVACComponent;
            var secondParam = this.Params.Output[1];

            if (component.Params.Output.Count > 1)
            {
                var refSecondOutput = component.Params.Output[1];

                secondParam.Name = refSecondOutput.Name;
                secondParam.NickName = refSecondOutput.NickName;
                secondParam.Description = refSecondOutput.Description;

                secondParam.ClearData();
                var data = this.Params.Output[0].VolatileData;
                data.Simplify(GH_SimplificationMode.CollapseAllOverlaps);

                secondParam.AddVolatileDataList(new GH_Path(0), data.AllData(false));
            }
            else
            {
                secondParam.Name = "-";
                secondParam.NickName = "-";
                secondParam.Description = "-";
            }

        }

        //protected override void AfterSolveInstance()
        //{
        //    base.AfterSolveInstance();
        //    if (this.Params.Input[0].Sources.Count == 0) return;

        //    var refComponent = this.Params.Input[0].Sources[0].Attributes.GetTopLevel.DocObject;
        //    if (!(refComponent is Ironbug_HVACComponent)) return;

        //    var component = refComponent as Ironbug_HVACComponent;
        //    var secondParam = this.Params.Output[1];

        //    if (component.Params.Output.Count > 1)
        //    {
        //        var refSecondOutput = component.Params.Output[1];

        //        secondParam.Name = refSecondOutput.Name;
        //        secondParam.NickName = refSecondOutput.NickName;
        //        secondParam.Description = refSecondOutput.Description;

        //        secondParam.ClearData();
        //        var data = this.Params.Output[0].VolatileData;
        //        data.Simplify(GH_SimplificationMode.CollapseAllOverlaps);
                
        //        secondParam.AddVolatileDataList(new GH_Path(0), data.AllData(false));
        //    }
        //    else
        //    {
        //        secondParam.Name = "-";
        //        secondParam.NickName = "-";
        //        secondParam.Description = "-";
        //    }

        //    //this.Params.OnParametersChanged();
        //}
    }
}