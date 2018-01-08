using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieLibrary.Data;
using MovieLibrary.Data.Models;
using MovieLibrary.Services.Interfaces;
using MovieLibrary.Services.ModelsDTO.UserServiceModels;

namespace MovieLibrary.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly MovieLibraryDbContext _db;
        private readonly UserManager<User> _userManager;

        public UserService(MovieLibraryDbContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }


        public async Task<User> FindUserByUserName(string username)=> await this._db.Users.Where(x => x.UserName == username).FirstOrDefaultAsync();

        public async Task<ProfileServiceModel> GetProfile(string username) => await this._db.Users.Where(u => u.UserName == username).ProjectTo<ProfileServiceModel>().FirstOrDefaultAsync();

        public async Task<User> FindUserById(string id) => await this._db.Users.FindAsync(id);

        public async Task EditProfile(string modelUserName, DateTime modelBirthDate, string modelImgPath, string modelPassword, string modelFirstName,
            string modelLastName, string modelEmail,string modelUserRole, string modelOldUserRole)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.UserName == modelUserName);

            user.Email = modelEmail;
            user.FirstName = modelFirstName;
            user.LastName = modelLastName;
            user.BirthDate = modelBirthDate;
            if (user.ImgPath != modelImgPath && !String.IsNullOrEmpty(modelImgPath))
            {
                user.ImgPath = modelImgPath;
            }

            if (!String.IsNullOrEmpty(modelPassword))
            {
                var pass = _userManager.PasswordHasher.HashPassword(user, modelPassword);
                if (user.PasswordHash != pass)
                {
                    user.PasswordHash = pass;
                }              
            }
            await this._db.SaveChangesAsync();

            if (modelOldUserRole != modelUserRole)
            {
                await this._userManager.RemoveFromRoleAsync(user, modelOldUserRole);

                await this._userManager.AddToRoleAsync(user, modelUserRole);
            }
        }

        
        public async Task AddNewUser(string modelUserName, string modelFirstName, string modelLastName, string modelEmail,
            DateTime modelBirthDate, string modelImgPath,string role, string pass)
        {
            var user = new User { UserName = modelUserName, Email = modelEmail, FirstName = modelFirstName, LastName = modelLastName, BirthDate = modelBirthDate, ImgPath = modelImgPath };
            await _userManager.CreateAsync(user, pass);

            await _userManager.AddToRoleAsync(user, role);

        }

        public async Task<int> Count(string type, string searchTerm, string searchType)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                if (type.ToLower() == "all")
                {
                    return await this._db.Users.CountAsync();
                }
                else
                {
                    var users = await _userManager.GetUsersInRoleAsync(type);
                    return users.Count;
                }
            }
            else
            {
                if (searchType.ToLower() == "username")
                {
                    if (type.ToLower() == "all")
                    {
                        return await this._db.Users.Where(x => x.UserName.ToLower().Contains(searchTerm.ToLower())).CountAsync();
                    }
                    else
                    {
                        var users = await _userManager.GetUsersInRoleAsync(type);
                        return users.Where(x => x.UserName.ToLower().Contains(searchTerm.ToLower())).ToList().Count;
                    }
                }
                else if (searchType.ToLower() == "e-mail")
                {
                    if (type.ToLower() == "all")
                    {
                        return await this._db.Users.Where(x => x.Email.ToLower().Contains(searchTerm.ToLower())).CountAsync();
                    }
                    else
                    {
                        var users = await _userManager.GetUsersInRoleAsync(type);
                        return users.Where(x => x.Email.ToLower().Contains(searchTerm.ToLower())).ToList().Count;
                    }
                }
                else if (searchType.ToLower() == "first name")
                {
                    if (type.ToLower() == "all")
                    {
                        return await this._db.Users.Where(x => x.FirstName.ToLower().Contains(searchTerm.ToLower())).CountAsync();
                    }
                    else
                    {
                        var users = await _userManager.GetUsersInRoleAsync(type);
                        return users.Where(x => x.FirstName.ToLower().Contains(searchTerm.ToLower())).ToList().Count;
                    }
                }
                else
                {
                    if (type.ToLower() == "all")
                    {
                        return await this._db.Users.Where(x => x.LastName.ToLower().Contains(searchTerm.ToLower())).CountAsync();
                    }
                    else
                    {
                        var users = await _userManager.GetUsersInRoleAsync(type);
                        return users.Where(x => x.LastName.ToLower().Contains(searchTerm.ToLower())).ToList().Count;
                    }
                }
            }
        }

        public async Task<IList<ListUsersServiceModel>> GetAllUsersByRoleType(string searchType, string type, int page, int pageSize, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                if (type.ToLower() == "all")
                {
                    return await _db.Users.OrderBy(x => x.UserName).Skip((page - 1) * pageSize).Take(pageSize)
                        .ProjectTo<ListUsersServiceModel>().ToListAsync();
                }
                else
                {
                    var users = await _userManager.GetUsersInRoleAsync(type);
                    return users.OrderBy(x => x.UserName).AsQueryable().Skip((page - 1) * pageSize).Take(pageSize)
                        .ProjectTo<ListUsersServiceModel>().ToList();
                }
            }
            else
            {
                if (searchType.ToLower() == "username")
                {
                    if (type.ToLower() == "all")
                    {
                        return await _db.Users.Where(u => u.UserName.ToLower().Contains(searchTerm.ToLower())).OrderBy(x => x.UserName).Skip((page - 1) * pageSize).Take(pageSize)
                            .ProjectTo<ListUsersServiceModel>().ToListAsync();
                    }
                    else
                    {
                        var users = await _userManager.GetUsersInRoleAsync(type);
                        return users.OrderBy(x => x.UserName).Where(u => u.UserName.ToLower().Contains(searchTerm.ToLower())).AsQueryable().Skip((page - 1) * pageSize).Take(pageSize)
                            .ProjectTo<ListUsersServiceModel>().ToList();
                    }
                }
                else if (searchType.ToLower() == "e-mail")
                {
                    if (type.ToLower() == "all")
                    {
                        return await _db.Users.Where(u => u.Email.ToLower().Contains(searchTerm.ToLower())).OrderBy(x => x.UserName).Skip((page - 1) * pageSize).Take(pageSize)
                            .ProjectTo<ListUsersServiceModel>().ToListAsync();
                    }
                    else
                    {
                        var users = await _userManager.GetUsersInRoleAsync(type);
                        return users.OrderBy(x => x.UserName).Where(u => u.Email.ToLower().Contains(searchTerm.ToLower())).AsQueryable().Skip((page - 1) * pageSize).Take(pageSize)
                            .ProjectTo<ListUsersServiceModel>().ToList();
                    }
                }
                else if (searchType.ToLower() == "first name")
                {
                    if (type.ToLower() == "all")
                    {
                        return await _db.Users.Where(u => u.FirstName.ToLower().Contains(searchTerm.ToLower())).OrderBy(x => x.UserName).Skip((page - 1) * pageSize).Take(pageSize)
                            .ProjectTo<ListUsersServiceModel>().ToListAsync();
                    }
                    else
                    {
                        var users = await _userManager.GetUsersInRoleAsync(type);
                        return users.OrderBy(x => x.UserName).Where(u => u.FirstName.ToLower().Contains(searchTerm.ToLower())).AsQueryable().Skip((page - 1) * pageSize).Take(pageSize)
                            .ProjectTo<ListUsersServiceModel>().ToList();
                    }
                }
                else
                {
                    if (type.ToLower() == "all")
                    {
                        return await _db.Users.Where(u => u.LastName.ToLower().Contains(searchTerm.ToLower())).OrderBy(x => x.UserName).Skip((page - 1) * pageSize).Take(pageSize)
                            .ProjectTo<ListUsersServiceModel>().ToListAsync();
                    }
                    else
                    {
                        var users = await _userManager.GetUsersInRoleAsync(type);
                        return users.OrderBy(x => x.UserName).Where(u => u.LastName.ToLower().Contains(searchTerm.ToLower())).AsQueryable().Skip((page - 1) * pageSize).Take(pageSize)
                            .ProjectTo<ListUsersServiceModel>().ToList();
                    }
                }
            }
            
            
        }
    }
}
