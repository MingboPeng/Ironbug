using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;
using NUnit.Framework;

namespace Ironbug.HVACTests
{
    public class HVACWorkflowTest
    {
        private string GenFileName => TestHelper.GenFileName;

        private HVAC.IB_SizingPlant setSizingDefault(HVAC.IB_SizingPlant sizingPlant)
        {
            var szFields = HVAC.IB_SizingPlant_FieldSet.Value;
            var sizing = sizingPlant.Duplicate() as HVAC.IB_SizingPlant;
        
            sizing.SetFieldValue(szFields.LoopType, "Cooling");
            sizing.SetFieldValue(szFields.DesignLoopExitTemperature, 7.22);
            sizing.SetFieldValue(szFields.LoopDesignTemperatureDifference, 6.67);
            
            return sizing;
        }

        [Test]
        public void AddVRF_Test()
        {
            var model = new OpenStudio.Model();
            var z1 = new OpenStudio.ThermalZone(model);
            var z2 = new OpenStudio.ThermalZone(model);
            var zones = model.getThermalZones();
            Assert.IsTrue(zones.Any());


            var savePath = GenFileName;

            var zoneNames = model.getThermalZones().Select(_ => _.nameString());
            Assert.True(zoneNames.Any());

            var noAirlp = new IB_NoAirLoop();
            var vrfSys = new IB_AirConditionerVariableRefrigerantFlow();
            foreach (var item in zoneNames)
            {
                var vrf = new IB_ZoneHVACTerminalUnitVariableRefrigerantFlow();
                var zone = new IB_ThermalZone(item);
                zone.AddZoneEquipment(vrf);

                noAirlp.AddThermalZones(zone);
                vrfSys.AddTerminal(vrf);
            }
         
            var hvac = new IB_HVACSystem(new List<IB_AirLoopHVAC>() { noAirlp }, new List<IB_PlantLoop>(), new List<IB_AirConditionerVariableRefrigerantFlow>() { vrfSys });
            
            var done = hvac.SaveHVAC(savePath);
            done &= File.Exists(savePath);
            Assert.IsTrue(done);

            var md2 = OpenStudio.Model.load(savePath.ToPath()).get();
            var zones2 = md2.getThermalZones();
            Assert.IsTrue(zones2.Count() == zoneNames.Count());

            foreach (var item in zones2)
            {
                var c = item.equipment().Count();
                var eqp = item.equipment().First();
                Assert.IsTrue(eqp.to_ZoneHVACTerminalUnitVariableRefrigerantFlow().is_initialized());
            }
            var savedVrfs = md2.getZoneHVACTerminalUnitVariableRefrigerantFlows();
            Assert.IsTrue(savedVrfs.Count() == zoneNames.Count());

        }

        [Test]
        public void IBChiller_Loop_Test()
        {
            //var md1 = new OpenStudio.Model();
            var cwlp = new IB_PlantLoop();
            var cwsz = setSizingDefault( new IB_SizingPlant());
            cwlp.SetSizingPlant(cwsz);

            var cdlp = new IB_PlantLoop();


            var chiller = new IB_ChillerElectricEIR();

            var branches = new IB_PlantLoopBranches();
            var branch = new List<IB_HVACObject>();
            branch.Add(chiller);
            branches.Add(branch);

            var branches2 = new IB_PlantLoopBranches();
            var branch2 = new List<IB_HVACObject>();
            branch2.Add(chiller);
            branches2.Add(branch2);


            cwlp.AddToSupply(branches);
            cdlp.AddToDemand(branches2);

            var md1 = new OpenStudio.Model();
            cwlp.ToOS(md1);
            cdlp.ToOS(md1);

            string saveFile = GenFileName;
            md1.Save(saveFile);

            var md2 = OpenStudio.Model.load(saveFile.ToPath()).get();
            var chillers = md2.getChillerElectricEIRs();
            var findChiller = chillers.Count() == 1;
            Assert.True(findChiller);


            var cwloopSz = chillers.First().plantLoop().get().sizingPlant();
            var isCooling = cwloopSz.loopType() == "Cooling";
            Assert.True(isCooling);

        }

        [Test]
        public void PlantBranches_Test()
        {
            

            var plantloop = new IB_PlantLoop();

            //var boiler0 = new IB_BoilerHotWater();
            //boiler0.SetAttribute(IB_BoilerHotWater_FieldSet.Name, "boiler00");
            //plantloop.AddToSupply(boiler0);

            var branches = new IB_PlantLoopBranches();
            var branch = new List<IB_HVACObject>();


            var boiler1 = new IB_BoilerHotWater();
            boiler1.SetFieldValue(IB_BoilerHotWater_FieldSet.Value.Name, "boiler1");
            branch.Add(boiler1);
            branch.Add(new IB_PumpConstantSpeed());

            branches.Add(branch);
            //plantloop.AddToSupply(branches);

            //add the second branch

            var branch2 = new List<IB_HVACObject>();
            var boiler2 = new IB_BoilerHotWater();
            boiler2.SetFieldValue(IB_BoilerHotWater_FieldSet.Value.Name, "boiler2");
            branch2.Add(boiler2);
            branch2.Add(new IB_PumpVariableSpeed());
            branches.Add(branch2);

            var branch3 = new List<IB_HVACObject>();
            branch3.Add(new IB_PumpVariableSpeed());
            branches.Add(branch3);

            var branch4 = new List<IB_HVACObject>();
            branch4.Add(new IB_PumpConstantSpeed());
            branches.Add(branch4);

            plantloop.AddToSupply(branches);

            var md1 = new OpenStudio.Model();
            plantloop.ToOS(md1);




            var findboilers = md1.getBoilerHotWaters().Count() == 2;
            Assert.True(findboilers);

            var findPumpv = md1.getPumpVariableSpeeds().Count == 2;
            Assert.True(findPumpv);

            var findPumpC = md1.getPumpConstantSpeeds().Count == 2;
            Assert.True(findPumpC);
            //var checkedTheFirstBoiler = md1.getPlantLoops().First().supplyMixer().inletModelObject(0).get().nameString() == "boiler1";


            string saveFile = GenFileName;
            var success = md1.Save(saveFile);
            Assert.True(success);


            var md2 = OpenStudio.Model.load(saveFile.ToPath()).get();
            findboilers = md2.getBoilerHotWaters().Count() == 2;
            Assert.True(findboilers);

            findPumpv = md2.getPumpVariableSpeeds().Count == 2;
            Assert.True(findPumpv);

            findPumpC = md2.getPumpConstantSpeeds().Count == 2;
            Assert.True(findPumpC);


        }

        [Test]
        public void AirloopWorkflow_Test()
        {
            //var md1 = new OpenStudio.Model();
            var airflow = new HVAC.IB_AirLoopHVAC();
            var oa = new IB_OutdoorAirSystem();
            var erv = new IB_HeatExchangerAirToAirSensibleAndLatent();
            var coil = new HVAC.IB_CoilHeatingWater();
            var fan = new IB_FanConstantVolume();

            var f = new IB_Field("LatentEffectivenessat100CoolingAirFlow", "LatentEffectivenessat100CoolingAirFlow", typeof(double));
            erv.SetFieldValue(f, 0.555);


            oa.SetHeatExchanger(erv);

            airflow.AddToSupplySide(oa);
            airflow.AddToSupplySide(coil);
            airflow.AddToSupplySide(fan);
            //airflow.ToOS(md1);

            var md2 = new OpenStudio.Model();
            airflow.ToOS(md2);

            var success1 = coil.IsInModel(md2);
            success1 &= coil.IsInModel(md2);
            success1 &= fan.IsInModel(md2); 
            success1 &= oa.IsInModel(md2);
            success1 &= erv.IsInModel(md2);
            success1 &= md2.getHeatExchangerAirToAirSensibleAndLatents().First().latentEffectivenessat100CoolingAirFlow() == 0.555;
            Assert.True(success1);


            string saveFile = GenFileName;
            var success = md2.Save(saveFile);
            Assert.True(success);

            var md3 = OpenStudio.Model.load(saveFile.ToPath()).get();

            var success3 = coil.IsInModel(md3);
            success3 &= coil.IsInModel(md3);
            success3 &= fan.IsInModel(md3);
            success3 &= oa.IsInModel(md3);
            success3 &= erv.IsInModel(md3);
            success3 &= md3.getHeatExchangerAirToAirSensibleAndLatents().First().latentEffectivenessat100CoolingAirFlow() == 0.555;
            Assert.True(success3);
        }


        [Test]
        public void HWloopWorkflow_Test()
        {
            var plant = new HVAC.IB_PlantLoop();

            HVAC.IB_SizingPlant sizingPlant = new HVAC.IB_SizingPlant();
            var szFields = HVAC.IB_SizingPlant_FieldSet.Value;
            sizingPlant.SetFieldValue(szFields.LoopType, "Heating");

            var sizing = sizingPlant.Duplicate() as HVAC.IB_SizingPlant;
            
            plant.SetSizingPlant(sizing);

            var plantFields = HVAC.IB_PlantLoop_FieldSet.Value;
            plant.SetFieldValue(plantFields.Name, "Hot Water Loop");
            plant.SetFieldValue(plantFields.FluidType, "Water");

            var newPlant = plant.Duplicate() as IB_PlantLoop;

            var md1 = new OpenStudio.Model();
            newPlant.ToOS(md1);
            
            var success = newPlant.IsInModel(md1);
            Assert.True(success);

            success = md1.Save(GenFileName);
            Assert.True(success);
        }
        [Test]
        public void VRF_Test()
        {
            //var md1 = new OpenStudio.Model();
            var vrf = new IB_AirConditionerVariableRefrigerantFlow();
            var vrfTerm = new IB_ZoneHVACTerminalUnitVariableRefrigerantFlow();
            var zone = new IB_ThermalZone();

            zone.AddZoneEquipment(vrfTerm);

            vrf.AddTerminal(vrfTerm);

            var md1 = new OpenStudio.Model();
            vrf.ToOS(md1);
            zone.ToOS(md1);

            var success = md1.getZoneHVACTerminalUnitVariableRefrigerantFlows().Any();
            Assert.True(success);


            string saveFile = GenFileName;
            success = md1.Save(saveFile);
            Assert.True(success);


            var md3 = OpenStudio.Model.load(saveFile.ToPath()).get();
            success = md3.getZoneHVACTerminalUnitVariableRefrigerantFlows().Any();
            Assert.True(success);

        }

        [Test]
        public void VAVReheat_Test()
        {
            //var md1 = new OpenStudio.Model();
            var airLp = new IB_AirLoopHVAC();
            var hwLp = new IB_PlantLoop();
            var zone1 = new IB_ThermalZone();
            var zone2 = new IB_ThermalZone();

            var reHeat = new IB_AirTerminalSingleDuctVAVReheat();
            var coil = new IB_CoilHeatingWater();
            coil.SetFieldValue(IB_CoilHeatingWater_FieldSet.Value.RatedInletAirTemperature, 15.6);
            reHeat.SetReheatCoil(coil);

            //reHeat.ToPuppetHost();
            var reHeatPuppet1 = (IB_AirTerminal)reHeat.Duplicate();
            reHeatPuppet1.SetTrackingID();
            zone1.SetAirTerminal(reHeatPuppet1);
            var firstCoilID = reHeatPuppet1.Children.First().GetTrackingID();
            Console.WriteLine($"ReheatCoil 1: {firstCoilID}");
         

            var reHeatPuppet2 = (IB_AirTerminal)reHeat.Duplicate();
            reHeatPuppet2.SetTrackingID();
            zone2.SetAirTerminal(reHeatPuppet2);
            var secondCoilID = reHeatPuppet2.Children.First().GetTrackingID();
            Console.WriteLine($"ReheatCoil 2: {secondCoilID}");

            var airBranches = new IB_AirLoopBranches();
            airBranches.Add(new List<IB_HVACObject>() { zone1 });
            airBranches.Add(new List<IB_HVACObject>() { zone2 });

            var branches = new IB_PlantLoopBranches();
            branches.Add(new List<IB_HVACObject>() { coil });
            hwLp.AddToDemand(branches);

            var fan = new IB_FanConstantVolume();
            airLp.AddToSupplySide(fan);
            airLp.AddToDemandSide(airBranches);

            var md1 = new OpenStudio.Model();
            airLp.ToOS(md1);
            hwLp.ToOS(md1);
            
            var reheatTerminals
                = md1
                .getAirTerminalSingleDuctVAVReheats();
            Assert.True(reheatTerminals.Count() == 2);


            var success 
                = reheatTerminals
                .Select(_ => _.reheatCoil().plantLoop().is_initialized())
                .Where(_ => _ == true).Count()==2;
            Assert.True(success);


            success &= reheatTerminals.First().nameString().EndsWith("1") && reheatTerminals.First().reheatCoil().nameString().EndsWith("1");
            Assert.True(success);


            string saveFile = GenFileName;
            success = md1.Save(saveFile);
            Assert.True(success);

        }

        [Test]
        public void DuplicateWithChild()
        {
            var oa = new IB_OutdoorAirSystem();
            var oac = new IB_ControllerOutdoorAir();
            oac.SetFieldValue(IB_ControllerOutdoorAir_FieldSet.Value.EconomizerControlType, "DifferentialDryBulb");
            oa.SetController(oac);

            var newOa = oa.Duplicate() as IB_OutdoorAirSystem;

            var s = newOa.Children
                .Get<IB_ControllerOutdoorAir>()
                .CustomAttributes
                .Where(_=>_.Value.ToString() == "DifferentialDryBulb")
                .Any();

            Assert.True(s);
        }
    }
}
