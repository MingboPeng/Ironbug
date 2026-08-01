using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;

namespace Ironbug.HVAC.Curves
{
    public class IB_TableIndependentVariable : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_TableIndependentVariable();

        private static TableIndependentVariable NewDefaultOpsObj(Model model) => new TableIndependentVariable(model);

        public List<double> Values
        {
            get => this.TryGetList<double>();
            private set => this.Set(value);
        }

        public IB_TableIndependentVariable():base(NewDefaultOpsObj)
        {
        }

        public void SetValues(List<double> vars)
        {

            foreach (var item in vars)
            {
                (this.GhostOSObject as TableIndependentVariable)?.addValue(item);
            }
            this.Values = vars;

        }
        
        public TableIndependentVariable ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            foreach (var item in Values)
            {
                obj.addValue(item);
            }
            return obj;
        }
        
        
    }

    public sealed class IB_TableIndependentVariable_FieldSet 
        : IB_FieldSet<IB_TableIndependentVariable_FieldSet, TableIndependentVariable>
    {
        private IB_TableIndependentVariable_FieldSet() { }
    }
}
