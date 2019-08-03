using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using System;
using System.Collections.Generic;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_Duplicate : Ironbug_Component
    {
        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override System.Drawing.Bitmap Icon => Properties.Resources.Duplicate;

        public override Guid ComponentGuid => new Guid("cb1cb9d6-e29e-4d26-9133-82b47c0e6d8d");

        
        public Ironbug_Duplicate()
          : base("Ironbug_Duplicate", "Duplicate",
              "Description",
              "Ironbug", "00:Ironbug")
        {
        }
        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Reference", "ref", "a reference obj for creating duplicates", GH_ParamAccess.list);
            pManager.AddNumberParameter("Amount", "n", "number of duplicates", GH_ParamAccess.list);
        
        }

        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Objects", "objs", "Objects", GH_ParamAccess.list);
            pManager.AddGenericParameter("-", "-", "-", GH_ParamAccess.list);
            pManager[1].DataMapping = GH_DataMapping.Graft;
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var objs = new List<HVAC.BaseClass.IB_ModelObject>();
            var amounts = new List<double>();
            DA.GetDataList(0, objs);
            DA.GetDataList(1, amounts);

            if (objs.Count<=0) return;
            

            var lis = new List<HVAC.BaseClass.IB_ModelObject>();

            int amount = (int)amounts[0];
            if (amounts.Count>1)
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, $"Duplicate amount {amount} is only used!");


            for (int i = 0; i < amount; i++)
            {
                var p = new GH_Path(i);
                foreach (var obj in objs)
                {
                    var dupObj = obj.Duplicate();
                    dupObj.SetTrackingID();
                    lis.Add(dupObj);
                }
                
            }

            
            DA.SetDataList(0, lis);

     

            var refComponent = this.Params.Input[0].Sources[0].Attributes.GetTopLevel.DocObject;

            if (refComponent is Ironbug_HVACComponent component)
            {
                if (component.Params.Output.Count > 1)
                {

                    var secondParam = this.Params.Output[1];
                    secondParam.Name = "-";
                    secondParam.NickName = "-";
                    secondParam.Description = "-";

                    var refSecondOutput = component.Params.Output[1];

                    secondParam.Name = refSecondOutput.Name;
                    secondParam.NickName = refSecondOutput.NickName;
                    secondParam.Description = refSecondOutput.Description;

                    DA.SetDataList(1, lis);
                }
            }



        }

      
    }
}