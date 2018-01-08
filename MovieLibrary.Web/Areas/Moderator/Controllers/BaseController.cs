using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieLibrary.Web.Infrastructure.Extesions;
using MovieLibrary.Web.WebConstants;

namespace MovieLibrary.Web.Areas.Moderator.Controllers
{
    [Area(AreaConstants.ModeratorArea)]
    [AuthorizeRoles(new string []{RoleConstants.AdministratorRole, RoleConstants.ModeratorRole})]
    public class BaseController : Controller
    {
    }
}
