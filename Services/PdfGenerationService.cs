﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Barcode;
using Syncfusion.Pdf.Graphics;
using Models;
using DbModels;
using Newtonsoft.Json;
using DbContext;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class PdfGenerationService : IPdfGenerationService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<PdfGenerationService> _logger;
        private string? _dblogin;
        public PdfGenerationService(ILogger<PdfGenerationService> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;
            _dblogin = _configuration["DbLogins"];
        }

        public async Task<List<TicketsDataDbM>> ReadJsonDataAsync()
        {
            try
            {

                using (var db = csMainDbContext.DbContext(_dblogin))
                {

                    var ticket = db.TicketRequest.ToListAsync();

                    return await ticket;
                }
                ////HttpClient httpClient = _httpClientFactory.CreateClient();
                //string ticketDetailsUrl = _configuration["TicketJsonDetailsUrl"];

                //HttpResponseMessage response = await httpClient.GetAsync(ticketDetailsUrl);
                //if (!response.IsSuccessStatusCode)
                //{
                //    _logger.LogError("Error fetching JSON data. Status code: {StatusCode}", response.StatusCode);
                //    return new List<TicketHandling>();
                //}

                //string ticketDetailsJson = await response.Content.ReadAsStringAsync();

                //// Deserialize the JSON array into a list of TicketHandling objects
                //List<TicketHandling> ticketRequests = JsonConvert.DeserializeObject<List<TicketHandling>>(ticketDetailsJson);

                ////// Log the deserialized list
                ////foreach (var ticketRequest in ticketRequests)
                ////{
                ////    _logger.LogInformation("Ticket Details: {@TicketHandling}", ticketRequest);
                ////}
                //return ticketRequests;
            }
            catch (JsonSerializationException jex)
            {
                _logger.LogError(jex, "An error occurred while deserializing the JSON data.");
                throw;
            }
            catch (HttpRequestException hrex)
            {
                _logger.LogError(hrex, "An error occurred while sending the HTTP request.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                throw;
            }
        }

        public async Task CreatePdfAsync(string outputPath, TicketsDataDbM ticketData, TicketHandling ticketDetails, string backgroundImagePath)
        {
            // Initialize PDF document
            using PdfDocument document = new PdfDocument();
            PdfFont regularFont = new PdfStandardFont(PdfFontFamily.Helvetica, 10);
            PdfFont boldFont = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);

            backgroundImagePath ??= _configuration["BackgroundImagePath"];

            for (int i = 0; i < 5; i++) // Change to amount of tickets that should be created
            {
                PdfPage page = document.Pages.Add();
                float scaleFactor = Math.Min(page.GetClientSize().Width / 1024f, 1);
                PointF ticketOrigin = new PointF((page.GetClientSize().Width - (1024 * scaleFactor)) / 2, 0);

                DrawBackgroundImage(page, backgroundImagePath, ticketOrigin, scaleFactor);
                DrawTextContent(page.Graphics, ticketOrigin, scaleFactor, regularFont, boldFont, ticketData, ticketDetails);
                DrawBarcode(page, ticketOrigin, scaleFactor, ticketDetails);

                foreach (var element in ticketDetails.Elements)
                {
                    page.Graphics.DrawString(
                        element.Content,
                        regularFont,
                    PdfBrushes.Black,
                    new PointF(ticketOrigin.X + element.Position.X * scaleFactor, ticketOrigin.Y + element.Position.Y * scaleFactor)
                    );
                }

            }
            // Save and close the document
            try
            {
                await using FileStream stream = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None);
                document.Save(stream);
            }
            catch (IOException ioEx)
            {
                _logger.LogError($"IO Exception in PDF Ticket creation: {ioEx.Message}");
                throw; // Consider retry logic or alternative response
            }
            catch (UnauthorizedAccessException uaEx)
            {
                _logger.LogError($"Access Exception in PDF Ticket creation: {uaEx.Message}");
                throw; // Access issue - might not want to retry
            }
            catch (Exception ex)
            {
                _logger.LogError($"PDF Ticket creation failed: {ex.Message}");
                throw; // General exception - unexpected
            }

            _logger.LogInformation($"PDF Ticket Creation succeeded and saved to {outputPath}");
        }

        private void DrawBackgroundImage(PdfPage page, string backgroundImagePath, PointF origin, float scale)
        {
            using FileStream imageStream = new FileStream(backgroundImagePath, FileMode.Open, FileAccess.Read);
            PdfBitmap background = new PdfBitmap(imageStream);
            page.Graphics.DrawImage(background, origin.X, origin.Y, 1024 * scale, 364 * scale);
        }

        private void DrawTextContent(PdfGraphics graphics, PointF origin, float scale, PdfFont regularFont, PdfFont boldFont, TicketsDataDbM ticketData, TicketHandling ticketDetails)
        {
            // Use 'graphics' to draw strings on the PDF, adjusting positions based on 'origin' and 'scale'
            graphics.DrawString(ticketData.eMail, regularFont, PdfBrushes.Black, new PointF(origin.X + 30 * scale, origin.Y + 80 * scale));
            graphics.DrawString(ticketData.KontaktPerson, regularFont, PdfBrushes.Black, new PointF(origin.X + 30 * scale, origin.Y + 105 * scale));
            graphics.DrawString($"Webbokningsnr: {ticketData.webbkod}", regularFont, PdfBrushes.Black, new PointF(origin.X + 30 * scale, origin.Y + 130 * scale));
            graphics.DrawString($"Bokningsnr: {ticketData.BokningsNr}", regularFont, PdfBrushes.Black, new PointF(origin.X + 30 * scale, origin.Y + 155 * scale));
            //graphics.DrawString(ticketData.TicketType, regularFont, PdfBrushes.Black, new PointF(origin.X + 30 * scale, origin.Y + 180 * scale));
            graphics.DrawString(ticketData.Pris.ToString("F2"), regularFont, PdfBrushes.Black, new PointF(origin.X + 30 * scale, origin.Y + 205 * scale));
            //graphics.DrawString($"Köpdatum: {ticketData.PurchaseDate:yyyy-MM-dd}", regularFont, PdfBrushes.Black, new PointF(origin.X + 30 * scale, origin.Y + 230 * scale));
            graphics.DrawString("- Köpt biljett återlöses ej -", regularFont, PdfBrushes.Black, new PointF(origin.X + 140 * scale, origin.Y + 265 * scale));
            graphics.DrawString("Serviceavgift", regularFont, PdfBrushes.Black, new PointF(origin.X + 330 * scale, origin.Y + 180 * scale));
            graphics.DrawString(ticketData.serviceavgift1_kr.ToString("F2"), regularFont, PdfBrushes.Black, new PointF(origin.X + 400 * scale, origin.Y + 205 * scale));
            graphics.DrawString(ticketData.namn1, boldFont, PdfBrushes.Black, new PointF(origin.X + 600 * scale, origin.Y + 40 * scale));
            graphics.DrawString(ticketData.namn, regularFont, PdfBrushes.Black, new PointF(origin.X + 620 * scale, origin.Y + 70 * scale));
            graphics.DrawString(ticketData.datumStart.ToString("yyyy-MM-dd HH:mm"), regularFont, PdfBrushes.Black, new PointF(origin.X + 640 * scale, origin.Y + 150 * scale));


            // Draw "Powered by Vitec Smart Visitor System AB" text at the bottom
            string bottomTxt = "Powered by Vitec Smart Visitor System AB";
            SizeF pageSize = graphics.ClientSize; // Assuming graphics is the PdfGraphics of the page
            PdfFont bottomTxtFont = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Italic);
            SizeF bottomTxtSize = bottomTxtFont.MeasureString(bottomTxt);
            PointF bottomTxtPosition = new PointF(
                (pageSize.Width - bottomTxtSize.Width) / 2, // Centered horizontally
                pageSize.Height - bottomTxtSize.Height - 30 * scale // 30 units from the bottom
            );
            graphics.DrawString(bottomTxt, bottomTxtFont, PdfBrushes.Black, bottomTxtPosition);

            // Draw scissors line below the ticket
            DrawScissorsLine(graphics, origin, scale);
        }

        private void DrawScissorsLine(PdfGraphics graphics, PointF origin, float scale)
        {
            string scissorsLineImagePath = _configuration["ScissorsLineImagePath"];
            using FileStream scissorsImageStream = new FileStream(scissorsLineImagePath, FileMode.Open, FileAccess.Read);
            PdfBitmap scissorsLineImage = new PdfBitmap(scissorsImageStream);

            // Position for the scissors line, adjust the Y position as needed
            PointF scissorsPosition = new PointF(
                origin.X, // Aligned with the left edge of the ticket
                origin.Y + 364 * scale + 10 * scale // Just below the ticket, 10 units of space
            );
            SizeF scissorsSize = new SizeF(1024 * scale, scissorsLineImage.Height * scale); // Full width and scaled height

            graphics.DrawImage(scissorsLineImage, scissorsPosition.X, scissorsPosition.Y, scissorsSize.Width, scissorsSize.Height);
        }

        private void DrawBarcode(PdfPage page, PointF origin, float scale, TicketHandling ticketDetails)
        {
            if (ticketDetails.UseQRCode)
            {
                // Draw QR code
                PdfQRBarcode qrCode = new PdfQRBarcode();
                qrCode.Text = ticketDetails.BarcodeContent;
                qrCode.Draw(page.Graphics, new PointF(
                    ticketDetails.BarcodePosition.X * scale,
                    ticketDetails.BarcodePosition.Y * scale
                ));

            }
            else
            {
                // Draw barcode
                PdfCode39Barcode barcode = new PdfCode39Barcode();
                barcode.Text = ticketDetails.BarcodeContent;
                barcode.Draw(page.Graphics, new PointF(
                    ticketDetails.BarcodePosition.X * scale,
                    ticketDetails.BarcodePosition.Y * scale
                ));

                //PdfCode39Barcode barcode = new PdfCode39Barcode
                //{
                //    Text = ticketDetails.BookingNumber,
                //    Size = new SizeF(300 * scale, 100 * scale)
                //};

                //PointF barcodePosition = new PointF(
                //    origin.X + (1024 * scale) - (barcode.Size.Height * scale) - 80 * scale,
                //    origin.Y + 330 * scale
                //);

                //page.Graphics.Save();
                //page.Graphics.TranslateTransform(barcodePosition.X, barcodePosition.Y);
                //page.Graphics.RotateTransform(-90);
                //barcode.Draw(page.Graphics, PointF.Empty);
                //page.Graphics.Restore();
            }
        }
    }
}