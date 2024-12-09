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