using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ironbug.Ladybug;

namespace Ironbug.Test.Ladybug
{
    [TestClass]
    public class TEST_Wea
    {
        [TestMethod]
        public void TEST_WEA()
        {
            var weaO = new Wea();

            string epwFile = @"C:\ladybug\BEIJING_CHN\BEIJING_CHN.epw";
            var wea = weaO.From_EpwFile(epwFile);
            var header = wea.header;

            bool successed = false;
            if (wea != null)
            {
                successed = true;
            }
            
            

            Assert.AreEqual(successed, true);
        }
    }
}
