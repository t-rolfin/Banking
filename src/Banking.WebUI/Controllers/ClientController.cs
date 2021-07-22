using Banking.Core;
using Banking.Infrastructure.Repositories;
using Banking.Shared.Enums;
using Banking.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Banking.WebUI.Controllers
{
    public class ClientController : Controller
    {
        private readonly IQueryRepository _queryRepository;
        private readonly IFacade _facade;

        public ClientController(IQueryRepository queryRepository, IFacade facade)
        {
            _queryRepository = queryRepository;
            _facade = facade;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var currentClientId = HttpContext.User.Claims.ToList().Find(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var accounts = await _queryRepository.GetClientAccounts(Guid.Parse(currentClientId));

            return View(accounts);
        }

        [HttpPost]
        public async Task<IActionResult> Withdrawal(decimal value, Guid accountId, CancellationToken cancellationToken)
        {
            var clientId = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

            await _facade.Withdrawal(Guid.Parse(clientId), accountId, value, cancellationToken);

            var currentClientId = HttpContext.User.Claims.ToList().Find(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var accounts = await _queryRepository.GetClientAccounts(Guid.Parse(currentClientId));

            return PartialView("_AccountListPartial", accounts);
        }

        [HttpPost]
        public async Task<IActionResult> Deposit(decimal value, Guid accountId, CancellationToken cancellationToken)
        {
            var clientId = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

            await _facade.Deposit(Guid.Parse(clientId), accountId, value, cancellationToken);

            var currentClientId = HttpContext.User.Claims.ToList().Find(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var accounts = await _queryRepository.GetClientAccounts(Guid.Parse(currentClientId));

            return PartialView("_AccountListPartial", accounts);
        }

        [HttpGet]
        public IActionResult Withdrawal(Guid accountId)
        {
            return PartialView("_WithdrawalPartial", accountId);
        }

        [HttpGet]
        public IActionResult Deposit(Guid accountId)
        {
            return PartialView("_DepositPartial", accountId);
        }

    }
}
