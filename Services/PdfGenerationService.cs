using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Barcode;
using Syncfusion.Pdf.Graphics;
using Models;

namespace Services
{
    public class PdfGenerationService : IPdfGenerationService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<PdfGenerationService> _logger;

        public PdfGenerationService(ILogger<PdfGenerationService> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public async Task CreatePdfAsync(string outputPath, TicketRequest ticketDetails, string backgroundImagePath)
        {
            // Initialize PDF document
            using PdfDocument document = new PdfDocument();
            PdfFont regularFont = new PdfStandardFont(PdfFontFamily.Helvetica, 10);
            PdfFont boldFont = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);

            backgroundImagePath ??= _configuration["BackgroundImagePath"];

            for (int i = 0; i < 5; i++)
            {
                PdfPage page = document.Pages.Add();
                float scaleFactor = Math.Min(page.GetClientSize().Width / 1024f, 1);
                PointF ticketOrigin = new PointF((page.GetClientSize().Width - (1024 * scaleFactor)) / 2, 0);

                DrawBackgroundImage(page, backgroundImagePath, ticketOrigin, scaleFactor);
                DrawTextContent(page.Graphics, ticketOrigin, scaleFactor, regularFont, boldFont, ticketDetails);
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

        private void DrawTextContent(PdfGraphics graphics, PointF origin, float scale, PdfFont regularFont, PdfFont boldFont, TicketRequest ticketRequests)
        {
            // Use 'graphics' to draw strings on the PDF, adjusting positions based on 'origin' and 'scale'
            graphics.DrawString(ticketRequests.Email, regularFont, PdfBrushes.Black, new PointF(origin.X + 30 * scale, origin.Y + 80 * scale));
            graphics.DrawString(ticketRequests.Name, regularFont, PdfBrushes.Black, new PointF(origin.X + 30 * scale, origin.Y + 105 * scale));
            graphics.DrawString($"Webbokningsnr: {ticketRequests.WebBookingNumber}", regularFont, PdfBrushes.Black, new PointF(origin.X + 30 * scale, origin.Y + 130 * scale));
            graphics.DrawString($"Bokningsnr: {ticketRequests.BookingNumber}", regularFont, PdfBrushes.Black, new PointF(origin.X + 30 * scale, origin.Y + 155 * scale));
            graphics.DrawString(ticketRequests.TicketType, regularFont, PdfBrushes.Black, new PointF(origin.X + 30 * scale, origin.Y + 180 * scale));
            graphics.DrawString(ticketRequests.Price.ToString("F2"), regularFont, PdfBrushes.Black, new PointF(origin.X + 30 * scale, origin.Y + 205 * scale));
            graphics.DrawString($"Köpdatum: {ticketRequests.PurchaseDate:yyyy-MM-dd}", regularFont, PdfBrushes.Black, new PointF(origin.X + 30 * scale, origin.Y + 230 * scale));
            graphics.DrawString("- Köpt biljett återlöses ej -", regularFont, PdfBrushes.Black, new PointF(origin.X + 140 * scale, origin.Y + 265 * scale));
            graphics.DrawString("Serviceavgift", regularFont, PdfBrushes.Black, new PointF(origin.X + 330 * scale, origin.Y + 180 * scale));
            graphics.DrawString(ticketRequests.ServiceFee.ToString("F2"), regularFont, PdfBrushes.Black, new PointF(origin.X + 400 * scale, origin.Y + 205 * scale));
            graphics.DrawString(ticketRequests.EventName, boldFont, PdfBrushes.Black, new PointF(origin.X + 600 * scale, origin.Y + 40 * scale));
            graphics.DrawString(ticketRequests.EventNameEnglish, regularFont, PdfBrushes.Black, new PointF(origin.X + 620 * scale, origin.Y + 70 * scale));
            graphics.DrawString(ticketRequests.EventDate.ToString("yyyy-MM-dd HH:mm"), regularFont, PdfBrushes.Black, new PointF(origin.X + 640 * scale, origin.Y + 150 * scale));


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

        private void DrawBarcode(PdfPage page, PointF origin, float scale, TicketRequest ticketDetails)
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