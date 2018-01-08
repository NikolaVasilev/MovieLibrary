using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MovieLibrary.Data.Models
{
    public class Person
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }

        public string Biography { get; set; }

        public string ImageUrl { get; set; }

        public IList<MovieActior> MoviesActior { get; set; } = new List<MovieActior>();

        public IList<MovieWriter> MoviesWriter { get; set; } = new List<MovieWriter>();

        public IList<MovieDirector> MoviesDirector { get; set; } = new List<MovieDirector>();

    }
}
