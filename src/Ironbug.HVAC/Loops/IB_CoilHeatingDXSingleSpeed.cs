using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingDXSingleSpeed: IB_Coil
    {
        private static CoilHeatingDXSingleSpeed InitMethod(Model model) => new CoilHeatingDXSingleSpeed(model);

        public IB_CoilHeatingDXSingleSpeed() : base(InitMethod(new Model()))
        {

        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((CoilHeatingDXSingleSpeed)this.ToOS(model)).addToNode(node);
        }

        public override IB_ModelObject Duplicate()
        {
            return base.DuplicateIBObj(() => new IB_CoilHeatingDXSingleSpeed());
        }

        public override ModelObject ToOS(Model model)
        {
            return base.ToOS(InitMethod, model).to_CoilHeatingDXSingleSpeed().get();
        }

    }

    public sealed class IB_CoilHeatingDXSingleSpeed_DataFieldSet
        : IB_DataFieldSet<IB_CoilHeatingDXSingleSpeed_DataFieldSet, CoilHeatingDXSingleSpeed>
    {
        private IB_CoilHeatingDXSingleSpeed_DataFieldSet() { }

    }
}
