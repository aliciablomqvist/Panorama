using TechTalk.SpecFlow;

namespace PanoramaApp.Tests.Steps
{
    [Binding]
    public class SortMoviesSteps
    {
        private readonly ScenarioContext _scenarioContext;

        public SortMoviesSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"that a list of movies exists in the database")]
        public void GivenThatAListOfMoviesExistsInTheDatabase()
        {
            // Simulate movies existing in the database
        }

        [Given(@"I am viewing the list of movies")]
        public void GivenIAmViewingTheListOfMovies()
        {
            // Simulate viewing the list
        }

        [When(@"I choose to sort movies by ""(.*)""")]
        public void WhenIChooseToSortMoviesBy(string criteria)
        {
            // Simulate sorting movies
        }

        [Then(@"the list should be displayed in ascending order of release dates")]
        public void ThenTheListShouldBeDisplayedInAscendingOrderOfReleaseDates()
        {
            // Verify sorting by release dates
        }

        [Then(@"the list should be displayed in alphabetical order of titles\.")]
        public void ThenTheListShouldBeDisplayedInAlphabeticalOrderOfTitles()
        {
            // Verify sorting by titles
        }
    }
}
