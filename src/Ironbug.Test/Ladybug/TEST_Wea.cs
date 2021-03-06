﻿using System;
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
            var header = wea.Header.ToString();

            var successed = header.StartsWith("place BEIJING");
            
            Assert.AreEqual(successed, true);
        }

        [TestMethod]
        public void TEST_WEA_RawObjInstance()
        {
            var weaO = new Wea();

            string epwFile = @"C:\ladybug\BEIJING_CHN\BEIJING_CHN.epw";
            var wea = weaO.From_EpwFile(epwFile);
            dynamic rawObj = wea.getRawObj();
            var header = rawObj.header;

            var successed = header.StartsWith("place BEIJING");

            Assert.AreEqual(successed, true);
        }

    }
}
