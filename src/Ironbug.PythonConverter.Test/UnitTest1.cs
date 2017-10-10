using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ironbug.PythonConverter;

namespace Ironbug.PythonConverter.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod()]
        public void ExtractClassInfoTest()
        {
            var extractedInfo = PyProcessing.TranslatePy();
            
            Assert.IsTrue(true);
        }
    }
}
