using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using Users.Infrastructure;

namespace Users.Controllers
{
    public class ClaimsController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            ClaimsIdentity ident = HttpContext.User.Identity as ClaimsIdentity; 

            if (ident == null)
            {
                return View("Error", new string[] { "Нет доступных требований" });
            }

            return View(ident.Claims);
        }

        //[Authorize(Roles = "DCStaff")] // оставлено для памятки о том что было ранее
        [ClaimsAccess(Issuer = "RemoteClaims", ClaimType = ClaimTypes.PostalCode,
            Value = "DC 20500")]
        public string OtherAction()
        {
            return "Это защищенный метод действия для группы DCStaff";
        }
    }
}