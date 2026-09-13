using KinoPoshuk.Application.DTO;
using KinoPoshuk.Application.Interfaces;
using KinoPoshuk.Application.Services;
using KinoPoshuk.Domain.Entities;
using KinoPoshuk.Domain.Interfaces;
using KinoPoshuk.SharedKernel.Exceptions;
using Moq;
using NUnit.Framework;

namespace KinoPoshuk.Tests;

[TestFixture]
public class MovieServiceTests
{
    private Mock<IOmdbApiClient> _omdbApiClient = null!;
    private Mock<IFavoriteMovieRepository> _favoriteRepo = null!;
    private Mock<ISearchHistoryRepository> _historyRepo = null!;
    private MovieService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _omdbApiClient = new Mock<IOmdbApiClient>();
        _favoriteRepo = new Mock<IFavoriteMovieRepository>();
        _historyRepo = new Mock<ISearchHistoryRepository>();
        _service = new MovieService(_omdbApiClient.Object, _favoriteRepo.Object, _historyRepo.Object);
    }

    [Test]
    public async Task SearchAsync_MovieFound_RecordsHistoryAsFound()
    {
        var movie = new MovieDto { ImdbId = "tt1375666", Title = "Inception" };
        _omdbApiClient.Setup(c => c.GetByTitleAsync("Inception")).ReturnsAsync(movie);

        var result = await _service.SearchAsync("Inception");

        Assert.That(result, Is.EqualTo(movie));
        _historyRepo.Verify(r => r.AddAsync(It.Is<SearchHistoryEntry>(
            e => e.Query == "Inception" && e.WasFound)), Times.Once);
        _historyRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task SearchAsync_MovieNotFound_RecordsHistoryAsNotFound()
    {
        _omdbApiClient.Setup(c => c.GetByTitleAsync("Nonexistent")).ReturnsAsync((MovieDto?)null);

        var result = await _service.SearchAsync("Nonexistent");

        Assert.That(result, Is.Null);
        _historyRepo.Verify(r => r.AddAsync(It.Is<SearchHistoryEntry>(
            e => e.Query == "Nonexistent" && !e.WasFound)), Times.Once);
    }

    [Test]
    public async Task AddFavoriteAsync_AlreadyExists_DoesNotAddAgain()
    {
        var existing = new FavoriteMovie { Id = 1, ImdbId = "tt1", Title = "Existing" };
        _favoriteRepo.Setup(r => r.GetByImdbIdAsync("tt1")).ReturnsAsync(existing);

        var result = await _service.AddFavoriteAsync(new AddFavoriteRequestDto { ImdbId = "tt1", Title = "Existing" });

        Assert.That(result.Id, Is.EqualTo(1));
        _favoriteRepo.Verify(r => r.AddAsync(It.IsAny<FavoriteMovie>()), Times.Never);
    }

    [Test]
    public async Task AddFavoriteAsync_NewMovie_AddsAndSaves()
    {
        _favoriteRepo.Setup(r => r.GetByImdbIdAsync("tt2")).ReturnsAsync((FavoriteMovie?)null);

        var result = await _service.AddFavoriteAsync(new AddFavoriteRequestDto { ImdbId = "tt2", Title = "New" });

        Assert.That(result.ImdbId, Is.EqualTo("tt2"));
        _favoriteRepo.Verify(r => r.AddAsync(It.Is<FavoriteMovie>(f => f.ImdbId == "tt2")), Times.Once);
        _favoriteRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public void RemoveFavoriteAsync_NotFound_ThrowsMovieNotFoundException()
    {
        _favoriteRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((FavoriteMovie?)null);

        Assert.ThrowsAsync<MovieNotFoundException>(() => _service.RemoveFavoriteAsync(99));
    }

    [Test]
    public async Task RemoveFavoriteAsync_Found_RemovesAndSaves()
    {
        var entity = new FavoriteMovie { Id = 5, ImdbId = "tt5" };
        _favoriteRepo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(entity);

        await _service.RemoveFavoriteAsync(5);

        _favoriteRepo.Verify(r => r.Remove(entity), Times.Once);
        _favoriteRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task GetHistoryAsync_ReturnsMappedEntries()
    {
        _historyRepo.Setup(r => r.GetRecentAsync(20)).ReturnsAsync(new List<SearchHistoryEntry>
        {
            new() { Query = "Matrix", WasFound = true, SearchedAt = DateTime.UtcNow }
        });

        var result = await _service.GetHistoryAsync();

        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Query, Is.EqualTo("Matrix"));
    }
}
