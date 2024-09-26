using Cinema.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using xxx.Repository.Interfaces;
using Cinema.Repository.Models;
using xxx.Repository.Models;
//using Cin.Repository.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder => builder
            .WithOrigins("http://localhost:4200") // Angular app's URL
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

////////////////////////////////////////////////////////////////////
//builder.Services.AddScoped<IGeneric<Horse>, GenericRepo<Horse>>();

///////////////////////////////////////////////////////////////////

//builder.Services.AddScoped<ICrudRepository<Movie>, GenericRepository<Movie>>();
builder.Services.AddScoped<ICrudRepository<Movie>, GenericRepository<Movie>>();
//builder.Services.AddScoped(typeof(ICrudRepository<>), typeof(GenericRepository<>));
//builder.Services.AddScoped < ICrudRepository, GenericRepository<Movie>();

//builder.Services.AddScoped<ICrudRepository<Battle>, GenericRepository<Battle>>();
//builder.Services.AddScoped<ICrudRepository<Horse>, GenericRepository<Horse>>();


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

app.UseAuthorization();

app.MapControllers();

app.Run();
