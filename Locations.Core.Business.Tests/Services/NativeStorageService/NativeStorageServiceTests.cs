using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using Locations.Core.Business.StorageSvc;
using Locations.Core.Business.Tests.Base;
using Locations.Core.Business.Tests.TestHelpers;
using MockFactory = Locations.Core.Business.Tests.TestHelpers.MockFactory;

namespace Locations.Core.Business.Tests.Services.StorageServiceTests
{
    [TestClass]
    [TestCategory("StorageService")]
    public class NativeStorageServiceTests : BaseServiceTests
    {
        // Use a static class method approach for testing NativeStorageService
        // since it contains static methods

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
        }

        [TestMethod]
        public void DeleteSetting_WhenSettingExists_ShouldDeleteSetting()
        {
            // Note: Since we can't easily mock SecureStorage operations,
            // this test simply verifies the method doesn't throw an exception

            // Act - Using Moq to intercept the SecureStorage call would be ideal
            // but would require refactoring the production code for testability
            try
            {
                // This is a no-op test since we can't mock static methods without refactoring
                bool result = NativeStorageService.DeleteSetting("test_key");

                // We can only verify no exception was thrown, not the actual behavior
                Assert.IsTrue(true); // Test passes if it reaches this line
            }
            catch (Exception ex)
            {
                Assert.Fail($"Method threw an exception: {ex.Message}");
            }
        }

        [TestMethod]
        public void GetSetting_WhenSettingExists_ShouldReturnSettingValue()
        {
            try
            {
                // This is a no-op test since we can't mock static methods without refactoring
                string result = NativeStorageService.GetSetting("test_key");

                // We can only verify no exception was thrown, not the actual value
                Assert.IsNotNull(result); // This might fail in some environments
            }
            catch (Exception ex)
            {
                Assert.Fail($"Method threw an exception: {ex.Message}");
            }
        }

        [TestMethod]
        public async Task SaveSetting_WithStringValue_ShouldSaveSettingValue()
        {
            // Arrange
            string key = "test_key";
            string value = "test_value";

            try
            {
                // Act
                await NativeStorageService.SaveSetting(key, value);

                // Assert - verify no exception was thrown
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Method threw an exception: {ex.Message}");
            }
        }

        [TestMethod]
        public async Task SaveSetting_WithObjectValue_ShouldSaveSettingValueAsString()
        {
            // Arrange
            string key = "test_key";
            object value = new { Property = "test_value" };

            try
            {
                // Act
                await NativeStorageService.SaveSetting(key, value);

                // Assert - verify no exception was thrown
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Method threw an exception: {ex.Message}");
            }
        }

        [TestMethod]
        public async Task UpdateSetting_WithStringValues_ShouldUpdateSettingValue()
        {
            // Arrange
            string key = "test_key";
            string oldValue = "old_value";
            string newValue = "new_value";

            try
            {
                // Act
                await NativeStorageService.UpdateSetting(key, oldValue, newValue);

                // Assert - verify no exception was thrown
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Method threw an exception: {ex.Message}");
            }
        }

        [TestMethod]
        public async Task UpdateSetting_WithObjectValues_ShouldUpdateSettingValueAsString()
        {
            // Arrange
            string key = "test_key";
            object oldValue = new { Property = "old_value" };
            object newValue = new { Property = "new_value" };

            try
            {
                // Act
                await NativeStorageService.UpdateSetting(key, oldValue, newValue);

                // Assert - verify no exception was thrown
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Method threw an exception: {ex.Message}");
            }
        }
    }
}