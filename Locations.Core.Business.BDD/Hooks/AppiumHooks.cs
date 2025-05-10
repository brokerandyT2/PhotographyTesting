// Locations.Core.Business.BDD.Hooks/AppiumHooks.cs
using TechTalk.SpecFlow;
using BoDi;
using Locations.Core.Business.Tests.UITests;
using Locations.Core.Business.BDD.Support;

namespace Locations.Core.Business.BDD.Hooks
{
    [Binding]
    public class AppiumHooks : BaseTest
    {
        private readonly IObjectContainer _objectContainer;

        public AppiumHooks(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            // Call the base class SetUp
            base.SetUp();

            // Create a wrapper for the driver
            var driverWrapper = new AppiumDriverWrapper(Driver, CurrentPlatform);

            // Register the wrapper for DI
            _objectContainer.RegisterInstanceAs(driverWrapper);

            // Also register the direct driver if needed
            _objectContainer.RegisterInstanceAs<object>(Driver, "RawDriver");
        }

        [AfterScenario]
        public void AfterScenario()
        {
            // Call the base class TearDown
            base.TearDown();
        }
    }
}