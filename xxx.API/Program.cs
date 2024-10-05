using Cinema.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using xxx.Repository.Interfaces;
using Cinema.Repository.Models;
using xxx.Repository.Models;
//using Cin.Repository.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Allow CORS for specific origins
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

// Dependency Injection for repositories
builder.Services.AddScoped<ICrudRepository<Movie>, GenericRepository<Movie>>();
builder.Services.AddScoped<ICrudRepository<Room>, GenericRepository<Room>>();
builder.Services.AddScoped<ICrudRepository<Seat>, GenericRepository<Seat>>();
builder.Services.AddScoped<ICrudRepository<Show>, GenericRepository<Show>>();
builder.Services.AddScoped<ICrudRepository<Genre>, GenericRepository<Genre>>();
builder.Services.AddScoped<ICrudRepository<Actor>, GenericRepository<Actor>>();
builder.Services.AddScoped<ICrudRepository<Country>, GenericRepository<Country>>();
builder.Services.AddScoped<ICrudRepository<Ticket>, GenericRepository<Ticket>>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("cinemaconstring")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable CORS before authorization middleware
app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();
