using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MovieLibrary.Data.Models;
using MovieLibrary.Services.ModelsDTO.MoviesServiceModel;

namespace MovieLibrary.Services.Interfaces
{
    public interface IMovieService
    {
        Task<bool> IsMovieExist(string modelTitle, int? modelYear, DateTime? modelReleaseDate);
        Task<int> AddMovieAsunc(string modelTitle, int? modelYear, int? modelDuration, string modelPlot, string modelTrailerUrl, string modelImageUrl, int? modelParrentControl, DateTime? modelReleaseDate, IList<string> modelSelectedCategories, string modelActiors, string modelWriters, string modelDirector, bool isUserInRoleModerator, string userId);
        Task<PreviewMovieServiceModel> GetMovieById(int movieId);
        Task<bool> IsMovieExistById(int movieId);
        Task<IList<ListMoviesServiceModel>> GetAllMovies(bool isAdminModeratorOrUploader, int page, int pageSize);
        Task<string> Approve(int movieId);
        Task<int> Count(bool isAdminModeratorOrUploader, IList<int> secectedGenres, string searchTerm = null);
        Task<IList<ListMoviesServiceModel>> GetMoviesByGenreAndSerachTerm(bool isAdminModeratorOrUploader, int page, int pageSize, string serchTerm, IList<int> secectedGenres);
        Task<IList<ListWaitingApprovementServiceModel>> GetAllWaitingApprovementMovies(int page, int pageSize);
        Task<int> CountWaitingApprovement();

        Task Edit(int modelId, int? modelYear, string modelTitle, int? modelDuration, string modelPlot, string modelTrailerUrl, string modelImageUrl, int? modelParrentControl, DateTime? modelReleaseDate, IList<string> modelSelectedCategories, bool modelIsApproved, string modelActiors, string modelWriters, string modelDirector);
        Task<IList<ListLatestFourServiceModel>> GetLatestFourMovies();
        Task Delete(int movieId);
    }
}

