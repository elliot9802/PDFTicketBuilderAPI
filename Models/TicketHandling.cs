using Newtonsoft.Json;
using Syncfusion.Drawing;

namespace Models
{
    public class TicketHandling
    {
        #region oklart
        //public string? TicketType { get; set; }
        //public DateTime? PurchaseDate { get; set; }
        #endregion

        // New properties for customization options
        #region Include DTO's
        public string? IncludePBookId { get; set; }
        public bool IncludeArtNr { get; set; }
        public bool IncludePrice { get; set; }
        public bool IncludeServiceFee { get; set; }
        public bool IncludeArtName { get; set; }
        public bool IncludeChairRow { get; set; }
        public bool IncludeChairNr { get; set; }
        public bool IncludeEventDateId { get; set; }
        public bool IncludeEventDate { get; set; }
        public bool IncludeEventName { get; set; }
        public bool IncludeSubEventName { get; set; }
        public bool IncludeBookingNr { get; set; }
        public bool IncludeWebBookingNr { get; set; }
        public bool IncludeFacilityName { get; set; }
        public bool IncludeAd { get; set; }
        public bool IncludeshowEventInfo { get; set; }
        public bool IncludeContactPerson { get; set; }
        public bool IncludeEmail { get; set; }
        #endregion

        #region Position DTO's
        // Properties for positioning elements on the ticket
        public PointF EmailPosition { get; set; }
        public PointF NamePosition { get; set; }
        public PointF WebBookingNumberPosition { get; set; }
        public PointF ArtNrPosition { get; set; }
        public PointF PricePosition { get; set; }
        public PointF ServiceFeePosition { get; set; }
        public PointF ArtNamePosition { get; set; }
        public PointF ChairRowPosition { get; set; }
        public PointF ChairNrPosition { get; set; }
        public PointF EventDateIdPosition { get; set; }
        public PointF EventDatePosition { get; set; }
        public PointF EventNamePosition { get; set; }
        #endregion

        public List<PdfElement>? Elements { get; set; }

        public string? BarcodeContent { get; set; }
        public PointF BarcodePosition { get; set; }

        // Property to choose between QR code and Barcode
        public bool UseQRCode { get; set; }
    }
}