using System;
using System.Collections.Generic;
using System.Text;
using MovieLibrary.Common.Mapping;
using MovieLibrary.Data.Models;

namespace MovieLibrary.Services.ModelsDTO.UserServiceModels
{
    public class ListUsersServiceModel : IMapFrom<User>
    {
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string Email { get; set; }

        public string LastName { get; set; }

        public string ImgPath { get; set; }
    }
}
