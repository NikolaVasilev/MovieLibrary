using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieLibrary.Data.Models;
using MovieLibrary.Services.Interfaces;
using MovieLibrary.Web.Infrastructure.Extesions;
using MovieLibrary.Web.Models.UserViewModel;
using MovieLibrary.Web.WebConstants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MovieLibrary.Web.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUserService _users;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private const int PageSize = 10;

        public UsersController(IUserService users, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _users = users;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Profile(string username)
        {
            
            return View(await this._users.GetProfile(username));
        }


        public async Task<IActionResult> EditProfile(string username)
        {
            var user = await this._users.FindUserByUserName(username);

            if (user == null)
            {
                return NotFound();
            }

            if (user.UserName != User.Identity.Name && !User.IsInRole(RoleConstants.AdministratorRole))
            {
                return BadRequest();
            }

            var userRole = await this._userManager.GetRolesAsync(user);
            var roles = await this._roleManager.Roles.Select(r => new SelectListItem()
            {
                Text = r.Name,
                Value = r.Name,
                Selected = r.Name == userRole.FirstOrDefault()
            }).ToListAsync();

            return View(new EditProfileViewModel()
            {
                UserName = user.UserName,
                BirthDate = user.BirthDate,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ImgPath = user.ImgPath,
                UserRole = userRole.FirstOrDefault(),
                OldUserRole = userRole.FirstOrDefault(),
                Roles = roles

                });
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string path = string.Empty;

            if (model.ImageFile != null)
            {
                var imagePath = $"wwwroot/images/users/{model.UserName}/Profile/{string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt_}", DateTime.UtcNow) + model.ImageFile.FileName}";

                var possibleExtesions = new string[] { ".jpeg", ".jpg", ".gif", ".png" };
                var extension = Path.GetExtension(model.ImageFile.FileName);

                var fileLength = model.ImageFile.Length;

                if (fileLength > 1024 * 1024 || !possibleExtesions.Contains(extension))
                {
                    ModelState.AddModelError(model.ImageFile.Name, "Your file must be a jpeg, jpg, gif or png image and must less than 1mb.");
                    var user = await this._users.FindUserByUserName(model.UserName);
                    var userRole = await this._userManager.GetRolesAsync(user);
                    var roles = await this._roleManager.Roles.Select(r => new SelectListItem()
                    {
                        Text = r.Name,
                        Value = r.Name,
                        Selected = r.Name == userRole.FirstOrDefault()
                    }).ToListAsync();
                    model.Roles = roles;
                    return View(model);
                }
                path = imagePath.Substring(7);

                await ImagesByUserExtesions.SaveProfileImage(model.ImageFile, model.UserName, imagePath);
                model.ImgPath = path;
            }


            var roleExist = await this._roleManager.RoleExistsAsync(model.UserRole);
            if (!roleExist)
            {
                ModelState.AddModelError("Roles", "Invalid Role");
                return View(model);
            }

            
            await this._users.EditProfile(model.UserName, model.BirthDate, model.ImgPath, model.Password, model.FirstName, model.LastName, model.Email, model.UserRole, model.OldUserRole);

            return RedirectToAction("Profile", new {username = model.UserName});
        }


        public async Task<IActionResult> ListUsers(string searchType, string searchTerm, int page = 1, string userType = "All")
        {
            IList<string> checkSearchType = new List<string>()
            {
               "Username",
               "E-mail",
               "First Name",
               "Last Name"
            };

            IList<string> checkType = new List<string>()
            {

                RoleConstants.AdministratorRole,
                RoleConstants.AuthenticatedUserRole,
                RoleConstants.ModeratorRole,
                RoleConstants.UploaderRole,
                RoleConstants.BannedUserRole,
                "All"
            };


            if (!string.IsNullOrEmpty(searchType))
            {
                if (!checkSearchType.Contains(searchType))
                {
                    return BadRequest();
                }
            }
            

            if (!checkType.Contains(userType))
            {
                return BadRequest();
            }

            ViewBag.Name = $"List {userType}";



            return View(new ListUsersViewModel()
            {
                Users = await this._users.GetAllUsersByRoleType(searchType, userType, page, PageSize, searchTerm),
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(await this._users.Count(userType, searchTerm, searchType) / (double)PageSize),
                UserType = userType,
                SearchType = searchType,
                SearchTerm = searchTerm,
                AllRoleListingTypes = checkType.Select(x => new SelectListItem()
                {
                    Text = x.ToString(),
                    Value = x.ToString()
                }).ToList(),
                AllSearchListingTypes = checkSearchType.Select(x => new SelectListItem()
                {
                    Text = x.ToString(),
                    Value = x.ToString()
                }).ToList()
            });
        }
    }
}
