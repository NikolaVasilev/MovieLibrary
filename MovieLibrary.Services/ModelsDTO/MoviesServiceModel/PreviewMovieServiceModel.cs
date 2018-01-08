using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using MovieLibrary.Common.Mapping;
using MovieLibrary.Data.Models;

namespace MovieLibrary.Services.ModelsDTO.MoviesServiceModel
{
    public class PreviewMovieServiceModel : IMapFrom<Movie>, IHaveCustomMapping
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int Year { get; set; }

        public int Duration { get; set; }

        public string Plot { get; set; }

        public string TrailerUrl { get; set; }

        public string ImageUrl { get; set; }

        public int? ParrentControl { get; set; }

        public DateTime ReleaseDate { get; set; }

        public bool IsApproved { get; set; } = false;

        public string UploaderUserName { get; set; }

        public IList<MovieCategory> Categories { get; set; }

        public IList<MovieActior> Actiors { get; set; }

        public IList<MovieWriter> Writers { get; set; }

        public IList<MovieDirector> Director { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<Movie, PreviewMovieServiceModel>()
                .ForMember(c => c.UploaderUserName, cfg => cfg.MapFrom(c => c.Uploader.UserName));
        }
    }
}
