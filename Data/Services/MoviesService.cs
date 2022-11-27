﻿using Microsoft.EntityFrameworkCore;
using NollyTickets.Ng.Data.Base;
using NollyTickets.Ng.Data.ViewModels;
using NollyTickets.Ng.Models;

namespace NollyTickets.Ng.Data.Services
{
    public class MoviesService : EntityBaseRepository<Movie>, IMoviesServices
    {
        private readonly ApplicationDbContext _context;
        public MoviesService(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddNewMovieAsync(NewMovieVM data)
        {
            var newMovie = new Movie()
            {
                Name = data.Name,
                Description = data.Description,
                Price = data.Price,
                ImageURL = data.ImageURL,
                CinemaId = data.CinemaId,
                StartDate = data.StartDate,
                EndDate = data.EndDate,
                MovieCategory = data.MovieCategory,
                ProducerId = data.ProducerId,
            };
            await _context.Movies.AddAsync(newMovie);
            await _context.SaveChangesAsync();
            //Add Movie Actors
            foreach (var actorid in data.ActorsIds)
            {
                var newActorMovie = new Actor_Movie()
                {
                    MovieId = newMovie.Id,
                    ActorId = actorid,
                };
                await _context.Actor_Movies.AddAsync(newActorMovie);
            }
           await _context.SaveChangesAsync();   
        }

        public async Task<Movie> GetMovieByIdAsync(int id)
        {
            var movieDetails = await _context.Movies
                 .Include(c => c.Cinema)
                 .Include(p => p.Producer)
                 .Include(am => am.Actor_Movies).ThenInclude(a => a.Actor)
                 .FirstOrDefaultAsync(n => n.Id == id);
            return movieDetails;
        }

        public async Task<NewMovieDropDownVM> GetNewMovieDropDownValues()
        {
            //var response = new NewMovieDropDownVM();
            // response.Actors = await _context.Actors.OrderBy(n => n.FullName).ToListAsync();
            // response.Cinemas = await _context.Cinemas.OrderBy(n => n.Name).ToListAsync();
            // response.Producers = await _context.Producers.OrderBy(n => n.FullName).ToListAsync();

            // return response;                                                                                                                                                                                    

            var response = new NewMovieDropDownVM()
            {
                Actors = await _context.Actors.OrderBy(n => n.FullName).ToListAsync(),
                Cinemas = await _context.Cinemas.OrderBy(n => n.Name).ToListAsync(),
                Producers = await _context.Producers.OrderBy(n => n.FullName).ToListAsync()
            };



            return response;
        }

        public async Task UpdateMovieAsync(NewMovieVM data)
        {
            var dbMovie = await _context.Movies.FirstOrDefaultAsync(n => n.Id == data.Id);
            if(dbMovie != null)
            {

                dbMovie.Name = data.Name;
                dbMovie.Description = data.Description;
                dbMovie.Price = data.Price;
                dbMovie.ImageURL = data.ImageURL;
                dbMovie.CinemaId = data.CinemaId;
                dbMovie.StartDate = data.StartDate;
                dbMovie.EndDate = data.EndDate;
                dbMovie.MovieCategory = data.MovieCategory;
                dbMovie.ProducerId = data.ProducerId;
                await _context.SaveChangesAsync();
            }
            //Remove existing actors
            var existingActorDb = _context.Actor_Movies.Where(n => n.MovieId == data.Id).ToList();
             _context.Actor_Movies.RemoveRange(existingActorDb);
            await _context.SaveChangesAsync();
            //Add Movie Actors
            foreach (var actorid in data.ActorsIds)
            {
                var newActorMovie = new Actor_Movie()
                {
                    MovieId = data.Id,
                    ActorId = actorid,
                };
                await _context.Actor_Movies.AddAsync(newActorMovie);
            }
            await _context.SaveChangesAsync();

        }
    }
}
