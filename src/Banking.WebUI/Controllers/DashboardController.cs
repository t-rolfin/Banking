using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.WebUI.Controllers
{
    public class DashboardController : Controller
    {
        [Authorize(Policy = "Operator")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
