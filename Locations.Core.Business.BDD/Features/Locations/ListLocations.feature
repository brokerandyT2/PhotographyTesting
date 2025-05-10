Feature: ListLocations
  As a user of the location application
  I want to view a list of my saved locations
  So that I can easily find and manage my locations

Background:
  Given I am logged into the application
  And I am on the locations list page

Scenario: View list of locations
  When the locations list loads
  Then I should see my saved locations
  And each location should display its title

Scenario: Select a location to view details
  When I select a location from the list
  Then I should be taken to the location details page
  And I should see details for the selected location

Scenario: No locations display when none exist
  Given I have no saved locations
  When the locations list loads
  Then I should see an empty state message
  And I should see an option to add a new location

Scenario: Filter locations by searching
  Given I have multiple saved locations
  When I enter search text "New York"
  Then I should only see locations matching "New York"

Scenario: Adding a new location
  When I tap the add location button
  Then I should be taken to the add location page

Scenario: Map button navigation
  When I tap the map button for a location
  Then the map should open with the location marked