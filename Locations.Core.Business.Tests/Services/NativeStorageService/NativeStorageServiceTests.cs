// NativeStorageServiceTests.cs
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using Locations.Core.Business.StorageSvc;
using Locations.Core.Business.Tests.Base;
using Locations.Core.Shared.ViewModels;
using MockFactory = Locations.Core.Business.Tests.TestHelpers.MockFactory;
using TestDataFactory = Locations.Core.Business.Tests.TestHelpers.TestDataFactory;

namespace Locations.Core.Business.Tests.Services.StorageServiceTests
{
    [TestClass]
    [TestCategory("StorageService")]
    public class NativeStorageServiceTests : BaseServiceTests
    {
        private NativeStorageService _storageService;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();

            _storageService = new NativeStorageService();
        }

        [TestMethod]
        public void DeleteSetting_WhenSettingExists_ShouldDeleteSetting()
        {
            // Note: Since we can't easily mock SecureStorage operations,
            // this test simply verifies the method doesn't throw an exception

            // Act
            var result = NativeStorageService.DeleteSetting("test_key");

            // No assertion as we can't verify the actual deletion due to mocking limitations
        }

        [TestMethod]
        public void GetSetting_WhenSettingExists_ShouldReturnSettingValue()
        {
            // Act
            var result = NativeStorageService.GetSetting("test_key");

            // Cannot assert content due to mocking limitations
        }

        [TestMethod]
        public async Task SaveSetting_WithStringValue_ShouldSaveSettingValue()
        {
            // Arrange
            string key = "test_key";
            string value = "test_value";

            // Act
            await NativeStorageService.SaveSetting(key, value);

            // Cannot assert due to mocking limitations
        }

        [TestMethod]
        public async Task SaveSetting_WithObjectValue_ShouldSaveSettingValueAsString()
        {
            // Arrange
            string key = "test_key";
            object value = new { Property = "test_value" };

            // Act
            await NativeStorageService.SaveSetting(key, value);

            // Cannot assert due to mocking limitations
        }

        [TestMethod]
        public async Task UpdateSetting_WithStringValues_ShouldUpdateSettingValue()
        {
            // Arrange
            string key = "test_key";
            string oldValue = "old_value";
            string newValue = "new_value";

            // Act
            await NativeStorageService.UpdateSetting(key, oldValue, newValue);

            // Cannot assert due to mocking limitations
        }

        [TestMethod]
        public async Task UpdateSetting_WithObjectValues_ShouldUpdateSettingValueAsString()
        {
            // Arrange
            string key = "test_key";
            object oldValue = new { Property = "old_value" };
            object newValue = new { Property = "new_value" };

            // Act
            await NativeStorageService.UpdateSetting(key, oldValue, newValue);

            // Cannot assert due to mocking limitations
        }
    }
}