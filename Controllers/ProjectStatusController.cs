using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ArchitectureProjectManagement.Contracts;
using ArchitectureProjectManagement.Data;
using ArchitectureProjectManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Messaging.Email;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ArchitectureProjectManagement.Controllers
{
    public class ProjectStatusController : Controller
    {

        private readonly IProjectItemStatusRepository _repo;
        private readonly IProjectRepository _projectrepo;
        private readonly IProjectItemRepository _itemrepo;
        private readonly IPropertyRepository _propertyrepo;
        private readonly IMapper _mapper;
        private readonly SiteUserManager<SiteUser> _siteUserManager;

        public ProjectStatusController(IProjectItemStatusRepository repo,
            IMapper mapper,
            IProjectRepository projectrepo,
            IProjectItemRepository itemrepo,
            IPropertyRepository propertyrepo,
            SiteUserManager<SiteUser> siteUserManager)


        {
            _repo = repo;
            _projectrepo = projectrepo;
            _itemrepo = itemrepo;
            _propertyrepo = propertyrepo;
            _mapper = mapper;
            _siteUserManager = siteUserManager;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index(int id)
        {
            var project = await _projectrepo.Get(id);
            var mappedproject = _mapper.Map<Project, ProjectViewModel>(project);
            var property = await _propertyrepo.Get(project.PropertyId);
           
            var mappedproperty = _mapper.Map<Property, PropertyViewModel>(property);

            var propertyOwner = await _siteUserManager.FindByIdAsync(project.PropertyOwnerId.ToString());
            var draughtsman = await _siteUserManager.FindByIdAsync(project.DraughtsmanId.ToString());
            //string siteid = "1732d901-82c3-4c48-9e02-3049c8ea2738";
            var siteId = User.GetUserSiteIdAsGuid();
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
               // DraughtsmanContactNo = draughtsman.PhoneNumber,

            };

        
            //mappedproject.Property = mappedproperty;*/

            var projectStatusItemViewModel = new ProjectStatusItemViewModel
            {
                ProjectId = id,
                Project = projectDetails
                // Project = mappedproject

            };

            var projectitemstatuses = await _repo.GetAllById(id);
            var projectItemStatusList = new List<ProjectItemStatusViewModel> { };
            foreach (var projectitemstatus in projectitemstatuses)
            {
                var mappeditemstatus = _mapper.Map<ProjectItemStatus, ProjectItemStatusViewModel>(projectitemstatus);
                projectItemStatusList.Add(mappeditemstatus);
            }
            projectStatusItemViewModel.ProjectItemStatuses = projectItemStatusList;
            return View(projectStatusItemViewModel);
        }

        public async Task<ActionResult> Create(int id)
        {
            var allProjectItems = await _itemrepo.GetAll();
            var projectitems = allProjectItems.ToList();
            //var project = _projectrepo.Get(id);
            //var mappedproject = _mapper.Map<ProjectViewModel>(project);
            var checkedprojectitems = new List<ProjectStatusViewModel>();
            foreach (var projectitem in projectitems)
            {
                var projectStatus = new ProjectStatusViewModel
                {
                    ProjectId = id,
                    IsChecked = false,
                    ProjectItemId = projectitem.ProjectItemId,
                    ProjItem = projectitem,
                    SiteId = projectitem.SiteId.ToString()
                    
                };
                checkedprojectitems.Add(projectStatus);
            }

            return View(checkedprojectitems);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(List<ProjectStatusViewModel> projectStatuses)
        {
            int projId = projectStatuses.FirstOrDefault().ProjectId;
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(projectStatuses);
                }

                //var projectid = projectStatuses.ProjectId;
                //var itemstatuses = projectStatus.CheckedProjectItems;
                foreach (var itemstatus in projectStatuses)
                {
                    if (itemstatus.IsChecked)
                    {
                        var projectitemstatus = new ProjectItemStatusViewModel
                        {
                            ProjectId = itemstatus.ProjectId,
                            ProjectItemId = itemstatus.ProjectItemId,
                            IsComplete = false,
                            DateCompleted = DateTime.Now,
                            SiteId = itemstatus.SiteId
                            
                        };
                        var mappeditemstatus = _mapper.Map<ProjectItemStatus>(projectitemstatus);
                        var isSuccess = await _repo.Add(mappeditemstatus);
                        if (!isSuccess)
                        {
                            ModelState.AddModelError("", "Something went wrong...");
                            return View(projId);
                        }
                    };

                };
                //var ProjecId = projId;
                return RedirectToAction("Index", new { id = projId });
            }
            catch
            {
                return View();
            }
        }



        public async Task<ActionResult> Edit(int id)
        { 

            var project = await _projectrepo.Get(id);
            var mappedproject = _mapper.Map<Project, ProjectViewModel>(project);
            var property = await _propertyrepo.Get(project.PropertyId);
            var mappedproperty = _mapper.Map<Property, PropertyViewModel>(property);

            var propertyOwner = await _siteUserManager.FindByIdAsync(project.PropertyOwnerId.ToString());
            var draughtsman = await _siteUserManager.FindByIdAsync(project.DraughtsmanId.ToString());
            //string siteid = "1732d901-82c3-4c48-9e02-3049c8ea2738";
            var siteId = User.GetUserSiteIdAsGuid();
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

               /* DraughtsmanFirstName = draughtsman.FirstName,
                DraughtsmanLastName = draughtsman.LastName,
                DraughtsmanEmail = draughtsman.Email,*/
                // DraughtsmanContactNo = draughtsman.PhoneNumber,

            };



            //mappedproject.Property = mappedproperty;
            var projectStatusItemViewModel = new ProjectStatusItemViewModel
            {
                ProjectId = id,
                Project = projectDetails
                // Project = mappedproject

            };

            var projectitemstatuses = await _repo.GetAllById(id);

            var projectItemStatusList = new List<ProjectItemStatusViewModel> { };
            foreach (var projectitemstatus in projectitemstatuses)
            {
                var mappeditemstatus = _mapper.Map<ProjectItemStatus, ProjectItemStatusViewModel>(projectitemstatus);
                projectItemStatusList.Add(mappeditemstatus);
            }
            projectStatusItemViewModel.ProjectItemStatuses = projectItemStatusList;
            return View(projectStatusItemViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProjectStatusItemViewModel projectStatusItemViewModel)
        {
            var projectId = projectStatusItemViewModel.ProjectItemStatuses.FirstOrDefault().ProjectId;
            
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(projectStatusItemViewModel);
                }
                foreach (var itemstatus in projectStatusItemViewModel.ProjectItemStatuses)
                {
                    var newitemstatus = await _repo.FindByDetails(itemstatus.ProjectId, itemstatus.ProjectItemId);
                    if (itemstatus.IsComplete)
                    {
                        newitemstatus.IsComplete = true;
                        newitemstatus.DateCompleted = DateTime.Now;
                        //var mappeditemstatus = _mapper.Map<ProjectItemStatus>(newitemstatus);
                    }
                    else { 
                    
                        newitemstatus.IsComplete = false;
                        newitemstatus.DateCompleted = DateTime.Now;
                        //var mappeditemstatus = _mapper.Map<ProjectItemStatus>(newitemstatus);
                    }
                    var isSuccess = await _repo.Update(newitemstatus);
                        if (!isSuccess)
                        {
                            ModelState.AddModelError("", "Something went wrong...");
                            return View(projectId);
                        }
                    
                };
                var project = await _projectrepo.Get(projectId);
                project.DateModified = DateTime.Now;
                //var mappedproject = _mapper.Map<Project>(project);
                var isProjectSuccess = await _projectrepo.Update(project);
                if (!isProjectSuccess)
                {
                    ModelState.AddModelError("", "Something went wrong...");
                    return View(projectStatusItemViewModel);

                };

                return RedirectToAction("Index", new { id = projectId });
            }
            catch
            {
                return View(projectStatusItemViewModel);
            }
        }



        /* private void emailClient(string logo, int clientId)
        {
            string htmlEmail;
            string textEmail;

            string plainTextMessage = new PlainTextMessage();
            HtmlEmail htmlMessage = new HtmlEmail();
            EmailSender.SendEmailAsync (
                    propertyOwner.Email,
                    "test@maxwebsite.co.za",
                    projectDetails.ProjectName ,
                    plainTextMessage,
                    htmlMessage,
                    replyTo = null,
                    Importance importance = Importance.Normal);
            

        }*/
    }
}
