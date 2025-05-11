// LocationViewModelTests.cs
using Locations.Core.Shared.ViewModels;
using Locations.Core.Shared.ViewModelServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Locations.Core.Business.Tests.ViewModels
{
    [TestClass]
    [TestCategory("ViewModels")]
    public class LocationViewModelTests
    {
        private Mock<ILocationService> _mockLocationService;
        private Mock<IMediaService> _mockMediaService;
        private Mock<IGeolocationService> _mockGeolocationService;
        private LocationViewModel _viewModel;

        [TestInitialize]
        public void Setup()
        {
            _mockLocationService = new Mock<ILocationService>();
            _mockMediaService = new Mock<IMediaService>();
            _mockGeolocationService = new Mock<IGeolocationService>();

            _viewModel = new LocationViewModel(
                _mockLocationService.Object,
                _mockMediaService.Object,
                _mockGeolocationService.Object);
        }

        [TestMethod]
        public void Constructor_WithNoDependencies_ShouldInitializeProperties()
        {
            // Arrange & Act
            var viewModel = new LocationViewModel();

            // Assert
            Assert.IsNotNull(viewModel.SaveCommand);
            Assert.IsNotNull(viewModel.DeleteCommand);
            Assert.IsNotNull(viewModel.TakePhotoCommand);
            Assert.IsNotNull(viewModel.PickPhotoCommand);
            Assert.IsNotNull(viewModel.StartLocationTrackingCommand);
            Assert.IsNotNull(viewModel.StopLocationTrackingCommand);
            Assert.AreEqual(DateTime.Now.Date, viewModel.Timestamp.Date);
            Assert.AreEqual("MM/dd/yyyy", viewModel.DateFormat);
            Assert.IsTrue(viewModel.VmIsNewLocation);
            Assert.IsFalse(viewModel.VmIsLocationTracking);
        }

        [TestMethod]
        public void Constructor_WithDependencies_ShouldInitializePropertiesAndServices()
        {
            // Assert
            Assert.IsNotNull(_viewModel.SaveCommand);
            Assert.IsNotNull(_viewModel.DeleteCommand);
            Assert.IsNotNull(_viewModel.TakePhotoCommand);
            Assert.IsNotNull(_viewModel.PickPhotoCommand);
            Assert.IsNotNull(_viewModel.StartLocationTrackingCommand);
            Assert.IsNotNull(_viewModel.StopLocationTrackingCommand);
            Assert.AreEqual(DateTime.Now.Date, _viewModel.Timestamp.Date);
            Assert.AreEqual("MM/dd/yyyy", _viewModel.DateFormat);
            Assert.IsTrue(_viewModel.VmIsNewLocation);
            Assert.IsFalse(_viewModel.VmIsLocationTracking);
        }

        [TestMethod]
        public void InitializeFromDTO_WithValidDTO_ShouldInitializeProperties()
        {
            // Arrange
            var dto = new Locations.Core.Shared.DTO.LocationDTO
            {
                Id = 1,
                City = "New York",
                State = "NY",
                Lattitude = 40.7128,
                Longitude = -74.0060,
                Title = "Empire State Building",
                Description = "Famous skyscraper",
                Photo = "photo.jpg",
                Timestamp = new DateTime(2023, 1, 1),
                IsDeleted = false,
                DateFormat = "yyyy-MM-dd"
            };

            // Act
            _viewModel.InitializeFromDTO(dto);

            // Assert
            Assert.AreEqual(1, _viewModel.Id);
            Assert.AreEqual("New York", _viewModel.City);
            Assert.AreEqual("NY", _viewModel.State);
            Assert.AreEqual(40.7128, _viewModel.Lattitude);
            Assert.AreEqual(-74.0060, _viewModel.Longitude);
            Assert.AreEqual("Empire State Building", _viewModel.Title);
            Assert.AreEqual("Famous skyscraper", _viewModel.Description);
            Assert.AreEqual("photo.jpg", _viewModel.Photo);
            Assert.AreEqual(new DateTime(2023, 1, 1), _viewModel.Timestamp);
            Assert.AreEqual(false, _viewModel.IsDeleted);
            Assert.AreEqual("yyyy-MM-dd", _viewModel.DateFormat);
            Assert.IsFalse(_viewModel.VmIsNewLocation);
        }

        [TestMethod]
        public void InitializeFromDTO_WithNullDTO_ShouldNotThrowException()
        {
            // Act & Assert - Should not throw
            _viewModel.InitializeFromDTO(null);
        }

        [TestMethod]
        public async Task SaveAsync_WithValidLocation_ShouldCallLocationService()
        {
            // Arrange
            _viewModel.Title = "Test Location";
            _viewModel.Description = "Test Description";

            var tcs = new TaskCompletionSource<bool>();
            var result = Locations.Core.Shared.ViewModelServices.OperationResult<LocationViewModel>.Success(_viewModel);

            _mockLocationService.Setup(service => service.SaveLocationAsync(It.IsAny<LocationViewModel>(),
                                                                    It.IsAny<bool>(), It.IsAny<bool>()))
                .Callback(() => tcs.SetResult(true))
                .ReturnsAsync(result);

            // Act
            _viewModel.SaveCommand.Execute(null);

            // Wait for the async operation to complete
            await Task.WhenAny(tcs.Task, Task.Delay(1000));

            // Assert
            _mockLocationService.Verify(service => service.SaveLocationAsync(It.IsAny<LocationViewModel>(),
                                                                     It.IsAny<bool>(), It.IsAny<bool>()),
                                                     Times.Once);
            Assert.IsFalse(_viewModel.VmIsNewLocation);
        }

        [TestMethod]
        public async Task SaveAsync_WhenValidationFails_ShouldSetErrorMessage()
        {
            // Arrange
            _viewModel.Title = ""; // Title is required
            _viewModel.Description = "Test Description";

            var tcs = new TaskCompletionSource<bool>();

            // Use a callback to set the result after the validation would happen
            _mockLocationService.Setup(service => service.SaveLocationAsync(It.IsAny<LocationViewModel>(),
                                                                    It.IsAny<bool>(), It.IsAny<bool>()))
                .Callback(() => tcs.SetResult(true));

            // Act
            _viewModel.SaveCommand.Execute(null);

            // Give time for validation to run
            await Task.Delay(100);

            // Assert
            Assert.IsTrue(_viewModel.IsError);
            Assert.IsTrue(_viewModel.ErrorMessage.Contains("Title is required"));
            _mockLocationService.Verify(service => service.SaveLocationAsync(It.IsAny<LocationViewModel>(),
                                                                     It.IsAny<bool>(), It.IsAny<bool>()),
                                                     Times.Never);
        }

        [TestMethod]
        public async Task SaveAsync_WhenLocationServiceFails_ShouldSetErrorMessage()
        {
            // Arrange
            _viewModel.Title = "Test Location";
            _viewModel.Description = "Test Description";

            var tcs = new TaskCompletionSource<bool>();
            var result = Locations.Core.Shared.ViewModelServices.OperationResult<LocationViewModel>.Failure(
                Locations.Core.Shared.ViewModelServices.OperationErrorSource.Unknown,
                "Failed to save location");

            _mockLocationService.Setup(service => service.SaveLocationAsync(It.IsAny<LocationViewModel>(),
                                                                    It.IsAny<bool>(), It.IsAny<bool>()))
                .Callback(() => tcs.SetResult(true))
                .ReturnsAsync(result);

            // Act
            _viewModel.SaveCommand.Execute(null);

            // Wait for the async operation to complete
            await Task.WhenAny(tcs.Task, Task.Delay(1000));

            // Assert
            Assert.IsTrue(_viewModel.IsError);
            Assert.AreEqual("Failed to save location", _viewModel.ErrorMessage);
        }

        [TestMethod]
        public async Task SaveAsync_WhenExceptionOccurs_ShouldSetErrorMessage()
        {
            // Arrange
            _viewModel.Title = "Test Location";
            _viewModel.Description = "Test Description";

            var exception = new Exception("Test exception");

            _mockLocationService.Setup(service => service.SaveLocationAsync(It.IsAny<LocationViewModel>(),
                                                                    It.IsAny<bool>(), It.IsAny<bool>()))
                .ThrowsAsync(exception);

            // Act
            _viewModel.SaveCommand.Execute(null);

            // Give time for exception handling
            await Task.Delay(100);

            // Assert
            Assert.IsTrue(_viewModel.IsError);
            Assert.IsTrue(_viewModel.ErrorMessage.Contains("Error saving location"));
        }

        [TestMethod]
        public async Task DeleteAsync_WithExistingLocation_ShouldCallLocationService()
        {
            // Arrange
            _viewModel.Id = 1;

            var tcs = new TaskCompletionSource<bool>();
            var result = Locations.Core.Shared.ViewModelServices.OperationResult<string>.Success(_viewModel.Id.ToString());

            _mockLocationService.Setup(service => service.DeleteLocationAsync(It.IsAny<LocationViewModel>()))
                .Callback(() => tcs.SetResult(true))
                .ReturnsAsync(Locations.Core.Shared.ViewModelServices.OperationResult<bool>.Success(true));

            // Act
            _viewModel.DeleteCommand.Execute(null);

            // Wait for the async operation to complete
            await Task.WhenAny(tcs.Task, Task.Delay(1000));

            // Assert
            _mockLocationService.Verify(service => service.DeleteLocationAsync(It.IsAny<LocationViewModel>()),
                                                     Times.Once);
            Assert.IsTrue(_viewModel.IsDeleted);
        }

        [TestMethod]
        public async Task DeleteAsync_WhenLocationServiceFails_ShouldSetErrorMessage()
        {
            // Arrange
            _viewModel.Id = 1;

            var tcs = new TaskCompletionSource<bool>();
            var result = Locations.Core.Shared.ViewModelServices.OperationResult<string>.Failure(
                Locations.Core.Shared.ViewModelServices.OperationErrorSource.Unknown,
                "Failed to delete location");

            _mockLocationService.Setup(service => service.DeleteLocationAsync(It.IsAny<LocationViewModel>()))
                .Callback(() => tcs.SetResult(true))
                .ReturnsAsync(Locations.Core.Shared.ViewModelServices.OperationResult<bool>.Failure(
                    Locations.Core.Shared.ViewModelServices.OperationErrorSource.Unknown,
                    "Failed to delete location"));

            // Act
            _viewModel.DeleteCommand.Execute(null);

            // Wait for the async operation to complete
            await Task.WhenAny(tcs.Task, Task.Delay(1000));

            // Assert
            Assert.IsTrue(_viewModel.IsError);
            Assert.AreEqual("Failed to delete location", _viewModel.ErrorMessage);
        }

        [TestMethod]
        public async Task TakePhotoAsync_WhenMediaServiceSucceeds_ShouldSetPhotoProperty()
        {
            // Arrange
            string filePath = "/path/to/photo.jpg";

            var tcs = new TaskCompletionSource<bool>();
            var result = Locations.Core.Shared.ViewModelServices.OperationResult<string>.Success(filePath);

            _mockMediaService.Setup(service => service.CapturePhotoAsync())
                .Callback(() => tcs.SetResult(true))
                .ReturnsAsync(result);

            // Act
            _viewModel.TakePhotoCommand.Execute(null);

            // Wait for the async operation to complete
            await Task.WhenAny(tcs.Task, Task.Delay(1000));

            // Assert
            Assert.AreEqual(filePath, _viewModel.Photo);
        }

        [TestMethod]
        public async Task TakePhotoAsync_WhenMediaServiceFails_ShouldSetErrorMessage()
        {
            // Arrange
            var tcs = new TaskCompletionSource<bool>();
            var result = Locations.Core.Shared.ViewModelServices.OperationResult<string>.Failure(
                Locations.Core.Shared.ViewModelServices.OperationErrorSource.Unknown,
                "Failed to capture photo");

            _mockMediaService.Setup(service => service.CapturePhotoAsync())
                .Callback(() => tcs.SetResult(true))
                .ReturnsAsync(result);

            // Act
            _viewModel.TakePhotoCommand.Execute(null);

            // Wait for the async operation to complete
            await Task.WhenAny(tcs.Task, Task.Delay(1000));

            // Assert
            Assert.IsTrue(_viewModel.IsError);
            Assert.AreEqual("Failed to capture photo", _viewModel.ErrorMessage);
        }

        [TestMethod]
        public async Task PickPhotoAsync_WhenMediaServiceSucceeds_ShouldSetPhotoProperty()
        {
            // Arrange
            string filePath = "/path/to/picked_photo.jpg";

            var tcs = new TaskCompletionSource<bool>();
            var result = Locations.Core.Shared.ViewModelServices.OperationResult<string>.Success(filePath);

            _mockMediaService.Setup(service => service.PickPhotoAsync())
                .Callback(() => tcs.SetResult(true))
                .ReturnsAsync(result);

            // Act
            _viewModel.PickPhotoCommand.Execute(null);

            // Wait for the async operation to complete
            await Task.WhenAny(tcs.Task, Task.Delay(1000));

            // Assert
            Assert.AreEqual(filePath, _viewModel.Photo);
        }

        [TestMethod]
        public async Task StartLocationTrackingAsync_WhenGeolocationServiceSucceeds_ShouldStartTracking()
        {
            // Arrange
            var tcs = new TaskCompletionSource<bool>();
            var result = Locations.Core.Shared.ViewModelServices.OperationResult<bool>.Success(true);

            _mockGeolocationService.Setup(service => service.StartTrackingAsync(It.IsAny<Locations.Core.Shared.ViewModelServices.GeolocationAccuracy>()))
                .Callback(() => tcs.SetResult(true))
                .ReturnsAsync(result);

            // Act
            _viewModel.StartLocationTrackingCommand.Execute(null);

            // Wait for the async operation to complete
            await Task.WhenAny(tcs.Task, Task.Delay(1000));

            // Assert
            Assert.IsTrue(_viewModel.VmIsLocationTracking);
        }

        [TestMethod]
        public async Task StartLocationTrackingAsync_WhenGeolocationServiceFails_ShouldSetErrorMessage()
        {
            // Arrange
            var tcs = new TaskCompletionSource<bool>();
            var result = Locations.Core.Shared.ViewModelServices.OperationResult<bool>.Failure(
                Locations.Core.Shared.ViewModelServices.OperationErrorSource.Unknown,
                "Failed to start location tracking");

            _mockGeolocationService.Setup(service => service.StartTrackingAsync(It.IsAny<Locations.Core.Shared.ViewModelServices.GeolocationAccuracy>()))
                .Callback(() => tcs.SetResult(true))
                .ReturnsAsync(result);

            // Act
            _viewModel.StartLocationTrackingCommand.Execute(null);

            // Wait for the async operation to complete
            await Task.WhenAny(tcs.Task, Task.Delay(1000));

            // Assert
            Assert.IsTrue(_viewModel.IsError);
            Assert.AreEqual("Failed to start location tracking", _viewModel.ErrorMessage);
            Assert.IsFalse(_viewModel.VmIsLocationTracking);
        }

        [TestMethod]
        public async Task StopLocationTrackingAsync_WhenTracking_ShouldStopTracking()
        {
            // Arrange
            _viewModel.VmIsLocationTracking = true;
            var tcs = new TaskCompletionSource<bool>();

            _mockGeolocationService.Setup(service => service.StopTrackingAsync())
                .Callback(() => {
                    tcs.SetResult(true);
                })
                .Returns(Task.CompletedTask);

            // Act
            _viewModel.StopLocationTrackingCommand.Execute(null);

            // Wait for the async operation to complete
            await Task.WhenAny(tcs.Task, Task.Delay(1000));

            // Assert
            Assert.IsFalse(_viewModel.VmIsLocationTracking);
            _mockGeolocationService.Verify(service => service.StopTrackingAsync(), Times.Once);
        }

        [TestMethod]
        public void OnLocationChanged_WithValidCoordinates_ShouldUpdateLocationCoordinates()
        {
            // Arrange
            double latitude = 40.7128;
            double longitude = -74.0060;
            var eventArgs = new Locations.Core.Shared.ViewModelServices.LocationChangedEventArgs(latitude, longitude);

            // We need to use reflection to access the private method
            var method = typeof(LocationViewModel).GetMethod("OnLocationChanged",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Act
            method.Invoke(_viewModel, new object[] { null, eventArgs });

            // Assert - coordinates should be rounded to 6 decimal places
            Assert.AreEqual(latitude, _viewModel.Lattitude);
            Assert.AreEqual(longitude, _viewModel.Longitude);
        }

        [TestMethod]
        public void Validate_WithValidLocation_ShouldReturnTrue()
        {
            // Arrange
            _viewModel.Title = "Test Location";
            _viewModel.Description = "Test Description";
            _viewModel.Lattitude = 40.7128;
            _viewModel.Longitude = -74.0060;

            // Act
            bool isValid = _viewModel.Validate(out List<string> errors);

            // Assert
            Assert.IsTrue(isValid);
            Assert.AreEqual(0, errors.Count);
        }

        [TestMethod]
        public void Validate_WithMissingTitle_ShouldReturnFalse()
        {
            // Arrange
            _viewModel.Title = "";
            _viewModel.Description = "Test Description";
            _viewModel.Lattitude = 40.7128;
            _viewModel.Longitude = -74.0060;

            // Act
            bool isValid = _viewModel.Validate(out List<string> errors);

            // Assert
            Assert.IsFalse(isValid);
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Title is required", errors[0]);
        }

        [TestMethod]
        public void Validate_WithMissingDescription_ShouldReturnFalse()
        {
            // Arrange
            _viewModel.Title = "Test Location";
            _viewModel.Description = "";
            _viewModel.Lattitude = 40.7128;
            _viewModel.Longitude = -74.0060;

            // Act
            bool isValid = _viewModel.Validate(out List<string> errors);

            // Assert
            Assert.IsFalse(isValid);
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Description is required", errors[0]);
        }

        [TestMethod]
        public void Validate_WithInvalidLatitude_ShouldReturnFalse()
        {
            // Arrange
            _viewModel.Title = "Test Location";
            _viewModel.Description = "Test Description";
            _viewModel.Lattitude = 95.0; // Outside valid range (-90 to 90)
            _viewModel.Longitude = -74.0060;

            // Act
            bool isValid = _viewModel.Validate(out List<string> errors);

            // Assert
            Assert.IsFalse(isValid);
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Latitude must be between -90 and 90", errors[0]);
        }

        [TestMethod]
        public void Validate_WithInvalidLongitude_ShouldReturnFalse()
        {
            // Arrange
            _viewModel.Title = "Test Location";
            _viewModel.Description = "Test Description";
            _viewModel.Lattitude = 40.7128;
            _viewModel.Longitude = 190.0; // Outside valid range (-180 to 180)

            // Act
            bool isValid = _viewModel.Validate(out List<string> errors);

            // Assert
            Assert.IsFalse(isValid);
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Longitude must be between -180 and 180", errors[0]);
        }

        [TestMethod]
        public void Validate_WithMultipleValidationErrors_ShouldReturnAllErrors()
        {
            // Arrange
            _viewModel.Title = "";
            _viewModel.Description = "";
            _viewModel.Lattitude = 95.0;
            _viewModel.Longitude = 190.0;

            // Act
            bool isValid = _viewModel.Validate(out List<string> errors);

            // Assert
            Assert.IsFalse(isValid);
            Assert.AreEqual(4, errors.Count);
        }
    }
}