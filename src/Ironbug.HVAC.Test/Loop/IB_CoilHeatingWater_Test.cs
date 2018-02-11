using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenStudio;

namespace Ironbug.HVACTests.Loop
{
    [TestClass]
    public class IB_CoilHeatingWater_Test
    {
        
        [TestMethod]
        public void IB_CoilHeatingWater_Initialize_Test()
        {
            var coil = new HVAC.IB_CoilHeatingWater();
            var dataFields = coil.GetDataFields();
            Assert.IsTrue(dataFields.Count() ==10);
        }

        [TestMethod]
        public void IB_CoilHeatingWater_SetAttris_Test()
        {
            var coil = new HVAC.IB_CoilHeatingWater();
            var testValue = 50.0;
            coil.SetAttribute(HVAC.IB_CoilHeatingWater_DataFieldSet.RatedInletWaterTemperature, testValue);
            var att = (double)coil.GetAttributeValue("ratedInletWaterTemperature");

            Assert.IsTrue(att == testValue);
        }

        [TestMethod]
        public void IB_FanConstantVolume_Fields_Test()
        {

            var fan = new HVAC.IB_CoilHeatingWater();
            var membs = typeof(CoilHeatingWater).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            var attrs = HVAC.IB_FanConstantVolume_DataFields.GetList();

            var results = new List<string>();
            foreach (var attr in attrs)
            {
                var n1 = attr.GetterMethodName;
                var t1 = attr.DataType;

                //getting method
                var matched = membs.Where(_ => (_.Name == n1) && (_.ReturnType == t1));

                //setting method
                var n2 = attr.SetterMethodName;
                var matched2 = membs.Where(_ => _.Name == n2);


                var result = string.Empty;
                if (matched.Any() && matched2.Any())
                {
                    result = String.Format("{0} founded", n1);
                }
                else
                {
                    result = String.Format("___ {0} ___", n1);
                }

                Console.WriteLine(result);

                results.Add(result);


            }

            Console.WriteLine(results);
            var success = results.Count() == attrs.Count();


            Assert.IsTrue(success);
        }


    }
}
