using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;
using Xunit;
using Xunit.Abstractions;

namespace Ironbug.HVACTests
{

    public class HVACBaselineSysTest
    {

        ITestOutputHelper output;

        OpenStudio.Model md1 = new OpenStudio.Model();
        string exampleBuildingFile = @"..\..\..\..\doc\osmFile\BuildingForTest.osm";
        

        [Fact]
        public void Sys01_PTAC()
        {
            string saveFile = @"..\..\..\..\doc\osmFile\HVACBaseline\sys01_PTAC.osm";
            File.Copy(exampleBuildingFile, saveFile, true);

            var m = OpenStudio.Model.load(OpenStudio.OpenStudioUtilitiesCore.toPath(saveFile)).get();
            var zoneNames = m.getThermalZones().Select(_=>_.nameString());

            var fan = new IB_FanConstantVolume();
            var heatingCoil = new IB_CoilHeatingWater();
            var coolingCoil = new IB_CoilCoolingDXSingleSpeed();

            var ptac = new IB_ZoneHVACPackagedTerminalAirConditioner(fan, heatingCoil, coolingCoil);
            //var eqpHost = ptac.ToPuppetHost();


            var noAirlp = new IB_NoAirLoop();

            foreach (var name in zoneNames)
            {
                var opzone = new HVAC.BaseClass.IB_ThermalZone(name);
                
                var eqpPuppet = ptac.Duplicate() as HVAC.BaseClass.IB_ZoneEquipment;
                opzone.AddZoneEquipment(eqpPuppet);

                noAirlp.AddThermalZones(opzone);
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
            b2.Add(new List<IB_HVACObject>() { heatingCoil });
            hwlp.AddToDemand(b2);

            var hvac = new IB_HVACSystem(
                new List<IB_AirLoopHVAC>() { noAirlp },
                new List<IB_PlantLoop>() { hwlp },
                new List<IB_AirConditionerVariableRefrigerantFlow>());


            var s = hvac.SaveHVAC(saveFile);

            //check the results
            m = OpenStudio.Model.load(OpenStudio.OpenStudioUtilitiesCore.toPath(saveFile)).get();
            var countOfeqps = m.getZoneHVACPackagedTerminalAirConditioners().Where(_ => _.thermalZone().is_initialized()).Count();
            s &= countOfeqps == zoneNames.Count();

            var countOfCoils = m.getCoilHeatingWaters().Where(_ => _.plantLoop().is_initialized()).Count();
            s &= countOfCoils == zoneNames.Count();

            Assert.True(s);
        }
    }
}
