namespace ATB.Entities
{
    enum FlightClass
    {
        Economy,
        Business,
        FirstClass
    }
    internal readonly struct Flight
    {
        public decimal Price { get; }
        public string DepartureCounrty { get; }
        public string DestinationCountry { get; }
        public DateOnly DepartureDate { get; }
        public string DepartureAirport { get; }
        public string ArrivalAirport { get; }

        public Flight(decimal price, string departureCounrty, string destinationCountry, DateOnly departureDate, string departureAirport, string arrivalAirport)
        {
            Price = price;
            DepartureCounrty = departureCounrty ?? throw new ArgumentNullException(nameof(departureCounrty));
            DestinationCountry = destinationCountry ?? throw new ArgumentNullException(nameof(destinationCountry));
            DepartureDate = departureDate;
            DepartureAirport = departureAirport ?? throw new ArgumentNullException(nameof(departureAirport));
            ArrivalAirport = arrivalAirport ?? throw new ArgumentNullException(nameof(arrivalAirport));
        }
    }
}
