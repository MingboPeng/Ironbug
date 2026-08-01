using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_TableIndependentVariable : Ironbug_HVACWithParamComponent
    {
        public Ironbug_TableIndependentVariable()
          : base("IB_TableIndependentVariable", "TableVariable",
              "Description",
              "Ironbug", "07:Curve & Load",
              typeof(HVAC.Curves.IB_TableIndependentVariable_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary | GH_Exposure.obscure;


        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Values", "values", "The full set of values for this independent variable. These values must be defined in ascending order (an error will be issued if this is not the case)", GH_ParamAccess.list);
            pManager[0].DataMapping = GH_DataMapping.Flatten;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Variable", "var", "TableIndependentVariable", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var vars = new List<double>();
            if (!DA.GetDataList(0, vars)) return;
            if (!vars.Any()) return;

            var obj = new HVAC.Curves.IB_TableIndependentVariable();
            obj.SetValues(vars);

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("72AD1BEA-E53F-4CBE-A75D-817D0222FCFC");
    }
}