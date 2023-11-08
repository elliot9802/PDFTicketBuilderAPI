using Syncfusion.Drawing;

namespace Models
{
    public class TicketRequest
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string WebBookingNumber { get; set; }
        public string BookingNumber { get; set; }
        public string TicketType { get; set; }
        public decimal Price { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal ServiceFee { get; set; }
        public string EventName { get; set; }
        public string EventNameEnglish { get; set; }
        public DateTime EventDate { get; set; }

        // New properties for customization options
        public bool IncludeEmail { get; set; }
        public bool IncludeName { get; set; }
        public bool IncludeWebBookingNumber { get; set; }
        public bool IncludeBookingNumber { get; set; }
        public bool IncludeTicketType { get; set; }
        public bool IncludePrice { get; set; }
        public bool IncludePurchaseDate { get; set; }
        public bool IncludeServiceFee { get; set; }
        public bool IncludeEventName { get; set; }
        public bool IncludeEventDate { get; set; }

        // Properties for positioning elements on the ticket
        public PointF EmailPosition { get; set; }
        public PointF NamePosition { get; set; }
        public PointF WebBookingNumberPosition { get; set; }

        public List<PdfElement> Elements { get; set; }

        public string BarcodeContent { get; set; }
        public PointF BarcodePosition { get; set; }
        // Property to choose between QR code and Barcode
        public bool UseQRCode { get; set; }
    }
}