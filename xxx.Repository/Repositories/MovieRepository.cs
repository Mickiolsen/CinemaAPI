using Cinema.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xxx.Repository.Models;

namespace Cinema.Repository.Repositories
{
    public class MovieRepository:IMovieRepository
    {
        private readonly DataContext _context;

        public MovieRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Movie>> GetAllMoviesWithActors()
        {
            return await _context.Movies.Include(m => m.Actors).ToListAsync();
        }

        public async Task AddActorToMovie(int movieId, int actorId)
        {
            var movie = await _context.Movies.FindAsync(movieId);
            var actor = await _context.Actors.FindAsync(actorId);

            if (movie != null && actor != null)
            {
                movie.Actors.Add(actor); 
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Movie>> GetMoviesByGenreId(int genreId)
        {
            return await _context.Movies
                .Where(m => m.GenreId == genreId)
                .ToListAsync();
        }

    }
}
