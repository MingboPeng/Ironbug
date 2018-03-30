using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingGas : IB_Coil
    {
        private static CoilHeatingGas InitMethod(Model model) => new CoilHeatingGas(model);

        public IB_CoilHeatingGas() : base(InitMethod(new Model()))
        {
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((CoilHeatingGas)this.ToOS(model)).addToNode(node);
        }

        public override IB_ModelObject Duplicate()
        {
            return base.DuplicateIBObj(() => new IB_CoilHeatingGas());
        }

        public override ModelObject ToOS(Model model)
        {
            return base.ToOS(InitMethod, model).to_CoilHeatingGas().get();
        }
    }

    public class IB_CoilCoolingGas_DataFieldSet : IB_DataFieldSet
    {
        protected override IddObject RefIddObject => new IdfObject(CoilHeatingGas.iddObjectType()).iddObject();

        protected override Type ParentType => typeof(CoilHeatingGas);

    }
}
