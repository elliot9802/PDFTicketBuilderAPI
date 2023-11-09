using Newtonsoft.Json;

namespace DbModels
{
    public class TicketsDataDbM
    {
        [JsonProperty("platsbokad_id")]
        public string? PlatsbokadId { get; set; }

        [JsonProperty("ArtikelNr")]
        public string? ArtNr { get; set; }

        [JsonProperty("Pris")]
        public decimal Price { get; set; }

        [JsonProperty("serviceavgift1_kr")]
        public decimal ServiceFee { get; set; }

        [JsonProperty("Artikelnamn")]
        public string? ArtName { get; set; }

        [JsonProperty("stolsrad")]
        public string? ChairRow { get; set; }

        [JsonProperty("stolsnr")]
        public string? ChairNr { get; set; }

        [JsonProperty("eventdatum_id")]
        public string? EventDateId { get; set; }

        [JsonProperty("datumStart")]
        public DateTime EventDate { get; set; }

        [JsonProperty("namn1")]
        public string? EventName { get; set; }

        //#region osäker på implementering

        //[JsonProperty("namn2")]
        //public string? Name2 { get; set; }

        //[JsonProperty("logorad1")]
        //public string? Logorad1 { get; set; }

        //[JsonProperty("logorad2")]
        //public string? Logorad2 { get; set; }

        //[JsonProperty("plats")]
        //public string? Plats { get; set; }

        //[JsonProperty("StrukturArtikel")]
        //public int? StrukturArtikel { get; set; }

        //[JsonProperty("Beskrivning")]
        //public string? Beskrivning { get; set; }

        //[JsonProperty("ArtNotText")]
        //public int? ArtNotText { get; set; }

        //[JsonProperty("Datum")]
        //public DateTime? Datum { get; set; }

        //[JsonProperty("Ingang")]
        //public string? Ingang { get; set; }

        //[JsonProperty("wbeventinfo")]
        //public string? wbeventinfo { get; set; }

        //[JsonProperty("wbarticleinfo")]
        //public string? WbArticleInfo { get; set; }

        //[JsonProperty("Rutbokstav")]
        //public string? Rutbokstav { get; set; }

        //[JsonProperty("Webbcode")]
        //public string? Webbcode { get; set; }
        //#endregion

        [JsonProperty("namn")]
        public string? SubEventName { get; set; }

        [JsonProperty("BokningsNr")]
        public string? BookingNumber { get; set; }

        [JsonProperty("webbkod")]
        public string? WebBookingNumber { get; set; }

        [JsonProperty("anamn")]
        public string? FacilityName { get; set; }

        [JsonProperty("reklam1")]
        public string? Ad { get; set; }

        [JsonProperty("showEventInfo")]
        public int? showEventInfo { get; set; }

        [JsonProperty("KontaktPerson")]
        public string? ContactPerson { get; set; }

        [JsonProperty("eMail")]
        public string? Email { get; set; }
    }
}