using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using OpenStudio;
using System.Reflection;
using System.Collections.Generic;

namespace Ironbug.HVACTests.Loop
{
    [TestClass]
    public class IB_FanConstantVolume_Test
    {

        Type type = typeof(FanConstantVolume);


        [TestMethod]
        public void IB_FanConstantVolume_Initialize_Test()
        {
            var fan = new HVAC.IB_FanConstantVolume();
            var dataFields = fan.GetDataFields();
            var success = dataFields.First().StartsWith("Fan Constant Volume 1");
            success = success && dataFields.Count() ==7;

            var sums = fan.ToString();

            Assert.IsTrue(success);
        }

        [TestMethod]
        public void IB_FanConstantVolume_SetAttris_Test()
        {
            var coil = new HVAC.IB_FanConstantVolume();
            var testValue = 0.5;
            coil.SetAttribute(HVAC.IB_FanConstantVolume_DataFields.FanEfficiency, testValue);
            var att = (double)coil.GetAttributeValue("fanEfficiency");

            Assert.IsTrue(att == testValue);
        }

        [TestMethod]
        public void IB_FanConstantVolume_Fields_Test()
        {
            var fan = new HVAC.IB_FanConstantVolume();
            var membs = typeof(FanConstantVolume).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
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
                results.Add(result);
                

            }
            
            
            var success = results.Count() == attrs.Count();
            

            Assert.IsTrue(success);
        }
    }
}
