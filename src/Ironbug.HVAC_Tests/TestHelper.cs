

using System.IO;

namespace Ironbug.HVACTests
{
   
    public static class TestHelper
    {
        public static string GenFileName => $"{System.Guid.NewGuid().ToString().Substring(0, 5)}.osm";

        private static string bin => Path.GetDirectoryName(typeof(TestHelper).Assembly.Location);
        public static string Root => bin.Split(new[] { "bin" }, System.StringSplitOptions.RemoveEmptyEntries)[0];
        public static string TestSourceFolder = Path.Combine(Root, "TestSource");
        public static string ExampleBuildingFile = Path.Combine(TestSourceFolder, "BuildingForTest.osm");

    }
}
