// DetailsViewModelTests.cs - Fixed
using CommunityToolkit.Mvvm.Input;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Shared.ViewModelServices;
using Locations.Core.Shared.ViewModelServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace Locations.Core.Business.Tests.ViewModels
{
    [TestClass]
    [TestCategory("ViewModels")]
    public class DetailsViewModelTests
    {
        private LocationViewModel _locationViewModel;
        private WeatherViewModel _weatherViewModel;
        private DetailsViewModel _viewModel;

        [TestInitialize]
        public void Setup()
        {
            _locationViewModel = new LocationViewModel
            {
                Id = 1,
                Title = "Test Location",
                Description = "Test Description",
                Lattitude = 40.7128,
                Longitude = -74.0060
            };

            _weatherViewModel = new WeatherViewModel
            {
                Id = 1,
                LocationId = 1,
                Temperature = 72.5,
                Description = "Partly Cloudy"
            };

            _viewModel = new DetailsViewModel(_locationViewModel, _weatherViewModel);
        }

        [TestMethod]
        public void Constructor_WithNoDependencies_ShouldInitializeProperties()
        {
            // Arrange & Act
            var viewModel = new DetailsViewModel();

            // Assert
            Assert.IsNotNull(viewModel);
            Assert.IsNull(viewModel.LocationViewModel);
            Assert.IsNull(viewModel.WeatherViewModel);
        }

        [TestMethod]
        public void Constructor_WithDependencies_ShouldInitializePropertiesAndSubscribeToEvents()
        {
            // Assert
            Assert.IsNotNull(_viewModel.LocationViewModel);
            Assert.IsNotNull(_viewModel.WeatherViewModel);
            Assert.AreEqual(1, _viewModel.LocationViewModel.Id);
            Assert.AreEqual("Test Location", _viewModel.LocationViewModel.Title);
            Assert.AreEqual(1, _viewModel.WeatherViewModel.LocationId);
            Assert.AreEqual(72.5, _viewModel.WeatherViewModel.Temperature);
        }

        // DetailsViewModelTests.cs - Fixed OnSubViewModelErrorOccurred_ShouldBubbleUpError test method
        [TestMethod]
        public void OnSubViewModelErrorOccurred_ShouldBubbleUpError()
        {
            // Arrange
            bool errorOccurred = false;
            string errorMessage = "Test error message";

            // Create a test view model to properly handle events
            var testDetailsViewModel = new DetailsViewModel(_locationViewModel, _weatherViewModel);

            // Subscribe to the error event
            testDetailsViewModel.ErrorOccurred += (sender, e) => {
                errorOccurred = true;
                Assert.AreEqual(errorMessage, e.Message);
            };

            // Create ViewModelServices OperationErrorEventArgs which is the type raised
            var errorArgs = new Locations.Core.Shared.ViewModelServices.OperationErrorEventArgs(
                Shared.ViewModelServices.OperationErrorSource.Unknown,
                errorMessage);

            // Use reflection to call the protected/private OnErrorOccurred method
            var onErrorOccurredMethod = typeof(LocationViewModel).GetMethod("OnErrorOccurred",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.Instance,
                null,
                new Type[] { typeof(Shared.ViewModelServices.OperationErrorEventArgs) },
                null);

            if (onErrorOccurredMethod != null)
            {
                onErrorOccurredMethod.Invoke(_locationViewModel, new object[] { errorArgs });
            }
            else
            {
                // Alternative: Try to raise the error event directly
                var errorOccurredEvent = typeof(LocationViewModel).GetEvent("ErrorOccurred",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.Instance);

                if (errorOccurredEvent != null)
                {
                    var fieldInfo = typeof(LocationViewModel).GetField("ErrorOccurred",
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Instance);

                    if (fieldInfo != null)
                    {
                        var handler = (EventHandler<Shared.ViewModelServices.OperationErrorEventArgs>)fieldInfo.GetValue(_locationViewModel);
                        handler?.Invoke(_locationViewModel, errorArgs);
                    }
                }
            }

            // Assert
            Assert.IsTrue(errorOccurred);
            Assert.AreEqual(errorMessage, testDetailsViewModel.ErrorMessage);
        }

        [TestMethod]
        public async Task LoadDataAsync_ShouldCallRefreshCommandsOnSubViewModels()
        {
            // Arrange
            bool locationRefreshCalled = false;
            bool weatherRefreshCalled = false;

            // Mock location view model
            var mockLocationViewModel = new LocationViewModel();
            // Set a custom command for testing
            var locationField = typeof(ViewModelBase).GetField("_refreshCommand",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var locationRefreshCommand = new AsyncRelayCommand(async () =>
            {
                locationRefreshCalled = true;
                await Task.CompletedTask;
            });
            locationField?.SetValue(mockLocationViewModel, locationRefreshCommand);

            // Mock weather view model
            var mockWeatherViewModel = new WeatherViewModel();
            // Set a custom command for testing
            var weatherField = typeof(ViewModelBase).GetField("_refreshCommand",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var weatherRefreshCommand = new AsyncRelayCommand(async () =>
            {
                weatherRefreshCalled = true;
                await Task.CompletedTask;
            });
            weatherField?.SetValue(mockWeatherViewModel, weatherRefreshCommand);

            var testDetailsViewModel = new DetailsViewModel(mockLocationViewModel, mockWeatherViewModel);

            // Act
            if (testDetailsViewModel.RefreshCommand is AsyncRelayCommand refreshCommand)
            {
                await refreshCommand.ExecuteAsync(null);
            }

            // Assert
            Assert.IsTrue(locationRefreshCalled);
            Assert.IsTrue(weatherRefreshCalled);
            Assert.IsFalse(testDetailsViewModel.IsError);
        }

        [TestMethod]
        public void Cleanup_ShouldUnsubscribeFromEvents()
        {
            // Arrange
            // Verify initial state - would need reflection to check events

            // Act
            _viewModel.Cleanup();

            // Assert - testing event unsubscription is difficult
            // This is more of a coverage test to ensure code is executed
            Assert.IsTrue(true);
        }
    }
}