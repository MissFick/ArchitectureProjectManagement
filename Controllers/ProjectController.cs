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
using Microsoft.AspNetCore.Mvc.Rendering;
using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using System.Threading;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ArchitectureProjectManagement.Controllers
{

    public class ProjectController : Controller
    {
    //    private IUserRoleStore<UserRole> _rolestore;

        private readonly SiteRoleManager<SiteRole> _siteRoleManager;
        private readonly IProjectRepository _repo;
        private readonly IProjectItemRepository _itemrepo;
        private readonly ICompanyRepository _companyrepo;
        private readonly IPropertyRepository _propertyrepo;    
        private readonly IMapper _mapper;
        private readonly SiteUserManager<SiteUser> _siteUserManager;
        private readonly IAppUserRoleRepository _appUserRoleRepo;
        private readonly IAppRoleRepository _appRoleRepo;



        public ProjectController(IProjectRepository repo,
            IMapper mapper,
            IProjectItemRepository itemrepo,
            ICompanyRepository companyrepo,
            IPropertyRepository propertyrepo,
            SiteUserManager<SiteUser> siteUserManager,
            SiteRoleManager<SiteRole> siteRoleMamager,
            IAppUserRoleRepository appUserRoleRepo,
            IAppRoleRepository appRoleRepo 
           )
        
        {
            _repo = repo;
            _itemrepo = itemrepo;
            _companyrepo = companyrepo;
            _propertyrepo = propertyrepo;
            _siteRoleManager = siteRoleMamager;
            _mapper = mapper;
            _siteUserManager = siteUserManager;
            _appUserRoleRepo = appUserRoleRepo;
            _appRoleRepo = appRoleRepo;
        }

        // GET: /<controller>/
        [Authorize]
        public async Task<IActionResult> Index()
        {
            ICollection<Project> projects = null;


            if (User.IsInRole("Draughtsman"))
            {
                projects = await _repo.FindAllDraughtsmanProjects(User.GetUserId());
            }
            else if (User.IsInRole("Property Owner"))
            {
                projects = await _repo.FindAllByPropertyOwnerProjects(User.GetUserId());

            }
            else
            {
                projects = await _repo.GetAll();
            }
            /*var projects = await _repo.GetAll();
            var model = _mapper.Map<List<Project>, List<ProjectViewModel>>(projects.ToList());
            */
            var model = _mapper.Map<List<Project>, List<ProjectViewModel>>(projects.ToList());
            return View(model);
            
        }

        [Authorize]
        public async Task<IActionResult> Archived()
        {
            //ICollection<Project> projects = null;
            try
            {
               
                    var projects = await _repo.GetAllArchivedProjects();
                    var model = _mapper.Map<List<Project>, List<ProjectViewModel>>(projects.ToList());

                    return View(model);
                
            }
            catch
            { 
                return View();
            }
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Concluded()
        {
            //ICollection<Project> projects = null;
            try
            {

                var projects = await _repo.GetAllConcludedProjects();
                var model = _mapper.Map<List<Project>, List<ProjectViewModel>>(projects.ToList());

                return View(model);

            }
            catch
            {
                return View();
            }
            return View();
        }


        [Authorize]
        public async Task<IActionResult> Dormant()
        {
            //ICollection<Project> projects = null;
            try
            {

                var projects = await _repo.GetAllDormantProjects();
                var model = _mapper.Map<List<Project>, List<ProjectViewModel>>(projects.ToList());

                return View(model);

            }
            catch
            {
                return View();
            }
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Lost()
        {
            //ICollection<Project> projects = null;
            try
            {

                var projects = await _repo.GetAllLostProjects();
                var model = _mapper.Map<List<Project>, List<ProjectViewModel>>(projects.ToList());

                return View(model);

            }
            catch
            {
                return View();
            }
            return View();
        }

        //GET: Project/Details/2
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {

            var project = await _repo.Get(id);
            var model = _mapper.Map<Project, ProjectViewModel>(project);
            var property = await _propertyrepo.Get(project.PropertyId);

            var propertyOwner = await _siteUserManager.FindByIdAsync(project.PropertyOwnerId.ToString());
            var draughtsman = await _siteUserManager.FindByIdAsync(project.DraughtsmanId.ToString());
            //string siteid = "1732d901-82c3-4c48-9e02-3049c8ea2738";
            ProjectDetailsViewModel projectDetails = new ProjectDetailsViewModel
            {
                ProjectId = project.ProjectId,
                ProjectName = project.ProjectName,
                ProjectDescription = project.ProjectDescription,
                MunicipalRefNo = project.MunicipalRefNo,
                DateofSubmission = project.DateofSubmission,
                MunicipalAssessmentOfficer = project.MunicipalAssessmentOfficer,
                AssessmentOfficerEmail = project.AssessmentOfficerEmail,
                AssessmentOfficerContactNo = project.AssessmentOfficerContactNo,
                DateCreated = project.DateCreated,
                DateModified = project.DateModified,
                PropertyId = project.PropertyId,
                DraughtsmanId = project.DraughtsmanId,
                ProjectStateId = project.ProjectStateId,
                SiteId = property.SiteId,

               PropertyName = property.PropertyName,
               PropertyAddress = property.PropertyAddress,
               PropertyERF_LotNo = property.PropertyERF_LotNo,
               IsComplex = property.IsComplex,
               IsEstate = property.IsEstate,
               Complex_Estate_No = property.Complex_Estate_No,
               PropertySGNo = property.PropertySGNo,

               PropertyOwnerFirstName = propertyOwner.FirstName,
               PropertyOwnerLastName = propertyOwner.LastName,
               PropertyOwnerEmail = propertyOwner.Email,
              // PropertyOwnerContactNo = propertyOwner.PhoneNumber,

               DraughtsmanFirstName = draughtsman.FirstName,
               DraughtsmanLastName = draughtsman.LastName,
               DraughtsmanEmail = draughtsman.Email,
               DraughtsmanContactNo = draughtsman.PhoneNumber,

            };
           


           //Client or Property Owner Details
           //Draughtman Details

           // var model = _mapper.Map<Project, ProjectViewModel>(project);
            return View(projectDetails);
        }


        [Authorize(Policy = "UserLookupPolicy")]
        //GET: Project/Create
        public async Task<ActionResult> Create()
        {
            var userId = User.GetUserIdAsGuid();
            var siteId = User.GetUserSiteIdAsGuid();
            var draughtsmanRole = await _appRoleRepo.GetDraughtsmanRoleId(siteId);
            var draughtsmanRoleId = draughtsmanRole.Id;
            var draughtsmen = await _appUserRoleRepo.GetAllDraughtsman(draughtsmanRoleId);
            //var draughtsmen = await _appUserRoleRepo.GetAllDraughtsman(siteId);

            List<DraughtsmanDDListViewModel> draftSelect = new List<DraughtsmanDDListViewModel> { };
            foreach (var draughtsman in draughtsmen)
            {
                var draftId = draughtsman.UserId;
                var draftsman = await _siteUserManager.FindByIdAsync(draftId.ToString());
                var draftListItem = new DraughtsmanDDListViewModel
                {
                    DraughtsmanId = draftId.ToString(),
                    FirstName = draftsman.FirstName,
                    LastName = draftsman.LastName
                };
                draftSelect.Add(draftListItem);

            }
            try
            {
                if (User.IsInRole("Draughtsman") || User.IsInRole("Administrators"))
                {
                    var userEmail = User.GetEmail();
                    var userDisplayName = User.GetEmail();
                   // var siteId = User.GetUserSiteIdAsGuid();

                    var draughtsmanSelect = draftSelect.Select(n => new SelectListItem
                    {
                        Text = n.FirstName + " " + n.LastName,
                        Value = n.DraughtsmanId.ToString()
                    });

                    var allProperties = await _propertyrepo.GetAll();
                    var properties = allProperties.ToList();
                    var propertySelect = properties.Select(n => new SelectListItem
                    {
                        Text = n.PropertyERF_LotNo + " " + n.PropertyName,
                        Value = n.PropertyId.ToString()
                    });

                    CreateProjectViewModel createProjectVM = new CreateProjectViewModel
                    {
                        Properties = propertySelect,
                        Draughtsmen = draughtsmanSelect
                    };
                    return View(createProjectVM);
                };

            }
            catch
            {
                throw new AccessViolationException("User not a Authorized");
            }

            return RedirectToAction("Index", "Home", new { userId });
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProjectViewModel entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(entity);
                }
                //string siteid = "1732d901-82c3-4c48-9e02-3049c8ea2738";
                var siteId = User.GetUserSiteIdAsGuid().ToString();
                var property = await _propertyrepo.Get(int.Parse(entity.PropertyId.ToString()));
                var propertyOwnerId = property.PropertyOwnerId;
                
                var project1 = new ProjectViewModel
                {
                    ProjectName = entity.ProjectName,
                    ProjectDescription = entity.ProjectDescription,
                    MunicipalRefNo = entity.MunicipalRefNo,
                    DateofSubmission = entity.DateofSubmission,
                    MunicipalAssessmentOfficer = entity.MunicipalAssessmentOfficer,
                    AssessmentOfficerEmail = entity.AssessmentOfficerEmail,
                    AssessmentOfficerContactNo = entity.AssessmentOfficerContactNo,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    PropertyId = int.Parse(entity.PropertyId.ToString()),
                    DraughtsmanId = entity.DraughtsmanId,
                    PropertyOwnerId = propertyOwnerId,
                    ProjectStateId = 1,
                    SiteId = siteId,
                };
                
                var project = _mapper.Map<Project>(project1);
                      //applicationUser.DateCreated = DateTime.Now;
               
                      var isSuccess = await _repo.Add(project);
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


        
        public async Task<ActionResult> DraughtsmanProjects(string draughtsmanId)
        {
            var projects = await _repo.FindAllDraughtsmanProjects(draughtsmanId);
            var draughtsmanProjects = _mapper.Map<List<Project>, List<ProjectViewModel>>(projects.ToList());
            return View(draughtsmanProjects);
        }


        public async Task<ActionResult> ClientProjects(string clientId)
        {
            var projects = await _repo.FindAllByPropertyOwnerProjects(clientId);
            var clientProjects = _mapper.Map<List<Project>, List<ProjectViewModel>>(projects.ToList());
            return View(clientProjects);
        }

        //GET: Draughtsman/Edit
        public ActionResult UpdateProjectDraughtman(int projectid)
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

        //Check how you're going to work this one.
        //It's for Transfer Project.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProjectDraughtman(int projectid, IFormCollection collection)
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


        //Update Project State

        public async Task<ActionResult> ProjectStateUpdate(int id)
        {
            var userId = User.GetUserIdAsGuid();
            try
            {
                if (User.IsInRole("Draughtsman") || User.IsInRole("Administrators"))
                {
                    var project = await _repo.Get(id);
                    var mappedProject = _mapper.Map<Project, ProjectViewModel>(project);

                    string projectCurrentStateName = null;
                    switch (mappedProject.ProjectStateId)
                    {
                        case 1:
                            projectCurrentStateName = "New";
                            break;
                        case 2:
                            projectCurrentStateName = "Active";
                            break;
                        case 3:
                            projectCurrentStateName = "Dormant";
                            break;
                        case 4:
                            projectCurrentStateName = "Lost";
                            break;
                        case 5:
                            projectCurrentStateName = "Concluded";
                            break;
                        case 6:
                            projectCurrentStateName = "Archived";
                            break;

                    }

                    List<ProjectStateViewModel> StateList = new List<ProjectStateViewModel>();
                    StateList.Add(new ProjectStateViewModel { ProjectStateId = 1, ProjectStateName = "New" });
                    StateList.Add(new ProjectStateViewModel { ProjectStateId = 2, ProjectStateName = "Active" });
                    StateList.Add(new ProjectStateViewModel { ProjectStateId = 3, ProjectStateName = "Dormant" });
                    StateList.Add(new ProjectStateViewModel { ProjectStateId = 4, ProjectStateName = "Lost" });
                    StateList.Add(new ProjectStateViewModel { ProjectStateId = 5, ProjectStateName = "Concluded" });
                    StateList.Add(new ProjectStateViewModel { ProjectStateId = 6, ProjectStateName = "Archived" });

                    var projectStateSelect = StateList.Select(n => new SelectListItem
                    {
                        Text = n.ProjectStateName,
                        Value = n.ProjectStateId.ToString()
                    });

                    var projectstate = new ProjectStateProjectVM
                    {
                        ProjectId = id,
                        Project = mappedProject,
                        ProjectStateName = projectCurrentStateName,
                        StateSelectList = projectStateSelect,
                    };

                    //return RedirectToAction(nameof(Index));
                    //return RedirectToAction("Details", "Property", new { id = entity.PropertyId });
                    return View(projectstate);
                }
            }
            catch
            {
                throw new AccessViolationException("User not a Authorized");
            }
            return RedirectToAction("Index", "Project", new { userId });
        }
    
        
         //Check how you're going to work this one.
         //It's for Transfer Project.
         [HttpPost]
         [ValidateAntiForgeryToken]
         public async Task<ActionResult> ProjectStateUpdate(ProjectStateProjectVM projectStateVM)
          {
             try
             {
             var siteId = User.GetUserSiteIdAsGuid();
                ProjectViewModel updatedProject = new ProjectViewModel
                {
                    ProjectId = projectStateVM.ProjectId,
                    ProjectName = projectStateVM.Project.ProjectName,
                    ProjectDescription = projectStateVM.Project.ProjectDescription,
                    MunicipalRefNo = projectStateVM.Project.MunicipalRefNo,
                    MunicipalAssessmentOfficer = projectStateVM.Project.MunicipalAssessmentOfficer,
                    AssessmentOfficerContactNo = projectStateVM.Project.AssessmentOfficerContactNo,
                    AssessmentOfficerEmail = projectStateVM.Project.AssessmentOfficerEmail,
                    DateCreated = projectStateVM.Project.DateCreated,
                    DateofSubmission = projectStateVM.Project.DateofSubmission,
                    DraughtsmanId = projectStateVM.Project.DraughtsmanId,
                    PropertyId = projectStateVM.Project.PropertyId,
                    PropertyOwnerId = projectStateVM.Project.PropertyOwnerId,
                    ProjectStateId = projectStateVM.Project.ProjectStateId,
                    DateModified = DateTime.Now,
                    SiteId = projectStateVM.Project.SiteId
                };
             var mappedProject = _mapper.Map<ProjectViewModel, Project>(updatedProject);
             var isSuccess = await _repo.Update(mappedProject);
             if (!isSuccess)
             {
                 ModelState.AddModelError("", "Something went wrong...");
                 return View(projectStateVM.ProjectId);
             }
            
                 return RedirectToAction(nameof(Index));
             }
             catch
             {
                 return View();
             }
         }
        

        //Edit Project
        public async Task<ActionResult> Edit(int id)
        {
            var userId = User.GetUserIdAsGuid();
            var siteId = User.GetUserSiteIdAsGuid();
            var project = await _repo.Get(id);

            var draughtsmen = await _appUserRoleRepo.GetAllDraughtsman(siteId);
            List<DraughtsmanDDListViewModel> draftSelect = new List<DraughtsmanDDListViewModel> { };
            foreach (var draughtsman in draughtsmen)
            {
                var draftId = draughtsman.UserId;
                var draftsman = await _siteUserManager.FindByIdAsync(draftId.ToString());
                var draftListItem = new DraughtsmanDDListViewModel
                {
                    DraughtsmanId = draftId.ToString(),
                    FirstName = draftsman.FirstName,
                    LastName = draftsman.LastName
                };
                draftSelect.Add(draftListItem);
            }
            try
            {
                if (User.IsInRole("Draughtsman") || User.IsInRole("Administrators"))
                {
                    var userEmail = User.GetEmail();
                    var userDisplayName = User.GetEmail();
                   // var siteId = User.GetUserSiteIdAsGuid();

                    var draughtsmanSelect = draftSelect.Select(n => new SelectListItem
                    {
                        Text = n.FirstName + " " + n.LastName,
                        Value = n.DraughtsmanId.ToString()
                    });

                    var allProperties = await _propertyrepo.GetAll();
                    var properties = allProperties.ToList();
                    var propertySelect = properties.Select(n => new SelectListItem
                    {
                        Text = n.PropertyERF_LotNo + " " + n.PropertyName,
                        Value = n.PropertyId.ToString()
                    });

                    CreateProjectViewModel createProjectVM = new CreateProjectViewModel
                    {
                        ProjectId = project.ProjectId,
                        ProjectName = project.ProjectName,
                        ProjectDescription = project.ProjectDescription,
                        MunicipalRefNo = project.MunicipalRefNo,
                        DateofSubmission = project.DateofSubmission,
                        MunicipalAssessmentOfficer = project.MunicipalAssessmentOfficer,
                        AssessmentOfficerEmail = project.AssessmentOfficerEmail,
                        AssessmentOfficerContactNo = project.AssessmentOfficerContactNo,
                        DateCreated = project.DateCreated,
                        DateModified = project.DateModified,
                        PropertyId = project.PropertyId,
                        DraughtsmanId = project.DraughtsmanId,
                        ProjectStateId = project.ProjectStateId,
                        SiteId = project.SiteId,

                     
                       // Properties = propertySelect,
                        Draughtsmen = draughtsmanSelect
                    };
                    return View(createProjectVM);
                }

            }
            catch
            {
                throw new AccessViolationException("User not a Authorized");
            }

            return RedirectToAction("Index", "Home", new { userId });
          
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProjectDetailsViewModel project)
        {
            var siteId = User.GetUserSiteIdAsGuid().ToString();
            if (!ModelState.IsValid)
            {
                return View(project);
            }

            try
            {
                Project updatedProject = new Project
                {
                    ProjectId = project.ProjectId,
                    ProjectName = project.ProjectName,
                    ProjectDescription = project.ProjectDescription,
                    DateofSubmission = project.DateofSubmission,
                    DateCreated = project.DateCreated,
                    DateModified = DateTime.Now,
                    MunicipalRefNo = project.MunicipalRefNo,
                    MunicipalAssessmentOfficer = project.MunicipalAssessmentOfficer,
                    AssessmentOfficerContactNo = project.AssessmentOfficerContactNo,
                    AssessmentOfficerEmail = project.AssessmentOfficerEmail,
                    DraughtsmanId = project.DraughtsmanId,
                    PropertyId = project.PropertyId,
                    PropertyOwnerId = project.PropertyOwnerId,
                    ProjectStateId = project.ProjectStateId,
                    SiteId = project.SiteId
                };
                var isSuccess = await _repo.Update(updatedProject);
                {
                    if (!isSuccess)
                        {
                        ModelState.AddModelError("", "Something went wrong...");
                        return View(project);
                    }
                }
                return RedirectToAction("Details", "Project", new { id = project.ProjectId });
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