using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Ironbug.HVAC
{
    public class IB_ElectricLoadCenterDistribution : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ElectricLoadCenterDistribution();

        private static ElectricLoadCenterDistribution NewDefaultOpsObj(Model model) => new ElectricLoadCenterDistribution(model);

        // DC => AC
        private IB_ElecInverter _inverter => this.GetChild<IB_ElecInverter>(0);
        private IB_ElecStorage _electricalStorage => this.GetChild<IB_ElecStorage>(1);

        // AC => DC
        private IB_ElectricLoadCenterStorageConverter _storageConverter => this.GetChild<IB_ElectricLoadCenterStorageConverter>(2);

        private IB_ElectricLoadCenterTransformer _transformer => this.GetChild<IB_ElectricLoadCenterTransformer>(3);

        [DataMember]
        public List<IB_Generator> Generators { get; private set; } = new List<IB_Generator>();

        public IB_ElectricLoadCenterDistribution() : base(NewDefaultOpsObj)
        {
        }


        public void SetInverter(IB_ElecInverter inverter)
        {
            this.SetChild(0, inverter);
        }

        public void SetElectricalStorage(IB_ElecStorage electricalStorage)
        {
            this.SetChild(1, electricalStorage);
        }

        public void SetStorageConverter(IB_ElectricLoadCenterStorageConverter storageConverter)
        {
            this.SetChild(2, storageConverter);
        }

        public void SetTransformer(IB_ElectricLoadCenterTransformer transformer)
        {
            this.SetChild(3, transformer);
        }

        public void SetGenerators(List<IB_Generator> generators)
        {
            this.Generators = generators;
        }
    


        public ElectricLoadCenterDistribution ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            if (_transformer != null)
                obj.setTransformer(this._transformer.ToOS(model));
            if (_storageConverter != null)
                obj.setStorageConverter(this._storageConverter.ToOS(model));
            if (_inverter != null)
                obj.setInverter(this._inverter.ToOS(model));
            if (_electricalStorage != null)
                obj.setElectricalStorage(this._electricalStorage.ToOS(model));

            foreach (var item in Generators)
            {
                var g = item.ToOS(model);
                obj.addGenerator(g);
            }


            return obj;
        }



    }

    public sealed class IB_ElectricLoadCenterDistribution_FieldSet
       : IB_FieldSet<IB_ElectricLoadCenterDistribution_FieldSet, ElectricLoadCenterDistribution>
    {

        private IB_ElectricLoadCenterDistribution_FieldSet() { }

    }


}
