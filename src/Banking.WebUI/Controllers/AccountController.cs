using Microsoft.AspNetCore.Mvc;
using Banking.Shared.Models;
using Banking.Core;

namespace Banking.WebUI.Controllers
{
    public class AccountController : Controller
    {

        [HttpGet]
        public IActionResult SignIn()
        {
            var model = new SignInModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult SignIn(SignInModel model)
        {
            if (!ModelState.IsValid)
                return View();


            return View();
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }
    }
}
