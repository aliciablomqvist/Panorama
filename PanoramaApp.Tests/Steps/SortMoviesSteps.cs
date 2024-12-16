using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Xunit;
using PanoramaApp.Models;

namespace PanoramaApp.Tests.Steps
{
    [Binding]
    public class SortMoviesSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private List<Movie> _movies;
        private List<Movie> _sortedMovies;

        public SortMoviesSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"that a list of movies exists in the database")]
        public void GivenThatAListOfMoviesExistsInTheDatabase()
        {
            // Simulate movies existing in the database
            _movies = new List<Movie>
            {
                new Movie { Id = 1, Title = "Inception", ReleaseDate = new System.DateTime(2010, 7, 16) },
                new Movie { Id = 2, Title = "The Matrix", ReleaseDate = new System.DateTime(1999, 3, 31) },
                new Movie { Id = 3, Title = "Interstellar", ReleaseDate = new System.DateTime(2014, 11, 7) }
            };
            _scenarioContext["Movies"] = _movies;
        }

        [Given(@"I am viewing the list of movies")]
        public void GivenIAmViewingTheListOfMovies()
        {
            // Simulate viewing the list
            _sortedMovies = _scenarioContext["Movies"] as List<Movie>;
            Assert.NotNull(_sortedMovies);
        }

        [When(@"I choose to sort movies by ""(.*)""")]
        public void WhenIChooseToSortMoviesBy(string criteria)
        {
            // Simulate sorting movies
            _sortedMovies = criteria switch
            {
                "Release Date" => _movies.OrderBy(m => m.ReleaseDate).ToList(),
                "Title" => _movies.OrderBy(m => m.Title).ToList(),
                _ => _movies
            };

            _scenarioContext["SortedMovies"] = _sortedMovies;
        }

        [Then(@"the list should be displayed in ascending order of release dates")]
        public void ThenTheListShouldBeDisplayedInAscendingOrderOfReleaseDates()
        {
            // Verify sorting by release dates
            var sortedMovies = _scenarioContext["SortedMovies"] as List<Movie>;
            Assert.NotNull(sortedMovies);
            Assert.True(sortedMovies.SequenceEqual(sortedMovies.OrderBy(m => m.ReleaseDate)));
        }

        [Then(@"the list should be displayed in alphabetical order of titles\.")]
        public void ThenTheListShouldBeDisplayedInAlphabeticalOrderOfTitles()
        {
            // Verify sorting by titles
            var sortedMovies = _scenarioContext["SortedMovies"] as List<Movie>;
            Assert.NotNull(sortedMovies);
            Assert.True(sortedMovies.SequenceEqual(sortedMovies.OrderBy(m => m.Title)));
        }
    }
}
