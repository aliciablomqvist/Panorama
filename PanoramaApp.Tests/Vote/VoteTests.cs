using Xunit;
using PanoramaApp.Pages.Vote;
using PanoramaApp.Models;
using System.Linq;
using PanoramaApp.Pages.Vote;

namespace PanoramaApp.Tests.Vote 
{
    public class VoteTests 
    {
        [Fact] 
        public void Vote_IncreasesVoteCount()
        {
            // Arrange
            var model = new VoteModel();
            var initialVotes = model.Movies.First(m => m.Id == 1).Votes; // Hämta initiala röster för första filmen

            // Act
            model.OnPostVote(1); 

            // Assert
            Assert.Equal(initialVotes + 1, model.Movies.First(m => m.Id == 1).Votes); // Kontrollera att röster har ökat med 1
        }

        [Fact] 
        public void Vote_DoesNothingForInvalidId()
        {
            // Arrange
            var model = new VoteModel();
            var totalVotesBefore = model.Movies.Sum(m => m.Votes); 

            // Act
            model.OnPostVote(99);

            // Assert
            var totalVotesAfter = model.Movies.Sum(m => m.Votes); 
            Assert.Equal(totalVotesBefore, totalVotesAfter); 
        }

        [Fact]
public void Vote_MultipleVotesIncreaseCorrectly()
{
    // Arrange
    var model = new VoteModel();
    var initialVotes = model.Movies.First(m => m.Id == 2).Votes;

    // Act
    model.OnPostVote(2); 
    model.OnPostVote(2); 

    // Assert
    Assert.Equal(initialVotes + 2, model.Movies.First(m => m.Id == 2).Votes); 
}

[Fact]
public void MoviesList_ShouldContainThreeMovies()
{
    // Arrange
    var model = new VoteModel();

    // Act
    var moviesCount = model.Movies.Count;

    // Assert
    Assert.Equal(3, moviesCount);
}

[Fact]
public void Vote_OnDifferentMovies_ShouldUpdateCorrectly()
{
    // Arrange
    var model = new VoteModel();
    var initialVotesFirstMovie = model.Movies.First(m => m.Id == 1).Votes;
    var initialVotesSecondMovie = model.Movies.First(m => m.Id == 2).Votes;

    // Act
    model.OnPostVote(1); 
    model.OnPostVote(2);

    // Assert
    Assert.Equal(initialVotesFirstMovie + 1, model.Movies.First(m => m.Id == 1).Votes);
    Assert.Equal(initialVotesSecondMovie + 1, model.Movies.First(m => m.Id == 2).Votes);
}

[Fact]
public void Vote_OnInvalidId_ShouldNotChangeVotes()
{
    // Arrange
    var model = new VoteModel();
    var totalVotesBefore = model.Movies.Sum(m => m.Votes);

    // Act
    model.OnPostVote(99); 

    // Assert
    var totalVotesAfter = model.Movies.Sum(m => m.Votes); 
    Assert.Equal(totalVotesBefore, totalVotesAfter); 
}
[Fact]
public void Vote_WithNullMovies_ShouldNotThrow()
{
    // Arrange
    var model = new VoteModel();
    model.Movies = null;

    // Act & Assert
    var exception = Record.Exception(() => model.OnPostVote(1));
    Assert.Null(exception); 
}
[Fact]
public void TotalVotes_ShouldBeAccurateAfterMultipleVotes()
{
    // Arrange
    var model = new VoteModel();
    var initialTotalVotes = model.Movies.Sum(m => m.Votes);

    // Act
    model.OnPostVote(1); 
    model.OnPostVote(2); 
    model.OnPostVote(2); 
    // Assert
    var expectedTotalVotes = initialTotalVotes + 3; 
    Assert.Equal(expectedTotalVotes, model.Movies.Sum(m => m.Votes));
}
[Fact]
public void Vote_OnNegativeId_ShouldNotChangeVotes()
{
    // Arrange
    var model = new VoteModel();
    var totalVotesBefore = model.Movies.Sum(m => m.Votes);

    // Act
    model.OnPostVote(-1); 

    // Assert
    var totalVotesAfter = model.Movies.Sum(m => m.Votes);
    Assert.Equal(totalVotesBefore, totalVotesAfter); 
}
[Fact]
public void Vote_OnZeroId_ShouldNotChangeVotes()
{
    // Arrange
    var model = new VoteModel();
    var totalVotesBefore = model.Movies.Sum(m => m.Votes);

    // Act
    model.OnPostVote(0); 

    // Assert
    var totalVotesAfter = model.Movies.Sum(m => m.Votes);
    Assert.Equal(totalVotesBefore, totalVotesAfter); 
}
    }
}
