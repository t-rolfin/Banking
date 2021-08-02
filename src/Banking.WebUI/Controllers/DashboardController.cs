using Banking.Core;
using Banking.Infrastructure.Repositories;
using Banking.Infrastructure.Services;
using Banking.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly IFileExportService _fileExportService;

        public DashboardController(IQueryRepository queryRepository, IFacade facade, IFileExportService fileExportService)
        {
            _queryRepository = queryRepository;
            _facade = facade;
            _fileExportService = fileExportService;
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

        [HttpGet]
        public async Task<IActionResult> TransactionsByRangeTime(Guid accountId, DateTime startDate, DateTime endDate)
        {
            var transactions = await _queryRepository.GetAccountTransactionsBetween(accountId, startDate, endDate);
            return PartialView("_TransactionListPartial", transactions);
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactionsAsPdf(Guid accountId, string dateRange)
        {
            if (string.IsNullOrEmpty(dateRange))
                return Json(new { });

            List<DateTime> dates = new();
            dateRange.Split("-").ToList().ForEach(x => dates.Add(DateTime.Parse(x)));

            var transactions = await _queryRepository.GetAccountTransactionsBetween(accountId, dates.First(), dates.Last());
            var content = await _fileExportService.GetStreamFor(transactions.Transactions);
            var contentType = "application/pdf";
            var fileName = "extras_account.pdf";

            return File(content, contentType, fileName);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(CreateAccountModel model, CancellationToken cancellationToken)
        {
            await _facade.CreateAccountFor(model.ClientId, model.AccountType, model.CurrencyType, cancellationToken);

            return RedirectPermanent($"Client/{model.ClientId}");
        }

        [HttpPost]
        public async Task<IActionResult> CloseAccount(Guid accountId, Guid clientId, CancellationToken cancellationToken)
        {
            var response = await _facade.CloseAccount(clientId, accountId, cancellationToken);
            return Json(new { IsSuccess = response });
        }

        [HttpGet]
        public  async Task<IActionResult> UpdateAccountList(Guid accountId, Guid clientId)
        {
            var accounts = await _queryRepository.GetClientAccounts(clientId);
            accounts.ClientId = clientId;
            return PartialView("_AccountListPartial", accounts);
        }
    }
}
