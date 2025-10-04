using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Ironbug.HVAC
{
    public class IB_ElectricLoadCenter
    {
        [DataMember]
        public List<IB_ElectricLoadCenterDistribution> SubPanels { get; private set; }
        [DataMember]
        public IB_ElectricLoadCenterTransformer PowerInTransformer { get; private set; }
        [DataMember]
        public IB_ElectricLoadCenterTransformer PowerOutTransformer { get; private set; }

        public IB_ElectricLoadCenter()
        {
        }

        public void SetSubPanels(List<IB_ElectricLoadCenterDistribution> subPanels)
        {
            SubPanels = subPanels;
        }

        public void SetPowerInTransformer(IB_ElectricLoadCenterTransformer transformer)
        {
            PowerInTransformer = transformer;
        }

        public void SetPowerOutTransformer(IB_ElectricLoadCenterTransformer transformer)
        {
            PowerOutTransformer = transformer;
        }

        public bool SaveELC(string filePath)
        {
            var osmFile = filePath;

            //get Model from file if exists
            var model = IB_Utility.GetOrNewModel(osmFile);

            if (SubPanels != null)
            {
                foreach (var item in SubPanels)
                {
                    var added = item.ToOS(model);
                }
            }

            if (PowerInTransformer != null)
            {
                var added = PowerInTransformer.ToOS(model);
            }

            if (PowerOutTransformer != null)
            {
                var added = PowerOutTransformer.ToOS(model);
            }

            //save osm file
            var osmPath = OpenStudio.OpenStudioUtilitiesCore.toPath(filePath);
            return model.save(osmPath, true);

        }


    }
}
