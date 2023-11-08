using Models;

namespace Services
{
    public interface IPdfCreationService
    {
        string GetTemporaryPdfFilePath();
        Task<byte[]> CreateAndSavePdfAsync(TicketRequest ticketDetails, string backgroundImagePath);

    }
}
