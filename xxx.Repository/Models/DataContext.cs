using Cinema.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xxx.Repository.Interfaces;

namespace xxx.Repository.Models
{
    
    public class DataContext : DbContext
    {
       // private readonly DbContextOptions _dbContextOptions;

        public DataContext(DbContextOptions<DataContext> dbContextOptions) : base(dbContextOptions)
        {
           // _dbContextOptions = dbContextOptions;
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Show> Shows { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Genre temp = new Genre() { Id = 1, Genretype = "Test"};

            modelBuilder.Entity<Genre>().HasData(temp);

            modelBuilder.Entity<Movie>()
                 .HasData(
                //new Movie()
                //{
                //    Id = 1,
                //    Title = "Fight With The Mongols",
                //    DurationMinutes = 51,
                //    //Genre = new() { Genretype = "test"},
                //    GenreId = 1,
                //});
                //new Movie()
                //{
                //    Id = 2,
                //    Title = "Fight With The Small Mongols - The Movie",
                //    DurationMinutes = 33,
                //    //Genre = "Comedy",
                //},
                //new Movie()
                //{
                //    Id = 3,
                //    Title = "Fight With The Big Mongols - The End",
                //    DurationMinutes = 33,
                //    //Genre = "Comedy",
                //}
                //);
                );


            // Mange-til-mange relationen mellem Movie og Actor
            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Actors)
                .WithMany(a => a.Movies);

            modelBuilder.Entity<Movie>()
                .HasKey(m => m.Id);

            modelBuilder.Entity<Actor>()
                .HasKey(a => a.Id);

            base.OnModelCreating(modelBuilder);
        }
    }

}
