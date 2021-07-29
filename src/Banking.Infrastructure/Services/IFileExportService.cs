using Banking.Core.Entities;
using Banking.Shared.Models;
using Syncfusion.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Infrastructure.Services
{
    public interface IFileExportService
    {
        Task<byte[]> GetStreamFor(IEnumerable<TransactionModel> transactions);
    }
}
