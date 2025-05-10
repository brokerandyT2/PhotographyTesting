Feature: SunCalculator
  As a photographer using the location application
  I want to view sun position information for my locations
  So that I can plan photography sessions at optimal lighting times

Background:
  Given I am logged into the application
  And I have at least one saved location
  And I am viewing the sun calculator for a location

Scenario: View sunrise and sunset times
  Then I should see the sunrise time for the location
  And I should see the sunset time for the location
  And the times should be displayed in my preferred time format

Scenario: View golden hour information
  Then I should see the morning golden hour time
  And I should see the evening golden hour time
  And I should see the duration of golden hour periods

Scenario: View blue hour information
  Then I should see the morning blue hour time
  And I should see the evening blue hour time
  And I should see the duration of blue hour periods

Scenario: Sun position display respects hemisphere setting
  Given my hemisphere preference is set to "North"
  Then the sun position display should be oriented for Northern hemisphere
  Given my hemisphere preference is set to "South"
  Then the sun position display should be oriented for Southern hemisphere

Scenario: Sun position adjusts based on date
  When I change the selected date to tomorrow
  Then the sun position information should update for tomorrow's date
  When I change the selected date to one month ahead
  Then the sun position information should update for the future date

Scenario: Sun calculator shows current sun position
  Then I should see an indicator of the current sun position
  And I should see the current altitude and azimuth of the sun

Scenario: Close sun calculator
  When I tap the close button
  Then I should be returned to the location details page