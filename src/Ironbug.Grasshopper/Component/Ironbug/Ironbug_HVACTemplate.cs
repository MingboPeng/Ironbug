using Grasshopper.Kernel;
using System;
using Grasshopper.Kernel.Special;
using GH = Grasshopper;
using System.Drawing;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_HVACTemplate : GH_Component
    {
        public Ironbug_HVACTemplate()
          : base("Ironbug_HVACTemplate", "HVACTemplate",
              "Description",
              "Ironbug", "HVAC")
        {
        }
        public override Guid ComponentGuid => new Guid("F11CEDF5-6613-4B0B-A6C3-C0FCCF1454FD");
        protected override System.Drawing.Bitmap Icon => null;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("run", "_run", "run to create from a template", GH_ParamAccess.item);
            
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var run = false;
            DA.GetData(0, ref run);
            if (!run) return;

            var ghBoiler = new Ironbug_BoilerHotWater();
            ghBoiler.CreateAttributes();
            var boilerP = new PointF(this.Attributes.Bounds.Left - 80, this.Attributes.Bounds.Y + 200);
            ghBoiler.Attributes.Pivot = boilerP;
            GH.Instances.ActiveCanvas.Document.AddObject(ghBoiler, false);
            //ghBoiler.ExpireSolution(true);

            var pLoop = new Ironbug_PlantBranches();
            pLoop.CreateAttributes();
            pLoop.Attributes.Pivot = new PointF(boilerP.X + 150, boilerP.Y);
            GH.Instances.ActiveCanvas.Document.AddObject(pLoop, false);

            pLoop.Params.Input[0].AddSource(ghBoiler.Params.Output[0]);

            //create a group
            var GhGroup = new GH_Group();
            GhGroup.CreateAttributes();
            GhGroup.NickName = "Extracted Coordinates";
            GhGroup.Colour = Color.White;
            GhGroup.AddObject(ghBoiler.InstanceGuid);
            GhGroup.AddObject(pLoop.InstanceGuid);
            GH.Instances.ActiveCanvas.Document.AddObject(GhGroup, false);
            //GhGroup.ExpireSolution(true);




            //GH.Instances.ActiveCanvas.Document.NewSolution(false);
        }
    }
}
