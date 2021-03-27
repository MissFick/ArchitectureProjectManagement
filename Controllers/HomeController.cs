using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ArchitectureProjectManagement.Controllers
{
    public class HomeController : Controller
    {


        public IActionResult Index()
        {
            /*if (User.IsInRole("Draughtsman") || User.IsInRole("Administrators") || User.IsInRole("Property Owner"){
                var userId = User.Identity.IsAuthenticated
                return RedirectToAction("Index", "Project", new { userId });
            }*/
                /*var userId = User.G
                try{
               return RedirectToAction("Index", "Project", new { userId });
               }
                */
                /*if (User.IsInRole("Property Owner"))
                {

                }*/
                return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        
    }
}
