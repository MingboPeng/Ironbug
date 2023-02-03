using OpenStudio;

namespace Ironbug.HVAC
{
    public static class Model_Extensions
    {
        public static bool Save(this Model model, string filePath)
        {
            return model.save(OpenStudioUtilitiesCore.toPath(filePath), true);
        }
        public static Path ToPath(this string filePath)
        {
            return OpenStudioUtilitiesCore.toPath(filePath);
        }

    }
}
