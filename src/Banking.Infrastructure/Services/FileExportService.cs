using Banking.Core.Entities;
using Banking.Shared.Enums;
using Banking.Shared.Models;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Grid;
using Syncfusion.Pdf.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Infrastructure.Services
{
    public class FileExportService : IFileExportService
    {
        public async Task<byte[]> GetStreamFor(IEnumerable<TransactionModel> transactions)
        {
            //Create a new PDF document.

            PdfDocument doc = new PdfDocument();

            //Add a page.

            PdfPage page = doc.Pages.Add();

            // Create a PdfLightTable.

            PdfGrid pdfGrid = new PdfGrid();

            // Initialize DataTable to assign as DateSource to the light table.

            DataTable table = new DataTable();

            //Include columns to the DataTable.

            table.Columns.Add("Date");

            table.Columns.Add("Transaction Type");

            table.Columns.Add("Destination");

            table.Columns.Add("Amount");


            //Include rows to the DataTable.
            foreach (var transaction in transactions)
            {
                table.Rows.Add(new string[] {
                    transaction.Date.ToShortDateString(),
                    ((OperationType)transaction.TransactionType).ToString(),
                    transaction.DestinationAccountId.ToString(),
                    transaction.Amount.ToString()
                });
            }

            //Assign data source.

            pdfGrid.DataSource = table;
            pdfGrid.Style = new PdfGridStyle() { CellPadding = new PdfPaddings(5, 5, 5, 5) };
            //Draw PdfLightTable.

            pdfGrid.Draw(page, new PointF(0, 0));

            //Save the document.

            MemoryStream stream = new MemoryStream();
            doc.Save(stream);

            //Close the document

            doc.Close(true);

            return await Task.FromResult(stream.ToArray());
        }
    }
}
