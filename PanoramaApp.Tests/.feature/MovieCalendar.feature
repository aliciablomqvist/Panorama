Feature: Movie Calendar
As a user,
I want to schedule movies in a calendar,
So that I can plan when to watch them.

Scenario: Schedule a movie
Given that a movie exists in the database
And I am viewing the movie
When I select a date and time of "2023-12-01 20:00"
Then the movie should be added to my calendar with the selected date and time.
Scenario: View movie calendar
Given that I have scheduled movies in my calendar
When I view the calendar
Then I should see all scheduled movies with their respective dates and times.