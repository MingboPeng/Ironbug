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
            var methodInfo = c.GetType().GetMethod("setCoefficient2X", new[] { value.GetType() });
            Assert.IsTrue(methodInfo !=null);
        }
        [TestMethod]
        public void IB_Curve_Test()
        {
           

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


            var cv = obj.ToOS();
            var md = cv.model();
            md.Save(saveFile);

            var findChiller = md.getCurveCubics().First().to_CurveCubic().get().coefficient1Constant() == 0.5;
            Assert.IsTrue(findChiller);

        }
        
    }
}
