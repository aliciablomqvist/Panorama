
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
Then the list should be displayed in ascending order of release dates.
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
Feature: Group Chat
As a group member,
I want to send chat messages in a group,
So that I can discuss and share opinions with other members.

Scenario: Send a chat message
Given that I am a member of a group
And the group exists in the database
When I send a message saying "This movie is amazing!"
Then the message should be saved in the group's chat history.
Scenario: View group chat history
Given that I am a member of a group
And the group has chat messages in its history
When I view the group chat
Then I should see all previous messages in the order they were sent.
Feature: Watch Movie Trailers
As a user,
I want to watch trailers for specific movies,
So that I can decide whether I want to watch the movie.

Scenario: Play a trailer for a movie
Given that a movie exists in the database
And the movie has a trailer URL
When I click "Play Trailer"
Then the trailer should play in a video player.