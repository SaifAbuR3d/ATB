namespace ATB.Entities
{
    public class FlightSearchCriteria
    {
        public decimal? Price { get; set; }
        public string? DepartureCountry { get; set; }
        public string? DestinationCountry { get; set; }
        public DateTime? DepartureDate { get; set; }
        public string? DepartureAirport { get; set; }
        public string? ArrivalAirport { get; set; }
        public FlightClass? FClass { get; set; }

        
    }
}
