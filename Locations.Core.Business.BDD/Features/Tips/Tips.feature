Feature: Tips
  As a photographer using the location application
  I want to view photography tips
  So that I can improve my photography skills

Background:
  Given I am logged into the application
  And I am on the tips page

Scenario: View photography tips
  Then I should see photography tips content
  And I should see camera settings information
  And I should see photography advice text

Scenario: Select different tip types
  When I select tip type "Landscape"
  Then I should see tips specific to landscape photography
  When I select tip type "Portrait"
  Then I should see tips specific to portrait photography
  When I select tip type "Night"
  Then I should see tips specific to night photography

Scenario: Tips display camera settings
  When I view a photography tip
  Then I should see f-stop information
  And I should see shutter speed information
  And I should see ISO information

Scenario: Random tip for selected type
  When I select tip type "Landscape"
  And I tap the refresh button
  Then I should see a different landscape photography tip

Scenario Outline: Different tip types show relevant settings
  When I select tip type "<type>"
  Then I should see camera settings appropriate for "<type>" photography

  Examples:
    | type      | 
    | Landscape |
    | Portrait  |
    | Night     |
    | Macro     |
    | Wildlife  |

Scenario: Access exposure calculator from tips
  When I tap the exposure calculator button
  Then I should be taken to the exposure calculator
  And I should see exposure calculation tools