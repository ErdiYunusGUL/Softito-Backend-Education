using CineCompare.Core.Entities;
using CineCompare.Core.Interfaces;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;
using System.Threading.Tasks;

namespace CineCompare.Web.Controllers
{
    public class ReportController : Controller
    {
        private readonly IGenericRepository<Movie> _movieRepository;

        public ReportController(IGenericRepository<Movie> movieRepository)
        {
            _movieRepository = movieRepository;

            // QuestPDF kütüphanesinin ücretsiz/öğrenci versiyonunu kullandığımızı belirtiyoruz
            QuestPDF.Settings.License = LicenseType.Community;
        }

        // --- 1. EXCEL ÇIKTISI ---
        public async Task<IActionResult> Excel()
        {
            var movies = await _movieRepository.GetAllAsync();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Film Arşivi");

            // Excel Başlıkları (1. Satır)
            worksheet.Cell(1, 1).Value = "ID";
            worksheet.Cell(1, 2).Value = "Film Adı";
            worksheet.Cell(1, 3).Value = "Tür";
            worksheet.Cell(1, 4).Value = "Süre (Dk)";

            // Başlıkları kalın yapıp arka planı gri yapalım
            var headerRow = worksheet.Row(1);
            headerRow.Style.Font.Bold = true;
            headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;

            // Verileri Excel'e Basıyoruz
            int currentRow = 2;
            foreach (var movie in movies)
            {
                worksheet.Cell(currentRow, 1).Value = movie.Id;
                worksheet.Cell(currentRow, 2).Value = movie.Title;
                worksheet.Cell(currentRow, 3).Value = movie.Genre;
                worksheet.Cell(currentRow, 4).Value = movie.DurationInMinutes;
                currentRow++;
            }

            // Sütun genişliklerini içeriğe göre otomatik ayarla
            worksheet.Columns().AdjustToContents();

            // Dosyayı oluştur ve kullanıcıya indir
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "FilmArsivi.xlsx");
        }

        // --- 2. PDF ÇIKTISI ---
        public async Task<IActionResult> Pdf()
        {
            var movies = await _movieRepository.GetAllAsync();

            // QuestPDF ile sıfırdan PDF çiziyoruz
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    // PDF Başlığı
                    page.Header()
                        .Text("CineCompare - Film Arşivi Raporu")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Darken2);

                    // PDF İçeriği (Tablo)
                    page.Content().PaddingVertical(1, Unit.Centimetre).Table(table =>
                    {
                        // 4 Sütun Tanımlıyoruz
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(50); // ID için sabit genişlik
                            columns.RelativeColumn();   // Film adı için esnek genişlik
                            columns.RelativeColumn();   // Tür için esnek genişlik
                            columns.ConstantColumn(80); // Süre için sabit genişlik
                        });

                        // Tablo Başlıkları
                        table.Header(header =>
                        {
                            header.Cell().BorderBottom(1).Padding(2).Text("ID").SemiBold();
                            header.Cell().BorderBottom(1).Padding(2).Text("Film Adı").SemiBold();
                            header.Cell().BorderBottom(1).Padding(2).Text("Tür").SemiBold();
                            header.Cell().BorderBottom(1).Padding(2).Text("Süre (Dk)").SemiBold();
                        });

                        // Tablo Satırları (Veritabanından Gelenler)
                        foreach (var movie in movies)
                        {
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(2).Text(movie.Id.ToString());
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(2).Text(movie.Title);
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(2).Text(movie.Genre);
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(2).Text($"{movie.DurationInMinutes} Dk");
                        }
                    });

                    // Sayfa Altı (Footer) - Sayfa Numarası
                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Sayfa ");
                            x.CurrentPageNumber();
                            x.Span(" / ");
                            x.TotalPages();
                        });
                });
            });

            // PDF'i belleğe yaz ve indir
            using var stream = new MemoryStream();
            document.GeneratePdf(stream);
            var content = stream.ToArray();

            return File(content, "application/pdf", "FilmArsivi.pdf");
        }
    }
}