Feature: Sort Movies
As a user,
I want to sort movies by different attributes,
So that I can organize and browse movies more efficiently.

Scenario: Sort movies by title
Given that a list of movies exists in the database
And I am viewing the list of movies
When I choose to sort movies by "title"
Then the list should be displayed in alphabetical order of titles.

Scenario: Sort movies by release date
Given that a list of movies exists in the database
And I am viewing the list of movies
When I choose to sort movies by "release date"
Then the list should be displayed in ascending order of release dates