﻿namespace ATB.Entities
{
    internal class FlightSearchCriteria
    {
        public decimal? Price { get; set; }
        public string? DepartureCountry { get; set; }
        public string? DestinationCountry { get; set; }
        public DateOnly? DepartureDate { get; set; }
        public string? DepartureAirport { get; set; }
        public string? ArrivalAirport { get; set; }
        public FlightClass? FClass { get; set; }

        
    }
}
