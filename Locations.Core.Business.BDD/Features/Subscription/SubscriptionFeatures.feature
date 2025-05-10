Feature: SubscriptionFeatures
  As a user of the location application
  I want to access premium features through subscription or ads
  So that I can use advanced functionality

Background:
  Given I am logged into the application

Scenario: Access premium feature with free account
  Given I have a free account
  When I attempt to access a premium feature
  Then I should be shown the subscription options page
  And I should see options to subscribe or watch an ad

Scenario: Subscribe to premium
  Given I am on the subscription options page
  When I select the subscription option
  And I complete the payment process
  Then my account should be upgraded to premium
  And I should have access to all premium features
  And my subscription expiration date should be set correctly

Scenario: Watch ad for temporary access
  Given I am on the subscription options page
  When I select the watch ad option
  And I complete viewing the ad
  Then I should have temporary access to the premium feature
  And the access duration should be set correctly

Scenario: Premium feature access with valid subscription
  Given I have an active premium subscription
  When I access a premium feature
  Then I should be able to use the feature without interruption

Scenario: Premium feature access with expired subscription
  Given I have an expired premium subscription
  When I access a premium feature
  Then I should be shown the subscription options page
  And I should see options to renew my subscription or watch an ad

Scenario: Premium features are disabled for free users
  Given I have a free account
  Then premium feature buttons should show a locked indicator
  And attempting to access premium features should show subscription options