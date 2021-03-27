using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ArchitectureProjectManagement.Contracts;
using AutoMapper;
using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Core.Web.ViewModels.Account;
using cloudscribe.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using MimeKit;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ArchitectureProjectManagement.Controllers
{
   
    public class EmailController : Controller
    {
        private readonly IProjectItemStatusRepository _repo;
        private readonly IProjectRepository _projectrepo;
        private readonly IProjectItemRepository _itemrepo;
        private readonly SiteUserManager<SiteUser> _siteUserManager;
        private readonly SignInManager<SiteUser> _signInManager;
        private readonly IEmailSender _emailSender;
        //private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;
        private readonly string _externalCookieScheme;
        private IWebHostEnvironment _env;

        public EmailController(IProjectItemStatusRepository repo,
            IMapper mapper,
            IProjectRepository projectrepo,
            IProjectItemRepository itemrepo,
            SiteUserManager<SiteUser> siteUserManager,
            IWebHostEnvironment env)
        {
            _projectrepo = projectrepo;
            _itemrepo = itemrepo;
            _siteUserManager = siteUserManager;
            _env = env;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmailClient(RegisterViewModel model, string returnUrl = null)
        {

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new SiteUser { UserName = model.Email, Email = model.Email };
                var result = await _siteUserManager.CreateAsync(user);
               // var result = await _siteUserManager.CreateAsync(user, model.Password);

                // var user = await _userManager.FindByEmailAsync(model.Email);  
                if (result.Succeeded)
                {
                    // Send an email with this link  
                    var code = await _siteUserManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(nameof(EmailClient), "Email", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    //Email from Email Template  
                    string Message = "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>";
                    // string body;  

                    var webRoot = _env.WebRootPath; //get wwwroot Folder  

                    //Get TemplateFile located at wwwroot/Templates/EmailTemplate/Register_EmailTemplate.html  
                    var pathToFile = _env.WebRootPath
                            + Path.DirectorySeparatorChar.ToString()
                            + "Templates"
                            + Path.DirectorySeparatorChar.ToString()
                            + "EmailTemplate"
                            + Path.DirectorySeparatorChar.ToString()
                            + "Confirm_Account_Registration.html";

                    var subject = "Confirm Account Registration";

                    var builder = new BodyBuilder();
                    using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                    {
                        builder.HtmlBody = SourceReader.ReadToEnd();
                    }
                    //{0} : Subject  
                    //{1} : DateTime  
                    //{2} : Email  
                    //{3} : Username  
                    //{4} : Password  
                    //{5} : Message  
                    //{6} : callbackURL  

                    string messageBody = string.Format(builder.HtmlBody,
                        subject,
                        String.Format("{0:dddd, d MMMM yyyy}", DateTime.Now),
                        model.Email,
                        model.Email,
                        model.Password,
                        Message,
                        callbackUrl
                        );


                    await _emailSender.SendEmailAsync("test@test.com", model.Email, model.Email, subject, messageBody);

                    ViewData["Message"] = $"Please confirm your account by clicking this link: <a href='{callbackUrl}' class='btn btn-primary'>Confirmation Link</a>";
                    ViewData["MessageValue"] = "1";

                    _logger.LogInformation(3, "User created a new account with password.");
                    return LocalRedirect(returnUrl);
                }
                ViewData["Message"] = $"Error creating user. Please try again later";
                ViewData["MessageValue"] = "0";
               // AddErrors(result);
            }

            // If we got this far, something failed, redisplay form  
            return View(model);
        }
    }
}
