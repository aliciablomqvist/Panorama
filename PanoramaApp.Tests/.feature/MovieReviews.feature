Feature: Movie Reviews
  As a user/groupmember
  I want to leave reviews for movies
  So that I can share my opinions and recomendations with others

  Scenario: Leave a review for a movie
    Given that a movie exists in the database
    And I am logged in
    When I submit a review with content "Wonderful movie!" and rating 5
    Then the review should be saved in the database
