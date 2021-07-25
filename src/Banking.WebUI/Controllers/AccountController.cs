using Microsoft.AspNetCore.Mvc;
using Banking.Shared.Models;
using Banking.Core;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;

namespace Banking.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IFacade _facade;

        public AccountController(IFacade facade)
        {
            _facade = facade ?? throw new ArgumentNullException();
        }

        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegisterModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View();

            var client = await _facade.RegisterClient(
                model.CNP,
                model.PIN,
                model.FirstName,
                model.LastName,
                model.Address,
                model.AccountType,
                model.CurrencyType,
                cancellationToken);
            

            if (client is not null)
            {
                var claims = new[] {
                        new Claim(ClaimTypes.NameIdentifier, client.Id.ToString()),
                        new Claim(ClaimTypes.Name, client.GetFullName()),
                    };

                var identity = new ClaimsIdentity(claims, "LogIn");

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                return RedirectPermanent("/");
            }
            else
                return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectPermanent("/");
        }

        [HttpGet]
        public IActionResult LogIn(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn(LogInModel model, string returnUrl = null)
        {
            if(ModelState.IsValid)
            {
                var client = await _facade.IdentifyClient(model.CNP, model.PIN);
                
                if(client is not null)
                {
                    var claims = new[] {
                        new Claim(ClaimTypes.NameIdentifier, client.Id.ToString()),
                        new Claim(ClaimTypes.Name, client.GetFullName()),
                    };

                    var identity = new ClaimsIdentity(claims, "LogIn");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                    return string.IsNullOrWhiteSpace(returnUrl) ? RedirectPermanent("/") : RedirectPermanent(returnUrl);
                }
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult OperatorLogIn(OperatorLoginModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                return View();
            }
            else
            {
                return View();
            }
        }

        public IActionResult ClientLogIn()
        {
            return PartialView("_LogInClientPartial", new LogInModel(null, null));
        }

        public IActionResult OperatorLogIn()
        {
            return PartialView("_LogInOperatorPartial", new OperatorLoginModel());
        }
    }
}
