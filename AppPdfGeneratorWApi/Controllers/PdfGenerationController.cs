using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace AppPdfGeneratorWApi.Controllers
{
    /// <summary>
    /// Controller responsible for handling PDF creation requests.
    /// </summary>
    [Route("api/pdf")]
    [ApiController]
    public class PdfGenerationController : ControllerBase
    {
        private readonly ILogger<PdfGenerationController> _logger;
        private readonly IFileService _fileService;
        private readonly IPdfCreationService _pdfService;
        private readonly IPdfGenerationService _pdfUtility;
        //    private static readonly string[] Summaries = new[]
        //    {
        //    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        //};

        public PdfGenerationController(IPdfCreationService pdfService, IPdfGenerationService pdfUtility, IFileService fileService, ILogger<PdfGenerationController> logger)
        {
            _pdfService = pdfService;
            _pdfUtility = pdfUtility;
            _logger = logger;            _fileService = fileService;

        }

        //[HttpGet(Name = "GetWeatherForecast")]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}

        [HttpPost("createPdf")]
        public async Task<IActionResult> CreatePdf([FromForm] TicketRequest ticketRequest, IFormFile bgFile)
        {
            try
            {
                if (bgFile == null || bgFile.Length == 0)
                {
                    _logger.LogWarning("Background file missing or empty.");
                    return BadRequest("Please provide a background image file.");
                }

                //if (Path.GetExtension(bgFile.FileName).ToLower() != ".jpg" || ".png")
                //{
                //    _logger.LogWarning($"Uploaded file '{bgFile.FileName}' is not an jpg or png file.");
                //    return BadRequest("Please provide a valid jpg or png file.");
                //}

                var tempBgFilePath = Path.GetTempFileName();
                using (var stream = new FileStream(tempBgFilePath, FileMode.Create))
                {
                    await bgFile.CopyToAsync(stream);
                }

                // Generate the PDF with the ticket details and the background image.
                string outputPath = _pdfService.GetTemporaryPdfFilePath();
                await _pdfUtility.CreatePdfAsync(outputPath, ticketRequest, tempBgFilePath);

                // Read the generated PDF into a byte array.
                //byte[] pdfBytes = await _pdfService.CreateAndSavePdfAsync(ticketRequest);
                byte[] pdfBytes = await _fileService.ReadAllBytesAsync(outputPath);

                await _fileService.DeleteAsync(tempBgFilePath);

                // Return the PDF file with a generated file name.
                return File(pdfBytes, "application/pdf", $"{Guid.NewGuid()}.pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating PDF");
                return StatusCode(500, new { message = "Error creating PDF", error = ex.Message });
            }
        }

    }
}