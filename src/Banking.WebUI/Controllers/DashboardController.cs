﻿using Banking.Infrastructure.Repositories;
using Banking.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.WebUI.Controllers
{
    [Authorize(Policy = "Operator")]
    public class DashboardController : Controller
    {

        private readonly IQueryRepository _queryRepository;

        public DashboardController(IQueryRepository queryRepository)
        {
            _queryRepository = queryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _queryRepository.GetClients();

            return View(result.ToList());
        }


    }
}
