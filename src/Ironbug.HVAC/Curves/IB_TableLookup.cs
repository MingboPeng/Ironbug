using Ironbug.HVAC.BaseClass;
using Newtonsoft.Json;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.HVAC.Curves
{
    public class IB_TableLookup : IB_Curve
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_TableLookup();
        private static TableLookup NewDefaultOpsObj(Model model)
            => new TableLookup(model);

        public List<IB_TableIndependentVariable> Variables
        {
            get => this.TryGetList<IB_TableIndependentVariable>();
            set => this.Set(value);
        }
        public List<double> OutputValues
        {
            get => this.TryGetList<double>();
            private set => this.Set(value);
        }


        public IB_TableLookup() :base(NewDefaultOpsObj)
        {
        }

        public void SetVariables(List<IB_TableIndependentVariable> vars)
        {
            this.Variables = vars;
        }

        public void SetOutputValues(List<double> vars)
        {
            foreach (var item in vars)
            {
                (this.GhostOSObject as TableLookup)?.addOutputValue(item);
            }
            this.OutputValues = vars;
        }

        public override Curve ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            //var variables = this.Children.OfType<IB_TableIndependentVariable>().ToList();
            foreach (var item in Variables)
            {
                obj.addIndependentVariable(item.ToOS(model));
            }

            foreach (var item in OutputValues)
            {
                obj.addOutputValue(item);
            }

            return obj;
        }

    }

    public sealed class IB_TableLookup_FieldSet
        : IB_FieldSet<IB_TableLookup_FieldSet, TableLookup>
    {
        private IB_TableLookup_FieldSet() { }
        
    }
}
