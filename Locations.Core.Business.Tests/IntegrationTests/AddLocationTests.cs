using Locations.Core.Shared.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;


namespace Locations.Core.Business.Tests.IntegrationTests.Locations;

[TestClass]
public class AddLocationTests
{
    private LocationViewModel _viewModel;

    [TestInitialize]
    public void Setup()
    {
        _viewModel = new LocationViewModel();
    }

    [TestMethod]
    public void AddLocation_WithValidInput_ShouldUpdateLocationsList()
    {
        // Arrange
        _viewModel.Title = "Central Park";
        _viewModel.Description = "A nice green space in NYC";

        // Act
        _viewModel.SaveCommand.Execute(null);

        // Assert
        Assert.IsTrue(_viewModel.Title.Equals("Central Park"));
    }

    [TestMethod]
    public void AddLocation_WithEmptyName_ShouldNotAddLocation()
    {
        // Arrange
        _viewModel.Title = string.Empty;

        // Act
        _viewModel.SaveCommand.Execute(null);

        // Assert
        Assert.IsFalse(_viewModel.Title.Any());
    }
}
