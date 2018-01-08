using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieLibrary.Data.Models;
using MovieLibrary.Services.Interfaces;
using MovieLibrary.Web.Areas.Admin.Models.UsersViewModel;
using MovieLibrary.Web.Infrastructure.Extesions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieLibrary.Web.WebConstants;

namespace MovieLibrary.Web.Areas.Admin.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUserService _users;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(IUserService users, RoleManager<IdentityRole> roleManager)
        {
            _users = users;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> AddUser()
        {

            ViewBag.Name = "AddUser";

            return View(new AddUserViewModel()
            {
                Roles = await this._roleManager.Roles.Select(r => new SelectListItem()
                {
                    Text = r.Name,
                    Value = r.Name

                }).ToListAsync()
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserViewModel model)
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
                    return View(model);
                }
                path = imagePath.Substring(7);

                await ImagesByUserExtesions.SaveProfileImage(model.ImageFile, model.UserName, imagePath);
                model.ImgPath = path;
            }

            
                    var roleExist = await this._roleManager.RoleExistsAsync(model.Role);
                    if (!roleExist)
                    {
                        ModelState.AddModelError("Roles", "Invalid Role");
                        return View(model);
                    }
                
            

            await this._users.AddNewUser(model.UserName, model.FirstName, model.LastName, model.Email, model.BirthDate, model.ImgPath, model.Role, model.Password);

            return RedirectToAction("Profile", "Users", new {area = "", username = model.UserName});
        }
    }
}
