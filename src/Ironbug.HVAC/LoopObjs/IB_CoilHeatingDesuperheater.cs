using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingDesuperheater : IB_Coil
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilHeatingDesuperheater(this.HeatingSource);
        private static CoilHeatingDesuperheater NewDefaultOpsObj(Model model) => new CoilHeatingDesuperheater(model);

        //Now supports CoilCoolingDXSingleSpeed and CoilCoolingDXTwoSpeed
        //TODO: will add later: RefrigerationCondenserAirCooled, RefrigerationCondenserEvaporativeCooled, RefrigerationCondenserWaterCooled
        private IB_CoilDX HeatingSource => this.GetChild<IB_CoilDX>();
        public IB_CoilHeatingDesuperheater(IB_CoilDX heatingSource) : base(NewDefaultOpsObj(new Model()))
        {
            this.AddChild(heatingSource);
        }
        public void SetHeatingSource(IB_CoilDX heatingSource)
        {
            this.SetChild(heatingSource);
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            var newObj = (CoilHeatingDesuperheater)this.ToOS(model);
            var htSource = newObj.heatingSource().get();
            
            if (htSource.to_CoilCoolingDXSingleSpeed().is_initialized())
            {
                htSource.to_CoilCoolingDXSingleSpeed().get().addToNode(node);
            }else if (htSource.to_CoilCoolingDXTwoSpeed().is_initialized())
            {
                htSource.to_CoilCoolingDXTwoSpeed().get().addToNode(node);
            }

            return newObj.addToNode(node);
            

        }

        public override HVACComponent ToOS(Model model)
        {
            var newObj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            var newHS = this.HeatingSource.ToOS(model);
            newObj.setHeatingSource(newHS);
            return newObj;
        }
        

    }

    public sealed class IB_CoilHeatingDesuperheater_FieldSet
        : IB_FieldSet<IB_CoilHeatingDesuperheater_FieldSet, CoilHeatingDesuperheater>
    {
        private IB_CoilHeatingDesuperheater_FieldSet() { }

    }
}
