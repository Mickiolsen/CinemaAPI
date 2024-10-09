using Cinema.Repository.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xxx.API.Controllers;
using xxx.Repository.Models;

namespace CinemaUnitTest
{
    public class GenericRepoMovieTest
    {
            private DbContextOptions<DataContext> options;
            private DataContext context;
            private MovieController _controller; // Tilføj controller reference

        public GenericRepoMovieTest()
            {
                // Opretter en in-memory database for test
                options = new DbContextOptionsBuilder<DataContext>()
                  .UseInMemoryDatabase(databaseName: "fakeDatabase")
                  .Options;

                context = new DataContext(options);
                context.Database.EnsureDeleted(); // Slet tidligere data for at sikre en ren start

                // Tilføj nogle film til databasen
                context.Set<Movie>().Add(new Movie
                {
                    Id = 1,
                    Title = "Inception",
                    DurationMinutes = 148,
                    TrailerLink = "http://trailerlink.com/inception",
                    IsPopular = true,
                    Description = "A thief who steals corporate secrets...",
                    Image = "inception.jpg",
                    GenreId = 1
                });

                context.Set<Movie>().Add(new Movie
                {
                    Id = 2,
                    Title = "Interstellar",
                    DurationMinutes = 169,
                    TrailerLink = "http://trailerlink.com/interstellar",
                    IsPopular = true,
                    Description = "A team of explorers travel through a wormhole...",
                    Image = "interstellar.jpg",
                    GenreId = 2
                });

                context.Set<Movie>().Add(new Movie
                {
                    Id = 3,
                    Title = "The Dark Knight",
                    DurationMinutes = 152,
                    TrailerLink = "http://trailerlink.com/darkknight",
                    IsPopular = true,
                    Description = "When the menace known as the Joker emerges...",
                    Image = "darkknight.jpg",
                    GenreId = 3
                });

                context.SaveChanges();

        }

            [Fact]
            public async Task GetMovieById_MovieExists()
            {
                // Arrange
                var repository = new GenericRepository<Movie>(context);

                // Act
                var result = await repository.GetById(1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(1, result.Id);
                Assert.Equal("Inception", result.Title);
                Assert.Equal(148, result.DurationMinutes);
                Assert.Equal("http://trailerlink.com/inception", result.TrailerLink);
                Assert.True(result.IsPopular);
                Assert.Equal("A thief who steals corporate secrets...", result.Description);
                Assert.Equal("inception.jpg", result.Image);
                Assert.Equal(1, result.GenreId);
            }

            [Fact]
            public async Task GetMovieById_MovieDoesNotExist()
            {
                // Arrange
                var repository = new GenericRepository<Movie>(context);

                // Act
                var result = await repository.GetById(4); // Id 4 findes ikke

                // Assert
                Assert.Null(result); // Forventer null, da filmen ikke findes
            }

            [Fact]
            public async Task GetAllMovies_ReturnsAllMovies()
            {
                // Arrange
                var repository = new GenericRepository<Movie>(context);

                // Act
                var result = await repository.GetAll();

                // Assert
                Assert.Equal(3, result.Count()); // Der er 3 film i databasen
            }

            [Fact]
            public async Task AddMovie_AddsMovieSuccessfully()
            {
                // Arrange
                var repository = new GenericRepository<Movie>(context);
                var newMovie = new Movie
                {
                    Id = 4,
                    Title = "Tenet",
                    DurationMinutes = 150,
                    TrailerLink = "http://trailerlink.com/tenet",
                    IsPopular = true,
                    Description = "Armed with only one word, Tenet...",
                    Image = "tenet.jpg",
                    GenreId = 4
                };

                // Act
                var result = await repository.Create(newMovie);
                var movieFromDb = await repository.GetById(4);

                // Assert
                Assert.NotNull(movieFromDb);
                Assert.Equal("Tenet", movieFromDb.Title);
                Assert.Equal(150, movieFromDb.DurationMinutes);
                Assert.Equal("http://trailerlink.com/tenet", movieFromDb.TrailerLink);
                Assert.Equal("Armed with only one word, Tenet...", movieFromDb.Description);
                Assert.Equal("tenet.jpg", movieFromDb.Image);
                Assert.Equal(4, movieFromDb.GenreId);
            }

            [Fact]
            public async Task DeleteMovie_RemovesMovieSuccessfully()
            {
                // Arrange
                var repository = new GenericRepository<Movie>(context);

                // Act
                var success = await repository.DeleteById(1); // Fjern filmen med Id 1
                var movieFromDb = await repository.GetById(1);

                // Assert
                Assert.True(success);
                Assert.Null(movieFromDb); // Forventer at filmen er blevet fjernet
            }

            [Fact]
            public async Task AddMovie_MissingRequiredFields_ShouldFail()
            {
                // Arrange
                var repository = new GenericRepository<Movie>(context);
                var newMovie = new Movie
                {
                    // Missing required fields like Title, Description, IsPopular
                    DurationMinutes = 120
                };

                // Act & Assert
                await Assert.ThrowsAsync<DbUpdateException>(() => repository.Create(newMovie));
            }

            [Fact]
            public async Task GetMoviesByGenreId_NonExistingGenreId_ReturnsNotFound()
            {
                // Act
                var result = await _controller.GetMoviesByGenreId(999); // Antager at 999 ikke eksisterer

                // Assert
                var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
                Assert.Equal("No movies found for genre ID '999'.", notFoundResult.Value);
            }
    }
}
