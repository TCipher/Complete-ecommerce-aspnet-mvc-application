﻿using Microsoft.EntityFrameworkCore;
using NollyTickets.Ng.Models;

namespace NollyTickets.Ng.Data
{
    //Inheriting from the Db context class
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options)    
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Defining the many to many relationships
            modelBuilder.Entity<Actor_Movie>().HasKey(am => new 
            {
                am.ActorId,
                am.MovieId
            });
            //Defining the joinning tables
            modelBuilder.Entity<Actor_Movie>().HasOne(m => m.Movie).WithMany(am => am.Actor_Movies).HasForeignKey(m
                => m.MovieId);
            modelBuilder.Entity<Actor_Movie>().HasOne(m => m.Actor).WithMany(am => am.Actor_Movies).HasForeignKey(m
              => m.ActorId);


            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Actor_Movie> Actor_Movies { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Producer> Producers { get; set; }
        

        //Order related tables
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }   
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }  
    }
}
