using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieLibrary.Web.Infrastructure.Extesions;
using MovieLibrary.Web.WebConstants;

namespace MovieLibrary.Web.Areas.Uploader.Controllers
{
    [Area(AreaConstants.UploaderArea)]
    [AuthorizeRoles(new string []{RoleConstants.AdministratorRole, RoleConstants.UploaderRole, RoleConstants.ModeratorRole})]
    public class BaseController : Controller
    {
    }
}
