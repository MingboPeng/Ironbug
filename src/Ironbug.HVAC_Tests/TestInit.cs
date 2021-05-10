using NUnit.Framework;
using System;
using System.IO;

namespace Ironbug.HVACTests
{
    [SetUpFixture]
    public class TestInit
    {

        [OneTimeSetUp]
        public void Init()
        {
            //var opsFolder = Ironbug.Core.OpenStudio.OpenStudioHelper.FindOpsFolder();
            //var dlls = Directory.GetFiles(opsFolder);
            //var destFolder = Path.GetDirectoryName(this.GetType().Assembly.Location);
            //foreach (var item in dlls)
            //{
            //    var file = Path.GetFileName(item);
            //    var dest = Path.Combine(destFolder, file);

            //    File.Copy(item, dest, true);
            //}

            //var loaded = Ironbug.Core.OpenStudio.OpenStudioHelper.LoadAssemblies((string s) => Console.WriteLine(s));
            //var openstudio = @"C:\openstudio-3.1.0\CSharp\openstudio";
        }
    }
}
