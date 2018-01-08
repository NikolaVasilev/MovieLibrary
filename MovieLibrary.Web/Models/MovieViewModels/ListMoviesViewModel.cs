using MovieLibrary.Services.ModelsDTO.MoviesServiceModel;
using System.Collections.Generic;
using MovieLibrary.Data.Models;

namespace MovieLibrary.Web.Models.MovieViewModels
{
    public class ListMoviesViewModel
    {
        public IList<ListMoviesServiceModel> Movies { get; set; }

        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }

        public int PreviousPage => this.CurrentPage == 1 ? 1 : this.CurrentPage - 1;

        public int NextPage => this.CurrentPage == TotalPages ? TotalPages : CurrentPage + 1;

        public string SearchTerm { get; set; }

        public IList<Category> Genres { get; set; }

        public IList<int> SelectedGenresIds { get; set; }

        public string PagingPath { get; set; }
    }
}
