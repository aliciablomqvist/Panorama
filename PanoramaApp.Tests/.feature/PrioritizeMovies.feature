Feature: Prioritize Movies
As a user,
I want to prioritize movies in my lists,
So that I can plan my watching order.

Scenario: Set priority for a movie
Given that I have a list of movies
And I am viewing my movie list
When I assign a priority of "1" to "Movie A"
Then "Movie A" should appear at the top of the list.
Scenario: View prioritized movie list
Given that I have prioritized movies in my list
When I view the list
Then the movies should be displayed in descending order of priority.