Feature: Login
  As a user of the Location app
  I want to log in with my email and settings preferences
  So that I can use the app with my personalized settings

  Scenario: Login with valid email
    Given I am on the login page
    When I enter email "test@example.com"
    And I click the save button
    Then I should be logged in successfully

  Scenario: Invalid email validation
    Given I am on the login page
    When I enter email "invalid-email"
    And I click the save button
    Then I should see an email validation message