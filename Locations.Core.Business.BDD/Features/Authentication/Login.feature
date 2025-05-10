Feature: Login
  As a new user of the location application
  I want to set up my profile and preferences
  So that I can use the application with my personalized settings

Background:
  Given I am on the login page

Scenario: Login with valid email
  When I enter email "user@example.com"
  And I select "North" hemisphere
  And I select "12-hour" time format
  And I select "MM/DD/YYYY" date format
  And I select "Towards Wind" wind direction
  And I select "Fahrenheit" temperature format
  And I tap the save button
  Then I should be logged in successfully
  And I should be taken to the main page

Scenario: Login validation for invalid email
  When I enter email "invalid-email"
  And I tap the save button
  Then I should see an email validation message
  And I should remain on the login page

Scenario: Login with minimal settings
  When I enter email "user@example.com"
  And I tap the save button
  Then I should be logged in successfully
  And I should be taken to the main page
  And default settings should be applied

Scenario Outline: Email validation scenarios
  When I enter email "<email>"
  And I tap the save button
  Then I should see "<result>"

  Examples:
    | email             | result                        |
    | user@example.com  | Login successful              |
    | invalid-email     | Please enter a valid email    |
    |                   | Email is required             |
    | very-long-email-address-that-exceeds-the-maximum-allowed-length@verylongdomainname.com | Email exceeds maximum length |