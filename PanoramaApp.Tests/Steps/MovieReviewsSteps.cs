using TechTalk.SpecFlow;
namespace PanoramaApp.Tests.Steps
{
    [Binding]
    public class MovieReviewsSteps
    {
        private readonly ScenarioContext _scenarioContext; 

        public MovieReviewsSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }
        

        [Given(@"that a movie exists in the database")]
        public void GivenThatAMovieExistsInTheDatabase()
        {
            // Simulate the movie existing in the database
        }

        [Given(@"I am logged in")]
        public void GivenIAmLoggedIn()
        {
            // Simulate user login
        }

        [When(@"I submit a review with content ""(.*)"" and rating (.*)")]
        public void WhenISubmitAReviewWithContentAndRating(string content, int rating)
        {
            // Simulate submitting a review
        }

        [Then(@"the review should be saved in the database")]
        public void ThenTheReviewShouldBeSavedInTheDatabase()
        {
            // Verify the review is saved in the database
        }
    }
}
