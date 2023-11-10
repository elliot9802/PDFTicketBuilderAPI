namespace DbModels
{
    public class TicketsDataDbM
    {
        public string? platsbokad_id { get; set; }

        public string? ArtikelNr { get; set; }

        public decimal Pris { get; set; }

        public decimal serviceavgift1_kr { get; set; }

        public string? Artikelnamn { get; set; }

        public string? stolsrad { get; set; }

        public string? stolsnr { get; set; }

        public string? eventdatum_id { get; set; }

        public DateTime datumStart { get; set; }

        public string? namn1 { get; set; }

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

        public string? namn { get; set; }

        public string? BokningsNr { get; set; }

        public string? webbkod { get; set; }

        public string? anamn { get; set; }

        public string? reklam1 { get; set; }

        public int? showEventInfo { get; set; }

        public string? KontaktPerson { get; set; }

        public string? eMail { get; set; }
    }
}