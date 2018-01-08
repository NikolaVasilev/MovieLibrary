using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MovieLibrary.Data.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Title { get; set; }

        public int? Year { get; set; }

        [Required]
        [Range(1, Int32.MaxValue)]
        public int Duration { get; set; }

        [Required]
        [StringLength(10000, MinimumLength = 1)]
        public string Plot { get; set; }

        [Required]
        [StringLength(10000, MinimumLength = 1)]
        public string TrailerUrl { get; set; }

        [Required]
        [StringLength(10000, MinimumLength = 1)]
        public string ImageUrl { get; set; }

        [Range(1, 18)]
        public int? ParrentControl { get; set; }

        public DateTime ReleaseDate { get; set; }

        public DateTime UploadDate { get; set; }

        public string UploaderId { get; set; }

        public User Uploader { get; set; }

        public bool IsApproved { get; set; } = false;

        public IList<MovieCategory> Categories { get; set; } = new List<MovieCategory>();

        public IList<Comment> Comments { get; set; } = new List<Comment>();

        public IList<Vote> Votes { get; set; } = new List<Vote>();

        public IList<MovieActior> Actiors { get; set; } = new List<MovieActior>(); //do tuk napraveno

        public IList<MovieWriter> Writers { get; set; } = new List<MovieWriter>();

        public IList<MovieDirector> Director { get; set; } = new List<MovieDirector>();

    }
}
