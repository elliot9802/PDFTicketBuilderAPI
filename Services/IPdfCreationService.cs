using DbModels;
using Models;

namespace Services
{
    public interface IPdfCreationService
    {
        string GetTemporaryPdfFilePath();
        Task<byte[]> CreateAndSavePdfAsync(TicketsDataDbM ticketData, TicketHandling ticketDetails, string backgroundImagePath);

    }
}
