using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MovieLibrary.Web.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult ListLog()
        {
            return View();
        }
    }
}
