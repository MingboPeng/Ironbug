using System;
using System.Collections.Generic;
using System.Linq;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;
using NUnit.Framework;

namespace Ironbug.HVACTests
{
    

    public class HVACComponentsTest
    {

        private string GenFileName => TestHelper.GenFileName;
        [Test]
        public void CurveType_Test()
        {
            var fSet = HVAC.Curves.IB_CurveCubic_FieldSet.Value;
            Assert.IsTrue(fSet.Coefficient1Constant.DataType == typeof(double));

            var curve = IB_BoilerHotWater_FieldSet.Value.NormalizedBoilerEfficiencyCurve;
            Assert.IsTrue(curve.DataType == typeof(OpenStudio.Curve));

            Assert.IsTrue(typeof(OpenStudio.Curve).IsAssignableFrom(typeof(OpenStudio.CurveCubic)));
            Assert.IsTrue(typeof(OpenStudio.CurveCubic).IsSubclassOf(typeof(OpenStudio.Curve)));
        }

        [Test]
        public void IB_Curve_GetMethodTest()
        {
            var md1 = new OpenStudio.Model();
            var c = new OpenStudio.CurveCubic(md1);
            ////c.GetType().get
            var value = 0.5;
            var methodInfo = c.GetType().GetMethod("setCoefficient2x", new[] { value.GetType() });
            Assert.True(methodInfo !=null);
        }

        [Test]
        public void IB_Curve_Test()
        {

            var model = new OpenStudio.Model();

            var obj = new HVAC.Curves.IB_CurveCubic();
            var coeffs = new List<double>()
            {
                0.5,0.6,0.7,0.8
            };

            
            if (coeffs.Count != 4)
            {
                throw new Exception("4 coefficient values is needed!");
            }
            var fSet = HVAC.Curves.IB_CurveCubic_FieldSet.Value;
            var fDic = new Dictionary<HVAC.BaseClass.IB_Field, object>();

            fDic.Add(fSet.Coefficient1Constant, coeffs[0]);
            fDic.Add(fSet.Coefficient2x, coeffs[1]);
            fDic.Add(fSet.Coefficient3xPOW2, coeffs[2]);
            fDic.Add(fSet.Coefficient4xPOW3, coeffs[3]);

            obj.SetFieldValues(fDic);

            var boiler = new IB_BoilerHotWater();
            boiler.SetFieldValue(IB_BoilerHotWater_FieldSet.Value.NormalizedBoilerEfficiencyCurve, obj);

            boiler.ToOS(model);
            
            var findChiller = model.getCurveCubics().First().to_CurveCubic().get().coefficient1Constant() == 0.5;
            Assert.True(findChiller);

            var boilerCurve = model.getBoilerHotWaters().First().normalizedBoilerEfficiencyCurve().get().to_CurveCubic().get();
            Assert.True(boilerCurve.coefficient1Constant() == 0.5);

        }

        [Test]
        public void IB_OutputVariables_Test()
        {

            var md1 = new OpenStudio.Model();

            var obj = new HVAC.IB_BoilerHotWater();
            var variableName = obj.SimulationOutputVariables.First();
            var outputVariable = new IB_OutputVariable(variableName, IB_OutputVariable.TimeSteps.Monthly);
            obj.AddOutputVariables(new List<IB_OutputVariable>() { outputVariable });
            obj.ToOS(md1);

            string saveFile = GenFileName;
            var saved = md1.Save(saveFile);
            Assert.True(saved);

            var findChiller = md1.getOutputVariables().Any();
            Assert.True(findChiller);

        }

        [Test]
        public void IB_ZoneHVACUnitVentilator_CoolingHeating_Test()
        {
            var model = new OpenStudio.Model();

            var obj = new HVAC.IB_ZoneHVACUnitVentilator_CoolingHeating();
            var fan = new HVAC.IB_FanOnOff();

            obj.SetFan(fan);
            var fanChild = obj.Children.Get<IB_Fan>();
            var children = obj.Children;

            var hc = new IB_CoilHeatingElectric();
            obj.SetHeatingCoil(hc);

            var cc = new IB_CoilCoolingWater();
            obj.SetCoolingCoil(cc);

            var hcChild = obj.Children.Get<IB_CoilHeatingBasic>();
            var ccChild = obj.Children.Get<IB_CoilCoolingBasic>();

            var success = fanChild is IB_FanOnOff;
            success &= !hcChild.Equals(ccChild);
            Assert.True(success);
        }

        [Test]
        public void IB_Schedule24Hrs()
        {

            var sch = new HVAC.Schedules.IB_ScheduleRuleset();

            var values = new List<double>()
            {
                0.1,0,0,0,0,0,0,0.2,0.5,1,2,2,2,1,1,1,1,0.2,0,0,0,0,0,0
            };
            var values2 = new List<double>()
            {
                2,1,1,0,0,0,0,1,1,1,0,1,1,1,1,1,1,1,0,0,0,0,1,3
            };
            var day = new HVAC.Schedules.IB_ScheduleDay(values);
            var schRule = new HVAC.Schedules.IB_ScheduleRule(day);
            sch.AddRule(schRule);

            var day2 = new HVAC.Schedules.IB_ScheduleDay(values2);
            var schRule2 = new HVAC.Schedules.IB_ScheduleRule(day2);
            sch.AddRule(schRule2);

            var md1 = new OpenStudio.Model();
            sch.ToOS(md1);

            string saveFile = GenFileName;
            var saved = md1.Save(saveFile);
            Assert.True(saved);
        }

    }
}
