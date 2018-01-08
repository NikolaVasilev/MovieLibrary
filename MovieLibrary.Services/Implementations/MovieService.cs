using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MovieLibrary.Data;
using MovieLibrary.Data.Models;
using MovieLibrary.Services.Interfaces;
using MovieLibrary.Services.ModelsDTO.MoviesServiceModel;

namespace MovieLibrary.Services.Implementations
{
    class MovieService : IMovieService
    {
        private readonly MovieLibraryDbContext _db;

        public MovieService(MovieLibraryDbContext db)
        {
            _db = db;
        }

        public async Task<bool> IsMovieExist(string modelTitle, int? modelYear, DateTime? modelReleaseDate)
        {
            return await _db.Movies.AnyAsync(x => x.Title == modelTitle && x.Year == modelYear && x.ReleaseDate == modelReleaseDate);
        }

        public async Task<int> AddMovieAsunc(string modelTitle, int? modelYear, int? modelDuration, string modelPlot, string modelTrailerUrl,
            string modelImageUrl, int? modelParrentControl, DateTime? modelReleaseDate, IList<string> modelSelectedCategories,
            string modelActiors, string modelWriters, string modelDirector, bool isUserInRoleModerator, string userId)
        {

            var actiorNames = modelActiors.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).Distinct().ToList();
            var writerNames = modelWriters.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).Distinct().ToList();
            var directorNames = modelDirector.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).Distinct().ToList();

            var persons = new List<string>(actiorNames);
            persons.AddRange(writerNames);
            persons.AddRange(directorNames);

            foreach (var personName in persons)
            {
                if (!await this._db.Persons.AnyAsync(x => x.Name == personName))
                {
                    await this._db.AddAsync(new Person()
                    {
                        Name = personName
                    });
                }
            }

            await _db.SaveChangesAsync();

            var movie = new Movie()
            {
                Title = modelTitle,
                Year = modelYear,
                Duration = modelDuration ?? 0,
                Plot = modelPlot,
                TrailerUrl = modelTrailerUrl,
                ImageUrl = modelImageUrl,
                ParrentControl = modelParrentControl,
                ReleaseDate = modelReleaseDate ?? DateTime.UtcNow,
                UploadDate = DateTime.UtcNow,
                UploaderId = userId,
                IsApproved = isUserInRoleModerator,

            };

            foreach (var categoryName in modelSelectedCategories)
            {
                movie.Categories.Add(new MovieCategory()
                {
                    CategoryId = _db.Categories.FirstOrDefault(x => x.Name == categoryName).Id
                });
            }

            foreach (var actiorName in actiorNames)
            {
                movie.Actiors.Add(new MovieActior()
                {
                    ActiorId = _db.Persons.FirstOrDefault(x => x.Name == actiorName).Id
                });
            }

            foreach (var writerName in writerNames)
            {
                movie.Writers.Add(new MovieWriter()
                {
                    WriterId = _db.Persons.FirstOrDefault(x => x.Name == writerName).Id
                });
            }

            foreach (var directorName in directorNames)
            {
                movie.Director.Add(new MovieDirector()
                {
                    DirectorId = _db.Persons.FirstOrDefault(x => x.Name == directorName).Id
                });
            }

            await this._db.Movies.AddAsync(movie);
            await this._db.SaveChangesAsync();

            return await this.GetMovieIdByParams(modelTitle, modelReleaseDate, modelYear);

        }

        public async Task<PreviewMovieServiceModel> GetMovieById(int movieId) => await this._db.Movies.Where(x => x.Id == movieId).ProjectTo<PreviewMovieServiceModel>().FirstOrDefaultAsync();

        public async Task<bool> IsMovieExistById(int movieId) => await this._db.Movies.AnyAsync(x => x.Id == movieId);

        public async Task<IList<ListMoviesServiceModel>> GetAllMovies(bool isAdminModeratorOrUploader, int page, int pageSize)
        {
            if (!isAdminModeratorOrUploader)
            {
                return await this._db.Movies.Where(m => m.IsApproved == true).OrderByDescending(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ProjectTo<ListMoviesServiceModel>().ToListAsync();
            }
            return await this._db.Movies.OrderByDescending(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ProjectTo<ListMoviesServiceModel>().ToListAsync();
        }

        public async Task<string> Approve(int movieId)
        {
            var movie = await this._db.Movies.FindAsync(movieId);
            movie.IsApproved = true;
            await this._db.SaveChangesAsync();
            return movie.Title;
        }

        public async Task<int> Count(bool isAdminModeratorOrUploader, IList<int> secectedGenres, string searchTerm = null)
        {
            //if (!isAdminModeratorOrUploader)
            //{
            //    return await this._db.Movies.Where(m => m.IsApproved == true).CountAsync();
            //}
            //return await this._db.Movies.CountAsync();
            if (!isAdminModeratorOrUploader)
            {
                if (searchTerm == null)
                {
                    if (secectedGenres == null || secectedGenres.Count == 0)
                    {
                        return await this._db.Movies.Where(m => m.IsApproved == true).CountAsync();
                    }
                    else
                    {
                        var result = new List<Movie>();
                        foreach (var genreId in secectedGenres)
                        {
                            var c = await this._db.Movies.Where(m => m.IsApproved == true)
                                .Where(x => x.Categories.Select(z => z.CategoryId).Contains(genreId)).ToListAsync();
                            result.AddRange(c);
                        }

                        return result.GroupBy(m => m.Id).Select(g => g.First()).Count();
                    }
                }
                else
                {
                    if (secectedGenres == null || secectedGenres.Count == 0)
                    {
                        return await this._db.Movies.Where(m => m.IsApproved == true && m.Title.ToLower().Contains(searchTerm.ToLower())).CountAsync();
                    }
                    else
                    {
                        var result = new List<Movie>();
                        foreach (var genreId in secectedGenres)
                        {
                            var c = await this._db.Movies.Where(m => m.IsApproved == true && m.Title.ToLower().Contains(searchTerm.ToLower()))
                                .Where(x => x.Categories.Select(z => z.CategoryId).Contains(genreId)).ToListAsync();
                            result.AddRange(c);
                        }

                        return result.GroupBy(m => m.Id).Select(g => g.First()).Count();
                    }
                }
            }
            else
            {
                if (searchTerm == null)
                {
                    if (secectedGenres == null || secectedGenres.Count == 0)
                    {
                        return await this._db.Movies.CountAsync();
                    }
                    else
                    {
                        var result = new List<Movie>();
                        foreach (var genreId in secectedGenres)
                        {
                            var c = await this._db.Movies.Where(x => x.Categories.Select(z => z.CategoryId).Contains(genreId)).ToListAsync();
                            result.AddRange(c);
                        }

                        return result.GroupBy(m => m.Id).Select(g => g.First()).Count();
                    }
                }
                else
                {
                    if (secectedGenres == null || secectedGenres.Count == 0)
                    {
                        return await this._db.Movies.Where(m => m.Title.ToLower().Contains(searchTerm.ToLower())).CountAsync();

                    }
                    else
                    {
                        var result = new List<Movie>();
                        foreach (var genreId in secectedGenres)
                        {
                            var c = await this._db.Movies.Where(m => m.Title.ToLower().Contains(searchTerm.ToLower()))
                                .Where(x => x.Categories.Select(z => z.CategoryId).Contains(genreId)).ToListAsync();
                            result.AddRange(c);
                        }

                        return result.GroupBy(m => m.Id).Select(g => g.First()).Count();
                    }
                }
            }
        }

        public async Task<IList<ListMoviesServiceModel>> GetMoviesByGenreAndSerachTerm(bool isAdminModeratorOrUploader, int page, int pageSize, string serchTerm,
            IList<int> secectedGenres)
        {
            if (!isAdminModeratorOrUploader)
            {
                if(serchTerm == null)
                {
                    if (secectedGenres.Count == 0)
                    {
                        return await this._db.Movies.Where(m => m.IsApproved == true).OrderByDescending(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ProjectTo<ListMoviesServiceModel>().ToListAsync();
                    }
                    else
                    {
                        var result = new List<ListMoviesServiceModel>();
                        foreach (var genreId in secectedGenres)
                        {
                            var c = await this._db.Movies.Where(m => m.IsApproved == true)
                                .Where(x => x.Categories.Select(z => z.CategoryId).Contains(genreId)).ProjectTo<ListMoviesServiceModel>().ToListAsync();
                            result.AddRange(c);
                        }

                        return result.GroupBy(m => m.Id).Select(g => g.First()).OrderByDescending(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    }
                }
                else
                {
                    if (secectedGenres.Count == 0)
                    {
                        return await this._db.Movies.Where(m => m.IsApproved == true && m.Title.ToLower().Contains(serchTerm.ToLower())).OrderByDescending(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ProjectTo<ListMoviesServiceModel>().ToListAsync();

                    }
                    else
                    {
                        var result = new List<ListMoviesServiceModel>();
                        foreach (var genreId in secectedGenres)
                        {
                            var c = await this._db.Movies.Where(m => m.IsApproved == true && m.Title.ToLower().Contains(serchTerm.ToLower()))
                                .Where(x => x.Categories.Select(z => z.CategoryId).Contains(genreId)).ProjectTo<ListMoviesServiceModel>().ToListAsync();
                            result.AddRange(c);
                        }

                        return result.GroupBy(m => m.Id).Select(g => g.First()).OrderByDescending(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    }
                }
            }
            else
            {
                if (serchTerm == null)
                {
                    if (secectedGenres.Count == 0)
                    {
                        return await this._db.Movies.OrderByDescending(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ProjectTo<ListMoviesServiceModel>().ToListAsync();
                    }
                    else
                    {
                        var result = new List<ListMoviesServiceModel>();
                        foreach (var genreId in secectedGenres)
                        {
                            var c = await this._db.Movies
                                .Where(x => x.Categories.Select(z => z.CategoryId).Contains(genreId)).ProjectTo<ListMoviesServiceModel>().ToListAsync();
                            result.AddRange(c);
                        }

                        return result.GroupBy(m => m.Id).Select(g => g.First()).OrderByDescending(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    }
                }
                else
                {
                    if (secectedGenres.Count == 0)
                    {
                        return await this._db.Movies.Where(m => m.Title.ToLower().Contains(serchTerm.ToLower())).OrderByDescending(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ProjectTo<ListMoviesServiceModel>().ToListAsync();

                    }
                    else
                    {
                        var result = new List<ListMoviesServiceModel>();
                        foreach (var genreId in secectedGenres)
                        {
                            var c = await this._db.Movies.Where(m => m.Title.ToLower().Contains(serchTerm.ToLower()))
                                .Where(x => x.Categories.Select(z => z.CategoryId).Contains(genreId)).ProjectTo<ListMoviesServiceModel>().ToListAsync();
                            result.AddRange(c);
                        }

                        return result.GroupBy(m => m.Id).Select(g => g.First()).OrderByDescending(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    }
                }
            }
        }

        public async Task<IList<ListWaitingApprovementServiceModel>> GetAllWaitingApprovementMovies(int page, int pageSize)
        {
            var movies = await this._db.Movies.Where(m => !m.IsApproved).OrderByDescending(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ProjectTo<ListWaitingApprovementServiceModel>().ToListAsync();
            return movies;
        }

        public async Task<int> CountWaitingApprovement()
        {
            return await this._db.Movies.Where(m => !m.IsApproved).CountAsync();
        }

        public async Task Edit(int modelId, int? modelYear, string modelTitle, int? modelDuration, string modelPlot, string modelTrailerUrl,
            string modelImageUrl, int? modelParrentControl, DateTime? modelReleaseDate, IList<string> modelSelectedCategories,
            bool modelIsApproved, string modelActiors, string modelWriters, string modelDirector)
        {
            var movie = await this._db.Movies.FindAsync(modelId);

            movie.Year = modelYear;
            movie.Title = modelTitle;
            movie.Duration = modelDuration ?? 0;
            movie.Plot = modelPlot;
            movie.TrailerUrl = modelTrailerUrl;
            movie.ImageUrl = modelImageUrl;
            movie.ParrentControl = modelParrentControl;
            movie.ReleaseDate = modelReleaseDate ?? DateTime.UtcNow;
            movie.IsApproved = modelIsApproved;

            var actiors = modelActiors.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).Distinct().ToList();
            var directors = modelDirector.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).Distinct().ToList();
            var writers = modelWriters.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).Distinct().ToList();

            var persons = new List<string>(actiors);
            persons.AddRange(directors);
            persons.AddRange(writers);

            foreach (var personName in persons)
            {
                if (!await this._db.Persons.AnyAsync(x => x.Name == personName))
                {
                    await this._db.AddAsync(new Person()
                    {
                        Name = personName
                    });
                }
            }

            await _db.SaveChangesAsync();


            foreach (var person in this._db.MovieActiors.Where(x => x.Movie.Id == modelId).Select(x => x.Actior))
            {
                if (!actiors.Contains(person.Name))
                {
                    this._db.MovieActiors.Remove(new MovieActior()
                    {
                        ActiorId = person.Id,
                        MovieId = modelId
                    });
                }
            }

            foreach (var actiorName in actiors)
            {
                var actior = this._db.Persons.First(x => x.Name == actiorName);

                if (!this._db.MovieActiors.Where(x => x.Movie.Id == modelId).Select(x => x.Actior).ToList().Contains(actior))
                {
                    movie.Actiors.Add(new MovieActior()
                    {
                        ActiorId = _db.Persons.FirstOrDefault(x => x.Name == actiorName).Id
                    });
                }                
            }

            foreach (var person in this._db.MovieDirectors.Where(x => x.Movie.Id == modelId).Select(x => x.Director))
            {
                if (!directors.Contains(person.Name))
                {
                    this._db.MovieDirectors.Remove(new MovieDirector()
                    {
                        DirectorId = person.Id,
                        MovieId = modelId
                    });
                }
            }

            foreach (var directorName in directors)
            {
                var director = this._db.Persons.First(x => x.Name == directorName);

                if (!this._db.MovieDirectors.Where(x => x.Movie.Id == modelId).Select(x => x.Director).ToList().Contains(director))
                {
                    movie.Director.Add(new MovieDirector()
                    {
                        DirectorId = _db.Persons.FirstOrDefault(x => x.Name == directorName).Id
                    });
                }
            }

            foreach (var person in this._db.MovieWriters.Where(x => x.Movie.Id == modelId).Select(x => x.Writer))
            {
                if (!writers.Contains(person.Name))
                {
                    this._db.MovieWriters.Remove(new MovieWriter()
                    {
                        WriterId = person.Id,
                        MovieId = modelId
                    });
                }
            }

            foreach (var writerName in writers)
            {
                var writer = this._db.Persons.First(x => x.Name == writerName);

                if (!this._db.MovieWriters.Where(x => x.Movie.Id == modelId).Select(x => x.Writer).ToList().Contains(writer))
                {
                    movie.Writers.Add(new MovieWriter()
                    {
                        WriterId = _db.Persons.FirstOrDefault(x => x.Name == writerName).Id
                    });
                }
            }

            foreach (var category in this._db.MovieCategories.Where(x => x.Movie.Id == modelId).Select(x => x.Category))
            {
                if (!modelSelectedCategories.Contains(category.Name))
                {
                    this._db.MovieCategories.Remove(new MovieCategory()
                    {
                        CategoryId = category.Id,
                        MovieId = modelId
                    });
                }
            }

            foreach (var categoryName in modelSelectedCategories)
            {
                var category = this._db.Categories.First(x => x.Name == categoryName);

                if (!this._db.MovieCategories.Where(x => x.Movie.Id == modelId).Select(x => x.Category).ToList().Contains(category))
                {
                    movie.Categories.Add(new MovieCategory()
                    {
                        CategoryId = _db.Categories.FirstOrDefault(x => x.Name == categoryName).Id
                    });
                }
            }

            await this._db.SaveChangesAsync();

        }

        public async Task<IList<ListLatestFourServiceModel>> GetLatestFourMovies() => await this._db.Movies
            .Where(x => x.IsApproved).OrderByDescending(x => x.Id).Take(4).ProjectTo<ListLatestFourServiceModel>()
            .ToListAsync();

        public async Task Delete(int movieId)
        {
            this._db.MovieWriters.RemoveRange(await this._db.MovieWriters.Where(x => x.MovieId == movieId).ToListAsync());
            this._db.MovieActiors.RemoveRange(await this._db.MovieActiors.Where(x => x.MovieId == movieId).ToListAsync());
            this._db.MovieDirectors.RemoveRange(await this._db.MovieDirectors.Where(x => x.MovieId == movieId).ToListAsync());
            this._db.MovieCategories.RemoveRange(await this._db.MovieCategories.Where(x => x.MovieId == movieId).ToListAsync());

            this._db.Movies.Remove(await this._db.Movies.FindAsync(movieId));

            await this._db.SaveChangesAsync();
        }

        private async Task<int> GetMovieIdByParams(string modelTitle, DateTime? modelReleaseDate, int? modelYear)
        {
            var result = await _db.Movies.FirstOrDefaultAsync(x => x.Title == modelTitle && x.Year == modelYear && x.ReleaseDate == modelReleaseDate);
            return result.Id;
        }
    }
}
