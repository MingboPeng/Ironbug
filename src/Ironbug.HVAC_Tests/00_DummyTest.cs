using System;
using System.Reflection;
using Ironbug.HVAC;
using System.Linq;
using System.Collections.Generic;
using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Ironbug.HVACTests
{
    public class DummyTest
    {
        [Test]
        public void DummyTest_OS_Coil_Heating_WaterFields()
        {
            var tp = typeof(OpenStudio.OS_Coil_Heating_WaterFields);
            var meths = tp.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            var membs = tp.GetMembers(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            Assert.True(true);
        }

        [Test]
        public void DummyTest_FourPipeFanCoil()
        {
            var model = new OpenStudio.Model();
            var zone = new ThermalZone(model);

            var elecC = new CoilHeatingElectric(model);
            var f = new ZoneHVACFourPipeFanCoil(model, model.alwaysOnDiscreteSchedule(), new FanConstantVolume(model), new CoilCoolingWater(model), elecC);
            f.addToThermalZone(zone);

            var hc = f.heatingCoil().OSType();
            Assert.True(hc == "OS:Coil:Heating:Electric");
            model.Save( System.IO.Path.Combine(System.IO.Path.GetTempPath(), "test.osm"));
 
         
        }

        [Test]
        public void IB_TypeTest()
        {
            var coil = new IB_CoilCoolingDXMultiSpeed();
            var success = typeof(IB_Coil).IsInstanceOfType(coil);
            Assert.True(success);
        }

        [Test]
        public void CoolingPlantLoop_Test()
        {
            var lp = new IB_PlantLoop();
            var sz = new IB_SizingPlant();
            sz.SetFieldValue(IB_SizingPlant_FieldSet.Value.LoopType, "Cooling");
            lp.SetSizingPlant(sz);


            var md1 = new Model();
            lp.ToOS(md1);
            string saveFile = TestHelper.GenFileName;
            var success = md1.Save(saveFile);
            Assert.True(success);

            success &= md1.getPlantLoops().First().sizingPlant().loopType() == "Cooling";
            Assert.True(success);
        }

        [Test]
        public void ThermalZoneAndSizingZone_Test()
        {
            var md1 = new OpenStudio.Model();

            var airflow = new OpenStudio.AirLoopHVAC(md1);


            var zone = new OpenStudio.ThermalZone(md1);

            airflow.addBranchForZone(zone);

            
            string saveFile = $"{Guid.NewGuid().ToString().Substring(0,5)}_.osm";
            var success = md1.Save(saveFile);
            

            Assert.True(success);
        }

        [Test]
        public void DOAS_Test()
        {

            var md1 = new OpenStudio.Model();
            var ctrl = new ControllerOutdoorAir(md1);
            var oa = new AirLoopHVACOutdoorAirSystem(md1 , ctrl);
            var doas = new AirLoopHVACDedicatedOutdoorAirSystem(oa);

            var airflow1 = new OpenStudio.AirLoopHVAC(md1);
            var airflow2 = new OpenStudio.AirLoopHVAC(md1);
            var done = doas.addAirLoop(airflow1);
            done &= doas.addAirLoop(airflow2);

            Assert.True(done);

            string saveFile = $"{Guid.NewGuid().ToString().Substring(0, 5)}_.osm";
            var success = md1.Save(saveFile);


            Assert.True(success);
        }

        [Test]
        public void RetrunIfInModel_Test()
        {
            var md1 = new OpenStudio.Model();

            var airflow = new OpenStudio.AirLoopHVAC(md1);

            var optional = airflow.GetIfInModel(md1);
            
            Assert.True(!(optional is null));
        }

        [Test]
        public void GetEPDoc_Test()
        {
            var note = Ironbug.EPDoc.CoilCoolingWater.Note;
            Assert.True(!string.IsNullOrEmpty(note));

            string temp = Ironbug.EPDoc.CoilCoolingWater.Field_DesignInletWaterTemperature;
            Assert.True(!string.IsNullOrEmpty(temp));
        }

        [Test]
        public void ReadJson()
        {

            //var dir = @"C:\Users\mingo\Documents\GitHub\EPDoc2Json\Doc\";
            //var files = Directory.GetFiles(dir, "*.json");
            //var arr = new List<object>();
            //foreach (var f in files)
            //{

            //    dynamic docObj = JsonConvert.DeserializeObject(File.ReadAllText(f));
            //    var ar = docObj.subsection;
            //    var items = ar.Children();
            //    arr.AddRange(items);
            //}

            ////var list = arr.Children().ToList();

            //Assert.True(true);

        }

        [Test]
        public void OverrideGUIDTest()
        {

            var m = new OpenStudio.Model();
            var p = new OpenStudio.CoilHeatingWater(m);
            p.setName("My Coil");
            var id = p.getString(0).get();

            var newID = $"{{{System.Guid.NewGuid()}}}";
            p.setString(0, newID);

            // override original uuid
            var objID = p.getString(0).get();
            Assert.IsTrue(objID != id);
            Assert.IsTrue(objID == newID);

            // get saving osm and read it back
            var root = System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location);
            var testOsm = System.IO.Path.Combine(root, "test.osm");
            m.Save(testOsm);

            var m2 = OpenStudio.Model.load(testOsm.ToPath()).get();
            var uuid = OpenStudioUtilitiesCore.toUUID(newID);
            var found = m2.getCoilHeatingWater(uuid).get();
            Assert.IsTrue(found != null);
            Assert.IsTrue(found.nameString() == "My Coil");


        }

        [Test]
        public void MultiAirloopsZoneTest()
        {

            var m = new OpenStudio.Model();
            var z = new OpenStudio.ThermalZone(m);
            var at1 = new OpenStudio.AirTerminalSingleDuctConstantVolumeNoReheat(m, m.alwaysOffDiscreteSchedule());
            var at2 = new OpenStudio.AirTerminalSingleDuctConstantVolumeNoReheat(m, m.alwaysOffDiscreteSchedule());

            var airloop1 = new OpenStudio.AirLoopHVAC(m);
            var airloop2 = new OpenStudio.AirLoopHVAC(m);

            airloop1.addBranchForZone(z, at1);
            airloop2.multiAddBranchForZone(z, at2);

            var al1 = at1.airLoopHVAC().get();
            var al2 = at2.airLoopHVAC().get();
            Assert.IsTrue(al1.handle().isEqual(airloop1.handle()));
            Assert.IsTrue(al2.handle().isEqual(airloop2.handle()));


            var root = System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location);
            var testOsm = System.IO.Path.Combine(root, "test.osm");
            m.Save(testOsm);

        }


        [Test]
        public void PlantComponentUserDefined()
        {

            var m = new OpenStudio.Model();
            var p = new OpenStudio.PlantComponentUserDefined(m);


            var dummy = new Node(m);
            var act = new EnergyManagementSystemActuator(dummy, "Plant Connection 1", "Minimum Loading Capacity");
            act.setName("new actuator");
            p.setMinimumLoadingCapacityActuator(act);

            var pManager = new EnergyManagementSystemProgramCallingManager(m);
            pManager.setName("new program manager");
            var program = new EnergyManagementSystemProgram(m);
            program.setName("new program");
            pManager.addProgram(program);

            p.setPlantInitializationProgramCallingManager(pManager);



            // get saving osm and read it back
            var root = System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location);
            var testOsm = System.IO.Path.Combine(root, "test.osm");
            m.Save(testOsm);

            //var m2 = OpenStudio.Model.load(testOsm.ToPath()).get();
            //var uuid = OpenStudioUtilitiesCore.toUUID(newID);
            //var found = m2.getCoilHeatingWater(uuid).get();
            //Assert.IsTrue(found != null);
            //Assert.IsTrue(found.nameString() == "My Coil");


        }

        [Test]
        public void EmsActuators()
        {

            var m = new OpenStudio.Model();
            var p = new OpenStudio.PlantComponentUserDefined(m);

            var actuators = p.emsActuatorNames().Select(_ => $"{_.controlTypeName()}_{_.componentTypeName()}");
            var internalVariables = p.emsInternalVariableNames().Select(_ => _.ToString());
            var sensors = p.outputVariableNames().ToList();

            var l = new PlantLoop(m);
            var lAs = l.emsActuatorNames().Select(_ => $"{_.controlTypeName()}_{_.componentTypeName()}");
            var livs = l.emsInternalVariableNames();
            var lSensors = l.outputVariableNames();

            var dic = l.emsActuatorNames().ToDictionary(_ => _.componentTypeName(), v => v.controlTypeName());

            var pump = new PumpVariableSpeed(m);
            var pAs = pump.emsActuatorNames().Select(_ => $"{_.controlTypeName()}_{_.componentTypeName()}");
            var pivs = pump.emsInternalVariableNames();
            var pSensors = pump.outputVariableNames();
        }

        [Test]
        public void LoadIdfToOsm()
        {
            //var m = new OpenStudio.Model();
            var idf = @"
  Construction,
    TCwindow_85,                !- Name
    R13LAYER;               !- Outside Layer
";
            var mIdf = @"
Material:NoMass,
R13LAYER,  ! Material Name
 Rough,  ! Roughness
   2.290965    ,  ! Resistance {M**2K/W}
  0.9000000    ,   ! Thermal Absorptance
  0.7500000    ,   ! Solar Absorptance
  0.7500000    ;   ! Visible Absorptance
";
        
            var ws = new OpenStudio.Workspace();
            var idfobj = OpenStudio.IdfObject.load(mIdf).get();
            ws.addObject(idfobj);

            var trans = new OpenStudio.EnergyPlusReverseTranslator();
            var om = trans.translateWorkspace(ws);
            var mat = om.getMasslessOpaqueMaterials().First();

            Assert.IsTrue(mat.nameString() == "R13LAYER");
            Assert.IsTrue(mat.thermalResistance() == 2.290965);

        }


        [Test]
        public void CreateEMSTest()
        {
            //https://github.com/NREL/openstudio-standards/blob/master/lib/openstudio-standards/prototypes/common/objects/Prototype.CentralAirSourceHeatPump.rb

            //var inPath = @"C:\Users\mingo\simulation\HB_Template\OpenStudio\run\in.osm";
            //var inPath = @"C:\Users\mingo\simulation\IB_EMS\OpenStudio\run\in_IB.osm";
            var outPath = @"EMS.osm";

            //var model = OpenStudio.Model.load(OpenStudioUtilitiesCore.toPath(inPath)).get();
            var model = new OpenStudio.Model();
            var hotWaterLoop = new OpenStudio.PlantLoop(model);
            hotWaterLoop.setName("Hot Water Loop");

            var userCom = new OpenStudio.PlantComponentUserDefined(model);
            hotWaterLoop.addSupplyBranchForComponent(userCom);

            //var hot_water_loop = model.getPlantLoopByName("Hot Water Loop").get();
            //var model = new Model();

            ////remove old one
            //var objs = model.getPlantComponentUserDefineds();
            //foreach (var item in objs)
            //{
            //    item.removeFromLoop();
            //    item.remove();
            //}
            //var emsInVariable = model.getEnergyManagementSystemInternalVariables();
            //foreach (var item in emsInVariable)
            //{
            //    item.remove();
            //}
            //var emsMeters = model.getEnergyManagementSystemMeteredOutputVariables();
            //foreach (var item in emsMeters)
            //{
            //    item.remove();
            //}
            //var plantOperation = hot_water_loop.plantEquipmentOperationHeatingLoad();
            //if (plantOperation.is_initialized())
            //{
            //    plantOperation.get().remove();
            //}





            AddCentralAirSouceHeatPump(model, hotWaterLoop);
            //var plant_comp = model.getPlantComponentUserDefineds().First();
            //var htg_op_scheme = new PlantEquipmentOperationHeatingLoad(model);
            //htg_op_scheme.addEquipment(1000000000, plant_comp);
            //hot_water_loop.setPlantEquipmentOperationHeatingLoad(htg_op_scheme);

            model.save(OpenStudioUtilitiesCore.toPath(outPath), true);
            //Assert.True(a.actuatedComponent().is_initialized());


        }

        private void AddCentralAirSouceHeatPump(Model model, PlantLoop hotwaterPlant)
        {
            var hot_water_loop = hotwaterPlant;

            //var plant_comp = new OpenStudio.PlantComponentUserDefined(model);
            var plant_comp = model.getPlantComponentUserDefineds().First();
            //# change equipment name for EMS validity
            plant_comp.setName("CentralAirSourceHeatPump");

            //# set plant component properties
            plant_comp.setPlantLoadingMode("MeetsLoadWithNominalCapacityHiOutLimit");
            plant_comp.setPlantLoopFlowRequestMode("NeedsFlowIfLoopIsOn");


            //# plant design volume flow rate internal variable
            var vdot_des_int_var = new EnergyManagementSystemInternalVariable(model, "Plant Design Volume Flow Rate");
            vdot_des_int_var.setName("Vdot_Des_Int_Var");
            vdot_des_int_var.setInternalDataIndexKeyName(hot_water_loop.handle().__str__());

            //# inlet temperature internal variable
            var tin_int_var = new EnergyManagementSystemInternalVariable(model, "Inlet Temperature for Plant Connection 1");
            tin_int_var.setName($"Tin_Int_Var");
            tin_int_var.setInternalDataIndexKeyName(plant_comp.handle().__str__());

            //# inlet mass flow rate internal variable
            var mdot_int_var = new EnergyManagementSystemInternalVariable(model, "Inlet Mass Flow Rate for Plant Connection 1");
            mdot_int_var.setName($"Mdot_Int_Var");
            mdot_int_var.setInternalDataIndexKeyName(plant_comp.handle().__str__());

            //# inlet specific heat internal variable
            var cp_int_var = new EnergyManagementSystemInternalVariable(model, "Inlet Specific Heat for Plant Connection 1");
            cp_int_var.setName($"Cp_Int_Var");
            cp_int_var.setInternalDataIndexKeyName(plant_comp.handle().__str__());

            //# inlet density internal variable
            var rho_int_var = new EnergyManagementSystemInternalVariable(model, "Inlet Density for Plant Connection 1");
            rho_int_var.setName($"rho_Int_Var");
            rho_int_var.setInternalDataIndexKeyName(plant_comp.handle().__str__());

            //#load request internal variable
            var load_int_var = new EnergyManagementSystemInternalVariable(model, "Load Request for Plant Connection 1");
            load_int_var.setName($"Load_Int_Var");
            load_int_var.setInternalDataIndexKeyName(plant_comp.handle().__str__());


            //# supply outlet node setpoint temperature sensor
            var setpt_mgr_sch_sen = new EnergyManagementSystemSensor(model, "Schedule Value");
            setpt_mgr_sch_sen.setName($"Setpt_Mgr_Temp_Sen");
            var stps = hot_water_loop.supplyOutletNode().setpointManagers();
            foreach (var item in stps)
            {
                var sch = item.to_SetpointManagerScheduled();
                if (sch.is_initialized())
                {
                    setpt_mgr_sch_sen.setKeyName(sch.get().schedule().nameString());
                }
            }


            //# outdoor air drybulb temperature sensor
            var oa_dbt_sen = new EnergyManagementSystemSensor(model, "Site Outdoor Air Drybulb Temperature");
            oa_dbt_sen.setName($"{plant_comp.nameString()}_OA_DBT_Sen");
            oa_dbt_sen.setKeyName("Environment");

            //# minimum mass flow rate actuator
            var mdot_min_act = plant_comp.minimumMassFlowRateActuator().get();
            mdot_min_act.setName($"{plant_comp.nameString()}_Mdot_Min_Act");

            //# maximum mass flow rate actuator
            var mdot_max_act = plant_comp.maximumMassFlowRateActuator().get();
            mdot_max_act.setName($"{plant_comp.nameString()}_Mdot_Max_Act");

            //# design flow rate actuator
            var vdot_des_act = plant_comp.designVolumeFlowRateActuator().get();
            vdot_des_act.setName($"{plant_comp.nameString()}_Vdot_Des_Act");

            //# minimum loading capacity actuator
            var cap_min_act = plant_comp.minimumLoadingCapacityActuator().get();
            cap_min_act.setName($"{plant_comp.nameString()}_Cap_Min_Act");

            //# maximum loading capacity actuator
            var cap_max_act = plant_comp.maximumLoadingCapacityActuator().get();
            cap_max_act.setName($"{plant_comp.nameString()}_Cap_Max_Act");

            //# optimal loading capacity actuator
            var cap_opt_act = plant_comp.optimalLoadingCapacityActuator().get();
            cap_opt_act.setName($"{plant_comp.nameString()}_Cap_Opt_Act");

            //# outlet temperature actuator
            var tout_act = plant_comp.outletTemperatureActuator().get();
            tout_act.setName($"{plant_comp.nameString()}_Tout_Act");

            //# mass flow rate actuator
            var mdot_req_act = plant_comp.massFlowRateActuator().get();
            mdot_req_act.setName($"{plant_comp.nameString()}_Mdot_Req_Act");

            //# heat pump COP curve
            var cop = 3.65;
            var constant_coeff = 1.932 + (cop - 3.65);
            var hp_cop_curve = new CurveQuadratic(model);
            hp_cop_curve.setCoefficient1Constant(constant_coeff);
            hp_cop_curve.setCoefficient2x(0.227674286);
            hp_cop_curve.setCoefficient3xPOW2(-0.007313143);
            hp_cop_curve.setMinimumValueofx(1.67);
            hp_cop_curve.setMaximumValueofx(12.78);
            hp_cop_curve.setInputUnitTypeforX("Temperature");
            hp_cop_curve.setOutputUnitType("Dimensionless");

            //# heat pump COP curve index variable
            var hp_cop_curve_idx_var = new EnergyManagementSystemCurveOrTableIndexVariable(model, hp_cop_curve);

            //# high outlet temperature limit actuator
            var tout_max_act = new EnergyManagementSystemActuator(plant_comp, "Plant Connection 1", "High Outlet Temperature Limit");
            tout_max_act.setName($"{plant_comp.nameString()}_Tout_Max_Act");

            //# init program
            var init_pgrm = plant_comp.plantInitializationProgram().get();
            init_pgrm.setName($"{plant_comp.nameString()}_Init_Pgrm");
            var init_pgrm_body = $@"
SET Loop_Exit_Temp = {hot_water_loop.sizingPlant().designLoopExitTemperature()}
SET Loop_Delta_Temp = {hot_water_loop.sizingPlant().loopDesignTemperatureDifference()}
SET Cp = @CPHW Loop_Exit_Temp
SET rho = @RhoH2O Loop_Exit_Temp
SET {vdot_des_act.handle().__str__()} = {vdot_des_int_var.handle().__str__()}
SET {mdot_min_act.handle().__str__()} = 0
SET Mdot_Max = {vdot_des_int_var.handle().__str__()} * rho
SET {mdot_max_act.handle().__str__()} = Mdot_Max
SET Cap = Mdot_Max * Cp * Loop_Delta_Temp
SET {cap_min_act.handle().__str__()} = 0
SET {cap_max_act.handle().__str__()} = Cap
SET {cap_opt_act.handle().__str__()} = 1 * Cap
    ";
            init_pgrm.setBody(init_pgrm_body);


            //# sim program
            var sim_pgrm = plant_comp.plantSimulationProgram().get();
            sim_pgrm.setName($"{plant_comp.nameString()}_Sim_Pgrm");
            var sim_pgrm_body = $@"
            SET tmp = {load_int_var.handle().__str__()}
            SET tmp = {tin_int_var.handle().__str__()}
            SET tmp = {mdot_int_var.handle().__str__()}
            SET {tout_max_act.handle().__str__()} = 75.0
            IF {load_int_var.handle().__str__()} == 0
            SET {tout_act.handle().__str__()} = {tin_int_var.handle().__str__()}
            SET {mdot_req_act.handle().__str__()} = 0
            SET Elec = 0
            RETURN
            ENDIF
            IF {load_int_var.handle().__str__()} >= {cap_max_act.handle().__str__()}
            SET Qdot = {cap_max_act.handle().__str__()}
            SET Mdot = {mdot_max_act.handle().__str__()}
            SET {mdot_req_act.handle().__str__()} = Mdot
            SET {tout_act.handle().__str__()} = (Qdot / (Mdot * {cp_int_var.handle().__str__()})) + {tin_int_var.handle().__str__()}
            IF {tout_act.handle().__str__()} > {tout_max_act.handle().__str__()}
            SET {tout_act.handle().__str__()} = {tout_max_act.handle().__str__()}
            SET Qdot = Mdot * {cp_int_var.handle().__str__()} * ({tout_act.handle().__str__()} - {tin_int_var.handle().__str__()})
            ENDIF
            ELSE
            SET Qdot = {load_int_var.handle().__str__()}
            SET {tout_act.handle().__str__()} = {setpt_mgr_sch_sen.handle().__str__()}
            SET Mdot = Qdot / ({cp_int_var.handle().__str__()} * ({tout_act.handle().__str__()} - {tin_int_var.handle().__str__()}))
            SET {mdot_req_act.handle().__str__()} = Mdot
            ENDIF
            SET Tdb = {oa_dbt_sen.handle().__str__()}
            SET COP = @CurveValue {hp_cop_curve_idx_var.handle().__str__()} Tdb
            SET EIR = 1 / COP
            SET Pwr = Qdot * EIR
            SET Elec = Pwr * SystemTimestep * 3600
            ";
            sim_pgrm.setBody(sim_pgrm_body);

            //# init program calling manager
            var init_mgr = plant_comp.plantInitializationProgramCallingManager().get();
            init_mgr.setName($"{plant_comp.nameString()}_Init_Pgrm_Mgr");

            //# sim program calling manager
            var sim_mgr = plant_comp.plantSimulationProgramCallingManager().get();
            sim_mgr.setName($"{plant_comp.nameString()}_Sim_Pgrm_Mgr");

            //# metered output variable
            var elec_mtr_out_var = new EnergyManagementSystemMeteredOutputVariable(model, $"{plant_comp.nameString()} Electricity Consumption");
            elec_mtr_out_var.setName($"{plant_comp.nameString()} Electricity Consumption");
            elec_mtr_out_var.setEMSVariableName("Elec");
            elec_mtr_out_var.setUpdateFrequency("SystemTimestep");
            elec_mtr_out_var.setString(4, sim_pgrm.handle().__str__());
            elec_mtr_out_var.setResourceType("Electricity");
            elec_mtr_out_var.setGroupType("HVAC");
            elec_mtr_out_var.setEndUseCategory("Heating");
            elec_mtr_out_var.setEndUseSubcategory("");
            elec_mtr_out_var.setUnits("J");


            //# add to supply side of hot water loop if specified
            //hot_water_loop.addSupplyBranchForComponent(plant_comp);

            //# add operation scheme
            var htg_op_scheme = new PlantEquipmentOperationHeatingLoad(model);
            htg_op_scheme.addEquipment(1000000000, plant_comp);
            hot_water_loop.setPlantEquipmentOperationHeatingLoad(htg_op_scheme);
        }



    }
}
