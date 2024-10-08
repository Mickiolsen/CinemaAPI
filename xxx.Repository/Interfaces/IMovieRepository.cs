using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xxx.Repository.Models;

namespace Cinema.Repository.Interfaces
{
    public interface IMovieRepository
    {
        Task<List<Movie>> GetAllMoviesWithActors();
        Task AddActorToMovie(int movieId, int actorId);

    }
}
