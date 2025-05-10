Feature: WeatherDisplay
  As a user of the location application
  I want to view weather information for my locations
  So that I can plan visits accordingly

Background:
  Given I am logged into the application
  And I have at least one saved location
  And I am viewing the weather for a location

Scenario: View current weather conditions
  Then I should see the current temperature
  And I should see the weather condition description
  And I should see the daily high and low temperatures

Scenario: View extended forecast
  When I expand day one forecast details
  Then I should see additional weather information
  And I should see wind speed and direction
  And I should see humidity and pressure data

Scenario: View multi-day forecast
  When I view the weather display
  Then I should see forecast information for multiple days
  When I expand day two forecast
  Then I should see day two's weather details
  When I expand day three forecast
  Then I should see day three's weather details

Scenario: Weather display uses correct units
  Given my temperature preference is set to "Fahrenheit"
  When I view the weather display
  Then I should see temperatures in Fahrenheit units
  Given my temperature preference is set to "Celsius"
  When I view the weather display
  Then I should see temperatures in Celsius units

Scenario: Weather display uses correct wind direction format
  Given my wind direction preference is set to "Towards Wind"
  When I view the weather display
  Then the wind direction arrow should point towards the wind
  Given my wind direction preference is set to "With Wind"
  When I view the weather display
  Then the wind direction arrow should point with the wind

Scenario: Close weather display
  When I tap the close button
  Then I should be returned to the location details page