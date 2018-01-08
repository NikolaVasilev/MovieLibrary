using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.ExceptionServices;
using Microsoft.AspNetCore.Identity;

namespace MovieLibrary.Data.Models
{
    // Add profile data for application users by adding properties to the User class
    public class User : IdentityUser
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }

        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }

        public string ImgPath { get; set; }

        public IList<Comment> Comments { get; set; } = new List<Comment>();

        public IList<Movie> UploadedMovies { get; set; } = new List<Movie>();

        public IList<Vote> Votes { get; set; } = new List<Vote>();

        
    }
}
