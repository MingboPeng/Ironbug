using Ironbug.HVAC;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenStudio;
using System;
using System.Linq;

namespace Ironbug.HVACTests
{
    [TestClass]
    public class OpsTest
    {
        [TestMethod]
        public void OS_Curve_Clone_Test()
        {
            var model = new Model();
            var c = new CurveBicubic(model);
            c.setCoefficient1Constant(22.22);

            var h1 = c.handle().__str__();

            var model2 = new Model();
            var c2 = c.clone(true);
            model2.addObject(c2);

            var h2 = c2.handle().__str__();

            string saveFile = @"..\..\..\..\doc\osmFile\empty_Added_.osm";
            var success = model2.Save(saveFile);
            success &= model2.getCurveBicubics().Any();

            Assert.IsTrue(success);
        }

        [TestMethod]
        public void OS_Curve_Test()
        {
            var model = new Model();
            var c = new CurveBicubic(model);
            c.setCoefficient1Constant(22.22);

            var model2 = new Model();
            var boiler = new BoilerHotWater(model2);
            var cv = ((Curve)c).toIdfObject().clone(true);
            var str = cv.__str__();

            //var newCrv = cv.to_Curve().get();
            var newCrv = model2.addObject(cv).get();
            boiler.setNormalizedBoilerEfficiencyCurve(newCrv.to_Curve().get());

            string saveFile = @"..\..\..\..\doc\osmFile\empty_Added_.osm";
            var success = model2.Save(saveFile);
            success &= model2.getCurveBicubics().Any();

            Assert.IsTrue(success);
        }

        [TestMethod]
        public void OS_Fields_Test()
        {
            //var f = new OS_HeatExchanger_AirToAir_SensibleAndLatentFields(1);
            //var v = f.valueName();
            //var fValues = OS_HeatExchanger_AirToAir_SensibleAndLatentFields.getValues().asVector();
            //var fieldNames = fValues.Select(_ => new OS_HeatExchanger_AirToAir_SensibleAndLatentFields(_).valueName());

            //var objs = new OpenStudio.IddFileAndFactoryWrapper().objects().Select(_=>_.name());
            var hxIDD = new IddFileAndFactoryWrapper().getObject("OS:Boiler:HotWater").get();

            var fields = hxIDD.nonextensibleFields()
                .Where(_ =>
                {
                    var dataType = _.properties().type.valueDescription();
                    var result = dataType.ToLower() != "object-list" ? true : !_.name().ToLower().Contains("node");
                    result &= dataType.ToLower() != "node";
                    result &= dataType.ToLower() != "handle";

                    return result;
                })
            .Select(_ => $"N:{_.name()} - T:{_.properties().type.valueName()}");
            //new OpenStudio.IddFieldType().
            //new OpenStudio.IddObjectTypeSet().asVector().
            ////var a = 1;
        }

        [TestMethod]
        public void EP_Fields_Test()
        {
            var f2 = new HeatExchanger_AirToAir_SensibleAndLatentFields();
            var v2 = f2.value();
            var obj = new HeatExchangerAirToAirSensibleAndLatent(new Model());

            var iddObj = obj.iddObject();
            var objList = iddObj.objectLists().asVector();
            var objmemo = iddObj.properties().memo;
            var values = HeatExchanger_AirToAir_SensibleAndLatentFields.getValues().asVector();
            var aa = new HeatExchanger_AirToAir_SensibleAndLatentFields().valueName();

            var names = OpenStudio.OpenStudioUtilitiesIdd.getIddKeyNames(iddObj, 1);
            var a = new baseUnitConversionFactor();
        }

        [TestMethod]
        public void UnderstandBranches2_Test()
        {
            var md1 = new OpenStudio.Model();

            var plantloop = new OpenStudio.PlantLoop(md1);

            var boiler1 = new OpenStudio.BoilerHotWater(md1);
            var boiler2 = new OpenStudio.BoilerHotWater(md1);
            var boiler3 = new OpenStudio.BoilerHotWater(md1);
            var pump1 = new OpenStudio.PumpConstantSpeed(md1);
            var pump2 = new OpenStudio.PumpVariableSpeed(md1);

            boiler1.setName("boiler1");
            boiler2.setName("boiler2");
            boiler3.setName("boiler3");

            plantloop.addSupplyBranchForComponent(boiler1);
            var node = plantloop.supplyMixer().inletModelObjects().Last().to_Node().get();
            pump1.addToNode(node);

            plantloop.addSupplyBranchForComponent(boiler2);
            node = plantloop.supplyMixer().inletModelObjects().Last().to_Node().get();
            pump2.addToNode(node);

            plantloop.addSupplyBranchForComponent(boiler3);

            //var components = plantloop.supplyComponents(boiler1.iddObject().type());

            //var success = components.First().nameString() == "boiler1";

            string saveFile = @"..\..\..\..\doc\osmFile\empty_Added_.osm";
            var success = md1.Save(saveFile);

            Assert.IsTrue(success);
        }

        [TestMethod]
        public void addObject_Test()
        {
            var md1 = new OpenStudio.Model();
            var zone = new OpenStudio.ThermalZone(md1);
            zone.setName("a zone name");
            zone.setMultiplier(2);
            zone.setUseIdealAirLoads(true);
            var siz = new OpenStudio.SizingZone(md1, zone);
            siz.setZoneHeatingDesignSupplyAirTemperature(20);
            var comment = "comment as an ID";
            siz.setComment(comment);
            var id1 = zone.handle();
            var zoneSt = zone.__str__();
            var sizSt = siz.__str__();

            var md2 = new OpenStudio.Model();
            var zone2 = md2.addObject(zone).get().to_ThermalZone().get();
            var sizing2 = md2.addObject(siz).get().to_SizingZone().get();

            var isTrue = sizing2.EqualEqual(siz);

            var zone2St = zone2.__str__();
            var sizing2st = sizing2.to_SizingZone().get().__str__();

            var tp = sizing2.iddObject().type();
            var objs = md2.getObjectsByType(tp);

            var success = false;
            foreach (var item in objs)
            {
                if (item.comment().Equals("! comment as an ID"))
                {
                    success = true;
                }
            }

            Assert.IsTrue(success);
        }

        [TestMethod]
        public void UnderstandBranches_Test()
        {
            var md1 = new OpenStudio.Model();

            var plantloop = new OpenStudio.PlantLoop(md1);

            var boiler1 = new OpenStudio.BoilerHotWater(md1);
            var boiler2 = new OpenStudio.BoilerHotWater(md1);

            boiler1.setName("boiler1");
            boiler2.setName("boiler2");

            var nd = plantloop.supplyInletNode();

            boiler2.addToNode(nd);
            boiler1.addToNode(nd);

            var components = plantloop.supplyComponents(boiler1.iddObject().type());

            var success = components.First().nameString() == "boiler1";

            string saveFile = @"..\..\..\..\doc\osmFile\empty_Added_.osm";
            success &= md1.Save(saveFile);

            Assert.IsTrue(success);
        }

        [TestMethod]
        public void IDDFields_Test()
        {
            var refIddObject = new IdfObject(BoilerHotWater.iddObjectType()).iddObject();
            var iddfield = refIddObject.getField("Design Water Outlet Temperature").get();

            var prettystr = iddfield.getUnits().get().prettyString();
            var standardStr = iddfield.getUnits().get().standardString();
            var str = iddfield.getUnits(true).get().__str__();

            //real, choice, alpha,node
            var dataType = iddfield.properties().type.valueDescription();

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void OSobjFromIDDObj_Test()
        {
            var refIddObject = new IdfObject(BoilerHotWater.iddObjectType()).iddObject();
            var boiler = new StraightComponent(refIddObject.type(), new Model()).to_BoilerHotWater();

            var iddfield = refIddObject.getField("Design Water Outlet Temperature").get();

            var prettystr = iddfield.getUnits().get().prettyString();
            var standardStr = iddfield.getUnits().get().standardString();
            var str = iddfield.getUnits(true).get().__str__();

            //real, choice, alpha,node
            var dataType = iddfield.properties().type.valueDescription();

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void GetIDDGroup_Test()
        {
            var iddobj = new OpenStudio.IdfObject(OpenStudio.CoilHeatingWater.iddObjectType()).iddObject();
            var gp = iddobj.group();

            Assert.IsTrue(true);
        }
    }

    [TestClass()]
    public class AirLoopHVACOutdoorAirSystemExtensionsTests
    {
        [TestMethod()]
        public void OutdoorAirSystem_CloneTo_Test()
        {
            string sFile = @"..\..\..\..\doc\osmFile\Sys_7.osm";

            var sModel = OpenStudio.Model.load(new OpenStudio.Path(sFile)).get();
            var tModel = new OpenStudio.Model();

            var oa = sModel.getAirLoopHVACOutdoorAirSystems().First();
            var newOA = oa.CloneTo(tModel);

            var oaComs = newOA.oaComponents();

            Assert.IsTrue(oaComs.Count > 1);
        }

        [TestMethod()]
        public void OutdoorAirSystem_copySetpoints_Test()
        {
            string sFile = @"..\..\..\..\doc\osmFile\Sys_7.osm";

            var sModel = OpenStudio.Model.load(new OpenStudio.Path(sFile)).get();
            var tModel = new OpenStudio.Model();

            var oa = sModel.getAirLoopHVACOutdoorAirSystems().First();
            var newOA = oa.CloneTo(tModel);

            var airloop = new OpenStudio.AirLoopHVAC(tModel);
            oa.addToNode(airloop.supplyOutletNode());

            var nd = airloop.supplyOutletNode();
            var sp = new OpenStudio.SetpointManagerOutdoorAirPretreat(tModel);
            var ok = sp.addToNode(nd);

            var oaSps = tModel.getSetpointManagers();

            Assert.IsTrue(oaSps.Any());
        }

        [TestMethod()]
        public void Version_Test()
        {
            string sFile = @"C:\Users\mingo\OneDrive\Desktop\ops2.7.osm";
            var p = OpenStudioUtilitiesCore.toPath(sFile);
            var model = Model.load(p).get();

            var ov = IdfFile.loadVersionOnly(p);
            if (ov.is_initialized())
            {
                var v = ov.get();
                var vs = v.str();
                var osv = Ironbug.Core.OpenStudio.OpenStudioHelper.SupportedVersion;

                var ifNewerVersion = v.GreaterThan(new VersionString(osv));
            }
            //var v1 = OpenStudioUtilitiesCore.openStudioVersion();
            //var v2 = new OpenStudio.OpenStudioOSVersion();

            try
            {
                //OpenStudioUtilitiesCore.openStudioVersion.
                var vt = new VersionTranslator();
                vt.setAllowNewerVersions(false);
                var om = vt.loadModel(p);
                var om2 = vt.loadModelFromString(sFile);
                var m = om2.get();

                var s = m.getVersion().__str__();
                //var v3 = vt.originalVersion().str();
                //var m = om.get().getVersion().versionIdentifier();
                ////var osVersion = OpenStudio.ModelMerger.getVersion().versionIdentifier();

                var warnings = vt.errors().Any();
            }
            catch (Exception e)
            {
                throw e;
            }

            Assert.IsTrue(false);
        }
    }
}