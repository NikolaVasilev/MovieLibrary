using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using MovieLibrary.Common.Mapping;
using MovieLibrary.Data.Models;

namespace MovieLibrary.Services.ModelsDTO.MoviesServiceModel
{
    public class ListMoviesServiceModel : IMapFrom<Movie>, IHaveCustomMapping
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Plot { get; set; }

        public string ImageUrl { get; set; }

        public int? Year { get; set; }

        public string UploaderName { get; set; }

        public bool IsApproved { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<Movie, ListMoviesServiceModel>()
                .ForMember(c => c.UploaderName, cfg => cfg.MapFrom(c => c.Uploader.UserName));
        }
    }
}
