using Models;
using DbModels;

namespace Services
{
    public interface IPdfGenerationService
    {
        Task CreatePdfAsync(string outputPath, TicketsDataDbM ticketData, TicketHandling ticketDetails, string backgroundImagePath);
        Task<List<TicketsDataDbM>> ReadJsonDataAsync();


    }
}
