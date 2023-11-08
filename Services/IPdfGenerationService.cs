using Models;

namespace Services
{
    public interface IPdfGenerationService
    {
        Task CreatePdfAsync(string outputPath, TicketRequest ticketDetails, string backgroundImagePath);
    }
}
