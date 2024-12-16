using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using PanoramaApp.Services;
using PanoramaApp.Models;
using TechTalk.SpecFlow;

namespace PanoramaApp.Tests.Steps
{
    [Binding]
    public class MovieCalendarSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly Mock<IMovieCalendarService> _movieCalendarServiceMock;
        private List<MovieCalendar> _scheduledMovies;
        private string _selectedDateTime;
        private Movie _selectedMovie;

        public MovieCalendarSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _movieCalendarServiceMock = new Mock<IMovieCalendarService>();
            _scheduledMovies = new List<MovieCalendar>();
        }

        [Given(@"I am viewing the movie")]
        public void GivenIAmViewingTheMovie()
        {
            // Simulate viewing a specific movie
            _selectedMovie = new Movie
            {
                Id = 1,
                Title = "Inception",
                ReleaseDate = new DateTime(2010, 7, 16)
            };
            _scenarioContext["SelectedMovie"] = _selectedMovie;
        }

        [When(@"I select a date and time of ""(.*)""")]
        public void WhenISelectADateAndTimeOf(string dateTime)
        {
            // Simulate selecting a date and time for scheduling
            _selectedDateTime = dateTime;
            var parsedDateTime = DateTime.Parse(dateTime);

            _movieCalendarServiceMock.Setup(service => service.ScheduleMovieAsync(
                It.Is<int>(id => id == _selectedMovie.Id),
                It.Is<DateTime>(dt => dt == parsedDateTime)))
            .Callback(() =>
            {
                _scheduledMovies.Add(new MovieCalendar
                {
                    MovieId = _selectedMovie.Id,
                    Movie = _selectedMovie,
                    Date = parsedDateTime
                });
            })
            .ReturnsAsync(true);
        }

        [Then(@"the movie should be added to my calendar with the selected date and time\.")]
        public void ThenTheMovieShouldBeAddedToMyCalendarWithTheSelectedDateAndTime_()
        {
            // Verify the movie was added to the calendar
            var parsedDateTime = DateTime.Parse(_selectedDateTime);
            var scheduledMovie = _scheduledMovies.FirstOrDefault(mc => mc.MovieId == _selectedMovie.Id && mc.Date == parsedDateTime);

            Assert.NotNull(scheduledMovie);
            Assert.Equal(_selectedMovie.Title, scheduledMovie.Movie.Title);
            Assert.Equal(parsedDateTime, scheduledMovie.Date);
        }

        [Given(@"that I have scheduled movies in my calendar")]
        public void GivenThatIHaveScheduledMoviesInMyCalendar()
        {
            // Simulate having scheduled movies
            _scheduledMovies = new List<MovieCalendar>
            {
                new MovieCalendar
                {
                    MovieId = 1,
                    Movie = new Movie { Id = 1, Title = "Inception" },
                    Date = new DateTime(2024, 12, 24, 18, 0, 0)
                },
                new MovieCalendar
                {
                    MovieId = 2,
                    Movie = new Movie { Id = 2, Title = "The Matrix" },
                    Date = new DateTime(2024, 12, 25, 20, 0, 0)
                }
            };
            _scenarioContext["ScheduledMovies"] = _scheduledMovies;
        }

        [When(@"I view the calendar")]
        public void WhenIViewTheCalendar()
        {
            // Simulate viewing the calendar
            _movieCalendarServiceMock.Setup(service => service.GetScheduledMoviesAsync())
                .ReturnsAsync(_scheduledMovies);
        }

        [Then(@"I should see all scheduled movies with their respective dates and times\.")]
        public void ThenIShouldSeeAllScheduledMoviesWithTheirRespectiveDatesAndTimes()
        {
            // Verify scheduled movies are displayed with dates and times
            var scheduledMovies = _movieCalendarServiceMock.Object.GetScheduledMoviesAsync().Result;

            Assert.NotEmpty(scheduledMovies);
            foreach (var movie in _scheduledMovies)
            {
                Assert.Contains(scheduledMovies, m => m.MovieId == movie.MovieId && m.Date == movie.Date);
            }
        }
    }
}
