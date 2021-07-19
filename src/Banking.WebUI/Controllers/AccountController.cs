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
        public IActionResult SignIn()
        {
            var model = new SignInModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInModel model)
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
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectPermanent("/");
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LogIn(LogInModel model)
        {
            return View();
        }
    }
}
