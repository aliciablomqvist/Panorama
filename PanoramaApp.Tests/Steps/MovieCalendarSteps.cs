using TechTalk.SpecFlow;

namespace PanoramaApp.Tests.Steps
{
    [Binding]
    public class MovieCalendarSteps
    {
        private readonly ScenarioContext _scenarioContext;

        public MovieCalendarSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"I am viewing the movie")]
public void GivenIAmViewingTheMovie()
{
    // Simulate viewing a specific movie
}

[When(@"I select a date and time of ""(.*)""")]
public void WhenISelectADateAndTimeOf(string dateTime)
{
    // Simulate selecting a date and time for scheduling
}

[Then(@"the movie should be added to my calendar with the selected date and time\.")]
public void ThenTheMovieShouldBeAddedToMyCalendarWithTheSelectedDateAndTime_()
{
    // Verify the movie was added to the calendar
}


        [Given(@"that I have scheduled movies in my calendar")]
        public void GivenThatIHaveScheduledMoviesInMyCalendar()
        {
            // Simulate having scheduled movies
        }

        [When(@"I view the calendar")]
        public void WhenIViewTheCalendar()
        {
            // Simulate viewing the calendar
        }

        [Then(@"I should see all scheduled movies with their respective dates and times\.")]
        public void ThenIShouldSeeAllScheduledMoviesWithTheirRespectiveDatesAndTimes()
        {
            // Verify scheduled movies are displayed with dates and times
        }
    }
}
