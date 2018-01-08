using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using MovieLibrary.Common.Mapping;
using MovieLibrary.Data.Models;

namespace MovieLibrary.Services.ModelsDTO.MoviesServiceModel
{
    public class ListWaitingApprovementServiceModel : IMapFrom<Movie>, IHaveCustomMapping
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string Uploader { get; set; }

        public DateTime UploadDate { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<Movie, ListWaitingApprovementServiceModel>()
                .ForMember(c => c.Uploader, cfg => cfg.MapFrom(c => c.Uploader.UserName));
        }
    }
}
