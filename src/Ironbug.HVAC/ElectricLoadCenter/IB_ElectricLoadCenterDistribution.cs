using System;
using System.Collections.Generic;
using System.Linq;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ElectricLoadCenterDistribution : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ElectricLoadCenterDistribution();

        private static ElectricLoadCenterDistribution NewDefaultOpsObj(Model model) => new ElectricLoadCenterDistribution(model);

        private IB_ModelObject _inverter => this.GetChild<IB_ModelObject>(0);
        private IB_ModelObject _electricalStorage => this.GetChild<IB_ModelObject>(1);
        private IB_ElectricLoadCenterStorageConverter _storageConverter => this.GetChild<IB_ElectricLoadCenterStorageConverter>(2);
        private IB_ElectricLoadCenterTransformer _transformer => this.GetChild<IB_ElectricLoadCenterTransformer>(3);
        public IB_ElectricLoadCenterDistribution() : base(NewDefaultOpsObj)
        {
        }
       
        public ElectricLoadCenterDistribution ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            obj.setTransformer(this._transformer.ToOS(model));
            obj.setStorageConverter(this._storageConverter.ToOS(model));
            obj.setInverter()

         
            return obj;
        }

        public void ApplyAttributesToObj(Model model, ModelObject osObj, Dictionary<string, string> idMapper)
        {
            base.ApplyAttributesToObj(model, osObj);
            var obj = osObj as ElectricLoadCenterDistribution;
            // replace mapper
            var mappedBody = obj.body();
            foreach (var id in idMapper)
            {
                mappedBody = mappedBody.Replace(id.Key, id.Value);
            }

            obj.setBody(mappedBody);
        }


    }

    public sealed class IB_ElectricLoadCenterDistribution_FieldSet
       : IB_FieldSet<IB_ElectricLoadCenterDistribution_FieldSet, ElectricLoadCenterDistribution>
    {

        private IB_ElectricLoadCenterDistribution_FieldSet() { }

    }


}
