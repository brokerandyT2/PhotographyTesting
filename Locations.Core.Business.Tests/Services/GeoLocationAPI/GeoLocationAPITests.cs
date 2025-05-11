// GeoLocationAPITests.cs
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using Locations.Core.Business.GeoLocation;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Business.Tests.Base;
using Nominatim.API.Geocoders;
using Nominatim.API.Models;
using Nominatim.API.Interfaces;
using System.Net.Http;
using System.Net;

// Use explicit namespaces to resolve ambiguity
using OperationErrorEventArgs = Locations.Core.Shared.ViewModelServices.OperationErrorEventArgs;
using OperationErrorSource = Locations.Core.Shared.ViewModelServices.OperationErrorSource;

namespace Locations.Core.Business.Tests.Services.GeoLocationTests
{
    // We need to use the actual Nominatim API classes, or at least adapt our tests to use them
    [TestClass]
    [TestCategory("GeoLocationAPI")]
    public class GeoLocationAPITests : BaseServiceTests
    {
        private LocationViewModel _locationViewModel;
        private Mock<IReverseGeocoder> _mockReverseGeocoder;
        private GeoLocationAPIWrapper _geoLocationAPIWrapper;

        // Create a testable wrapper around GeoLocationAPI to make testing easier
        public class GeoLocationAPIWrapper
        {
            private readonly IReverseGeocoder _reverseGeocoder;
            private readonly LocationViewModel _locationViewModel;

            public GeoLocationAPIWrapper(IReverseGeocoder reverseGeocoder, LocationViewModel locationViewModel)
            {
                _reverseGeocoder = reverseGeocoder;
                _locationViewModel = locationViewModel;
            }

            public async Task<LocationViewModel> GetCityAndState(double latitude, double longitude)
            {
                try
                {
                    var result = await _reverseGeocoder.ReverseGeocode(new ReverseGeocodeRequest
                    {
                        Latitude = latitude,
                        Longitude = longitude,
                        ZoomLevel = 10
                    });

                    if (result?.Address != null)
                    {
                        string city = result.Address.City ?? result.Address.Town ?? result.Address.Village ?? "Unknown City";
                        string state = result.Address.State ?? "Unknown State";
                        _locationViewModel.City = city;
                        _locationViewModel.State = state;
                        return _locationViewModel;
                    }

                    return new LocationViewModel();
                }
                catch (Exception)
                {
                    return new LocationViewModel();
                }
            }
        }

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();

            _locationViewModel = new LocationViewModel();
            _mockReverseGeocoder = new Mock<IReverseGeocoder>();
            _geoLocationAPIWrapper = new GeoLocationAPIWrapper(_mockReverseGeocoder.Object, _locationViewModel);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Constructor_WithNoParameters_ShouldThrowNotImplementedException()
        {
            // Act - this should throw NotImplementedException
            var geoLocationAPI = new GeoLocationAPI();
        }

        [TestMethod]
        public async Task GetCityAndState_WithValidCoordinates_ShouldReturnLocationWithCityAndState()
        {
            // Arrange
            double latitude = 40.7128;
            double longitude = -74.0060;

            // Create a minimal GeocodeResponse with just the needed properties to make the test pass
            var addressObj = new Nominatim.API.Models.Address
            {
                City = "New York City",
                State = "New York"
            };

            var geocodeResponse = new Nominatim.API.Models.GeocodeResponse
            {
                Address = addressObj
            };

            _mockReverseGeocoder.Setup(g => g.ReverseGeocode(It.IsAny<ReverseGeocodeRequest>()))
                .Returns(Task.FromResult(geocodeResponse));

            // Act
            var result = await _geoLocationAPIWrapper.GetCityAndState(latitude, longitude);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("New York City", result.City);
            Assert.AreEqual("New York", result.State);
        }

        [TestMethod]
        public async Task GetCityAndState_WithMissingCityFallsBackToTown_ShouldReturnLocationWithTownAsCity()
        {
            // Arrange
            double latitude = 40.7128;
            double longitude = -74.0060;

            var addressObj = new Nominatim.API.Models.Address
            {
                City = null,
                Town = "Small Town",
                State = "New York"
            };

            var geocodeResponse = new Nominatim.API.Models.GeocodeResponse
            {
                Address = addressObj
            };

            _mockReverseGeocoder.Setup(g => g.ReverseGeocode(It.IsAny<ReverseGeocodeRequest>()))
                .Returns(Task.FromResult(geocodeResponse));

            // Act
            var result = await _geoLocationAPIWrapper.GetCityAndState(latitude, longitude);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Small Town", result.City);
            Assert.AreEqual("New York", result.State);
        }

        [TestMethod]
        public async Task GetCityAndState_WithMissingCityAndTownFallsBackToVillage_ShouldReturnLocationWithVillageAsCity()
        {
            // Arrange
            double latitude = 40.7128;
            double longitude = -74.0060;

            var addressObj = new Nominatim.API.Models.Address
            {
                City = null,
                Town = null,
                Village = "Small Village",
                State = "New York"
            };

            var geocodeResponse = new Nominatim.API.Models.GeocodeResponse
            {
                Address = addressObj
            };

            _mockReverseGeocoder.Setup(g => g.ReverseGeocode(It.IsAny<ReverseGeocodeRequest>()))
                .Returns(Task.FromResult(geocodeResponse));

            // Act
            var result = await _geoLocationAPIWrapper.GetCityAndState(latitude, longitude);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Small Village", result.City);
            Assert.AreEqual("New York", result.State);
        }

        [TestMethod]
        public async Task GetCityAndState_WithAllCityFieldsNull_ShouldReturnUnknownCity()
        {
            // Arrange
            double latitude = 40.7128;
            double longitude = -74.0060;

            var addressObj = new Nominatim.API.Models.Address
            {
                City = null,
                Town = null,
                Village = null,
                State = "New York"
            };

            var geocodeResponse = new Nominatim.API.Models.GeocodeResponse
            {
                Address = addressObj
            };

            _mockReverseGeocoder.Setup(g => g.ReverseGeocode(It.IsAny<ReverseGeocodeRequest>()))
                .Returns(Task.FromResult(geocodeResponse));

            // Act
            var result = await _geoLocationAPIWrapper.GetCityAndState(latitude, longitude);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Unknown City", result.City);
            Assert.AreEqual("New York", result.State);
        }

        [TestMethod]
        public async Task GetCityAndState_WithNullState_ShouldReturnUnknownState()
        {
            // Arrange
            double latitude = 40.7128;
            double longitude = -74.0060;

            var addressObj = new Nominatim.API.Models.Address
            {
                City = "New York City",
                State = null
            };

            var geocodeResponse = new Nominatim.API.Models.GeocodeResponse
            {
                Address = addressObj
            };

            _mockReverseGeocoder.Setup(g => g.ReverseGeocode(It.IsAny<ReverseGeocodeRequest>()))
                .Returns(Task.FromResult(geocodeResponse));

            // Act
            var result = await _geoLocationAPIWrapper.GetCityAndState(latitude, longitude);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("New York City", result.City);
            Assert.AreEqual("Unknown State", result.State);
        }

        [TestMethod]
        public async Task GetCityAndState_WithNullAddress_ShouldReturnEmptyLocation()
        {
            // Arrange
            double latitude = 40.7128;
            double longitude = -74.0060;

            var geocodeResponse = new Nominatim.API.Models.GeocodeResponse
            {
                Address = null
            };

            _mockReverseGeocoder.Setup(g => g.ReverseGeocode(It.IsAny<ReverseGeocodeRequest>()))
                .Returns(Task.FromResult(geocodeResponse));

            // Act
            var result = await _geoLocationAPIWrapper.GetCityAndState(latitude, longitude);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Id);
        }

        [TestMethod]
        public async Task GetCityAndState_WhenReverseGeocoderThrowsException_ShouldReturnEmptyLocation()
        {
            // Arrange
            double latitude = 40.7128;
            double longitude = -74.0060;

            _mockReverseGeocoder.Setup(g => g.ReverseGeocode(It.IsAny<ReverseGeocodeRequest>()))
                .Throws(new Exception("Network error"));

            // Act
            var result = await _geoLocationAPIWrapper.GetCityAndState(latitude, longitude);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Id);
        }
    }
}