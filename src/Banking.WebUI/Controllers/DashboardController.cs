﻿using Banking.Core;
using Banking.Infrastructure.Repositories;
using Banking.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Banking.WebUI.Controllers
{
    [Authorize(Policy = "Operator")]
    public class DashboardController : Controller
    {
        private readonly IQueryRepository _queryRepository;
        private readonly IFacade _facade;

        public DashboardController(IQueryRepository queryRepository, IFacade facade)
        {
            _queryRepository = queryRepository;
            _facade = facade;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var clients = await _queryRepository.GetClients();
            return View(clients.ToList());
        }

        [HttpGet]
        public async Task<IActionResult> Client(Guid id)
        {
            var accounts = await _queryRepository.GetClientAccounts(id);
            accounts.ClientId = id;
            return View(accounts);
        }

        [HttpGet]
        public async Task<IActionResult> Transactions(Guid accountId)
        {
            var transactions = await _queryRepository.GetAccountTransactions(accountId);
            transactions.AccountId = accountId;
            return View(transactions);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(CreateAccountModel model, CancellationToken cancellationToken)
        {
            await _facade.CreateAccountFor(model.ClientId, model.AccountType, model.CurrencyType, cancellationToken);

            return RedirectPermanent($"Client/{model.ClientId}");
        }

        public async Task<IActionResult> CloseAccount(Guid accountId, Guid clientId, CancellationToken cancellationToken)
        {
            var response = await _facade.CloseAccount(clientId, accountId, cancellationToken);

            var accounts = await _queryRepository.GetClientAccounts(clientId);

            return PartialView("_AccountListPartial", accounts);
        }
    }
}
