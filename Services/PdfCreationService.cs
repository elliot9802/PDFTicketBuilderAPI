using Microsoft.Extensions.Logging;
using Models;

namespace Services
{
    public class PdfCreationService : IPdfCreationService
    {
        private readonly IFileService _fileService;
        private readonly IPdfGenerationService _pdfUtility;
        private readonly ILogger<PdfCreationService> _logger;

        public PdfCreationService(IFileService fileService, IPdfGenerationService pdfUtility, ILogger<PdfCreationService> logger)
        {
            _fileService = fileService;
            _pdfUtility = pdfUtility;
            _logger = logger;
        }

        public string GetTemporaryPdfFilePath()
        {
            string tempDirectory = Path.GetTempPath();
            string fileName = Guid.NewGuid().ToString() + ".pdf";
            return Path.Combine(tempDirectory, fileName);
        }

        private async Task<byte[]> ConvertToPdfBytesAsync(Func<string, Task> conversionAction)
        {
            string outputPath = GetTemporaryPdfFilePath();
            try
            {
                _logger.LogInformation($"Starting PDF conversion. Temporary output path: {outputPath}");
                await conversionAction(outputPath);
                if (!_fileService.Exists(outputPath))
                {
                    throw new FileNotFoundException("Pdf file not found after conversion", outputPath);
                }
                byte[] pdfBytes = await _fileService.ReadAllBytesAsync(outputPath);
                _logger.LogInformation($"Pdf conversion completed. Temporary output path: {outputPath}");
                return pdfBytes;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Failed to convert to PDF. Temporary output path: {outputPath}";
                _logger.LogError(errorMessage, ex);
                throw new PdfCreationException($"{errorMessage}. Exception: {ex.Message}", ex);
            }
            finally
            {
                if (_fileService.Exists(outputPath))
                {
                    await _fileService.DeleteAsync(outputPath);
                }
            }
        }

        public async Task<byte[]> CreateAndSavePdfAsync(TicketRequest ticketDetails, string backgroundImagePath)
        {
            return await ConvertToPdfBytesAsync(outputPath => _pdfUtility.CreatePdfAsync(outputPath, ticketDetails, backgroundImagePath));
        }
    }
}
