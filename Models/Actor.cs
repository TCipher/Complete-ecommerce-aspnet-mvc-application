﻿using NollyTickets.Ng.Data.Base;
using System.ComponentModel.DataAnnotations;

namespace NollyTickets.Ng.Models
{
    public class Actor:IEntityBase
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Profile Picture")]
        [Required(ErrorMessage ="Profile Picture is Required")]
        public string ProfilePictureURL { get; set; }

        [Display(Name = "FullName")]
        [Required(ErrorMessage = "FullName is Required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage ="FullNAme must be between 3 and 50 chars")]
        public string FullName { get; set; }

        [Display(Name = "Biography")]
        [Required(ErrorMessage = "Biography  is Required")]
        public string Bio { get; set; }
        /// <summary>
        ///defining the MANY -> MANY relationships between the actors model and movie model
        //this is done using the defined join table Actor_movie
        //Actors has MANY -> MANY relationship with the movie because.an actor can act many movies, and a movie can have multiple actors
        /// </summary>
        public ICollection<Actor_Movie> Actor_Movies { get; set; }

       
    }
}
