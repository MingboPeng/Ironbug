using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;
using NUnit.Framework;

namespace Ironbug.HVACTests
{

    public class HVACBaselineSysTest
    {
        [Test]
        public void Sys01_PTAC()
        {
            var exampleBuildingFile = TestHelper.ExampleBuildingFile;
            Assert.True(File.Exists(exampleBuildingFile));

            string saveFile = TestHelper.GenFileName;

            var m = OpenStudio.Model.load(exampleBuildingFile.ToPath()).get();
            var zoneNames = m.getThermalZones().Select(_=>_.nameString());
            Assert.True(zoneNames.Any());

            

            var noAirlp = new IB_NoAirLoop();
            var waterCoils = new List<IB_CoilHeatingWater>();
            foreach (var name in zoneNames)
            {
                var opzone = new IB_ThermalZone(name);

                var fan = new IB_FanConstantVolume();
                var heatingCoil = new IB_CoilHeatingWater();
                var coolingCoil = new IB_CoilCoolingDXSingleSpeed();
                var ptac = new IB_ZoneHVACPackagedTerminalAirConditioner(fan, heatingCoil, coolingCoil);

                opzone.AddZoneEquipment(ptac);
                noAirlp.AddThermalZones(opzone);
                waterCoils.Add(heatingCoil);
            }

            //hot water loop
            var hwlp = new IB_PlantLoop();
            var pump = new IB_PumpVariableSpeed();
            var boiler = new IB_BoilerHotWater();
            var setpointManager = new IB_SetpointManagerScheduled(67);

            //hot water supply side
            hwlp.AddToSupply(pump);

            var b1 = new IB_PlantLoopBranches();
            b1.Add(new List<IB_HVACObject>() { boiler });
            hwlp.AddToSupply(b1);

            hwlp.AddToSupply(setpointManager);
            
            //hot water demand side
            var b2 = new IB_PlantLoopBranches();
            foreach (var item in waterCoils)
            {
                b2.Add(new List<IB_HVACObject>() { item });
            }
        
            hwlp.AddToDemand(b2);

            var hvac = new IB_HVACSystem(
                new List<IB_AirLoopHVAC>() { noAirlp },
                new List<IB_PlantLoop>() { hwlp },
                new List<IB_AirConditionerVariableRefrigerantFlow>());


            var s = hvac.SaveHVAC(saveFile);
            Assert.True(s);

            //check the results
            var m2 = OpenStudio.Model.load(saveFile.ToPath()).get();
            var countOfeqps = m2.getZoneHVACPackagedTerminalAirConditioners().Where(_ => _.thermalZone().is_initialized()).Count();
            Assert.True(countOfeqps == zoneNames.Count());

            var countOfCoils = m2.getCoilHeatingWaters().Where(_ => _.plantLoop().is_initialized()).Count();
            Assert.True(countOfCoils == zoneNames.Count());
        }
    }
}
