using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingDesuperheater : IB_Coil
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilHeatingDesuperheater(this.HeatingSource.To<IB_CoilDX>());
        private static CoilHeatingDesuperheater InitMethod(Model model) => new CoilHeatingDesuperheater(model);

        //Now supports CoilCoolingDXSingleSpeed and CoilCoolingDXTwoSpeed
        //TODO: will add later: RefrigerationCondenserAirCooled, RefrigerationCondenserEvaporativeCooled, RefrigerationCondenserWaterCooled
        private IB_Child HeatingSource => this.Children.GetChild<IB_CoilDX>();
        public IB_CoilHeatingDesuperheater(IB_CoilDX heatingSource) : base(InitMethod(new Model()))
        {
            var hs = new IB_Child(heatingSource, (obj) => this.SetHeatingSource(obj as IB_CoilDX));
            this.Children.Add(hs);
        }
        public void SetHeatingSource(IB_CoilDX heatingSource)
        {
            this.HeatingSource.Set(heatingSource);
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
        

        protected override ModelObject InitOpsObj(Model model)
        {
            var newObj = base.OnInitOpsObj(InitMethod, model).to_CoilHeatingDesuperheater().get();
            var newHS = this.HeatingSource.To<IB_CoilDX>().ToOS(model);
            newObj.setHeatingSource(newHS);
            return newObj;
        }

    }

    public sealed class IB_CoilHeatingDesuperheater_DataFieldSet
        : IB_FieldSet<IB_CoilHeatingDesuperheater_DataFieldSet, CoilHeatingDesuperheater>
    {
        private IB_CoilHeatingDesuperheater_DataFieldSet() { }

    }
}
