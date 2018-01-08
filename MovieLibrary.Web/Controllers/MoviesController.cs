using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MovieLibrary.Data.Models;
using MovieLibrary.Services.Interfaces;
using MovieLibrary.Web.Models.MovieViewModels;
using System.Linq;
using System.Threading.Tasks;
using MovieLibrary.Web.WebConstants;

namespace MovieLibrary.Web.Controllers
{
    public class MoviesController : Controller
    {
       
        private readonly IMovieService _movies;
        private readonly ICategoryService _cetegories;
        private readonly IPersonService _persons;
        private const int PageSize = 10;

        public MoviesController(IMovieService movies, ICategoryService cetegories, IPersonService persons)
        {
            _movies = movies;
            _cetegories = cetegories;
            _persons = persons;
        }

        public async Task<IActionResult> MoviePreview(int movieId)
        {
            if (!await this._movies.IsMovieExistById(movieId))
            {
                return BadRequest();
            }

            var movie = await this._movies.GetMovieById(movieId);
            var categoryNames = new List<string>();
            var actorNames = new List<string>();
            var directorNames = new List<string>();
            var writerNames = new List<string>();

            foreach (var movieCategory in movie.Categories)
            {
                categoryNames.Add(await this._cetegories.GetCategoryNameById(movieCategory.CategoryId));
            }
            foreach (var movieActior in movie.Actiors)
            {
                actorNames.Add(await this._persons.GetPersonNameById(movieActior.ActiorId));
            }
            foreach (var movieWriter in movie.Writers)
            {
                writerNames.Add(await this._persons.GetPersonNameById(movieWriter.WriterId));
            }
            foreach (var movieDirector in movie.Director)
            {
                directorNames.Add(await this._persons.GetPersonNameById(movieDirector.DirectorId));
            }



            return View(new PreviewMovieViewModel()
            {
                Id = movieId,
                Title = movie.Title,
                Plot = movie.Plot,
                Duration = movie.Duration,
                TrailerUrl = movie.TrailerUrl,
                ImageUrl = movie.ImageUrl,
                ReleaseDate = movie.ReleaseDate,
                ParrentControl = movie.ParrentControl,
                CategoryNames = categoryNames,
                ActiorNames = actorNames,
                WriterNames = writerNames,
                DirectorNames = directorNames,
                IsApproved = movie.IsApproved,
                UploaderName = movie.UploaderUserName
            });
        }

        public async Task<IActionResult> ListMovies(int page = 1)
        {

            ViewBag.Name = "List Movies";
            bool isAdminModeratorOrUploader = User.IsInRole(RoleConstants.AdministratorRole)
                                              || User.IsInRole(RoleConstants.UploaderRole)
                                              || User.IsInRole(RoleConstants.ModeratorRole);

            return View(new ListMoviesViewModel()
            {
                Movies = await this._movies.GetAllMovies(isAdminModeratorOrUploader, page, PageSize),
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(await this._movies.Count(isAdminModeratorOrUploader, null) / (double)PageSize),
                PagingPath = "/movies/listmovies"
            });
        }

        public async Task<IActionResult> SearchMovies(IList<int> secectedGenres, string searchTerm, int page = 1)
        {

            ViewBag.Name = "Search Movies";
            bool isAdminModeratorOrUploader = User.IsInRole(RoleConstants.AdministratorRole)
                                              || User.IsInRole(RoleConstants.UploaderRole)
                                              || User.IsInRole(RoleConstants.ModeratorRole);

            if(secectedGenres.Count != 0)
            {
                foreach (var categoryId in secectedGenres)
                {
                    if(!await this._cetegories.IsCategoryExistByIdAsync(categoryId))
                    {
                        return BadRequest();
                    }
                }
            }

            return View(new ListMoviesViewModel()
            {
                Movies = await this._movies.GetMoviesByGenreAndSerachTerm(isAdminModeratorOrUploader, page, PageSize, searchTerm, secectedGenres),
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(await this._movies.Count(isAdminModeratorOrUploader, secectedGenres, searchTerm) / (double)PageSize),
                Genres = await this._cetegories.GetAllCategories(),
                SearchTerm = searchTerm,
                SelectedGenresIds = secectedGenres,
                PagingPath = "/movies/searchmovies"
            });
        }
    }
}
