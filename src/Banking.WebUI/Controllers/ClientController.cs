using Banking.Core;
using Banking.Shared.Enums;
using Banking.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Banking.WebUI.Controllers
{
    public class ClientController : Controller
    {
        private readonly IFacade _facade;

        public ClientController(IFacade facade)
        {
            _facade = facade;
        }

        public async Task<IActionResult> Index()
        {
            var model = new AccountListModel(new List<AccountModel>());

            var currentClientCNP = HttpContext.User.Claims.ToList().Find(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var accounts = _facade.GetUserAccounts(currentClientCNP);

            foreach (var account in accounts)
            {
                model.Accounts.Add(
                        new AccountModel(account.Amount, account.CurrencyType, account?.AccountType?.Name));
            }

            return View(model);
        }
    }
}
