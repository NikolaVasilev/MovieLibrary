using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieLibrary.Services.Interfaces;
using MovieLibrary.Web.Models;

namespace MovieLibrary.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMovieService _movies;

        public HomeController(IMovieService movies)
        {
            _movies = movies;
        }

        public async Task<IActionResult> Index()
        {
            return View(await this._movies.GetLatestFourMovies());
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
