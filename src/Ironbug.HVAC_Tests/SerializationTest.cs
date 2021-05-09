using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Ironbug.HVACTests
{
    public class SerializationTest
    {
        [Test]
        public void HVACSystem_Test()
        {
           
            //var hvac = new IB_HVACSystem(new List<IB_AirLoopHVAC>() { }, new List<IB_PlantLoop>(), new List<IB_AirConditionerVariableRefrigerantFlow>() { });
            //hvac.SaveAsIBJson();


        }

        [Test]
        public void PlantLoop_Test()
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


            var json = plant.ToJson();
            var readDis = IB_PlantLoop.FromJson<IB_PlantLoop>(json);
            Assert.IsTrue(readDis != null);

            Assert.IsTrue(readDis.CustomAttributes.TryGetValue(plantFields.Name, out var name));
            Assert.IsTrue(name.Equals("Hot Water Loop"));

            Assert.IsTrue(readDis.SizingPlant == sizing);

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

            var json = JsonConvert.SerializeObject(dis, Formatting.Indented);

            var readDis = JsonConvert.DeserializeObject<IB_FieldArgumentSet>(json);
            Assert.IsTrue(readDis != null);

            Assert.IsTrue(readDis.TryGetValue(szFields.LoopType, out var l));
            Assert.IsTrue(l.Equals("Heating"));
            Assert.IsTrue(readDis.TryGetValue(szFields.DesignLoopExitTemperature, out var t));
            Assert.IsTrue(t.Equals(25D));
            Assert.IsTrue(readDis.Equals(dis));
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
