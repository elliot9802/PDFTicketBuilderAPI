using Newtonsoft.Json;
using Syncfusion.Drawing;

namespace Models
{
    public class TicketHandling
    {
        //#region oklart
        //public string? TicketType { get; set; }
        //public DateTime? PurchaseDate { get; set; }
        //#endregion

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

        public List<PdfElement>? Elements { get; set; }

        public string? BarcodeContent { get; set; }
        public PointF BarcodePosition { get; set; }

        // Property to choose between QR code and Barcode
        public bool UseQRCode { get; set; }
    }
}