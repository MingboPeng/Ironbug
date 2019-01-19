using System;
using System.Collections.Generic;
using System.Linq;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ironbug.HVACTests
{
    

    [TestClass]
    public class HVACComponentsTest
    {
        public TestContext TestContext { get; set; }

        OpenStudio.Model md1 = new OpenStudio.Model();
        string saveFile = @"..\..\..\..\doc\osmFile\empty_Added_.osm";

        [TestMethod]
        public void IB_Curve_GetMethodTest()
        {
            var c = new OpenStudio.CurveCubic(md1);
            ////c.GetType().get
            var value = 0.5;
            var methodInfo = c.GetType().GetMethod("setCoefficient2x", new[] { value.GetType() });
            Assert.IsTrue(methodInfo !=null);
        }

        [TestMethod]
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
            var fSet = HVAC.Curves.IB_CurveCubic_DataFieldSet.Value;
            var fDic = new Dictionary<HVAC.BaseClass.IB_Field, object>();

            fDic.Add(fSet.Coefficient1Constant, coeffs[0]);
            fDic.Add(fSet.Coefficient2x, coeffs[1]);
            fDic.Add(fSet.Coefficient3xPOW2, coeffs[2]);
            fDic.Add(fSet.Coefficient4xPOW3, coeffs[3]);

            obj.SetFieldValues(fDic);

            var boiler = new IB_BoilerHotWater();
            var cv = obj.ToOS();
            boiler.SetFieldValue(IB_BoilerHotWater_DataFields.Value.NormalizedBoilerEfficiencyCurve, cv);

            boiler.ToOS(model);
            
            var findChiller = model.getCurveCubics().First().to_CurveCubic().get().coefficient1Constant() == 0.5;
            Assert.IsTrue(findChiller);

        }

        [TestMethod]
        public void IB_OutputVariables_Test()
        {

            var model = new OpenStudio.Model();

            var obj = new HVAC.IB_BoilerHotWater();
            var variableName = obj.SimulationOutputVariables.First();
            var outputVariable = new IB_OutputVariable(variableName, IB_OutputVariable.TimeSteps.Monthly);
            obj.AddOutputVariables(new List<IB_OutputVariable>() { outputVariable });
            obj.ToOS(model);

            model.Save(saveFile);
            var findChiller = model.getOutputVariables().Any();
            Assert.IsTrue(findChiller);

        }

        [TestMethod]
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
            Assert.IsTrue(success);
        }

    }
}
