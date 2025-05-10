Feature: Settings
  As a user of the location application
  I want to configure application settings
  So that I can personalize my experience

Background:
  Given I am logged into the application
  And I am on the settings page

Scenario: Change hemisphere setting
  When I toggle the hemisphere setting to "North"
  Then the hemisphere value should be saved as "North"
  When I toggle the hemisphere setting to "South"
  Then the hemisphere value should be saved as "South"

Scenario: Change time format setting
  When I toggle the time format setting to "12-hour"
  Then the time format value should be saved as "12-hour"
  When I toggle the time format setting to "24-hour"
  Then the time format value should be saved as "24-hour"

Scenario: Change date format setting
  When I toggle the date format setting to "MM/DD/YYYY"
  Then the date format value should be saved as "MM/DD/YYYY"
  When I toggle the date format setting to "DD/MM/YYYY"
  Then the date format value should be saved as "DD/MM/YYYY"

Scenario: Change wind direction setting
  When I toggle the wind direction setting to "Towards Wind"
  Then the wind direction value should be saved as "Towards Wind"
  When I toggle the wind direction setting to "With Wind"
  Then the wind direction value should be saved as "With Wind"

Scenario: Change temperature format setting
  When I toggle the temperature format setting to "Fahrenheit"
  Then the temperature format value should be saved as "Fahrenheit"
  When I toggle the temperature format setting to "Celsius"
  Then the temperature format value should be saved as "Celsius"

Scenario: Change ad support setting
  When I toggle the ad support setting to "Enabled"
  Then the ad support value should be saved as "Enabled"
  When I toggle the ad support setting to "Disabled"
  Then the ad support value should be saved as "Disabled"

Scenario: Settings are persisted between app restarts
  When I set all settings to custom values
  And I close and reopen the application
  Then all my custom settings should be preserved