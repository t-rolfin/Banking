using System.Collections.Generic;
using Syncfusion.Pdf.Graphics;
using System.Threading.Tasks;
using Banking.Shared.Models;
using Banking.Shared.Enums;
using Syncfusion.Pdf.Grid;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using System.IO;
using System;

namespace Banking.Infrastructure.Services
{
    public class FileExportService : IFileExportService
    {
        public async Task<byte[]> GetStreamFor(IEnumerable<TransactionModel> transactions)
        {
            PdfDocument pdfDocument = new PdfDocument();
            PdfPage pdfPage = pdfDocument.Pages.Add();

            PdfGraphics graphics = pdfPage.Graphics;
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 10);
            PdfFont fontName = new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Bold);
            PdfFont fontHeader = new PdfStandardFont(PdfFontFamily.Helvetica, 24);

            graphics.DrawString("Extras Cont", fontHeader, PdfBrushes.Black, new PointF(200, 0));

            graphics.DrawString("Nume: ", fontName, PdfBrushes.Black, new PointF(0, 30));
            graphics.DrawString("Adresa: ", font, PdfBrushes.Black, new PointF(0, 45));
            graphics.DrawString("CNP: ", font, PdfBrushes.Black, new PointF(0, 60));
            graphics.DrawString("Valuta: ", font, PdfBrushes.Black, new PointF(0, 75));
            graphics.DrawString($"Data Extras: { DateTime.UtcNow.ToShortDateString() }", font, PdfBrushes.Black, new PointF(0, 90));
            PdfGrid pdfGrid = new PdfGrid();

            pdfGrid.Columns.Add(4);
            pdfGrid.Headers.Add(1);
            PdfGridRow pdfGridHeader = pdfGrid.Headers[0];
            pdfGridHeader.Cells[0].Value = "Date";
            pdfGridHeader.Cells[1].Value = "Destination";
            pdfGridHeader.Cells[2].Value = "Transaction Type";
            pdfGridHeader.Cells[3].Value = "Amount";

            pdfGridHeader.ApplyStyle(new PdfGridCellStyle() { 
                BackgroundBrush = PdfBrushes.DarkBlue, 
                Borders = new PdfBorders() { All = new PdfPen(Color.Transparent), Right = new PdfPen(Color.White, 3) },
                TextBrush = PdfBrushes.White,
                StringFormat = new PdfStringFormat(PdfTextAlignment.Center),
                CellPadding = new PdfPaddings() { Top = 5, Bottom = 5 },
                Font = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold)
            });

            foreach (var transaction in transactions)
            {
                PdfGridRow pdfGridRow = pdfGrid.Rows.Add();
                pdfGridRow.Cells[0].Value = transaction.Date.ToShortDateString();
                pdfGridRow.Cells[1].Value = transaction.DestinationAccountId.ToString();
                pdfGridRow.Cells[2].Value = ((OperationType)transaction.TransactionType).ToString();
                pdfGridRow.Cells[3].Value = $"{transaction.Amount} {transaction.CurrencyType}";
                pdfGridRow.Height = 30;

                pdfGridRow.ApplyStyle(new PdfGridCellStyle()
                {
                    Borders = new PdfBorders() {  All = new PdfPen(Color.Transparent, 0) },
                    StringFormat = new PdfStringFormat() { Alignment = PdfTextAlignment.Center},
                    Font = new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Bold)
                });
            }


            pdfGrid.Draw(pdfPage, new Point(0, 120));
            MemoryStream stream = new MemoryStream();
            pdfDocument.Save(stream);
            pdfDocument.Close(true);

            return await Task.FromResult(stream.ToArray());
        }
    }
}
