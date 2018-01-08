using System;
using System.Collections.Generic;
using System.Text;

namespace MovieLibrary.Data.Models
{
    public class MovieDirector
    {
        public int MovieId { get; set; }

        public Movie Movie { get; set; }

        public int DirectorId { get; set; }

        public Person Director { get; set; }
    }
}
