using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;
using Newtonsoft.Json;
using NUnit.Framework;
using Ironbug.Core;

namespace Ironbug.HVACTests
{
    public class SerializationTest
    {
        [Test]
        public void HVACSystem_Test()
        {
            var airloop = BuildAirloop();
            var airloops = new List<IB_AirLoopHVAC>() { airloop };

            var plantloop = BuildPlantLoop();
            var plantloops = new List<IB_PlantLoop>() { plantloop };
            var vrf = new IB_AirConditionerVariableRefrigerantFlow();
            vrf.AddTerminal(BuildVrfTerminal());
            var vrfs = new List<IB_AirConditionerVariableRefrigerantFlow>() {vrf};

            var hvac = new IB_HVACSystem(airloops, plantloops, vrfs);
            var j = hvac.ToJson();

            var newHvac = IB_HVACSystem.FromJson(j);
            Assert.IsTrue(newHvac == hvac);
        }

        IB_AirLoopHVAC BuildAirloop()
        {
            var loop = new IB_AirLoopHVAC();

            var outdoorAir = new IB_OutdoorAirSystem();
            loop.AddToSupplySide(outdoorAir);

            var heatingcoil = new IB_CoilHeatingElectric();
            loop.AddToSupplySide(heatingcoil);

            var coolingcoil = new IB_CoilCoolingDXSingleSpeed();
            loop.AddToSupplySide(coolingcoil);

            var fan = new IB_FanConstantVolume();
            loop.AddToSupplySide(fan);

            var spt = new IB_SetpointManagerScheduled();
            loop.AddToSupplySide(spt);

            return loop;

        }

        [Test]
        public void Vrf_Test()
        {
            //var vrfSys = new IB_AirConditionerVariableRefrigerantFlow();
            var vrf = new IB_AirConditionerVariableRefrigerantFlow();
            vrf.AddTerminal(BuildVrfTerminal());

            var json = vrf.ToJson(true);
            var newVrf = IB_ModelObject.FromJson<IB_AirConditionerVariableRefrigerantFlow>(json);
            Assert.IsTrue(newVrf == vrf);

        }
        [Test]
        public void Children_Test()
        {
            //var vrfSys = new IB_AirConditionerVariableRefrigerantFlow();
          
            var vrf = BuildVrfTerminal();

            var coolingCoil = vrf.GetChild<IB_CoilCoolingDXVariableRefrigerantFlow>();
            var heatingCoil = vrf.GetChild<IB_CoilHeatingDXVariableRefrigerantFlow>();
            var fan = vrf.GetChild<IB_FanOnOff>();

            //vrfSys.AddTerminal(vrf);
            var jsonFan = fan.ToJson(true);
            var jsonCCoil = coolingCoil.ToJson(true);
            var jsonHCoil = heatingCoil.ToJson(true);
        

            var json = vrf.ToJson(true);
            var newVrf = IB_ModelObject.FromJson<IB_ZoneHVACTerminalUnitVariableRefrigerantFlow>(json);
            Assert.IsTrue(newVrf == vrf);

            var newCCoil = newVrf.GetChild<IB_CoilCoolingDXVariableRefrigerantFlow>();
            Assert.IsTrue(newCCoil == coolingCoil);
            var newHCoil = newVrf.GetChild<IB_CoilHeatingDXVariableRefrigerantFlow>();
            Assert.IsTrue(newHCoil == heatingCoil);
            var newFan = newVrf.GetChild<IB_FanOnOff>();
            Assert.IsTrue(newFan == fan);

        }
        IB_ZoneHVACTerminalUnitVariableRefrigerantFlow BuildVrfTerminal()
        {
            var coolingCoil = new IB_CoilCoolingDXVariableRefrigerantFlow();
            var heatingCoil = new IB_CoilHeatingDXVariableRefrigerantFlow();
            var fan = new IB_FanOnOff();
            var vrf = new IB_ZoneHVACTerminalUnitVariableRefrigerantFlow(coolingCoil, heatingCoil, fan);
            return vrf;
        }


        [Test]
        public void PlantLoop_Test()
        {
            var plant = BuildPlantLoop();
            var sizing = plant.SizingPlant;

            // ToJson()
            var json = plant.ToJson();
            var readDis = IB_PlantLoop.FromJson<IB_PlantLoop>(json);
            Assert.IsTrue(readDis != null);

            var plantFields = HVAC.IB_PlantLoop_FieldSet.Value;
            Assert.IsTrue(readDis.CustomAttributes.TryGetValue(plantFields.Name, out var name));
            Assert.IsTrue(name.Equals("Hot Water Loop"));

            Assert.IsTrue(readDis.SizingPlant == sizing);
            Assert.IsTrue(readDis == plant);

        }

        IB_PlantLoop BuildPlantLoop()
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

            // Add children
            var coilControl = new HVAC.IB_ControllerWaterCoil();
            coilControl.SetFieldValue(new IB_Field("Action", "Action"), "Normal");
            var coil = new HVAC.IB_CoilCoolingWater(coilControl);
            plant.AddToDemand(coil);
            return plant;
        }

        [Test]
        public void PlantLoopBranches_Test()
        {

            var plant = new HVAC.IB_PlantLoop();

         
            // Add children
            var coilControl = new HVAC.IB_ControllerWaterCoil();
            var ctrlField = new IB_Field("Action", "Action");
            coilControl.SetFieldValue(ctrlField, "Normal");
            var coil = new HVAC.IB_CoilCoolingWater(coilControl);
            plant.AddToDemand(coil);

            var pump = new HVAC.IB_PumpVariableSpeed();
            plant.AddToDemand(pump);

            // ToJson()
            var json = plant.ToJson();
            var readDis = IB_PlantLoop.FromJson<IB_PlantLoop>(json);
            Assert.IsTrue(readDis != null);

            var prop = plant.GetType().GetProperty("demandComponents", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            var readDemands = prop.GetValue(plant) as List<IB_HVACObject>;

            Assert.IsNotNull(readDemands);
            Assert.AreEqual(readDemands.Count, 2);

            var readCoil = readDemands.OfType<IB_CoilCoolingWater>().FirstOrDefault();
            Assert.IsNotNull(readCoil);

            var readCoilControl = readCoil.Children.GetChild<IB_ControllerWaterCoil>();
            Assert.IsTrue(readCoilControl.CustomAttributes.TryGetValue(ctrlField, out var actionValue));
            Assert.AreEqual(actionValue, "Normal");

            Assert.IsTrue(readDis == plant);

        }

        [Test]
        public void CoilCoolingWater_Test()
        {

            var coil = new HVAC.IB_CoilCoolingWater();


            // ToJson()
            var json = coil.ToJson();
            var readDis = IB_ModelObject.FromJson<IB_CoilCoolingWater>(json);
            Assert.IsTrue(readDis != null);

            Assert.IsTrue(readDis == coil);

        }
        [Test]
        public void Sizing_Test()
        {
            var sizingPlant = new HVAC.IB_SizingPlant();
            var szFields = HVAC.IB_SizingPlant_FieldSet.Value;
            sizingPlant.SetFieldValue(szFields.LoopType, "Heating");

            var json = sizingPlant.ToJson();

            var readDis = IB_SizingPlant.FromJson<IB_SizingPlant>(json);
            Assert.IsTrue(readDis != null);
            Assert.IsTrue(readDis.Equals(sizingPlant));

        }

        [Test]
        public void FieldArguments_Test()
        {
            var dis = new IB_FieldArgumentSet();
            var szFields = HVAC.IB_SizingPlant_FieldSet.Value;
            dis.TryAdd(szFields.LoopType, "Heating");
            dis.TryAdd(szFields.DesignLoopExitTemperature, 25D);


            var json = JsonConvert.SerializeObject(dis, Formatting.Indented, IB_JsonSetting.ConvertSetting);

            var readDis = JsonConvert.DeserializeObject<IB_FieldArgumentSet>(json, IB_JsonSetting.ConvertSetting);
            Assert.IsTrue(readDis != null);

            Assert.IsTrue(readDis.TryGetValue(szFields.LoopType, out var l));
            Assert.IsTrue(l.Equals("Heating"));
            Assert.IsTrue(readDis.TryGetValue(szFields.DesignLoopExitTemperature, out var t));
            Assert.IsTrue(t.Equals(25D));
            Assert.IsTrue(readDis.Equals(dis));
        }

        [Test]
        public void FieldArgumentList_Test()
        {
            var args = new List<IB_FieldArgument>();
            var szFields = HVAC.IB_SizingPlant_FieldSet.Value;
            args.Add(new IB_FieldArgument(szFields.LoopType, "Heating"));
            args.Add(new IB_FieldArgument(szFields.DesignLoopExitTemperature, 25D));

            // manually added field
            args.Add(new IB_FieldArgument(new IB_Field("SizingOption", "SizingOption", typeof(string)), "Coincident"));

            var json = JsonConvert.SerializeObject(args, Formatting.Indented, IB_JsonSetting.ConvertSetting);

            var readDis = JsonConvert.DeserializeObject<List<IB_FieldArgument>>(json, IB_JsonSetting.ConvertSetting);
            Assert.IsTrue(readDis != null);

            Assert.AreEqual(readDis[0].Value.ToString(), "Heating");
            Assert.AreEqual(readDis[1].Value, 25D);
            Assert.AreEqual(readDis[2].Value, "Coincident");
        }

        [Test]
        public void FieldEqual_Test()
        {
            var szFields = HVAC.IB_SizingPlant_FieldSet.Value;
            var source = szFields.DesignLoopExitTemperature;
            var fieldJson = JsonConvert.SerializeObject(source, Formatting.Indented);
            var readField = JsonConvert.DeserializeObject<IB_Field>(fieldJson);


            Assert.IsTrue(source == readField);
        }

        [Test]
        public void TryAddFieldArgumentList_Test()
        {
            var dis = new IB_FieldArgumentSet();
            var commentField = IB_Field_Comment.Instance;

            dis.TryAdd(commentField, "Tracking ID");
            dis.TryAdd(commentField, "Tracking ID2");


            Assert.IsTrue(dis.Count() == 1);
            Assert.IsTrue(dis.TryGetValue(commentField, out var data));
            Assert.IsTrue(data.Equals("Tracking ID2"));
        }

        [Test]
        public void Type_Test()
        {
            var typeName = typeof(string).FullName;
            var tp = Type.GetType(typeName);
            var defaultType =  default(Type);
            Assert.IsTrue(typeName == tp.FullName);
        }

    }
}
