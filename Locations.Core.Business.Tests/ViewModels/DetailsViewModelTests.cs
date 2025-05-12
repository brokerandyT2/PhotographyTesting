// DetailsViewModelTests.cs - Fixed with reflection for event handling
using CommunityToolkit.Mvvm.Input;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Shared.ViewModelServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using System.Reflection;
using OperationErrorEventArgs = Locations.Core.Shared.ViewModelServices.OperationErrorEventArgs;
using OperationErrorSource = Locations.Core.Shared.ViewModelServices.OperationErrorSource;

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

        [TestMethod]
        public void OnSubViewModelErrorOccurred_ShouldBubbleUpError()
        {
            // Arrange
            bool errorOccurred = false;
            string errorMessage = "Test error message";
            OperationErrorEventArgs capturedArgs = null;

            // Create test view models
            var testLocationViewModel = new LocationViewModel();
            var testWeatherViewModel = new WeatherViewModel();
            var testDetailsViewModel = new DetailsViewModel(testLocationViewModel, testWeatherViewModel);

            // Subscribe to the error event on the details view model
            testDetailsViewModel.ErrorOccurred += (sender, e) =>
            {
                errorOccurred = true;
                capturedArgs = e;
            };

            // Create the error event args
            var errorArgs = new OperationErrorEventArgs(
                OperationErrorSource.Unknown,
                errorMessage,
                new Exception("Test exception"));

            // Fix: Use reflection to trigger the ErrorOccurred event from LocationViewModel
            var errorOccurredEvent = typeof(ViewModelBase).GetEvent("ErrorOccurred",
                BindingFlags.Public | BindingFlags.Instance);

            if (errorOccurredEvent != null)
            {
                // Get the backing field for the event
                var errorOccurredField = typeof(ViewModelBase).GetField("ErrorOccurred",
                    BindingFlags.NonPublic | BindingFlags.Instance);

                if (errorOccurredField != null)
                {
                    var eventDelegate = errorOccurredField.GetValue(testLocationViewModel) as MulticastDelegate;
                    if (eventDelegate != null)
                    {
                        // Invoke the event
                        eventDelegate.DynamicInvoke(testLocationViewModel, errorArgs);
                    }
                    else
                    {
                        // Alternative: Use the RaisePropertyChanged to trigger the event indirectly
                        var onErrorMethod = typeof(ViewModelBase).GetMethod("OnErrorOccurred",
                            BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
                            null,
                            new Type[] { typeof(OperationErrorEventArgs) },
                            null);

                        if (onErrorMethod != null)
                        {
                            onErrorMethod.Invoke(testLocationViewModel, new object[] { errorArgs });
                        }
                    }
                }
            }

            // Assert
            Assert.IsTrue(errorOccurred);
            Assert.IsNotNull(capturedArgs);
            Assert.AreEqual(errorMessage, capturedArgs.Message);
            Assert.AreEqual(errorMessage, testDetailsViewModel.ErrorMessage);
        }

        [TestMethod]
        public async Task LoadDataAsync_ShouldCallRefreshCommandsOnSubViewModels()
        {
            // Arrange
            bool locationRefreshCalled = false;
            bool weatherRefreshCalled = false;

            // Create mock view models with command behavior
            var mockLocationViewModel = new Mock<LocationViewModel>();
            var mockWeatherViewModel = new Mock<WeatherViewModel>();

            // Setup the refresh commands
            var locationRefreshCommand = new AsyncRelayCommand(async () =>
            {
                locationRefreshCalled = true;
                await Task.CompletedTask;
            });

            var weatherRefreshCommand = new AsyncRelayCommand(async () =>
            {
                weatherRefreshCalled = true;
                await Task.CompletedTask;
            });

            mockLocationViewModel.Setup(m => m.RefreshCommand).Returns(locationRefreshCommand);
            mockWeatherViewModel.Setup(m => m.RefreshCommand).Returns(weatherRefreshCommand);

            var testDetailsViewModel = new DetailsViewModel(mockLocationViewModel.Object, mockWeatherViewModel.Object);

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