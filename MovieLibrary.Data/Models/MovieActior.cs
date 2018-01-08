using System;
using System.Collections.Generic;
using System.Text;

namespace MovieLibrary.Data.Models
{
    public class MovieActior
    {
        public int MovieId { get; set; }

        public Movie Movie { get; set; }

        public int ActiorId { get; set; }

        public Person Actior { get; set; }
    }
}
