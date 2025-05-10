// Locations.Core.Business.Tests.UITests/Tests/Shared/PageTutorialModalTests.cs
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using System;
using System.Threading;
using Locations.Core.Business.Tests.UITests.PageObjects.Authentication;
using Locations.Core.Business.Tests.UITests.PageObjects.Shared;

namespace Locations.Core.Business.Tests.UITests.Tests.Shared
{
    [TestFixture]
    [Category("Tutorial")]
    public class PageTutorialModalTests : BaseTest
    {
        private PageTutorialModalPage _tutorialPage;

        [Test]
        [Description("Verify tutorial modal appears on first visit to a page")]
        [Ignore("This test requires a fresh install or reset of first-visit settings")]
        public void TutorialModal_FirstVisit_ShouldAppear()
        {
            Log("Testing tutorial modal on first visit");

            // This test requires a fresh install or a way to reset the "first visit" settings
            // For demonstration purposes, we'll show how it would be structured

            // First login if needed
            var loginPage = new LoginPage(Driver, CurrentPlatform);
            if (loginPage.IsCurrentPage())
            {
                loginPage.Login();
            }

            // Navigate to a page that would show a tutorial on first visit
            // This depends on your app's navigation and which pages show tutorials

            // Check if tutorial modal appears
            _tutorialPage = new PageTutorialModalPage(Driver, CurrentPlatform);
            Assert.That(_tutorialPage.IsCurrentPage(), Is.True, "Tutorial modal did not appear on first visit");

            // Verify tutorial has web view content
            Assert.That(_tutorialPage.HasWebViewContent(), Is.True, "Tutorial modal does not have web view content");
        }

        [Test]
        [Description("Verify tutorial modal can be dismissed")]
        public void TutorialModal_Dismiss_ShouldCloseModal()
        {
            Log("Testing tutorial modal dismissal");

            // This test assumes we can navigate directly to a tutorial modal
            // For a real implementation, we might need a way to force a tutorial to appear

            // First login if needed
            var loginPage = new LoginPage(Driver, CurrentPlatform);
            if (loginPage.IsCurrentPage())
            {
                loginPage.Login();
            }

            // Check if we're already on a tutorial page
            _tutorialPage = new PageTutorialModalPage(Driver, CurrentPlatform);
            if (!_tutorialPage.IsCurrentPage())
            {
                // If not on a tutorial page, try to navigate to one
                Log("Not currently on a tutorial page, test may need to be skipped");
                Assert.Ignore("Could not navigate to a tutorial modal");
            }

            // If we are on a tutorial page, try to dismiss it
            _tutorialPage.ClickBack();

            // Wait for dismissal
            Thread.Sleep(2000);

            // Verify tutorial modal is no longer visible
            Assert.That(_tutorialPage.IsCurrentPage(), Is.False, "Tutorial modal still visible after dismissal");
        }

        [Test]
        [Description("Verify tutorial modal does not appear on subsequent visits")]
        [Ignore("This test depends on tutorial state tracking")]
        public void TutorialModal_SubsequentVisits_ShouldNotAppear()
        {
            Log("Testing tutorial modal on subsequent visits");

            // This test assumes we've already visited a page and dismissed its tutorial

            // First login if needed
            var loginPage = new LoginPage(Driver, CurrentPlatform);
            if (loginPage.IsCurrentPage())
            {
                loginPage.Login();
            }

            // Navigate away from and then back to a page that has a tutorial
            // For example, navigate between tabs or screens

            // Check if tutorial modal appears
            _tutorialPage = new PageTutorialModalPage(Driver, CurrentPlatform);
            Assert.That(_tutorialPage.IsCurrentPage(), Is.False, "Tutorial modal appeared on subsequent visit");
        }
    }
}