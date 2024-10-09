using Microsoft.AspNetCore.Mvc;
using xxx.Repository.Interfaces;
using xxx.Repository.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using Cinema.Repository.Interfaces;
using Cinema.Repository.Repositories;

namespace xxx.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly ICrudRepository<Movie> _context;
        private readonly IMovieRepository _context2;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MovieController(ICrudRepository<Movie> context, IWebHostEnvironment webHostEnvironment, IMovieRepository context2)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _context2 = context2;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetAllMovies()
        {
            try
            {
                var movies = await _context.GetAll();
                return Ok(movies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving movies: {ex.Message}");
            }
        }

        // GET: api/Movies/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetMovieById(int id)
        {
            try
            {
                var foundMovie = await _context.GetById(id);

                if (foundMovie == null)
                {
                    return NotFound();
                }

                return Ok(foundMovie);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving the movie with ID {id}: {ex.Message}");
            }
        }

        // POST: api/Movies
        [HttpPost]
        [HttpPost("Movie")]
        public async Task<IActionResult> CreateMovie([FromForm] Movie movie)
        {
            try
            {
                if (movie == null)
                {
                    return BadRequest("Movie is null.");
                }

                // Håndterer billedfilen, hvis den er angivet
                if (movie.ImageFile != null)
                {
                    var fileName = Path.GetFileNameWithoutExtension(movie.ImageFile.FileName);
                    var extension = Path.GetExtension(movie.ImageFile.FileName);
                    var uniqueFileName = $"{fileName}_{DateTime.Now.Ticks}{extension}";
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                    // Opret mappen, hvis den ikke findes
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Gem billedfilen på serveren
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await movie.ImageFile.CopyToAsync(fileStream);
                    }

                    // Gem filstien i Movie objektet (til senere brug)
                    movie.Image = $"/images/{uniqueFileName}";
                }

                // Gem filmen i databasen (forudsat at _context.Create gemmer i databasen)
                var newMovieCreated = await _context.Create(movie);
                if (newMovieCreated != null)
                {
                    // Returnerer en succesrespons med den oprettede film
                    return CreatedAtAction(nameof(GetMovieById), new { id = movie.Id }, movie);
                }

                return BadRequest("Failed to create movie.");
            }
            catch (Exception ex)
            {
                // Returnerer en serverfejl med fejlbesked
                return StatusCode(500, $"An error occurred while creating the movie: {ex.Message}");
            }
        }

        //public async Task<IActionResult> CreateMovie([FromForm] Movie movie)
        //{
        //    try
        //    {
        //        if (movie.ImageFile != null)
        //        {
        //            var fileName = Path.GetFileNameWithoutExtension(movie.ImageFile.FileName);
        //            var extension = Path.GetExtension(movie.ImageFile.FileName);
        //            var uniqueFileName = $"{fileName}_{DateTime.Now.Ticks}{extension}";
        //            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");

        //            if (!Directory.Exists(uploadsFolder))
        //                Directory.CreateDirectory(uploadsFolder);

        //            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        //            using (var fileStream = new FileStream(filePath, FileMode.Create))
        //            {
        //                await movie.ImageFile.CopyToAsync(fileStream);
        //            }

        //            movie.Image = $"/images/{uniqueFileName}";
        //        }

        //        if (movie == null)
        //        {
        //            return BadRequest("Movie is null.");
        //        }

        //        var newMovieCreated = await _context.Create(movie);
        //        if (newMovieCreated != null)
        //        {
        //            return CreatedAtAction(nameof(GetMovieById), new { id = movie.Id }, movie);
        //        }

        //        return BadRequest("Failed to create movie.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"An error occurred while creating the movie: {ex.Message}");
        //    }
        //}

        // DELETE: api/Movies/5
        
        
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteMovieById(int id)
        {
            try
            {
                var isDeleted = await _context.DeleteById(id);

                if (isDeleted)
                {
                    return NoContent();
                }

                return NotFound($"Movie with id {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the movie with ID {id}: {ex.Message}");
            }
        }



        // GET: api/Movies/with-actors
        [HttpGet("with-actors")]
        public async Task<IActionResult> GetAllMoviesWithActors()
        {
            try
            {
                // Henter alle film med deres tilknyttede skuespillere
                var movies = await _context2.GetAllMoviesWithActors();

                if (movies == null || movies.Count == 0)
                {
                    throw new Exception("No movies found.");
                }

                return Ok(movies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving movies with actors: {ex.Message}");
            }
        }

        // POST: api/Movies/{movieId}/actors/{actorId}
        [HttpPost("{movieId:int}/actors/{actorId:int}")]
        public async Task<IActionResult> AddActorToMovie(int movieId, int actorId)
        {
            try
            {
                // Tilføjer skuespiller til film
                await _context2.AddActorToMovie(movieId, actorId);
                return Ok($"Actor with ID {actorId} has been added to movie with ID {movieId}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding the actor to the movie: {ex.Message}");
            }
        }


        [HttpGet("genre/{genreId:int}")]
        public async Task<ActionResult<List<Movie>>> GetMoviesByGenreId(int genreId)
        {
            var movies = await _context2.GetMoviesByGenreId(genreId);

            if (movies == null || !movies.Any())
            {
                return NotFound($"No movies found for genre ID '{genreId}'.");
            }

            return Ok(movies);
        }

    }
}
