Feature: SunLocation
  As a photographer using the location application
  I want to track the sun's position in real-time
  So that I can position my camera for optimal lighting

Background:
  Given I am logged into the application
  And I have at least one saved location
  And I am viewing the sun location tracker for a location

Scenario: View current sun position
  Then I should see the current sun direction
  And I should see the current sun elevation

Scenario: Monitor device orientation
  When I start monitoring device sensors
  Then I should see the device tilt angle
  And I should see the compass direction

Scenario: Find sun with device
  Given I have started monitoring device sensors
  When I align the device with the sun's elevation
  Then I should receive a vibration notification
  And the elevation matched indicator should be displayed

Scenario: Change date and time
  When I select a different date
  Then the sun position should update for the selected date
  When I select a different time
  Then the sun position should update for the selected time

Scenario: Stop monitoring sensors
  Given I have started monitoring device sensors
  When I stop monitoring device sensors
  Then the device orientation tracking should stop