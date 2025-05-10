// Locations.Core.Business.Tests.UITests/TestSuiteConfig.cs
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Locations.Core.Business.Tests.UITests
{
    /// <summary>
    /// Provides configuration for the UI test suite.
    /// </summary>
    public static class TestSuiteConfig
    {
        // Platform configuration
        public static AppiumSetup.Platform DefaultPlatform = AppiumSetup.Platform.Android;

        // Timeouts
        public static int DefaultWaitTimeoutSeconds = 10;
        public static int LongOperationTimeoutSeconds = 30;

        // App-specific configuration
        public static string AppPackageName = "com.yourcompany.location.photography";
        public static string AppActivityName = "com.yourcompany.location.photography.MainActivity";

        // Test data
        public static class TestData
        {
            public static string ValidEmail = "test@example.com";
            public static string InvalidEmail = "invalid-email";
            public static string DefaultLocationTitle = "Test Location";
            public static string DefaultLocationDescription = "This is a test location created by automated UI tests.";
        }

        // Domain definitions for grouping tests
        public static class Domains
        {
            public const string Authentication = "Authentication";
            public const string Configuration = "Configuration";
            public const string Locations = "Locations";
            public const string Weather = "Weather";
            public const string Tips = "Tips";
            public const string Tutorial = "Tutorial";
            public const string FeatureNotSupported = "FeatureNotSupported";
            public const string Subscription = "Subscription";

            // Get all domains for discovery
            public static IEnumerable<string> GetAllDomains()
            {
                return new[]
                {
                    Authentication,
                    Configuration,
                    Locations,
                    Weather,
                    Tips,
                    Tutorial,
                    FeatureNotSupported,
                    Subscription
                };
            }
        }

        // Load configuration from external file if present
        static TestSuiteConfig()
        {
            try
            {
                string configPath = Path.Combine(GetExecutingDirectory(), "testsuite.config.xml");
                if (File.Exists(configPath))
                {
                    LoadConfiguration(configPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading test suite configuration: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads configuration from an XML file.
        /// </summary>
        /// <param name="configPath">Path to the configuration file.</param>
        private static void LoadConfiguration(string configPath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(configPath);

            // Load platform configuration
            var platformNode = doc.SelectSingleNode("//Configuration/Platform");
            if (platformNode != null)
            {
                string platformValue = platformNode.InnerText;
                if (Enum.TryParse<AppiumSetup.Platform>(platformValue, true, out var platform))
                {
                    DefaultPlatform = platform;
                }
            }

            // Load timeout configuration
            var defaultTimeoutNode = doc.SelectSingleNode("//Configuration/DefaultTimeout");
            if (defaultTimeoutNode != null && int.TryParse(defaultTimeoutNode.InnerText, out int defaultTimeout))
            {
                DefaultWaitTimeoutSeconds = defaultTimeout;
            }

            var longTimeoutNode = doc.SelectSingleNode("//Configuration/LongOperationTimeout");
            if (longTimeoutNode != null && int.TryParse(longTimeoutNode.InnerText, out int longTimeout))
            {
                LongOperationTimeoutSeconds = longTimeout;
            }

            // Load app configuration
            var appPackageNode = doc.SelectSingleNode("//Configuration/AppPackage");
            if (appPackageNode != null)
            {
                AppPackageName = appPackageNode.InnerText;
            }

            var appActivityNode = doc.SelectSingleNode("//Configuration/AppActivity");
            if (appActivityNode != null)
            {
                AppActivityName = appActivityNode.InnerText;
            }

            // Load test data
            var testDataNode = doc.SelectSingleNode("//Configuration/TestData");
            if (testDataNode != null)
            {
                var validEmailNode = testDataNode.SelectSingleNode("ValidEmail");
                if (validEmailNode != null)
                {
                    TestData.ValidEmail = validEmailNode.InnerText;
                }

                var invalidEmailNode = testDataNode.SelectSingleNode("InvalidEmail");
                if (invalidEmailNode != null)
                {
                    TestData.InvalidEmail = invalidEmailNode.InnerText;
                }

                var defaultTitleNode = testDataNode.SelectSingleNode("DefaultLocationTitle");
                if (defaultTitleNode != null)
                {
                    TestData.DefaultLocationTitle = defaultTitleNode.InnerText;
                }

                var defaultDescriptionNode = testDataNode.SelectSingleNode("DefaultLocationDescription");
                if (defaultDescriptionNode != null)
                {
                    TestData.DefaultLocationDescription = defaultDescriptionNode.InnerText;
                }
            }
        }

        /// <summary>
        /// Gets the directory of the executing assembly.
        /// </summary>
        /// <returns>Directory path.</returns>
        private static string GetExecutingDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
    }
}