using System;
using System.Collections.Generic;
using MovieLibrary.Data.Models;

namespace MovieLibrary.Web.Models.MovieViewModels
{
    public class PreviewMovieViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int Duration { get; set; }

        public string Plot { get; set; }

        public string TrailerUrl { get; set; }

        public string ImageUrl { get; set; }

        public int? ParrentControl { get; set; }

        public bool IsApproved { get; set; }

        public string UploaderName { get; set; }

        public DateTime ReleaseDate { get; set; }

        public IList<string> CategoryNames { get; set; }

        public IList<string> ActiorNames { get; set; }

        public IList<string> WriterNames { get; set; }

        public IList<string> DirectorNames { get; set; }
    }
}
