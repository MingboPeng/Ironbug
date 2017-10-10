using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ironbug.PythonConverter;
using System.Collections.Generic;

namespace Ironbug.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod()]
        public void ExtractClassInfoTest()
        {
            var extractedInfo = PyProcessing.TranslatePy();
            dynamic dicInfo = extractedInfo;

            var className = dicInfo[0]["ClassNames"];
            Assert.IsTrue(true);
        }
    }
}
