Feature: ExposureCalculator
  As a photographer using the location application
  I want to calculate equivalent camera exposure settings
  So that I can maintain proper exposure while changing settings

Background:
  Given I am logged into the application
  And I am on the exposure calculator page

Scenario: Set initial exposure values
  When I set aperture to "f/2.8"
  And I set shutter speed to "1/125"
  And I set ISO to "400"
  Then the initial exposure values should be saved

Scenario: Calculate new aperture when changing shutter speed and ISO
  Given I have set initial exposure values
  And I select to calculate aperture
  When I change shutter speed to "1/250"
  And I change ISO to "800"
  Then the calculated aperture should be "f/4"

Scenario: Calculate new shutter speed when changing aperture and ISO
  Given I have set initial exposure values
  And I select to calculate shutter speed
  When I change aperture to "f/4"
  And I change ISO to "200"
  Then the calculated shutter speed should be "1/60"

Scenario: Calculate new ISO when changing aperture and shutter speed
  Given I have set initial exposure values
  And I select to calculate ISO
  When I change aperture to "f/2"
  And I change shutter speed to "1/250"
  Then the calculated ISO should be "200"

Scenario: Change between full stop, half stop, and third stop increments
  When I select "Full" stop increments
  Then the exposure values should update to full stop options
  When I select "Half" stop increments
  Then the exposure values should update to half stop options
  When I select "Thirds" stop increments
  Then the exposure values should update to third stop options

Scenario: Handle overexposure warning
  Given I have set initial exposure values
  And I select to calculate aperture
  When I change shutter speed to "1/4000"
  And I change ISO to "50"
  Then I should see an overexposure warning

Scenario: Handle underexposure warning
  Given I have set initial exposure values
  And I select to calculate shutter speed
  When I change aperture to "f/22"
  And I change ISO to "3200"
  Then I should see an underexposure warning