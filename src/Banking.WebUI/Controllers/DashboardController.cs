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
            var fileName = "extras_account.pdf";

            Guid handle = Guid.NewGuid();
            TempData[handle.ToString()] = content;

            return new JsonResult(new { FileGuid = handle, FileName = fileName });
        }

        [HttpGet]
        public  async Task<IActionResult> GetSelectedTransactionsAsPdf(Guid accountId, string transactionsIds)
        {
            var transactions = await _queryRepository.GetAccountTransactionsByIds(accountId, transactionsIds);
            var content = await _fileExportService.GetStreamFor(transactions.Transactions);
            Guid handle = Guid.NewGuid();
            TempData[handle.ToString()] = content;

            var fileName = "extras_account.pdf";

            return new JsonResult(new { FileGuid = handle, FileName = fileName } );
        }

        public virtual IActionResult DownlaodPdf(string fileGuid, string fileName)
        {
            if (TempData[fileGuid] != null)
            {
                var contentType = "application/pdf";
                byte[] data = TempData[fileGuid] as byte[];
                return File(data, contentType, fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                return new EmptyResult();
            }
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

        [HttpGet]
        public async Task<IActionResult> SearchByName(string searchedName)
        {
            var clients = await _queryRepository.GetClientsByName(searchedName);
            return PartialView("_ClientListPartial", clients);
        }

        [HttpGet]
        public async Task<IActionResult> SortingBy(string searchedName, string sortingType)
        {
            IEnumerable<ClientModel> clients;
            if (sortingType.Contains("name"))
            {
                var orderType = sortingType.Split("_")[1];
                clients = await _queryRepository.GetClientsSortedByName(searchedName, orderType);
            }
            else if(sortingType.Contains("amount"))
            {
                var orderType = sortingType.Split("_")[1];
                clients = await _queryRepository.GetClientsSortedByAmount(searchedName, orderType);
            }
            else
            {
                clients = await _queryRepository.GetClientsByName(searchedName);
            }

            return PartialView("_ClientListPartial", clients);
        }
    }
}
