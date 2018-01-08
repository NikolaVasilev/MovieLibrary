using System;
using System.Collections.Generic;
using System.Text;

namespace MovieLibrary.Data.Models
{
    public class MovieWriter
    {
        public int MovieId { get; set; }

        public Movie Movie { get; set; }

        public int WriterId { get; set; }

        public Person Writer { get; set; }
    }
}
