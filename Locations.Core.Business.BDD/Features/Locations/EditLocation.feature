Feature: EditLocation
  As a user of the location application
  I want to edit my existing locations
  So that I can update information as needed

Background:
  Given I am logged into the application
  And I have at least one saved location
  And I have selected a location to edit

Scenario: Edit location title
  When I change the location title to "Updated Location Title"
  And I tap the save button
  Then the location should be updated with the new title
  And I should see "Updated Location Title" in my locations list

Scenario: Edit location description
  When I change the location description to "Updated description text"
  And I tap the save button
  Then the location should be updated with the new description

Scenario: Delete location
  When I tap the delete button
  And I confirm the deletion
  Then the location should be removed from my saved locations
  And I should be returned to the locations list
  And I should not see the deleted location in my list

Scenario: View location weather
  When I tap the weather button
  Then I should see the weather details for this location
  And I should see forecast information for multiple days

Scenario: View location sun events
  When I tap the sun events button
  Then I should see the sun calculator for this location
  And I should see sunrise and sunset times
  And I should see golden hour information

Scenario: Cancel edit without saving
  When I make changes to the location
  And I tap the close button without saving
  Then I should be prompted to confirm discarding changes
  When I confirm discarding changes
  Then I should be returned to the locations list
  And the location should remain unchanged

Scenario: Change location photo
  When I tap the change photo button
  Then I should be able to select a new photo
  When I save the location
  Then the location should be updated with the new photo