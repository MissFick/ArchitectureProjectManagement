using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ArchitectureProjectManagement.Contracts;
using ArchitectureProjectManagement.Data;
using ArchitectureProjectManagement.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using cloudscribe.Core.Models;
using cloudscribe.Core.Identity;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ArchitectureProjectManagement.Controllers
{
    public class PropertyController : Controller
    {
        private readonly IPropertyRepository _repo;
        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepo;
        private readonly IProjectRepository _projectRepo;
        private SiteUserManager<SiteUser> _siteUserManager;
        //private readonly SiteRoleManager<SiteRole> _siteRoleManager;
        private readonly IAppUserRoleRepository _appUserRoleRepo;
        private readonly IAppRoleRepository _appRoleRepo;


        public PropertyController(IPropertyRepository repo,
            IMapper mapper,
            IProjectRepository projectRepo,
            ICompanyRepository companyRepo,
            SiteUserManager<SiteUser> siteUserManager,
            //SiteRoleManager<SiteRole> siteRoleManager,
            IAppUserRoleRepository appUserRoleRepo,
            IAppRoleRepository appRoleRepo
            )
        {
            _repo = repo;
            _mapper = mapper;
           _projectRepo = projectRepo;
            _companyRepo = companyRepo;
            _siteUserManager = siteUserManager;
            //_siteRoleManager = siteRoleManager;
            _appUserRoleRepo = appUserRoleRepo;
            _appRoleRepo = appRoleRepo;
        }


        // GET: /<controller>/
        [Authorize(Policy = "UserLookupPolicy")]
        public async Task<IActionResult> Index()
        {
            var allProperties = await _repo.GetAll();
            var tb_property = allProperties.ToList();


            var siteId = User.GetUserSiteIdAsGuid();
            var propertyOwnerRole = await _appRoleRepo.GetPropertyOwnerRoleId(siteId);
            var propertyOwnerRoleId = propertyOwnerRole.Id;
            var propertyOwners = await _appUserRoleRepo.GetAllPropertyOwners(propertyOwnerRoleId);
            var propertyOwnersList = propertyOwners.ToList();

            var propertyList = new List<PropertyViewModel> { };
            //CREATE A PROPERTIES VIEWMODEL TO INCLUDE ALL THE OWNERS HERE
            foreach (var property in tb_property)
            {
                PropertyViewModel PropertywithOwner = new PropertyViewModel
                {
                    PropertyId = property.PropertyId,
                    PropertyName = property.PropertyName,
                    PropertySGNo = property.PropertySGNo,
                    PropertyAddress = property.PropertyAddress,
                    PropertyERF_LotNo = property.PropertyERF_LotNo,
                    IsComplex = property.IsComplex,
                    IsEstate = property.IsEstate,
                    Complex_Estate_No = property.Complex_Estate_No,
                    PropertyOwnerId = property.PropertyOwnerId
                    
                };
                foreach (var propertyOwner in propertyOwnersList)
                {
                    if (propertyOwner.UserId.ToString() == property.PropertyOwnerId)
                    {
                        var propertyOwnerItem = await _siteUserManager.FindByIdAsync(propertyOwner.UserId.ToString());
                        PropertywithOwner.PropertyOwnerFirstName = propertyOwnerItem.FirstName;
                        PropertywithOwner.PropertyOwnerLastName = propertyOwnerItem.LastName;
                        PropertywithOwner.PropertyOwnerEmailAddress = propertyOwnerItem.Email;
                        PropertywithOwner.PropertyOwnerContactNo = propertyOwnerItem.PhoneNumber;
                    }
                };
                propertyList.Add(PropertywithOwner);
                //Assign Project Owner Name , Surname , Contact Number and Email.

            }
            //var model = _mapper.Map<List<Property>, List<PropertyViewModel>>(tb_property);
            return View(propertyList);
        }

        //GET: Property/Details/2
        public async Task<IActionResult> Details(int id)
        {
            var property = await _repo.Get(id);
            //var propertyView = _mapper.Map<PropertyViewModel>(property);
            var propOwn = await _siteUserManager.FindByIdAsync(property.PropertyOwnerId.ToString());
            PropertyDetailsViewModel propertyDetails = new PropertyDetailsViewModel
            {
                PropertyId = property.PropertyId,
                PropertyOwnerId = property.PropertyOwnerId,
                PropertyName = property.PropertyName,
                PropertyAddress = property.PropertyAddress,
                PropertyERF_LotNo = property.PropertyERF_LotNo,
                PropertySGNo = property.PropertySGNo,
                IsComplex = property.IsComplex,
                IsEstate = property.IsEstate,
                Complex_Estate_No = property.Complex_Estate_No,
                PropertyOwnerFirstName = propOwn.FirstName,
                PropertyOwnerLastName = propOwn.LastName,
                PropertyOwnerEmailAddress = propOwn.Email,
                PropertyOwnerContactNo = propOwn.PhoneNumber

            };
            return View(propertyDetails);
        }

        public async Task<IActionResult> Create()
        {
            var userId = User.GetUserIdAsGuid();
            var siteId = User.GetUserSiteIdAsGuid();

            //Get All Users in Property Owner Role
            //var propertyOwners = await _projectRepo.getAllPropertyOwners();
            var propertyOwnerRole = await _appRoleRepo.GetPropertyOwnerRoleId(siteId);
            var propertyOwnerRoleId = propertyOwnerRole.Id;
            var propertyOwners = await _appUserRoleRepo.GetAllPropertyOwners(propertyOwnerRoleId);
          //  var propertyOwnersList = propertyOwners.ToList();
            List<PropertyOwnerDDListViewModel> propertyOwnerSelectList = new List<PropertyOwnerDDListViewModel> { };
            foreach (var propertyOwner in propertyOwners)
            {
                var propertyOwnerId = propertyOwner.UserId;
                var propertyOwnerItem = await _siteUserManager.FindByIdAsync(propertyOwnerId.ToString());
                var propOwnerListItem = new PropertyOwnerDDListViewModel
                {
                    PropertyOwnerId = propertyOwnerId.ToString(),
                    FirstName = propertyOwnerItem.FirstName,
                    LastName = propertyOwnerItem.LastName
                };
                propertyOwnerSelectList.Add(propOwnerListItem);
            }
           
                //Put Property Owners in a Select List.
                //set up config for this , include correct packages.
                try
            { 
            if (User.IsInRole("Draughtsman")) {
                
                var userEmail = User.GetEmail();
                var userDisplayName = User.GetEmail();

                /*KEEPING THIS HERE .. This snippet will give you all clients with properties 
                 * COULD BE HANDY FOR YOU FICK If the two ever diverge - doubt it backup if the other code fails
                 * var PropertyOwnerProperties = await _repo.GetAll();
                var PropertyOwnerPropertiesList = PropertyOwnerProperties.ToList();*/

                var propertyOwnerSelect = propertyOwnerSelectList.Select(n => new SelectListItem
                {
                    Text = n.FirstName + n.LastName,
                    Value = n.PropertyOwnerId //check the conversion back to GUID below
                }) ;
                var createmodel = new CreatePropertyViewModel
                {
                    PropertyOwners = propertyOwnerSelect
                };
                return View(createmodel);
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
        public async Task<ActionResult> Create(PropertyViewModel entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(entity);
                }
                /*  var NewProperty = new Property
                  {
                      PropertyOwnerId = Guid.Parse(entity.PropertyOwnerId);
                  };*/
                //string siteid = "1732d901-82c3-4c48-9e02-3049c8ea2738";
                var siteid = User.GetUserSiteIdAsGuid().ToString();

                var property = _mapper.Map<Property>(entity);
                property.SiteId = siteid;

                //client.DateCreated = DateTime.Now;
                var isSuccess = await _repo.Add(property);
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

        public async Task<IActionResult> PropertyOwnerProperty(string id)
        {
            try { 
            if (User.IsInRole("Property Owner"))
            {
                var userId = User.GetUserId();
                var userEmail = User.GetEmail();
                var userDisplayName = User.GetEmail();
                var siteId = User.GetUserSiteIdAsGuid();
                var PropertyOwnerProperties = await _repo.GetAllByPropertyOwnerId(id); //ICollection<Property>
                var PropertyOwnerPropertiesListPre = PropertyOwnerProperties.ToList();
                var PropertyOwnerPropertiesList = _mapper.Map<List<Property>, List<PropertyViewModel>>(PropertyOwnerPropertiesListPre);
                //Check Here ^If weird property error. 
                var clientPropertyView = new PropertyOwnerPropertyViewModel
                {
                    PropertyOwnerId = userId,
                    PropertyOwnerEmail = userEmail,
                    PropertyOwnerDisplayName = userDisplayName,
                    PropertyownerSiteId = siteId,
                    Properties = PropertyOwnerPropertiesList,
                };
                return View(clientPropertyView);
            }
            }
            catch
            {
                throw new AccessViolationException("User Not a Poperty Owner");
            }
 
            return RedirectToAction("Index", "Home" , new { id });


        }

        //GET : Property/Edit/id
        public async Task<IActionResult> Edit(int id)
        {
            var property = await _repo.Get(id);
            var model = _mapper.Map<PropertyViewModel>(property);

            var siteId = User.GetUserSiteIdAsGuid();
            var propertyOwnerRole = await _appRoleRepo.GetPropertyOwnerRoleId(siteId);
            var propertyOwnerRoleId = propertyOwnerRole.Id;
            var propertyOwners = await _appUserRoleRepo.GetAllPropertyOwners(propertyOwnerRoleId);
            //var propertyOwners = await _appUserRoleRepo.GetAllPropertyOwners();
            List<PropertyOwnerDDListViewModel> propertyOwnerSelectList = new List<PropertyOwnerDDListViewModel> { };
            foreach (var propertyOwner in propertyOwners)
            {
                var propertyOwnerId = propertyOwner.UserId;
                var propertyOwnerItem = await _siteUserManager.FindByIdAsync(propertyOwnerId.ToString());
                var propOwnerListItem = new PropertyOwnerDDListViewModel
                {
                    PropertyOwnerId = propertyOwnerId.ToString(),
                    FirstName = propertyOwnerItem.FirstName,
                    LastName = propertyOwnerItem.LastName
                };
                propertyOwnerSelectList.Add(propOwnerListItem);
            }

            //Put Property Owners in a Select List.
            //set up config for this , include correct packages.
            try
            {
                if (User.IsInRole("Draughtsman"))
                {

                    var userEmail = User.GetEmail();
                    var userDisplayName = User.GetEmail();
                 //   var siteId = User.GetUserSiteIdAsGuid();

                    /*KEEPING THIS HERE .. This snippet will give you all clients with properties 
                     * COULD BE HANDY FOR YOU FICK If the two ever diverge - doubt it backup if the other code fails
                     * var PropertyOwnerProperties = await _repo.GetAll();
                    var PropertyOwnerPropertiesList = PropertyOwnerProperties.ToList();*/

                    var propertyOwnerSelect = propertyOwnerSelectList.Select(n => new SelectListItem
                    {
                        Text = n.FirstName + n.LastName,
                        Value = n.PropertyOwnerId //check the conversion back to GUID below
                    });
                    var createmodel = new CreatePropertyViewModel
                    {
                        PropertyId = model.PropertyId,
                        PropertyName = model.PropertyName,
                        PropertyAddress = model.PropertyAddress,
                        PropertySGNo = model.PropertySGNo,
                        PropertyERF_LotNo = model.PropertyERF_LotNo,
                        PropertyOwnerId = model.PropertyOwnerId,
                        PropertyOwners = propertyOwnerSelect
                    };
                    return View(createmodel);
                };
             
            }
            catch
            {

            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PropertyViewModel entity)
        {
            //var userid = User.GetUserId();
            var siteId = User.GetUserSiteIdAsGuid().ToString();
            //string siteid = "1732d901-82c3-4c48-9e02-3049c8ea2738";
            if  (!ModelState.IsValid)
            {
                return View(entity);
            }                
            
            try
            {
                Property model = new Property
                {
                    PropertyId = entity.PropertyId,
                    PropertyName = entity.PropertyName,
                    PropertyERF_LotNo = entity.PropertyERF_LotNo,
                    PropertyAddress = entity.PropertyAddress,
                    PropertySGNo = entity.PropertySGNo,
                    IsComplex = entity.IsComplex,
                    IsEstate = entity.IsEstate,
                    Complex_Estate_No = entity.Complex_Estate_No,
                    PropertyOwnerId = entity.PropertyOwnerId,
                    SiteId = siteId

                };

                //var property = _mapper.Map<Property>(entity);
                var isSuccess = await _repo.Update(model);
                {
                    ModelState.AddModelError("", "Error while saving");
                }

                return RedirectToAction("Details", "Property", new { id = entity.PropertyId });
            }
            catch
            {
                return View();
            }
        }


        /*
        public async Task<IActionResult> AddProperty(int id)
        {
            var client = await _clientrepo.Get(id);
            var appuser = await _appUserRepo.Get(client.ApplicationUserId);
            var mappedAppUser = _mapper.Map<ApplicationUser, ApplicationUserViewModel>(appuser);
            var company = await _companyRepo.Get(client.CompanyId);
            var mappedCompany = _mapper.Map<Company, CompanyViewModel>(company);
            var mappedclient = _mapper.Map<Client, ClientViewModel>(client);
            mappedclient.Company = mappedCompany;
            mappedclient.ApplicationUser = mappedAppUser;

            var createmodel = new PropertyViewModel
            {
                Client = mappedclient,
                ClientId = id,
            };
            return View(createmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddProperty(PropertyViewModel entity)
        {
            var clientId = entity.ClientId;
         
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(entity);
                }
                var company = _mapper.Map<Property>(entity);
                //client.DateCreated = DateTime.Now;
                var isSuccess = await _repo.Add(company);
                if (!isSuccess)
                {
                    ModelState.AddModelError("", "Something went wrong...");
                    return await AddProperty(entity);

                }
                return RedirectToAction("Details" ,new { id = clientId });
            }
            catch
            {
                return View();
            }
        }*/
    }
}
               