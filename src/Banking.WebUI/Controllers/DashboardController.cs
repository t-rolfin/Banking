using Banking.Infrastructure.Repositories;
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

        [HttpGet]
        public async Task<IActionResult> Client(Guid id)
        {
            ViewData["clientId"] = id;
            var accounts = await _queryRepository.GetClientAccounts(id);
            return View(accounts);
        }

        [HttpGet]
        public async Task<IActionResult> Transactions(Guid accountId)
        {
            var transactions = await _queryRepository.GetAccountTransactions(accountId);
            return View(transactions);
        }
    }
}
