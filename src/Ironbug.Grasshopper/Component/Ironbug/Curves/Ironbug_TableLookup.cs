using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_TableLookup : Ironbug_HVACWithParamComponent
    {
        public Ironbug_TableLookup()
          : base("IB_TableLookup", "TableLookup",
              "Description",
              "Ironbug", "07:Curve & Load",
              typeof(HVAC.Curves.IB_TableLookup_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary | GH_Exposure.obscure;

        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Variables", "variables", "The independent variables that comprise the dimensions of the tabular data", GH_ParamAccess.list);
            pManager[0].DataMapping = GH_DataMapping.Flatten;
            pManager.AddNumberParameter("Output Values", "output values", "The data for this particular output is ordered according to the order of the corresponding independent variables.", GH_ParamAccess.list);
            pManager[1].DataMapping = GH_DataMapping.Flatten;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("TableLookup", "Curve", "TableLookup", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var vars = new List<HVAC.Curves.IB_TableIndependentVariable>();
            if (!DA.GetDataList(0, vars)) return;
            if (!vars.Any()) return;

            var values = new List<double>();
            if (!DA.GetDataList(1, values)) return;
            if (!values.Any()) return;

            var obj = new HVAC.Curves.IB_TableLookup();
            obj.SetVariables(vars);
            obj.SetOutputValues(values);

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("3C674B58-D50E-436E-913A-5A2367CE3797");
    }
}