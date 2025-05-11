// LocationViewModelTests.cs - Fixed
using Locations.Core.Shared.ViewModels;
using Locations.Core.Shared.ViewModelServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using GeolocationAccuracy = Locations.Core.Shared.ViewModelServices.GeolocationAccuracy;
using LocationChangedEventArgs = Locations.Core.Shared.ViewModelServices.LocationChangedEventArgs;

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
            var viewModel = new LocationViewModel();

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

            _viewModel.InitializeFromDTO(dto);

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
            _viewModel.InitializeFromDTO(null);
        }

        [TestMethod]
        public async Task SaveAsync_WithValidLocation_ShouldCallLocationService()
        {
            _viewModel.Title = "Test Location";
            _viewModel.Description = "Test Description";
            _viewModel.IsError = false;
            _viewModel.ErrorMessage = string.Empty;

            _mockLocationService
                .Setup(service => service.SaveLocationAsync(It.IsAny<LocationViewModel>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(Locations.Core.Shared.ViewModelServices.OperationResult<LocationViewModel>.Success(_viewModel));

            var saveMethod = typeof(LocationViewModel).GetMethod("SaveAsync",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (saveMethod != null)
            {
                await (Task)saveMethod.Invoke(_viewModel, null);
            }

            _mockLocationService.Verify(service => service.SaveLocationAsync(It.IsAny<LocationViewModel>(),
                It.IsAny<bool>(), It.IsAny<bool>()), Times.Once);
            Assert.IsFalse(_viewModel.VmIsNewLocation);
        }

        [TestMethod]
        public async Task SaveAsync_WhenValidationFails_ShouldSetErrorMessage()
        {
            _viewModel.Title = "";
            _viewModel.Description = "Test Description";
            _viewModel.IsError = false;
            _viewModel.ErrorMessage = string.Empty;

            var saveMethod = typeof(LocationViewModel).GetMethod("SaveAsync",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (saveMethod != null)
            {
                await (Task)saveMethod.Invoke(_viewModel, null);
            }

            Assert.IsTrue(_viewModel.IsError);
            Assert.IsTrue(_viewModel.ErrorMessage.Contains("Title is required"));
            _mockLocationService.Verify(service => service.SaveLocationAsync(It.IsAny<LocationViewModel>(),
                It.IsAny<bool>(), It.IsAny<bool>()), Times.Never);
        }

        [TestMethod]
        public async Task SaveAsync_WhenLocationServiceFails_ShouldSetErrorMessage()
        {
            _viewModel.Title = "Test Location";
            _viewModel.Description = "Test Description";
            _viewModel.IsError = false;
            _viewModel.ErrorMessage = string.Empty;

            _mockLocationService
                .Setup(service => service.SaveLocationAsync(It.IsAny<LocationViewModel>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(Locations.Core.Shared.ViewModelServices.OperationResult<LocationViewModel>.Failure(
                    Locations.Core.Shared.ViewModelServices.OperationErrorSource.Unknown,
                    "Failed to save location"));

            var saveMethod = typeof(LocationViewModel).GetMethod("SaveAsync",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (saveMethod != null)
            {
                await (Task)saveMethod.Invoke(_viewModel, null);
            }

            Assert.IsTrue(_viewModel.IsError);
            Assert.IsTrue(_viewModel.ErrorMessage.Contains("Failed to save location"));
        }

        [TestMethod]
        public async Task SaveAsync_WhenExceptionOccurs_ShouldSetErrorMessage()
        {
            _viewModel.Title = "Test Location";
            _viewModel.Description = "Test Description";
            _viewModel.IsError = false;
            _viewModel.ErrorMessage = string.Empty;

            var exception = new Exception("Test exception");

            _mockLocationService
                .Setup(service => service.SaveLocationAsync(It.IsAny<LocationViewModel>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ThrowsAsync(exception);

            var saveMethod = typeof(LocationViewModel).GetMethod("SaveAsync",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (saveMethod != null)
            {
                await (Task)saveMethod.Invoke(_viewModel, null);
            }

            Assert.IsTrue(_viewModel.IsError);
            Assert.IsTrue(_viewModel.ErrorMessage.Contains("Error saving location"));
        }

        [TestMethod]
        public async Task DeleteAsync_WithExistingLocation_ShouldCallLocationService()
        {
            _viewModel.Id = 1;
            _viewModel.IsError = false;
            _viewModel.ErrorMessage = string.Empty;

            _mockLocationService
                .Setup(service => service.DeleteLocationAsync(It.IsAny<LocationViewModel>()))
                .ReturnsAsync(Locations.Core.Shared.ViewModelServices.OperationResult<bool>.Success(true));

            var deleteMethod = typeof(LocationViewModel).GetMethod("DeleteAsync",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (deleteMethod != null)
            {
                await (Task)deleteMethod.Invoke(_viewModel, null);
            }

            _mockLocationService.Verify(service => service.DeleteLocationAsync(It.IsAny<LocationViewModel>()), Times.Once);
            Assert.IsTrue(_viewModel.IsDeleted);
        }

        [TestMethod]
        public async Task DeleteAsync_WhenLocationServiceFails_ShouldSetErrorMessage()
        {
            _viewModel.Id = 1;
            _viewModel.IsError = false;
            _viewModel.ErrorMessage = string.Empty;

            _mockLocationService
                .Setup(service => service.DeleteLocationAsync(It.IsAny<LocationViewModel>()))
                .ReturnsAsync(Locations.Core.Shared.ViewModelServices.OperationResult<bool>.Failure(
                    Locations.Core.Shared.ViewModelServices.OperationErrorSource.Unknown,
                    "Failed to delete location"));

            var deleteMethod = typeof(LocationViewModel).GetMethod("DeleteAsync",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (deleteMethod != null)
            {
                await (Task)deleteMethod.Invoke(_viewModel, null);
            }

            Assert.IsTrue(_viewModel.IsError);
            Assert.IsTrue(_viewModel.ErrorMessage.Contains("Failed to delete location"));
        }

        [TestMethod]
        public async Task TakePhotoAsync_WhenMediaServiceSucceeds_ShouldSetPhotoProperty()
        {
            string filePath = "/path/to/photo.jpg";

            _mockMediaService
                .Setup(service => service.CapturePhotoAsync())
                .ReturnsAsync(Locations.Core.Shared.ViewModelServices.OperationResult<string>.Success(filePath));

            await ((AsyncRelayCommand)_viewModel.TakePhotoCommand).ExecuteAsync(null);

            Assert.AreEqual(filePath, _viewModel.Photo);
        }

        [TestMethod]
        public async Task TakePhotoAsync_WhenMediaServiceFails_ShouldSetErrorMessage()
        {
            _mockMediaService
                .Setup(service => service.CapturePhotoAsync())
                .ReturnsAsync(Locations.Core.Shared.ViewModelServices.OperationResult<string>.Failure(
                    Locations.Core.Shared.ViewModelServices.OperationErrorSource.Unknown,
                    "Failed to capture photo"));

            await ((AsyncRelayCommand)_viewModel.TakePhotoCommand).ExecuteAsync(null);

            Assert.IsTrue(_viewModel.IsError);
            Assert.AreEqual("Failed to capture photo", _viewModel.ErrorMessage);
        }

        [TestMethod]
        public async Task PickPhotoAsync_WhenMediaServiceSucceeds_ShouldSetPhotoProperty()
        {
            string filePath = "/path/to/picked_photo.jpg";

            _mockMediaService
                .Setup(service => service.PickPhotoAsync())
                .ReturnsAsync(Locations.Core.Shared.ViewModelServices.OperationResult<string>.Success(filePath));

            await ((AsyncRelayCommand)_viewModel.PickPhotoCommand).ExecuteAsync(null);

            Assert.AreEqual(filePath, _viewModel.Photo);
        }

        [TestMethod]
        public async Task StartLocationTrackingAsync_WhenGeolocationServiceSucceeds_ShouldStartTracking()
        {
            _mockGeolocationService
                .Setup(service => service.StartTrackingAsync(It.IsAny<GeolocationAccuracy>()))
                .ReturnsAsync(Locations.Core.Shared.ViewModelServices.OperationResult<bool>.Success(true));

            await ((AsyncRelayCommand)_viewModel.StartLocationTrackingCommand).ExecuteAsync(null);

            Assert.IsTrue(_viewModel.VmIsLocationTracking);
        }

        [TestMethod]
        public async Task StartLocationTrackingAsync_WhenGeolocationServiceFails_ShouldSetErrorMessage()
        {
            _mockGeolocationService
                .Setup(service => service.StartTrackingAsync(It.IsAny<Shared.ViewModelServices.GeolocationAccuracy>()))
                .ReturnsAsync(Locations.Core.Shared.ViewModelServices.OperationResult<bool>.Failure(
                    Locations.Core.Shared.ViewModelServices.OperationErrorSource.Unknown,
                    "Failed to start location tracking"));

            await ((AsyncRelayCommand)_viewModel.StartLocationTrackingCommand).ExecuteAsync(null);

            Assert.IsTrue(_viewModel.IsError);
            Assert.AreEqual("Failed to start location tracking", _viewModel.ErrorMessage);
            Assert.IsFalse(_viewModel.VmIsLocationTracking);
        }

        [TestMethod]
        public async Task StopLocationTrackingAsync_WhenTracking_ShouldStopTracking()
        {
            _viewModel.VmIsLocationTracking = true;

            _mockGeolocationService
                .Setup(service => service.StopTrackingAsync())
                .Returns(Task.CompletedTask);

            await ((AsyncRelayCommand)_viewModel.StopLocationTrackingCommand).ExecuteAsync(null);

            Assert.IsFalse(_viewModel.VmIsLocationTracking);
            _mockGeolocationService.Verify(service => service.StopTrackingAsync(), Times.Once);
        }

        [TestMethod]
        public void OnLocationChanged_WithValidCoordinates_ShouldUpdateLocationCoordinates()
        {
            double latitude = 40.7128;
            double longitude = -74.0060;

            var eventArgs = new LocationChangedEventArgs(latitude, longitude);

            _mockGeolocationService.Raise(service => service.LocationChanged += null, eventArgs);

            Assert.AreEqual(latitude, _viewModel.Lattitude);
            Assert.AreEqual(longitude, _viewModel.Longitude);
        }

        [TestMethod]
        public void Validate_WithValidLocation_ShouldReturnTrue()
        {
            _viewModel.Title = "Test Location";
            _viewModel.Description = "Test Description";
            _viewModel.Lattitude = 40.7128;
            _viewModel.Longitude = -74.0060;

            bool isValid = _viewModel.Validate(out List<string> errors);

            Assert.IsTrue(isValid);
            Assert.AreEqual(0, errors.Count);
        }

        [TestMethod]
        public void Validate_WithMissingTitle_ShouldReturnFalse()
        {
            _viewModel.Title = "";
            _viewModel.Description = "Test Description";
            _viewModel.Lattitude = 40.7128;
            _viewModel.Longitude = -74.0060;

            bool isValid = _viewModel.Validate(out List<string> errors);

            Assert.IsFalse(isValid);
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Title is required", errors[0]);
        }

        [TestMethod]
        public void Validate_WithMissingDescription_ShouldReturnFalse()
        {
            _viewModel.Title = "Test Location";
            _viewModel.Description = "";
            _viewModel.Lattitude = 40.7128;
            _viewModel.Longitude = -74.0060;

            bool isValid = _viewModel.Validate(out List<string> errors);

            Assert.IsFalse(isValid);
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Description is required", errors[0]);
        }

        [TestMethod]
        public void Validate_WithInvalidLatitude_ShouldReturnFalse()
        {
            _viewModel.Title = "Test Location";
            _viewModel.Description = "Test Description";
            _viewModel.Lattitude = 95.0;
            _viewModel.Longitude = -74.0060;

            bool isValid = _viewModel.Validate(out List<string> errors);

            Assert.IsFalse(isValid);
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Latitude must be between -90 and 90", errors[0]);
        }

        [TestMethod]
        public void Validate_WithInvalidLongitude_ShouldReturnFalse()
        {
            _viewModel.Title = "Test Location";
            _viewModel.Description = "Test Description";
            _viewModel.Lattitude = 40.7128;
            _viewModel.Longitude = 190.0;

            bool isValid = _viewModel.Validate(out List<string> errors);

            Assert.IsFalse(isValid);
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Longitude must be between -180 and 180", errors[0]);
        }

        [TestMethod]
        public void Validate_WithMultipleValidationErrors_ShouldReturnAllErrors()
        {
            _viewModel.Title = "";
            _viewModel.Description = "";
            _viewModel.Lattitude = 95.0;
            _viewModel.Longitude = 190.0;

            bool isValid = _viewModel.Validate(out List<string> errors);

            Assert.IsFalse(isValid);
            Assert.AreEqual(4, errors.Count);
        }
    }
}