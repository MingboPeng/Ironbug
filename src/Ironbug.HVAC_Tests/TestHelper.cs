

using System.IO;

namespace Ironbug.HVACTests
{
   
    public static class TestHelper
    {
        public static string GenFileName => $"{System.Guid.NewGuid().ToString().Substring(0, 5)}.osm";

        public static string Root => Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(typeof(TestHelper).Assembly.Location))));
        public static string TestSourceFolder = Path.Combine(Root, "TestSource");
        public static string ExampleBuildingFile = Path.Combine(TestSourceFolder, "BuildingForTest.osm");

    }
}
