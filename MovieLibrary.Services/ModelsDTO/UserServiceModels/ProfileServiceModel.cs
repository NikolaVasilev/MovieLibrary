using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using AutoMapper;
using MovieLibrary.Common.Mapping;
using MovieLibrary.Data.Models;

namespace MovieLibrary.Services.ModelsDTO.UserServiceModels
{
    public class ProfileServiceModel : IMapFrom<User>, IHaveCustomMapping
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string Email { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public string ImgPath { get; set; }

        public IList<Movie> UploadedMovies { get; set; } = new List<Movie>();

        public void ConfigureMapping(Profile mapper)
            => mapper
                .CreateMap<User, ProfileServiceModel>()
                .ForMember(c => c.Username, cfg => cfg.MapFrom(c => c.UserName))
                .ForMember(c => c.UploadedMovies, cfg => cfg.MapFrom(c => c.UploadedMovies));
    }
}
