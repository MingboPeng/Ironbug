using System;
using System.Collections.Generic;
using System.Linq;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;
using Ironbug.HVAC.Schedules;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Ironbug.HVACTests
{
    public class PropArgumentSetTest
    {
        [Test]
        public void ListTypes_Test()
        {
            //box and unbox between object and list
            var list = new List<object>() { 2.5, 3.4 };
            var list2 = new List<double>() { 2.5, 3.4 };
            var obj = (object)list;
            var type = typeof(List<double>);

            Console.WriteLine(obj.GetType().ToString());
            //System.Collections.Generic.List`1[System.Object]
            Console.WriteLine(list.GetType().ToString());
            //System.Collections.Generic.List`1[System.Object]

            if (list is List<object> eo)
            {
                var itemType = type.GenericTypeArguments[0];
                var objList = eo.ToList();
                if (eo.All(_ => _.GetType() == itemType))
                {
                    Console.WriteLine($"All child type is {itemType}");
                    //All child type is System.Double
                }

                var objTypedList = objList.Select(x => Convert.ChangeType(x, itemType)).ToList();

                var objItemType = objTypedList.FirstOrDefault().GetType();
                Console.WriteLine(objItemType.ToString());

                var objListType = objTypedList.GetType();
                Console.WriteLine(objListType.ToString());


                var itemtype2 = objList.GetType().GenericTypeArguments[0];
                //Console.WriteLine()
                Assert.AreEqual(list.GetType(), objTypedList.GetType());
                //Assert.AreEqual(type, objTypedList.GetType());



            }

        }

        [Test]
        public void SchduleRule_Test()
        {
            var sch = new IB_ScheduleRule(new IB_ScheduleDay(12));

            var json = JsonConvert.SerializeObject(sch, Formatting.Indented, IB_JsonSetting.ConvertSetting);
            var readDis = JsonConvert.DeserializeObject<IB_ScheduleRule>(json, IB_JsonSetting.ConvertSetting);

            Assert.AreEqual(sch, readDis);

            // test list 
            var list = new List<IB_ScheduleRule>() { sch };
            var json2 = JsonConvert.SerializeObject(list, Formatting.Indented, IB_JsonSetting.ConvertSetting);
            var readDis2 = JsonConvert.DeserializeObject<List<IB_ScheduleRule>>(json2, IB_JsonSetting.ConvertSetting);

            Assert.AreEqual(list, readDis2);

            // test List<object>
            var listobj = new List<object>() { sch };
            var json3 = JsonConvert.SerializeObject(listobj, Formatting.Indented, IB_JsonSetting.ConvertSetting);
            var readDis3 = JsonConvert.DeserializeObject<List<IB_ScheduleRule>>(json3, IB_JsonSetting.ConvertSetting);
            Assert.AreEqual(listobj, readDis3);


            // test dic<object>
            var listdic = new Dictionary<string, object>();
            listdic.Add("rule", sch);
            var json4 = JsonConvert.SerializeObject(listdic, Formatting.Indented, IB_JsonSetting.ConvertSetting);
            //var readDis3 = JsonConvert.DeserializeObject<List<IB_ScheduleRule>>(json3, IB_JsonSetting.ConvertSetting);
            Assert.AreEqual(listobj, readDis3);

        }

        [Test]
        public void SchduleRuleSet_Test()
        {

            var sch = new IB_ScheduleRule(new IB_ScheduleDay(12));
            var schSet = new IB_ScheduleRuleset();
            schSet.Rules.Add(sch);

            var json = JsonConvert.SerializeObject(schSet, Formatting.Indented, IB_JsonSetting.ConvertSetting);
            var readDis = JsonConvert.DeserializeObject<IB_ScheduleRuleset>(json, IB_JsonSetting.ConvertSetting);
            Assert.AreEqual(sch, readDis.Rules.First());
            Assert.AreEqual(sch.IBProperties.FirstOrDefault(), readDis.Rules.First().IBProperties.FirstOrDefault());
            Assert.AreEqual(schSet, readDis);

        }

        [Test]
        public void AirloopWithSch_Test()
        {

            var sch = new IB_ScheduleRule(new IB_ScheduleDay(12));
            var schSet = new IB_ScheduleRuleset();
            schSet.Rules.Add(sch);

            var airLoop = new IB_AirLoopHVAC();
            airLoop.AddCustomAttribute(new IB_Field("AvailabilitySchedule", ""), schSet);

            var json2 = JsonConvert.SerializeObject(airLoop, Formatting.Indented, IB_JsonSetting.ConvertSetting);
            var airloop2 = JsonConvert.DeserializeObject<IB_AirLoopHVAC>(json2, IB_JsonSetting.ConvertSetting);
            Assert.AreEqual(airLoop, airloop2);

        }


        [Test]
        public void IB_PropArgumentSet_Test()
        {
            var pT = new IB_PropArgumentSet();
            pT.SetByKey("rule", new IB_ScheduleRule(new IB_ScheduleDay(12)));


            var json = JsonConvert.SerializeObject(pT, Formatting.Indented, IB_JsonSetting.ConvertSetting);
            var readDis = JsonConvert.DeserializeObject<IB_PropArgumentSet>(json, IB_JsonSetting.ConvertSetting);
            readDis.TryGetValue("rule", out var readRule);
            Assert.IsTrue(readDis != null);
            Assert.AreNotEqual(readRule, null);

            var schRule = readRule as IB_ScheduleRule;
            Assert.AreNotEqual(schRule, null);
            Assert.AreEqual(schRule.ScheduleDay.IBProperties.First().Value, 12);

            var dup = pT.Duplicate();
            Assert.AreEqual(pT, dup);
            Assert.AreEqual(pT, readDis);

        }

        [Test]
        public void IBProperties_list_Test()
        {
            var pT = new IBPropertyTester();
            pT.values.AddRange(new[] { 13.0, 1, 0 });

            var json = JsonConvert.SerializeObject(pT, Formatting.Indented, IB_JsonSetting.ConvertSetting);
            var readDis = JsonConvert.DeserializeObject<IBPropertyTester>(json, IB_JsonSetting.ConvertSetting);
            Assert.IsTrue(readDis != null);

            Assert.AreNotEqual(readDis.values, null);
            Assert.AreEqual(readDis.values.Count, 3);
            Assert.AreEqual(readDis.values.First(), 13.0);

            var dup = pT.Duplicate();
            Assert.AreEqual(pT, dup);
        }


        [Test]
        public void IBProperties_listOfObjs_Test()
        {
            var pT = new IBPropertyTester();
            pT.rules.Add(new IB_ScheduleRule(new IB_ScheduleDay(12)));
            pT.rules.Add(new IB_ScheduleRule(new IB_ScheduleDay(15.6)));



            var json = JsonConvert.SerializeObject(pT, Formatting.Indented, IB_JsonSetting.ConvertSetting);
            var readDis = JsonConvert.DeserializeObject<IBPropertyTester>(json, IB_JsonSetting.ConvertSetting);
            Assert.IsTrue(readDis != null);

            Assert.AreEqual(readDis.rules.Count, 2);
            var firstSchDay = readDis.rules.First().ScheduleDay;
            Assert.AreNotEqual(firstSchDay, null);
            Assert.AreNotEqual(firstSchDay, pT.rules.First());
            Assert.AreEqual(readDis.rules, pT.rules);
            Assert.IsTrue(readDis.rules.SequenceEqual(pT.rules));
            Assert.IsFalse(readDis.rules.Equals(pT.rules));

            Assert.AreEqual(readDis.rule, pT.rule);
            Assert.AreEqual(readDis.constantNumber, pT.constantNumber);
            Assert.AreEqual(firstSchDay.IBProperties.First().Value, 12.0);

            var dup = pT.Duplicate();
            Assert.AreEqual(pT, dup);
            Assert.AreEqual(pT, readDis);
        }

        [Test]
        public void IBProperties_listOfObjs2_Test()
        {
            var pT = new IBPropertyTester();
            pT.GFuncs.Add((0.1, 66));
            pT.GFuncs.Add((0.4, 11));



            var json = JsonConvert.SerializeObject(pT, Formatting.Indented, IB_JsonSetting.ConvertSetting);
            var readDis = JsonConvert.DeserializeObject<IBPropertyTester>(json, IB_JsonSetting.ConvertSetting);
            Assert.IsTrue(readDis != null);

            Assert.AreEqual(readDis.GFuncs.Count, 2);
            Assert.AreEqual(readDis.GFuncs.First().Ln, 0.1);
            Assert.AreEqual(readDis.GFuncs.First(), pT.GFuncs.First());
            Assert.AreEqual(readDis.GFuncs, pT.GFuncs);
            Assert.IsTrue(readDis.GFuncs.SequenceEqual(pT.GFuncs));
            Assert.IsFalse(readDis.GFuncs.Equals(pT.GFuncs));


            var dup = pT.Duplicate();
            Assert.AreEqual(pT, dup);
            Assert.AreEqual(pT, readDis);
        }

        [Test]
        public void IBProperties_IBModelObj_Test()
        {
            var pT = new IBPropertyTester();
            var sTp = new IB_ScheduleTypeLimits();
            sTp.AddCustomAttribute(IB_ScheduleTypeLimits.FieldSet.LowerLimitValue, 2);
            pT.Set(nameof(pT.scheduleTypeLimits), sTp);


            var json = JsonConvert.SerializeObject(pT, Formatting.Indented, IB_JsonSetting.ConvertSetting);
            var readDis = JsonConvert.DeserializeObject<IBPropertyTester>(json, IB_JsonSetting.ConvertSetting);
            Assert.IsTrue(readDis != null);
            Assert.AreNotEqual(readDis.scheduleTypeLimits, null);

            var dup = pT.Duplicate();
            Assert.AreEqual(pT, dup);
        }

        [Test]
        public void IBProperties_IBModelObj2_Test()
        {
            var pT = new IBPropertyTester();
            pT.Set(nameof(pT.rule), new IB_ScheduleRule(new IB_ScheduleDay(12)));


            var json = JsonConvert.SerializeObject(pT, Formatting.Indented, IB_JsonSetting.ConvertSetting);
            var readDis = JsonConvert.DeserializeObject<IBPropertyTester>(json, IB_JsonSetting.ConvertSetting);
            Assert.IsTrue(readDis != null);
            Assert.AreNotEqual(readDis.rule, null);

            var dup = pT.Duplicate();
            Assert.AreEqual(pT, dup);
        }

        [Test]
        public void IBProperties_CsingleWithDefault_Test()
        {
            var pT = new IBPropertyTester();

            var json = JsonConvert.SerializeObject(pT, Formatting.Indented, IB_JsonSetting.ConvertSetting);
            var readDis = JsonConvert.DeserializeObject<IBPropertyTester>(json, IB_JsonSetting.ConvertSetting);
            Assert.IsTrue(readDis != null);

            Assert.AreEqual(pT.constantNumberWithDef, 4.1);
            Assert.AreEqual(readDis.constantNumberWithDef, 4.1);
            var dup = pT.Duplicate();
            Assert.AreEqual(pT, dup);
        }

        [Test]
        public void IBProperties_Test()
        {
            var pT = new IBPropertyTester();
            pT.constantNumber = 2.1;
            pT.constantNumberWithDef = 33.7;
            pT.values.AddRange(new[] { 1.1, 1.2, 0 });
            pT.rules.Add(new IB_ScheduleRule(new IB_ScheduleDay(12)));
            pT.rules.Add(new IB_ScheduleRule(new IB_ScheduleDay(15)));

            pT.dateRange = (2, 1, 6, 31);
            Assert.AreEqual(pT.dateRanges.First(), (1,5));
            pT.dateRanges.Add((2, 1));
            var sTp = new IB_ScheduleTypeLimits();
            sTp.AddCustomAttribute(IB_ScheduleTypeLimits.FieldSet.LowerLimitValue, 2);
            pT.Set(nameof(pT.scheduleTypeLimits), sTp);


            var json = JsonConvert.SerializeObject(pT, Formatting.Indented, IB_JsonSetting.ConvertSetting);
            var readDis = JsonConvert.DeserializeObject<IBPropertyTester>(json, IB_JsonSetting.ConvertSetting);
            Assert.IsTrue(readDis != null);
            Assert.AreEqual(readDis, pT);

            Assert.AreEqual(readDis.constantNumber, 2.1);
            Assert.AreEqual(readDis.constantNumberWithDef, 33.7);
            Assert.AreEqual(readDis.values.Count, 3);
            Assert.AreEqual(readDis.values.First(), 1.1);
            Assert.AreEqual(readDis.rules.Count, 2);
            Assert.AreNotEqual(readDis.rules.First().ScheduleDay, null);
            Assert.AreEqual(readDis.dateRange, (2, 1, 6, 31));
            Assert.AreEqual(readDis.dateRanges[1], (2, 1));
            Assert.AreEqual(readDis.scheduleTypeLimits, sTp);

            var dup = pT.Duplicate();
            Assert.AreEqual(pT, dup);
        }
    }

    internal class IBPropertyTester: IB_ModelObject
    {
        internal double constantNumber 
        { 
            get => this.Get<double>();
            set => this.Set(value);
        }
        internal double constantNumberWithDef
        {
            get => this.Get<double>(4.1);
            set => this.Set(value);
        }

        // list of values
        internal List<double> values
        {
            get => this.TryGetList<double>();
            set => this.Set(value);
        }

        // list of IB_ModelObject
        internal List<IB_ScheduleRule> rules
        {
            get => this.TryGetList<IB_ScheduleRule>();
            set => this.Set(value);
        } 

        internal List<(int sMonth, int sDay)> dateRanges
        {
            get => this.GetList(initDefault: () => new List<(int, int)> { (1, 5) });
            set => this.Set(value);
        }


        internal IB_ScheduleRule rule
        {
            get => this.Get<IB_ScheduleRule>();
            set => this.Set(value);
        }

        // tuple
        internal (int sMonth, int sDay, int eMonth, int eDay) dateRange
        {
            get => this.Get((1, 1, 12, 31));
            set => this.Set(value);
        }

        // IB_ModelObject
        internal IB_ScheduleTypeLimits scheduleTypeLimits
        {
            get => this.Get<IB_ScheduleTypeLimits>();
            set => this.Set(value);
        }

        public List<(double Ln, double GValue)> GFuncs
        {
            get => TryGetList<(double Ln, double GValue)>();
            private set => Set(value);
        }

        public IBPropertyTester(): base(new OpenStudio.Node(new OpenStudio.Model()))
        {
           
        }

        protected override Func<IB_ModelObject> IB_InitSelf => ()=> new IBPropertyTester();
    }
}
