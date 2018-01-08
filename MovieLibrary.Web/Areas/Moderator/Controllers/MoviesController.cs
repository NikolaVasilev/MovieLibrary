using Microsoft.AspNetCore.Mvc;
using MovieLibrary.Services.Interfaces;
using MovieLibrary.Web.Areas.Moderator.Models.MovieViewModel;
using MovieLibrary.Web.Infrastructure.Extesions;
using System;
using System.Threading.Tasks;

namespace MovieLibrary.Web.Areas.Moderator.Controllers
{
    public class MoviesController : BaseController
    {
        private readonly IMovieService _movies;
        private const int PageSize = 10;

        public MoviesController(IMovieService movies)
        {
            _movies = movies;
        }

        public async Task<IActionResult> Approve(int movieId)
        {

            if (!await _movies.IsMovieExistById(movieId))
            {
                return NotFound();
            }

           string movieName = await this._movies.Approve(movieId);

           TempData.AddSuccessMessage($"Movie {movieName} was approved successfully!");

            

           return RedirectToAction("MoviePreview", "Movies", new {area = "", movieId});
        }

        public async Task<IActionResult> ListWaitingForApprovement(int page = 1)
        {
            ViewBag.Name = "Movies For Approvement";



            return View(new ListWaitingApprovementViewModel()
            {
                Movies = await this._movies.GetAllWaitingApprovementMovies(page, PageSize),
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(await this._movies.CountWaitingApprovement() / (double)PageSize),
                PagingPath = "/moderator/movies/listwaitingforapprovement"
            });
        }


    }
}
