using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ArchitectureProjectManagement.Contracts;
using ArchitectureProjectManagement.Data;
using ArchitectureProjectManagement.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ArchitectureProjectManagement.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyRepository _repo;
        private readonly IMapper _mapper;

        public CompanyController(ICompanyRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var companies = await _repo.GetAll();
            var model = _mapper.Map<List<Company>, List<CompanyViewModel>>(companies.ToList());
            return View(model);
        }

        //GET: Company/Details/2
        public IActionResult Details(int id)
        {
            return View();
        }

        //GET: Company/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateCompanyViewModel entity)
        {
           // ViewData["ReturnUrl"] = returnUrl;
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(entity);
                }
                /*   var companyObject = new CompanyViewModel
                   {
                       CompanyName = entity.CompanyName,
                       CompanyEmail = entity.CompanyEmail,
                       CompanyContactNo = entity.CompanyContactNo,
                   };*/
                var company = _mapper.Map<Company>(entity);
                using (var memoryStream = new MemoryStream())
                {
                    await entity.CompanyLogo.CopyToAsync(memoryStream);
                    company.CompanyLogo = memoryStream.ToArray();
                }
                //client.DateCreated = DateTime.Now;
                var isSuccess = await _repo.Add(company);
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

        //GET: Company/Edit
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
                return RedirectToAction(nameof(Index));
                //TO DO Add Delete Logic Here
            }
            catch
            {

            }



            {
                return View();
            }
        }
    }
}