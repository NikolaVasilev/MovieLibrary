using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieLibrary.Web.WebConstants;

namespace MovieLibrary.Web.Areas.Admin.Controllers
{
    [Area(AreaConstants.AdminArea)]
    [Authorize(Roles = RoleConstants.AdministratorRole)]
    public class BaseController : Controller
    {
    }
}
