using System;
using System.Collections.Generic;
using System.Linq;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ironbug.HVACTests
{
    [TestClass]
    public class HVACWorkflowTest
    {
       
        [TestMethod]
        public void IBChiller_Loop_Test()
        {
            var md1 = new OpenStudio.Model();
            var cwlp = new IB_PlantLoop();
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

            cwlp.ToOS(md1);
            cdlp.ToOS(md1);

            string saveFile = @"..\..\..\..\doc\osmFile\empty_Added_.osm";
            md1.Save(saveFile);

            var findChiller = md1.getChillerElectricEIRs().Count() == 1;
            Assert.IsTrue(findChiller);

        }

        [TestMethod]
        public void PlantBranches_Test()
        {
            var md1 = new OpenStudio.Model();

            var plantloop = new IB_PlantLoop();

            //var boiler0 = new IB_BoilerHotWater();
            //boiler0.SetAttribute(IB_BoilerHotWater_DataFields.Name, "boiler00");
            //plantloop.AddToSupply(boiler0);

            var branches = new IB_PlantLoopBranches();
            var branch = new List<IB_HVACObject>();


            var boiler1 = new IB_BoilerHotWater();
            boiler1.SetAttribute(IB_BoilerHotWater_DataFields.Value.Name, "boiler1");
            branch.Add(boiler1);
            branch.Add(new IB_PumpConstantSpeed());

            branches.Add(branch);
            //plantloop.AddToSupply(branches);

            //add the second branch

            var branch2 = new List<IB_HVACObject>();
            var boiler2 = new IB_BoilerHotWater();
            boiler2.SetAttribute(IB_BoilerHotWater_DataFields.Value.Name, "boiler2");
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


            plantloop.ToOS(md1);




            var findboilers = md1.getBoilerHotWaters().Count() == 2;
            var findPumpv = md1.getPumpVariableSpeeds().Count == 2;
            var findPumpC = md1.getPumpConstantSpeeds().Count == 2;
            //var checkedTheFirstBoiler = md1.getPlantLoops().First().supplyMixer().inletModelObject(0).get().nameString() == "boiler1";


            var success = findPumpv && findPumpC && findboilers;

            string saveFile = @"..\..\..\..\doc\osmFile\empty_Added_.osm";
            success &= md1.Save(saveFile);


            Assert.IsTrue(success);

        }

        [TestMethod]
        public void AirloopWorkflow_Test()
        {
            var md1 = new OpenStudio.Model();

            var airflow = new HVAC.IB_AirLoopHVAC();
            var coil = new HVAC.IB_CoilHeatingWater();
            var fan = new IB_FanConstantVolume();


            airflow.AddToSupplySide(coil);
            airflow.AddToSupplySide(fan);
            airflow.ToOS(md1);

            var md2 = new OpenStudio.Model();
            airflow.ToOS(md2);

            var success1 = coil.IsInModel(md1);
            var success2 = coil.IsInModel(md2);
            var success_fan = fan.IsInModel(md1);

            string saveFile = @"..\..\..\..\doc\osmFile\empty_Added_.osm";
            var success3 = md2.Save(saveFile);

            var success = success1 && success2 && success3 & success_fan;

            Assert.IsTrue(success);
        }
    }
}
