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
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return View();

            var result = _facade.RegisterClient(
                model.CNP,
                model.PIN,
                model.FirstName,
                model.LastName,
                model.Address,
                model.AccountType,
                model.CurrencyType);
            

            if (result)
            {
                var claims = new[] {
                        new Claim(ClaimTypes.NameIdentifier, model.CNP.ToString()),
                        new Claim(ClaimTypes.Name, model.FirstName),
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
                var result = _facade.IdentifyClient(model.CNP, model.PIN);
                
                if(result)
                {
                    var claims = new[] {
                        new Claim(ClaimTypes.NameIdentifier, model.CNP.ToString()),
                        new Claim(ClaimTypes.Name, model.CNP),
                    };

                    var identity = new ClaimsIdentity(claims, "LogIn");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                    return string.IsNullOrWhiteSpace(returnUrl) ? RedirectPermanent("/") : RedirectPermanent(returnUrl);
                }
            }

            return View();
        }
    }
}
