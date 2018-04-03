using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ironbug.HVACTests
{
    [TestClass]
    public class DataFieldSetTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void DataFieldNames_Test()
        {
            var testNames = new List<string>(){
                "Coefficient3OfthePartLoadPerformanceCurve",
                "Coefficient3OfThePartLoadPerformanceCurve",
                "Coefficient3ofThePartLoadPerformanceCurve",
                "Coefficient 3 Of the Part Load Performance Curve",
                "Coefficient 3 Of The Part Load Performance Curve",
                "Coefficient_3 Of the Part Load_Performance Curve",
                "Coefficient 3 Ofthe Part Load Performance Curve",
                "Coefficient 3 OfThe Part Load Performance Curve",
            };

            var expected = "COEFFICIENT3OFTHEPARTLOADPERFORMANCECURVE";
            var success = true;

            foreach (var name in testNames)
            {
                var datafields = new IB_DataField(name, name);
                var testedName = datafields.FULLNAME;
                if (testedName != expected)
                {
                    TestContext.WriteLine(name + "\r\n >> " + testedName);
                    success = false;
                }
            }
            
            Assert.IsTrue(success);

        }

        [TestMethod]
        public void GetCustomizedDataFields_Test()
        {
            var datafields = IB_PumpVariableSpeed_DataFields.Value;
            var customizedDataFields = datafields.GetCustomizedDataFields();

            var success = customizedDataFields.Count() == 8;

            Assert.IsTrue(success);

        }

        [TestMethod]
        public void DataFieldType_Test()
        {
            var datafields = IB_PumpVariableSpeed_DataFields.Value;

            var type = typeof(IB_PumpVariableSpeed_DataFields);
            IB_DataFieldSet datafields2 = Convert.ChangeType( Activator.CreateInstance(type,true),type) as IB_DataFieldSet;

            ////Old way
            //var basetype = datafields.GetType().BaseType;
            //var success = (basetype.IsGenericType? basetype.GetGenericTypeDefinition(): basetype.GetType()) == typeof(IB_DataFieldSet<,>);

            //New way
            var success = datafields is IB_DataFieldSet;
            success &= datafields2 is IB_DataFieldSet;

            Assert.IsTrue(success);

        }
        [TestMethod]
        public void AllDataFieldClass_ifSealed_Test()
        {
            var logs = new List<string>();

            var allDataFieldsClasses = typeof(IB_PumpVariableSpeed_DataFields).Assembly.GetTypes()
                .Where(_ => (!_.IsAbstract) && _.IsSubclassOf(typeof(IB_DataFieldSet)))
                .ToList();

            allDataFieldsClasses.ForEach(_ => {
                if (!_.IsSealed)
                    logs.Add(_.Name + " is not sealed!");
            });

            TestContext.WriteLine(string.Join("\r\n", logs));
            var success = !logs.Any();

            Assert.IsTrue(success);

        }

        [TestMethod]
        public void AllDataFieldClass_hasPrivateConstructor_Test()
        {
            var logs = new List<string>();

            var allDataFieldsClasses = typeof(IB_PumpVariableSpeed_DataFields).Assembly.GetTypes()
                .Where(_ => (!_.IsAbstract) && _.IsSubclassOf(typeof(IB_DataFieldSet)))
                .ToList();

            allDataFieldsClasses.ForEach(_ => {
                var constructor = _.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                if (!constructor.Any())
                {
                    logs.Add(_.Name + " has no private constructor. Use snippet 'ctorPrivate' to create!");
                }

            });

            TestContext.WriteLine(string.Join("\r\n", logs));
            var success = !logs.Any();

            Assert.IsTrue(success);

        }
        [TestMethod]
        public void AllCustomizedDataFields_Test()
        {
            var allDataFieldsClasses = typeof(IB_PumpVariableSpeed_DataFields).Assembly.GetTypes()
                .Where(_ => (!_.IsAbstract) && _.IsSealed && _.IsSubclassOf(typeof(IB_DataFieldSet)));

            var logs = new List<List<string>>();

            foreach (var dfClass in allDataFieldsClasses)
            {
                var instance = dfClass.BaseType.GetProperty("Value").GetValue(null) as IB_DataFieldSet;

                //check each customized data field if can be found in IddObject,
                //mainly for checking the name's spelling or formatting.
                //and check the customized data field fullname if matches OpenStudion setter's name.
                var log = new List<string>();
                var props = instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

                props.ToList()
                    .ForEach(_ => {
                        var curPropName = _.Name;
                        var found = instance.FirstOrDefault(item => (item.FULLNAME == curPropName.ToUpper()));
                        if (found is null)
                        {
                            log.Add(curPropName + "\r\n\tcannot be found in :\r\n\t\t" + instance.GetType());
                        }
                        else if (curPropName != "Name" && curPropName != found.SetterMethodName?.Substring(3))
                        {
                           
                            log.Add(curPropName + "\r\n\tshould be ["+ found.SetterMethodName?.Substring(3) + "] in :\r\n\t\t" + instance.GetType());
                        }
                    });

                if (log.Any())
                {
                    logs.Add(log);
                    TestContext.WriteLine(string.Join("\r\n",log));
                }


            }
            


            var success = !logs.Any();

            Assert.IsTrue(success);

        }
    }
}
