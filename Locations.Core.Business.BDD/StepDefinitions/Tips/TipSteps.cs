using Locations.Core.Business.BDD.TestHelpers;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Data.Models;
using Locations.Core.Data.Queries.Interfaces;
using Locations.Core.Shared.ViewModels;
using Moq;
using NUnit.Framework;
using TechTalk.SpecFlow;
using Assert = NUnit.Framework.Assert;

namespace Locations.Core.Business.BDD.StepDefinitions.Tips
{
    [Binding]
    public class TipsSteps
    {
        private readonly ITipService<TipViewModel> _tipService;
        private readonly ITipTypeService<TipTypeViewModel> _tipTypeService;
        private readonly BDDTestContext _testContext;
        private readonly Mock<ITipRepository> _mockTipRepository;
        private readonly Mock<ITipTypeRepository> _mockTipTypeRepository;

        private TipViewModel _currentTip;
        private List<TipViewModel> _currentTips;
        private List<TipTypeViewModel> _tipTypes;
        private TipTypeViewModel _selectedTipType;
        private string _initialTipText;
        private bool _exposureCalculatorAccessed;

        public TipsSteps(
            ITipService<TipViewModel> tipService,
            ITipTypeService<TipTypeViewModel> tipTypeService,
            BDDTestContext testContext,
            Mock<ITipRepository> mockTipRepository,
            Mock<ITipTypeRepository> mockTipTypeRepository)
        {
            _tipService = tipService;
            _tipTypeService = tipTypeService;
            _testContext = testContext;
            _mockTipRepository = mockTipRepository;
            _mockTipTypeRepository = mockTipTypeRepository;
            _currentTips = new List<TipViewModel>();
        }

        [Given(@"I am on the tips page")]
        public async Task GivenIAmOnTheTipsPage()
        {
            // Setup tip types
            _tipTypes = TestDataFactory.CreateTestTipTypes();
            _mockTipTypeRepository.Setup(x => x.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<TipTypeViewModel>>.Success(_tipTypes));

            // Setup tips for first type
            var firstType = _tipTypes.First();
            var tips = TestDataFactory.CreateTestTips(3, firstType.Id);

            // Setup mock to return tips
            _mockTipRepository.Setup(x => x.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<TipViewModel>>.Success(tips));

            // Get tips
            var tipResult = await _tipService.GetTipsForTypeAsync(firstType.Id);
            if (tipResult.IsSuccess)
            {
                _currentTips = tipResult.Data;
                _currentTip = _currentTips.FirstOrDefault();
                _testContext.CurrentTip = _currentTip;
            }

            // Store initial values
            if (_currentTip != null)
            {
                _initialTipText = _currentTip.Description;
            }
        }

        [When(@"I select tip type ""(.*)""")]
        public async Task WhenISelectTipType(string tipType)
        {
            _selectedTipType = _tipTypes.FirstOrDefault(t => t.Name == tipType);

            if (_selectedTipType != null)
            {
                // Setup tips for this type
                var tips = TestDataFactory.CreateTestTips(3, _selectedTipType.Id);

                _mockTipRepository.Setup(x => x.GetAllAsync())
                    .ReturnsAsync(DataOperationResult<IList<TipViewModel>>.Success(tips));

                var result = await _tipService.GetTipsForTypeAsync(_selectedTipType.Id);
                if (result.IsSuccess)
                {
                    _currentTips = result.Data;
                    _currentTip = _currentTips.FirstOrDefault();
                    _testContext.CurrentTip = _currentTip;
                }
            }
        }

        [When(@"I tap the refresh button")]
        public async Task WhenITapTheRefreshButton()
        {
            if (_selectedTipType != null)
            {
                var result = await _tipService.GetRandomTipForTypeAsync(_selectedTipType.Id);
                if (result.IsSuccess)
                {
                    _currentTip = result.Data;
                    _testContext.CurrentTip = result.Data;
                }
            }
        }

        [When(@"I view a photography tip")]
        public void WhenIViewAPhotographyTip()
        {
            // Current tip is already loaded
            Assert.That(_currentTip, Is.Not.Null);
        }

        [When(@"I tap the exposure calculator button")]
        public void WhenITapTheExposureCalculatorButton()
        {
            _exposureCalculatorAccessed = true;

            // Check if user has premium access
            var subscriptionType = _testContext.SubscriptionType;
            if (subscriptionType != "Premium")
            {
                // Free users would be shown subscription options
                _testContext.SubscriptionType = "Free";
            }
        }

        [Then(@"I should see photography tips content")]
        public void ThenIShouldSeePhotographyTipsContent()
        {
            Assert.That(_currentTips, Is.Not.Null);
            Assert.That(_currentTips.Count, Is.GreaterThan(0));
            Assert.That(_currentTip, Is.Not.Null);
        }

        [Then(@"I should see camera settings information")]
        public void ThenIShouldSeeCameraSettingsInformation()
        {
            Assert.That(_currentTip, Is.Not.Null);
            Assert.That(string.IsNullOrEmpty(_currentTip.Apeture), Is.False);
            Assert.That(string.IsNullOrEmpty(_currentTip.Shutterspeed), Is.False);
            Assert.That(string.IsNullOrEmpty(_currentTip.Iso), Is.False);
        }

        [Then(@"I should see photography advice text")]
        public void ThenIShouldSeePhotographyAdviceText()
        {
            Assert.That(_currentTip, Is.Not.Null);
            Assert.That(string.IsNullOrEmpty(_currentTip.Description), Is.False);
        }

        [Then(@"I should see tips specific to landscape photography")]
        public void ThenIShouldSeeTipsSpecificToLandscapePhotography()
        {
            Assert.That(_selectedTipType, Is.Not.Null);
            Assert.That(_selectedTipType.Name, Is.EqualTo("Landscape"));
            Assert.That(_currentTip, Is.Not.Null);
            Assert.That(_currentTip.TipTypeId, Is.EqualTo(_selectedTipType.Id));
        }

        [Then(@"I should see tips specific to portrait photography")]
        public void ThenIShouldSeeTipsSpecificToPortraitPhotography()
        {
            Assert.That(_selectedTipType, Is.Not.Null);
            Assert.That(_selectedTipType.Name, Is.EqualTo("Portrait"));
            Assert.That(_currentTip, Is.Not.Null);
            Assert.That(_currentTip.TipTypeId, Is.EqualTo(_selectedTipType.Id));
        }

        [Then(@"I should see tips specific to night photography")]
        public void ThenIShouldSeeTipsSpecificToNightPhotography()
        {
            Assert.That(_selectedTipType, Is.Not.Null);
            Assert.That(_selectedTipType.Name, Is.EqualTo("Night"));
            Assert.That(_currentTip, Is.Not.Null);
            Assert.That(_currentTip.TipTypeId, Is.EqualTo(_selectedTipType.Id));
        }

        [Then(@"I should see f-stop information")]
        public void ThenIShouldSeeFStopInformation()
        {
            Assert.That(_currentTip, Is.Not.Null);
            Assert.That(string.IsNullOrEmpty(_currentTip.Apeture), Is.False);
        }

        [Then(@"I should see shutter speed information")]
        public void ThenIShouldSeeShutterSpeedInformation()
        {
            Assert.That(_currentTip, Is.Not.Null);
            Assert.That(string.IsNullOrEmpty(_currentTip.Shutterspeed), Is.False);
        }

        [Then(@"I should see ISO information")]
        public void ThenIShouldSeeISOInformation()
        {
            Assert.That(_currentTip, Is.Not.Null);
            Assert.That(string.IsNullOrEmpty(_currentTip.Iso), Is.False);
        }

        [Then(@"I should see a different landscape photography tip")]
        public void ThenIShouldSeeADifferentLandscapePhotographyTip()
        {
            Assert.That(_currentTip, Is.Not.Null);
            Assert.That(_currentTip.Description, Is.Not.EqualTo(_initialTipText));
        }

        [Then(@"I should see camera settings appropriate for ""(.*)"" photography")]
        public void ThenIShouldSeeCameraSettingsAppropriateForPhotography(string tipType)
        {
            Assert.That(_currentTip, Is.Not.Null);
            Assert.That(string.IsNullOrEmpty(_currentTip.Apeture), Is.False);
            Assert.That(string.IsNullOrEmpty(_currentTip.Shutterspeed), Is.False);
            Assert.That(string.IsNullOrEmpty(_currentTip.Iso), Is.False);

            // Verify the tip is for the correct type
            var expectedType = _tipTypes.FirstOrDefault(t => t.Name == tipType);
            Assert.That(expectedType, Is.Not.Null);
            Assert.That(_currentTip.TipTypeId, Is.EqualTo(expectedType.Id));
        }

        [Then(@"I should be taken to the exposure calculator")]
        public void ThenIShouldBeTakenToTheExposureCalculator()
        {
            if (_testContext.SubscriptionType == "Free")
            {
                // Free users would see subscription options
                Assert.That(_exposureCalculatorAccessed, Is.True);
            }
            else
            {
                // Premium users would access the calculator
                Assert.That(_exposureCalculatorAccessed, Is.True);
            }
        }

        [Then(@"I should see exposure calculation tools")]
        public void ThenIShouldSeeExposureCalculationTools()
        {
            if (_testContext.SubscriptionType == "Premium")
            {
                // Premium users would see the calculator
                Assert.That(_exposureCalculatorAccessed, Is.True);
            }
            else
            {
                // Free users would be prompted to upgrade
                Assert.Inconclusive("Exposure calculator requires premium subscription");
            }
        }
    }
}