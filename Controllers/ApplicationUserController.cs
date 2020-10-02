using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ArchitectureProjectManagement.Contracts;
using ArchitectureProjectManagement.Data;
using ArchitectureProjectManagement.Models;
using Microsoft.AspNetCore.Identity;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ArchitectureProjectManagement.Controllers
{
    public class ApplicationUserController : Controller
    {

        private readonly IApplicationUserRepository _repo;
        private readonly IMapper _mapper;
        private UserManager<IdentityUser> _user;

        public ApplicationUserController(IApplicationUserRepository repo, IMapper mapper, UserManager<IdentityUser> user)
        {
            _repo = repo;
            _mapper = mapper;
            _user = user;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var applicationUsers = await _repo.GetAll();

            var model = _mapper.Map<List<ApplicationUser>, List<ApplicationUserViewModel>>(applicationUsers.ToList());
            return View(model);
        }

        //GET: ApplicationUser/Details/2
        public async Task<IActionResult> Details(int id)
        {
            var applicationUser = await _repo.Get(id);
            var model = _mapper.Map<ApplicationUser, ApplicationUserViewModel>(applicationUser);
            return View(model);
        }

        //GET: ApplicationUser/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateApplicationUser entity)
        {

            if (_user.FindByNameAsync(entity.IdentityUser.UserName).Result == null)
            {
                var email = entity.IdentityUser.UserName;
                var user = new IdentityUser
                {
                    UserName = email,
                    Email = email
                };
                var result = _user.CreateAsync(user, entity.Password).Result;
                
            }
            try
            {
                var newuser = _user.FindByNameAsync(entity.IdentityUser.UserName).Result;
                var newentity = new ApplicationUser
                {
                    FirstName = entity.FirstName,
                    LastName = entity.LastName,
                    Address = entity.Address,
                    Id = _user.FindByNameAsync(entity.IdentityUser.UserName).Result.Id,
                };
                if (!ModelState.IsValid)
                {
                    return View(newentity);
                }
                var applicationUser = _mapper.Map<ApplicationUser>(newentity);
                //applicationUser.DateCreated = DateTime.Now;
                
                var isSuccess = await _repo.Add(applicationUser);
                if (!isSuccess)
                {
                    ModelState.AddModelError("", "Something went wrong...");
                    return View(entity);

                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        //GET: Clients/Edit
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                //TO DO Add Delete Logic Here
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}