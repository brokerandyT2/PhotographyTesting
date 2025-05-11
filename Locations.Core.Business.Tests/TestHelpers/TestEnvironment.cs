namespace Locations.Core.Business.Tests.TestHelpers
{
    public static class TestEnvironment
    {
        public static void InitializeMagicStrings()
        {
            // Use reflection to set private static fields that depend on FileSystem
            typeof(Locations.Core.Shared.MagicStrings)
                .GetField("_appDataDirectory", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, "C:\\TestAppData");

            // Initialize any other fields as needed
        }
    }
}