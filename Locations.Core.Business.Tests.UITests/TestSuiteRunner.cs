// Locations.Core.Business.Tests.UITests/TestSuiteRunner.cs
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;

namespace Locations.Core.Business.Tests.UITests
{
    /// <summary>
    /// Provides methods to run test suites by domain or as a full regression test.
    /// This class isn't meant to be run directly as a test fixture, but rather 
    /// provides helper methods to execute tests from a command line interface.
    /// </summary>
    public class TestSuiteRunner
    {
        private static readonly string NUnitConsolePath = GetNUnitConsolePath();
        private static readonly string TestAssemblyPath = GetTestAssemblyPath();

        /// <summary>
        /// Runs all tests in the test assembly.
        /// </summary>
        public static void RunAllTests()
        {
            ExecuteNUnitCommand($"\"{TestAssemblyPath}\"");
        }

        /// <summary>
        /// Runs tests for a specific domain based on category.
        /// </summary>
        /// <param name="domain">The domain/category to run tests for.</param>
        public static void RunDomainTests(string domain)
        {
            if (string.IsNullOrEmpty(domain))
            {
                Console.WriteLine("No domain specified. Please specify a valid test domain/category.");
                return;
            }

            ExecuteNUnitCommand($"\"{TestAssemblyPath}\" --where \"cat == {domain}\"");
        }

        /// <summary>
        /// Runs tests for multiple domains based on categories.
        /// </summary>
        /// <param name="domains">Array of domains/categories to run tests for.</param>
        public static void RunMultipleDomainTests(string[] domains)
        {
            if (domains == null || domains.Length == 0)
            {
                Console.WriteLine("No domains specified. Please specify valid test domains/categories.");
                return;
            }

            string categoryFilter = string.Join(" || ", Array.ConvertAll(domains, domain => $"cat == {domain}"));
            ExecuteNUnitCommand($"\"{TestAssemblyPath}\" --where \"{categoryFilter}\"");
        }

        /// <summary>
        /// Helper method to execute NUnit console with specified parameters.
        /// </summary>
        /// <param name="arguments">Command line arguments for NUnit console.</param>
        private static void ExecuteNUnitCommand(string arguments)
        {
            if (string.IsNullOrEmpty(NUnitConsolePath))
            {
                Console.WriteLine("NUnit console not found. Please ensure it is installed.");
                return;
            }

            try
            {
                Console.WriteLine($"Executing: {NUnitConsolePath} {arguments}");

                using (var process = new Process())
                {
                    process.StartInfo.FileName = NUnitConsolePath;
                    process.StartInfo.Arguments = arguments;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;

                    process.OutputDataReceived += (sender, e) => { if (e.Data != null) Console.WriteLine(e.Data); };
                    process.ErrorDataReceived += (sender, e) => { if (e.Data != null) Console.WriteLine(e.Data); };

                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    process.WaitForExit();

                    Console.WriteLine($"Tests completed with exit code {process.ExitCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing NUnit tests: {ex.Message}");
            }
        }

        /// <summary>
        /// Helper method to find the NUnit console executable.
        /// </summary>
        /// <returns>Path to NUnit console, or null if not found.</returns>
        private static string GetNUnitConsolePath()
        {
            // Try to locate nunit3-console.exe in common locations
            string[] possiblePaths = new[]
            {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "NUnit.org", "nunit-console", "nunit3-console.exe"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "NUnit.org", "nunit-console", "nunit3-console.exe"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "packages", "NUnit.Console", "tools", "nunit3-console.exe"),
                "nunit3-console.exe" // Assume it's in the PATH
            };

            foreach (string path in possiblePaths)
            {
                if (File.Exists(path))
                {
                    return path;
                }
            }

            // Try to find in packages directory by searching recursively
            try
            {
                string solutionDirectory = FindSolutionDirectory(AppDomain.CurrentDomain.BaseDirectory);
                if (solutionDirectory != null)
                {
                    string packagesDirectory = Path.Combine(solutionDirectory, "packages");
                    if (Directory.Exists(packagesDirectory))
                    {
                        foreach (string directory in Directory.GetDirectories(packagesDirectory, "NUnit.Console*", SearchOption.TopDirectoryOnly))
                        {
                            string nunitPath = Path.Combine(directory, "tools", "nunit3-console.exe");
                            if (File.Exists(nunitPath))
                            {
                                return nunitPath;
                            }
                        }
                    }
                }
            }
            catch
            {
                // Ignore errors in recursive search
            }

            return null;
        }

        /// <summary>
        /// Helper method to find the test assembly.
        /// </summary>
        /// <returns>Path to the test assembly.</returns>
        private static string GetTestAssemblyPath()
        {
            // By default, use the current assembly
            string assemblyPath = typeof(TestSuiteRunner).Assembly.Location;

            // If the assembly path is empty (e.g., when running in .NET Core), try to build the path
            if (string.IsNullOrEmpty(assemblyPath))
            {
                string outputPath = AppDomain.CurrentDomain.BaseDirectory;
                assemblyPath = Path.Combine(outputPath, "Locations.Core.Business.Tests.UITests.dll");
            }

            return assemblyPath;
        }

        /// <summary>
        /// Helper method to find the solution directory by walking up the directory tree.
        /// </summary>
        /// <param name="startDirectory">Directory to start searching from.</param>
        /// <returns>Solution directory path, or null if not found.</returns>
        private static string FindSolutionDirectory(string startDirectory)
        {
            DirectoryInfo directory = new DirectoryInfo(startDirectory);

            while (directory != null)
            {
                if (Directory.GetFiles(directory.FullName, "*.sln").Length > 0)
                {
                    return directory.FullName;
                }

                directory = directory.Parent;
            }

            return null;
        }

        // Example of usage from a command line program
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Running all tests...");
                RunAllTests();
            }
            else if (args.Length == 1 && args[0].Equals("--help", StringComparison.OrdinalIgnoreCase))
            {
                ShowHelp();
            }
            else if (args.Length >= 1)
            {
                if (args[0].Equals("--domain", StringComparison.OrdinalIgnoreCase))
                {
                    if (args.Length < 2)
                    {
                        Console.WriteLine("No domain specified. Please specify a valid test domain/category.");
                        ShowHelp();
                        return;
                    }

                    if (args.Length == 2)
                    {
                        Console.WriteLine($"Running tests for domain: {args[1]}");
                        RunDomainTests(args[1]);
                    }
                    else
                    {
                        string[] domains = new string[args.Length - 1];
                        Array.Copy(args, 1, domains, 0, args.Length - 1);

                        Console.WriteLine($"Running tests for domains: {string.Join(", ", domains)}");
                        RunMultipleDomainTests(domains);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid argument. Use --domain followed by domain names or --help for usage information.");
                    ShowHelp();
                }
            }
        }

        /// <summary>
        /// Shows help information for the test suite runner.
        /// </summary>
        private static void ShowHelp()
        {
            Console.WriteLine("Test Suite Runner - Usage Information");
            Console.WriteLine("-------------------------------------");
            Console.WriteLine("Run all tests:");
            Console.WriteLine("  TestSuiteRunner");
            Console.WriteLine();
            Console.WriteLine("Run tests for a specific domain:");
            Console.WriteLine("  TestSuiteRunner --domain Authentication");
            Console.WriteLine();
            Console.WriteLine("Run tests for multiple domains:");
            Console.WriteLine("  TestSuiteRunner --domain Authentication Locations Weather");
            Console.WriteLine();
            Console.WriteLine("Available domains:");
            Console.WriteLine("  Authentication - Login tests");
            Console.WriteLine("  Configuration - Settings tests");
            Console.WriteLine("  Locations - Location management tests");
            Console.WriteLine("  Weather - Weather display tests");
            Console.WriteLine("  Tips - Photography tips tests");
            Console.WriteLine("  Tutorial - Page tutorial modal tests");
            Console.WriteLine("  FeatureNotSupported - Feature not supported page tests");
            Console.WriteLine("  Subscription - Subscription/ad feature tests");
        }
    }
}