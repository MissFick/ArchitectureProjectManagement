using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ArchitectureProjectManagement.Contracts;
using ArchitectureProjectManagement.Data;
using ArchitectureProjectManagement.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ManageProjects.Controllers
{
    [Authorize(Roles ="Administrator")]
    public class ProjectItemController : Controller
    {
        private readonly IProjectItemRepository _repo;
        private readonly IMapper _mapper;

        public ProjectItemController(IProjectItemRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var projectItems = await _repo.GetAll();
            var model = _mapper.Map<List<ProjectItem>, List<ProjectItemViewModel>>(projectItems.ToList());
            return View(model);
        }

        //GET: ProjectItem/Details/2
        public IActionResult Details(int id)
        {
           
            return View();
        }

        //GET: ProjectItem/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProjectItem entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(entity);
                }
                var projectitem = _mapper.Map<ProjectItem>(entity);
                //projectitem.DateCreated = DateTime.Now;
                var isSuccess = await _repo.Add(projectitem);
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


       
        //GET: ProjectItem/Edit
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
