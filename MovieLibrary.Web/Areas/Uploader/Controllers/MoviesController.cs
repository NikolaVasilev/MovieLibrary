using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieLibrary.Data.Models;
using MovieLibrary.Services.Interfaces;
using MovieLibrary.Web.Areas.Uploader.Models;
using MovieLibrary.Web.Infrastructure.Extesions;
using MovieLibrary.Web.WebConstants;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieLibrary.Web.Models.MovieViewModels;

namespace MovieLibrary.Web.Areas.Uploader.Controllers
{
    public class MoviesController : BaseController
    {
        private readonly ICategoryService _categories;
        private readonly IPersonService _persons;
        private readonly IMovieService _movies;
        private readonly UserManager<User> _userManager;

        public MoviesController(ICategoryService categories, IPersonService persons, IMovieService movies, UserManager<User> userManager)
        {
            _categories = categories;
            _persons = persons;
            _movies = movies;
            _userManager = userManager;
        }

        public async Task<IActionResult> AddMovie()
        {

            ViewBag.Name = "AddMovie";

            var categories = await this._categories.GetAllCategories();
            return View( new AddMovieViewModel()
            {
                Categories = this.GetCategories(categories)
            });
        }

        private IList<SelectListItem> GetCategories(IList<Category> categories)
        {
            return categories.Select(c => new SelectListItem()
            {
                Value = c.Name,
                Text = c.Name
            }).ToList();
        }


        [HttpPost]
        public async Task<IActionResult> AddMovie(AddMovieViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var categories = await this._categories.GetAllCategories();
                model.Categories = this.GetCategories(categories);
                return View(model);
            }

            if (await this._movies.IsMovieExist(model.Title, model.Year, model.ReleaseDate))
            {
                TempData.AddErrorMessage($"Movie {model.Title} is already uploaded!");
                var categories = await this._categories.GetAllCategories();
                model.Categories = GetCategories(categories);
                return View(model);
            }

            foreach (var categoryName in model.SelectedCategories)
            {
                if (!await this._categories.CategoryExistsAsync(categoryName))
                {
                    return BadRequest();
                }
            }

            var isUserInRoleModerator = User.IsInRole(RoleConstants.ModeratorRole);
            var userId = _userManager.GetUserId(User);

            var movieId = await this._movies.AddMovieAsunc(
                model.Title,
                model.Year,
                model.Duration,
                model.Plot,
                model.TrailerUrl,
                model.ImageUrl,
                model.ParrentControl,
                model.ReleaseDate,
                model.SelectedCategories,
                model.Actiors,
                model.Writers,
                model.Director,
                isUserInRoleModerator,
                userId
                );

            TempData.AddSuccessMessage($"Movie {model.Title} is successfuly uploaded!");
            return RedirectToAction("MoviePreview", "Movies", new {area = "", movieId = movieId});
        }

        public async Task<IActionResult> Edit(int movieId)
        {
            if (!await this._movies.IsMovieExistById(movieId))
            {
                return NotFound();
            }

            var movie = await this._movies.GetMovieById(movieId);

            var selectedCategories = new List<string>();
            var actors = new List<string>();
            var writers = new List<string>();
            var directors = new List<string>();

            foreach (var c in movie.Categories.Select(x => x.CategoryId))
            {
              selectedCategories.Add(await this._categories.GetCategoryNameById(c));   
            }

            foreach (var c in movie.Actiors.Select(x => x.ActiorId))
            {
                actors.Add(await this._persons.GetPersonNameById(c)); 
            }

            foreach (var c in movie.Director.Select(x => x.DirectorId))
            {
                directors.Add(await this._persons.GetPersonNameById(c));
            }

            foreach (var c in movie.Writers.Select(x => x.WriterId))
            {
                writers.Add(await this._persons.GetPersonNameById(c));
            }


            var categories = await this._categories.GetAllCategories();
            return View(new EditMovieViewModel()
            {
                Id = movieId,
                Title = movie.Title,
                Year = movie.Year,
                Duration = movie.Duration,
                Plot = movie.Plot,
                TrailerUrl = movie.TrailerUrl,
                ImageUrl = movie.ImageUrl,
                ParrentControl = movie.ParrentControl,
                ReleaseDate = movie.ReleaseDate,
                Categories = this.GetCategories(categories),
                SelectedCategories = selectedCategories,
                Actiors = string.Join(", ", actors),
                Director = string.Join(", ", directors),
                Writers = string.Join(", ", writers),
                IsApproved = movie.IsApproved,
                UploaderUsername = movie.UploaderUserName
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditMovieViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var categories = await this._categories.GetAllCategories();
                model.Categories = this.GetCategories(categories);
                return View(model);
            }
            if (!await this._movies.IsMovieExistById(model.Id))
            {
                return NotFound();
            }

            if (!User.IsInRole(RoleConstants.AdministratorRole) && !User.IsInRole(RoleConstants.ModeratorRole) && User.Identity.Name != model.UploaderUsername)
            {
                return BadRequest();
            }

            await this._movies.Edit(
                model.Id,
                model.Year,
                model.Title,
                model.Duration,
                model.Plot,
                model.TrailerUrl,
                model.ImageUrl,
                model.ParrentControl,
                model.ReleaseDate,
                model.SelectedCategories,
                model.IsApproved,
                model.Actiors,
                model.Writers,
                model.Director);
        
            return RedirectToAction("MoviePreview", "Movies", new {area = "", movieId = model.Id});
        }


        public async Task<IActionResult> DeleteMovie(int movieId)
        {
            if (!await this._movies.IsMovieExistById(movieId))
            {
                return NotFound();
            }

            var movie = await this._movies.GetMovieById(movieId);

            if (User.Identity.Name != movie.UploaderUserName && !User.IsInRole(RoleConstants.ModeratorRole) && !User.IsInRole(RoleConstants.AdministratorRole))
            {
                return BadRequest();
            }

            var categoryNames = new List<string>();
            var actorNames = new List<string>();
            var directorNames = new List<string>();
            var writerNames = new List<string>();

            foreach (var movieCategory in movie.Categories)
            {
                categoryNames.Add(await this._categories.GetCategoryNameById(movieCategory.CategoryId));
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

        public async Task<IActionResult> ConfirmDelete(int movieId)
        {
            if (!await this._movies.IsMovieExistById(movieId))
            {
                return NotFound();
            }

            var movie = await this._movies.GetMovieById(movieId);

            if (User.Identity.Name != movie.UploaderUserName && !User.IsInRole(RoleConstants.ModeratorRole) && !User.IsInRole(RoleConstants.AdministratorRole))
            {
                return BadRequest();
            }

            await this._movies.Delete(movieId);

            TempData.AddSuccessMessage("Movie was seccessfully deleted!");

            return RedirectToAction("ListMovies", "Movies", new {area = ""});
        }
    }
}
