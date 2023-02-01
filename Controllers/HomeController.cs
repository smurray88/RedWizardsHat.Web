using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RedWizardsHatWeb.Models;
using RedWizardsHatWeb.Models.ViewModel;
using RedWizardsHatWeb.Repository.IRepository;

namespace RedWizardsHatWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //private readonly INationalParkRepository _npRepo;
        //private readonly ITrailRepository _trailRepo;
        //private readonly IAccountRepository _accountRepo;
        //private readonly IAccountRepository _loginRepo;
        public HomeController(ILogger<HomeController> logger, ITrailRepository trailRepo, INationalParkRepository npRepo, IAccountRepository accountRepo)
        {
            _logger = logger;
            // _trailRepo = trailRepo;
            //_npRepo = npRepo;
            //_accountRepo = accountRepo;
        }

        public async Task<IActionResult> Index()
        {
            /*
            IndexVM listOfParksAndTrails = new IndexVM()
            {
                NationalParkList = await _npRepo.GetAllAsync(SD.NationalParkAPIPath, HttpContext.Session.GetString("JWToken")),
                TrailList = await _trailRepo.GetAllAsync(SD.TrailAPIPath, HttpContext.Session.GetString("JWToken"))
            };
            */
            //return View(listOfParksAndTrails);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public IActionResult Login()
        {
            User userObj = new User();
            return View(userObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User userObjInput)
        {
            /* User userObj = await _accountRepo.LoginAsync(SD.UsersAPIPath + "authenticate/", userObjInput);
             if (userObj == null || userObj.Token == null)
             {
                 return View();
             }

             var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
             identity.AddClaim(new Claim(ClaimTypes.Name, userObj.Username));
             identity.AddClaim(new Claim(ClaimTypes.Role, userObj.Role));
             var principal = new ClaimsPrincipal(identity);
             await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);//automcaticall sign the user in

             TempData["alert"] = "Welcome " + userObj.Username;

             HttpContext.Session.SetString("JWToken", userObj.Token);*/
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Register()
        {
            User userObj = new User();
            return View();
        }

        public IActionResult GameIdea()
        {
            return View();
        }

        public IActionResult FAQ()
        {
            return View();
        }
        public IActionResult News()
        {
            return View();
        }
        public IActionResult RoadMap()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User userObjInput)
        {
            /* bool isResult = await _accountRepo.RegisterAsync(SD.UsersAPIPath + "register/", userObjInput);
              if (!isResult)
              {
                  return View();
              }
              TempData["alert"] = "Registration Successful";*/
            return RedirectToAction("Login");//Need to just do the action so call action of login can do full path ~/Home/Login with redirect
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString("JWTToken", "");
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
