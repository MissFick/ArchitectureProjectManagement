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
    public class ProjectItemStatusController : Controller
    {
        private readonly IProjectItemStatusRepository _repo;
        private readonly IProjectRepository _projectrepo;
        private readonly IProjectItemRepository _itemrepo;

        private readonly IMapper _mapper;

        public ProjectItemStatusController(IProjectItemStatusRepository repo,
            IMapper mapper,
            IProjectRepository projectrepo,
            IProjectItemRepository itemrepo)
        {
            _repo = repo;
            _projectrepo = projectrepo;
            _itemrepo = itemrepo;
            _mapper = mapper;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        //GET: ProjectItemStatus/Details/2
        public IActionResult Details(int id)
        {
            return View();
        }


       

        //GET: ProjectItemStatus/Create
            public async Task<ActionResult> Create(List<ProjectStatusViewModel> projectStatuses)
            {

            
            /*
                var projectitemstatus = _repo.GetAll().ToList();
                var mappeditemstatus = _mapper.Map<List<ProjectItemStatus>, List<ProjectItemStatusViewModel>>(projectitemstatus);
            */
            //var projectitems = _itemrepo.GetAll().ToList();
            //var mappeditems = _mapper.Map<List<ProjectItem>, List<ProjectItemViewModel>>(projectitem);

            //var projectid = projectStatuses.ProjectId;
            //var itemstatuses = projectStatus.CheckedProjectItems;
            foreach(var itemstatus in projectStatuses)
            {
                if(itemstatus.IsChecked)
                {
                    var projectitemstatus = new ProjectItemStatusViewModel
                    {
                        ProjectId = itemstatus.ProjectId,
                        ProjectItemId = itemstatus.ProjItem.ProjectItemId,
                        IsComplete = false
                    };
                    var mappeditemstatus = _mapper.Map<ProjectItemStatus>(projectitemstatus);
                    await _repo.Add(mappeditemstatus);
                }
            };
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProjectItemStatusViewModel entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(entity);
                }
                var projectitemstatus = _mapper.Map<ProjectItemStatus>(entity);
                //projectitemstatus.DateCreated = DateTime.Now;
                var isSuccess = await _repo.Add(projectitemstatus);
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
