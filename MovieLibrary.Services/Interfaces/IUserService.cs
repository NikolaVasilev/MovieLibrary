using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieLibrary.Data.Models;
using MovieLibrary.Services.ModelsDTO.UserServiceModels;

namespace MovieLibrary.Services.Interfaces
{
    public interface IUserService
    {

        Task<User> FindUserByUserName(string username);
        Task<ProfileServiceModel> GetProfile(string username);
        Task EditProfile(string modelUserName, DateTime modelBirthDate, string modelImgPath, string pass, string modelFirstName, string modelLastName, string modelEmail, string modelUserRole, string modelOldUserRole);
        Task AddNewUser(string modelUserName, string modelFirstName, string modelLastName, string modelEmail, DateTime modelBirthDate, string modelImgPath, string role, string pass);
        Task<int> Count(string type, string searchTerm,string searchType);
        Task<IList<ListUsersServiceModel>> GetAllUsersByRoleType(string searchType, string type, int page, int pageSize, string searchTerm);
        Task<User> FindUserById(string userId);
    }
}
