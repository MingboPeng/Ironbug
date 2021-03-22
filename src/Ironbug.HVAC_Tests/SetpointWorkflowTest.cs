using System.Collections.Generic;
using System.Linq;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;
using NUnit.Framework;

namespace Ironbug.HVACTests
{
    public class SetpointWorkflowTest
    {
        [Test]
        public void SpInAirloopFirst_Test()
        {
            var md1 = new OpenStudio.Model();
            var af = new IB_AirLoopHVAC();
            var coil = new IB_CoilHeatingWater();
            var setPt = new IB_SetpointManagerOutdoorAirReset();
            af.AddToSupplySide(setPt);
            af.AddToSupplySide(coil);
            af.AddToSupplySide(new IB_FanConstantVolume());

            af.ToOS(md1);


            string saveFile = @"..\..\..\..\doc\osmFile\empty_Saved.osm";
            var success = md1.Save(saveFile);


            var addedSetPt = md1.getAirLoopHVACs()[0].SetPointManagers().First();
            var objAfterSetp = addedSetPt.setpointNode().get().outletModelObject().get();
            success &= objAfterSetp.comment() == coil.GetTrackingID();

            Assert.True(success);
        }

        [Test]
        public void SpInAirloopLast_Test()
        {
            var md1 = new OpenStudio.Model();
            var af = new IB_AirLoopHVAC();
            var coil = new IB_CoilHeatingWater();
            var fan = new IB_FanConstantVolume();
            var setPt = new IB_SetpointManagerOutdoorAirReset();
            af.AddToSupplySide(coil);
            af.AddToSupplySide(fan);
            af.AddToSupplySide(setPt);
            

            af.ToOS(md1);


            string saveFile = @"..\..\..\..\doc\osmFile\empty_Saved.osm";
            var success = md1.Save(saveFile);


            var addedSetPt = md1.getAirLoopHVACs()[0].SetPointManagers().First();
            var objAfterSetp = addedSetPt.setpointNode().get().inletModelObject().get();
            success &= objAfterSetp.comment() == fan.GetTrackingID();

            Assert.True(success);
        }

        [Test]
        public void SpInPlantloopOnly_Test()
        {
            var md1 = new OpenStudio.Model();
            var pl = new IB_PlantLoop();
            
            var setPt = new IB_SetpointManagerOutdoorAirReset();
            pl.AddToSupply(setPt);

            pl.ToOS(md1);
            
            string saveFile = @"..\..\..\..\doc\osmFile\empty_Saved.osm";
            var success = md1.Save(saveFile);


            var addedSetPt = md1.getPlantLoops()[0].supplyInletNode().setpointManagers().First();
            success &= addedSetPt.comment() == setPt.GetTrackingID();

            Assert.True(success);
        }

        [Test]
        public void SpInPlantloopAtFirst_Test()
        {
            var md1 = new OpenStudio.Model();
            var pl = new IB_PlantLoop();

            var boiler = new IB_BoilerHotWater();
            var branches = new IB_PlantLoopBranches();
            var branch = new List<IB_HVACObject>();
            branch.Add(boiler);
            branches.Add(branch);

            var setPt = new IB_SetpointManagerOutdoorAirReset();
            pl.AddToSupply(setPt);
            pl.AddToSupply(branches);
            pl.ToOS(md1);

            string saveFile = @"..\..\..\..\doc\osmFile\empty_Saved.osm";
            var success = md1.Save(saveFile);


            var addedSetPt = md1.getPlantLoops()[0].supplyInletNode().setpointManagers().First();
            success &= addedSetPt.comment() == setPt.GetTrackingID();

            Assert.True(success);
        }

        [Test]
        public void SpInPlantloopAtFirstWithPump_Test()
        {
            var md1 = new OpenStudio.Model();
            var pl = new IB_PlantLoop();
            var pump = new IB_PumpConstantSpeed();
            var boiler = new IB_BoilerHotWater();
            var branches = new IB_PlantLoopBranches();
            var branch = new List<IB_HVACObject>();
            branch.Add(boiler);
            branches.Add(branch);

            var setPt = new IB_SetpointManagerOutdoorAirReset();
            pl.AddToSupply(setPt);
            pl.AddToSupply(pump);
            pl.AddToSupply(branches);
            pl.ToOS(md1);

            string saveFile = @"..\..\..\..\doc\osmFile\empty_Saved.osm";
            var success = md1.Save(saveFile);


            var addedSetPt = md1.getPlantLoops()[0].supplyInletNode().setpointManagers().First();
            success &= addedSetPt.comment() == setPt.GetTrackingID();

            Assert.True(success);
        }

        [Test]
        public void SpInPlantloopAfterPump_Test()
        {
            var md1 = new OpenStudio.Model();
            var pl = new IB_PlantLoop();
            var pump = new IB_PumpConstantSpeed();
            var boiler = new IB_BoilerHotWater();
            var branches = new IB_PlantLoopBranches();
            var branch = new List<IB_HVACObject>();
            branch.Add(boiler);
            branches.Add(branch);

            var setPt = new IB_SetpointManagerOutdoorAirReset();
            pl.AddToSupply(pump);
            pl.AddToSupply(setPt);
            pl.AddToSupply(branches);
            pl.ToOS(md1);

            string saveFile = @"..\..\..\..\doc\osmFile\empty_Saved.osm";
            var success = md1.Save(saveFile);

            var addedSetPt = md1.getPlantLoops()[0].SetPointManagers().First();
            var objAfterSetp = addedSetPt.setpointNode().get().inletModelObject().get();
            success &= objAfterSetp.comment() == pump.GetTrackingID();
            
            Assert.True(success);
        }

        [Test]
        public void SpInPlantloopAfterBranch_Test()
        {
            var md1 = new OpenStudio.Model();
            var pl = new IB_PlantLoop();
            var pump = new IB_PumpConstantSpeed();
            var boiler = new IB_BoilerHotWater();
            var branches = new IB_PlantLoopBranches();
            var branch = new List<IB_HVACObject>();
            branch.Add(boiler);
            branches.Add(branch);

            var setPt = new IB_SetpointManagerOutdoorAirReset();
            pl.AddToSupply(pump);
            pl.AddToSupply(branches);
            pl.AddToSupply(setPt);
            
            pl.ToOS(md1);

            string saveFile = @"..\..\..\..\doc\osmFile\empty_Saved.osm";
            var success = md1.Save(saveFile);

            var addedSetPt = md1.getPlantLoops()[0].supplyOutletNode().setpointManagers().First();
            success &= addedSetPt.comment() == setPt.GetTrackingID();

            Assert.True(success);
        }
    }
}
