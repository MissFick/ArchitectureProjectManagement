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


namespace ArchitectureProjectManagement.Controllers
{
    public class ProjectStateController : Controller
    {
        private readonly IProjectStateRepository _repo;
        private readonly IMapper _mapper;

        public ProjectStateController(IProjectStateRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var projectStates = await _repo.GetAll();
            var model = _mapper.Map<List<ProjectState>, List<ProjectStateViewModel>>(projectStates.ToList());
            return View(model);
        }

        //GET: ProjectState/Details/2
        public IActionResult Details(int id)
        {
            return View();
        }

        //GET: ProjectState/Create
        public ActionResult Create()
        {
            return View();
        }

        //GET: ProjectState/Edit
        public ActionResult Edit(int id)
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

        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProjectStateViewModel projectState)
        {
            var project = away
            try
            {
                //TO DO Add Delete Logic Here
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }*/

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
