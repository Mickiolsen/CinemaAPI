using Xunit;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using xxx.API.Controllers;
using xxx.Repository.Interfaces;
using xxx.Repository.Models;
using Cinema.Repository.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

public class FakeCrudRepository : ICrudRepository<Movie>
{
    private readonly List<Movie> _movies = new();

    public Task<IEnumerable<Movie>> GetAll()
    {
        return Task.FromResult<IEnumerable<Movie>>(_movies);
    }

    public Task<Movie?> GetById(int id)
    {
        var movie = _movies.Find(m => m.Id == id);
        return Task.FromResult(movie);
    }

    public Task<Movie> Create(Movie entity)
    {
        // Tildel et unikt ID
        entity.Id = _movies.Count > 0 ? _movies.Max(m => m.Id) + 1 : 1;
        _movies.Add(entity);
        return Task.FromResult(entity);
    }

    public Task<bool> DeleteById(int id)
    {
        var movie = _movies.Find(m => m.Id == id);
        if (movie != null)
        {
            _movies.Remove(movie);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }
}

public class FakeMovieRepository : IMovieRepository
{
    public Task<List<Movie>> GetAllMoviesWithActors()
    {
        // Returner en tom liste eller et par testdata efter behov
        return Task.FromResult(new List<Movie>());
    }

    public Task AddActorToMovie(int movieId, int actorId)
    {
        // Ingen handling for mock
        return Task.CompletedTask;
    }
}

// Dummy implementation for IWebHostEnvironment
public class FakeWebHostEnvironment : IWebHostEnvironment
{
    public string EnvironmentName { get; set; } = "Development";
    public string WebRootPath { get; set; } = "wwwroot";
    public string ContentRootPath { get; set; } = Directory.GetCurrentDirectory();
    public IFileProvider FileProvider { get; set; } = null!;
    IFileProvider IWebHostEnvironment.WebRootFileProvider { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    string IHostEnvironment.ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    IFileProvider IHostEnvironment.ContentRootFileProvider { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}

public class MovieControllerTests
{
    private readonly FakeCrudRepository _fakeCrudRepository;
    private readonly FakeMovieRepository _fakeMovieRepository;
    private readonly FakeWebHostEnvironment _fakeWebHostEnvironment;
    private readonly MovieController _controller;

    public MovieControllerTests()
    {
        // Arrange - Setup fake repositories
        _fakeCrudRepository = new FakeCrudRepository();
        _fakeMovieRepository = new FakeMovieRepository();
        _fakeWebHostEnvironment = new FakeWebHostEnvironment();

        // Initialize controller with fake dependencies
        _controller = new MovieController(_fakeCrudRepository, _fakeWebHostEnvironment, _fakeMovieRepository);
    }

    // Tests
    [Fact]
    public async Task CreateMovie_ExistingId_ReturnsBadRequest()
    {
        // Arrange
        var existingMovie = new Movie { Id = 1, Title = "Existing Movie", DurationMinutes = 100, IsPopular = true };
        await _fakeCrudRepository.Create(existingMovie);

        // Act
        var newMovie = new Movie { Id = 1, Title = "New Movie", DurationMinutes = 90, IsPopular = false }; // Samme ID som eksisterende
        var result = await _controller.CreateMovie(newMovie);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Movie is null.", badRequestResult.Value);
    }

    [Fact]
    public async Task GetMovieById_InvalidId_ReturnsNotFound()
    {
        // Arrange
        // Intet behov for at tilføje en film her

        // Act
        var result = await _controller.GetMovieById(999); // Ugyldigt ID

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteMovieById_MovieDoesNotExist_ReturnsNotFound()
    {
        // Act
        var result = await _controller.DeleteMovieById(999); // Ugyldigt ID

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Movie with id 999 not found.", notFoundResult.Value);
    }

    // Existing tests...

    [Fact]
    public async Task GetAllMovies_ReturnsOkResult_WithMovies()
    {
        // Arrange
        var movies = new List<Movie>
        {
            new Movie { Id = 1, Title = "Movie 1", DurationMinutes = 120, IsPopular = true },
            new Movie { Id = 2, Title = "Movie 2", DurationMinutes = 90, IsPopular = false }
        };

        foreach (var movie in movies)
        {
            await _fakeCrudRepository.Create(movie);
        }

        // Act
        var result = await _controller.GetAllMovies();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnMovies = Assert.IsType<List<Movie>>(okResult.Value);
        Assert.Equal(2, returnMovies.Count);
    }

    [Fact]
    public async Task GetMovieById_MovieExists_ReturnsOkResult()
    {
        // Arrange
        var movie = new Movie { Id = 1, Title = "Movie 1", DurationMinutes = 120, IsPopular = true };
        await _fakeCrudRepository.Create(movie);

        // Act
        var result = await _controller.GetMovieById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnMovie = Assert.IsType<Movie>(okResult.Value);
        Assert.Equal(1, returnMovie.Id);
    }

    [Fact]
    public async Task CreateMovie_NullMovie_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.CreateMovie(null);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Movie is null.", badRequestResult.Value);
    }

    [Fact]
    public async Task DeleteMovieById_MovieExists_ReturnsNoContent()
    {
        // Arrange
        var movie = new Movie { Id = 1, Title = "Movie 1", DurationMinutes = 120, IsPopular = true };
        await _fakeCrudRepository.Create(movie);

        // Act
        var result = await _controller.DeleteMovieById(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}


