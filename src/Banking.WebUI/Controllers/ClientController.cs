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
            var clientId = GetCurrentClientId();
            var accounts = await _queryRepository.GetClientAccounts(clientId);
            return View(accounts);
        }

        [HttpPost]
        public async Task<IActionResult> Withdrawal(decimal value, Guid accountId, CancellationToken cancellationToken)
        {
            var clientId = GetCurrentClientId();
            await _facade.Withdrawal(clientId, accountId, value, cancellationToken);
            return await GetAccountForCurrentClient(clientId);
        }


        [HttpPost]
        public async Task<IActionResult> Deposit(decimal value, Guid accountId, CancellationToken cancellationToken)
        {
            Guid clientId = GetCurrentClientId();
            await _facade.Deposit(clientId, accountId, value, cancellationToken);
            return await GetAccountForCurrentClient(clientId);
        }


        [HttpPost]
        public async Task<IActionResult> Transfer(Guid accountId, string destinationAccountIBAN, decimal value, CancellationToken cancellationToken)
        {
            await _facade.Transfer(accountId, destinationAccountIBAN, value, cancellationToken);
            var clientId = GetCurrentClientId();
            return await GetAccountForCurrentClient(clientId);
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

        [HttpGet]
        public IActionResult Transfer(Guid accountId)
        {
            return PartialView("_TransferPartial", accountId);
        }



        private async Task<IActionResult> GetAccountForCurrentClient(Guid clientId)
        {
            var accounts = await _queryRepository.GetClientAccounts(clientId);
            accounts.ClientId = clientId;
            return PartialView("_AccountListPartial", accounts);
        }
        private Guid GetCurrentClientId()
        {
            var clientId = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            Guid clientIdAdGuid = Guid.Parse(clientId);
            return clientIdAdGuid;
        }
    }
}
