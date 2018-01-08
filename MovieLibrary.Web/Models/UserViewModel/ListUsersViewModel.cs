using MovieLibrary.Services.ModelsDTO.UserServiceModels;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MovieLibrary.Web.Models.UserViewModel
{
    public class ListUsersViewModel
    {
        public IList<ListUsersServiceModel> Users { get; set; }

        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }

        public int PreviousPage => this.CurrentPage == 1 ? 1 : this.CurrentPage - 1;

        public int NextPage => this.CurrentPage == TotalPages ? TotalPages : CurrentPage + 1;

        public string UserType { get; set; }

        public string SearchType { get; set; }

        public string SearchTerm { get; set; }

        public IList<SelectListItem> AllRoleListingTypes { get; set; }

        public IList<SelectListItem> AllSearchListingTypes { get; set; }
        
    }
}
