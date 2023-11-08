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
    }
}