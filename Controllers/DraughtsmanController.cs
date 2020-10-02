using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ArchitectureProjectManagement.Contracts;
using ArchitectureProjectManagement.Data;
using Microsoft.AspNetCore.Identity;
using ArchitectureProjectManagement.Models;
using System.IO;

namespace ArchitectureProjectManagement.Controllers
{
    public class DraughtsmanController : Controller
    {
        private readonly IDraughtsmanRepository _repo;
        private readonly IMapper _mapper;
        private readonly IApplicationUserRepository _appUserRepo;
        private UserManager<IdentityUser> _user;
        private readonly ICompanyRepository _companyRepo;


        public DraughtsmanController(IDraughtsmanRepository repo, IMapper mapper, IApplicationUserRepository applicationUserRepository, UserManager<IdentityUser> user, ICompanyRepository companyRepo)
        {
            _repo = repo;
            _appUserRepo = applicationUserRepository;
            _mapper = mapper;
            _user = user;
            _companyRepo = companyRepo;
           
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var draughtsmen = await _repo.GetAll();
            var draftList = draughtsmen.ToList();
            foreach (var draughtsman in draftList)
            {
                var appuserid = draughtsman.ApplicationUserId;
                var appuser = _appUserRepo.Get(appuserid);
               // draughtsman.ApplicationUser.FirstName = appuser.FirstName;
               // draughtsman.ApplicationUser.LastName = appuser.LastName;
               // draughtsman.ApplicationUser.Address = appuser.Address;
                var companyid = draughtsman.CompanyId;
                var company = _companyRepo.Get(companyid);
               // draughtsman.Company.CompanyName = company.CompanyName;
               // draughtsman.Company.CompanyContactNo = company.CompanyContactNo;
               // draughtsman.Company.CompanyEmail = company.CompanyEmail;
               
            }
             var model = _mapper.Map<List<Draughtsman>, List<DraughtsmanViewModel>>(draughtsmen.ToList());
            /*foreach (var draughtsmanVM in model)
            {
                byte[] logo = draughtsmanVM.Company.CompanyLogo;
                Image 
                using (MemoryStream memoryStream = new MemoryStream(draughtsmanVM.Company.CompanyLogo, 0 , draughtsmanVM.Company.CompanyLogo.Length))
                {

                    draughtsmanVM.Company.CompanyLogo = Image.FromStream.memoryStream;
                }
            }*/
            
            return View(model);
        }

        //GET: Draughtsman/Details/2
        public IActionResult Details(int id)
        {
            return View();
        }

        //GET: Draughtsman/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateDraughtsmanViewModel entity)
        {
            try
            {

                if (_user.FindByNameAsync(entity.ApplicationUser.IdentityUser.UserName).Result == null)
                {
                    var email = entity.ApplicationUser.IdentityUser.UserName;
                    var user = new IdentityUser
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true
                    };
                    var result = _user.CreateAsync(user, entity.Password).Result;
                    var role = _user.AddToRoleAsync(user, "Draughtsman");
                }
            

                int companyId;
                var newuser = _user.FindByNameAsync(entity.ApplicationUser.IdentityUser.UserName).Result;
                var newentity = new ApplicationUser
                {
                    FirstName = entity.ApplicationUser.FirstName,
                    LastName = entity.ApplicationUser.LastName,
                    Address = entity.ApplicationUser.Address,
                    Id = _user.FindByNameAsync(entity.ApplicationUser.IdentityUser.UserName).Result.Id,
                };
                if (!ModelState.IsValid)
                {
                    return View(newentity);
                }
                var applicationUser = _mapper.Map<ApplicationUser>(newentity);
                //applicationUser.DateCreated = DateTime.Now;
                
                var isAppUserSuccess = await _appUserRepo.Add(applicationUser);
                if (!isAppUserSuccess)
                {
                    ModelState.AddModelError("", "Something went wrong...");
                    return View(entity);
                }
                if (!ModelState.IsValid)
                {
                    return View(entity);
                }
                
                var centity = new CreateCompanyViewModel
                {
                     CompanyName = entity.Company.CompanyName,
                     CompanyContactNo = entity.Company.CompanyContactNo,
                     CompanyEmail = entity.Company.CompanyEmail
                };
                var newCompany = _mapper.Map<Company>(centity);
                using (var memoryStream = new MemoryStream())
                {
                    await centity.CompanyLogo.CopyToAsync(memoryStream);
                    newCompany.CompanyLogo = memoryStream.ToArray();
                }
                var exists = await _companyRepo.DoesExist(centity.CompanyEmail);
                if (!exists)
                {
                    await _companyRepo.Add(newCompany);
                }
                
                if (!ModelState.IsValid)
                {
                    return View(entity);
                }
                companyId = await _companyRepo.FindIdByDetails(centity.CompanyName, centity.CompanyEmail);
                
                var dentity = new Draughtsman
                {
                    DraughtsmanRegNo = entity.DraughtsmanRegNo,
                    ApplicationUserId = newentity.ApplicationUserId,
                    CompanyId = companyId
                };
                var draughtsman = _mapper.Map<Draughtsman>(dentity);
                
                var isSuccess = await _repo.Add(draughtsman);
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

        //GET: Draughtsman/Edit
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
