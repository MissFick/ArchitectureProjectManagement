using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ArchitectureProjectManagement.Contracts;
using ArchitectureProjectManagement.Data;
using ArchitectureProjectManagement.DataMapping;
using ArchitectureProjectManagement.Models;
using System.IO;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ArchitectureProjectManagement.Controllers
{
    public class ClientController : Controller
    {
        private readonly IClientRepository _repo;
        private readonly IMapper _mapper;
        private readonly IApplicationUserRepository _appUserRepo;
        private readonly ICompanyRepository _companyRepo;
        private UserManager<IdentityUser> _user;


        public ClientController(IClientRepository repo, IMapper mapper, IApplicationUserRepository applicationUserRepository, UserManager<IdentityUser> user, ICompanyRepository companyRepo)
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
            var tb_clients = await _repo.GetAll();
            var clientList = tb_clients.ToList();
            foreach (var client in clientList)
            {
                var appuserid = client.ApplicationUserId;
                var appuser = _appUserRepo.Get(appuserid);
                //client.ApplicationUser.FirstName = appuser.FirstName;
                //client.ApplicationUser.LastName = appuser.LastName;
                //client.ApplicationUser.Address = appuser.Address;
                var companyid = client.CompanyId;
                var company = _companyRepo.Get(companyid);
                //client.Company.CompanyName = company.CompanyName;
                //client.Company.CompanyContactNo = company.CompanyContactNo;
                //client.Company.CompanyEmail = company.CompanyEmail;
            }
            var model = _mapper.Map<List<Client>, List<ClientViewModel>>(clientList);
            return View(model);
        }

        //GET: Clients/Details/2
        public async Task<IActionResult> Details(int id)
        {
            var client = await _repo.Get(id);
            var mappedClient = _mapper.Map<Client, ClientViewModel>(client);
            return View(mappedClient);
        }

        //GET: Client/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateClientViewModel entity)

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
                var role = _user.AddToRoleAsync(user, "Client");
            }
            try
            {

                int companyId;
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
                var centity = new Company
                {
                    CompanyName = entity.Company.CompanyName,
                    CompanyContactNo = entity.Company.CompanyContactNo,
                    CompanyEmail = entity.Company.CompanyEmail
                };
                var newCompany = _mapper.Map<Company>(centity);

                /* using (var memoryStream = new MemoryStream())
                 {
                     entity.Company.CompanyLogo.CopyToAsync(memoryStream);
                     newCompany.CompanyLogo = memoryStream.ToArray();
                 }*/
                var isCompanySuccess = await _companyRepo.Add(newCompany);
                if (!isCompanySuccess)
                {
                    ModelState.AddModelError("", "Something went wrong...");
                    return View(entity);
                }

                companyId = await _companyRepo.FindIdByDetails(centity.CompanyName, centity.CompanyEmail);

                var clentity = new Client
                {
                    ApplicationUserId = newentity.ApplicationUserId,
                    CompanyId = companyId
                };
                var client = _mapper.Map<Client>(clentity);

                var isSuccess = await _repo.Add(clentity);
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
