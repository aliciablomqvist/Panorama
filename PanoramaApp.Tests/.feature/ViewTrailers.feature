Feature: Watch Movie Trailers
  As a user,
  I want to watch trailers for specific movies,
  So that I can decide if I want to watch the movie.

  Scenario: View a trailer for a specific movie
    Given a movie with the title "Inception" exists in the database
    When I select the "Trailer" button for the movie
    Then I should see the trailer for the movie
    And the trailer should play successfully
