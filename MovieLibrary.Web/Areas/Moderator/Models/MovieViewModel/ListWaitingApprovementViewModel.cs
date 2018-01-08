using MovieLibrary.Services.ModelsDTO.MoviesServiceModel;
using System.Collections.Generic;

namespace MovieLibrary.Web.Areas.Moderator.Models.MovieViewModel
{
    public class ListWaitingApprovementViewModel
    {
        public IList<ListWaitingApprovementServiceModel> Movies { get; set; }

        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }

        public int PreviousPage => this.CurrentPage == 1 ? 1 : this.CurrentPage - 1;

        public int NextPage => this.CurrentPage == TotalPages ? TotalPages : CurrentPage + 1;

        public string PagingPath { get; set; }
    }
}
