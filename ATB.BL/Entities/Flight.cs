namespace ATB.Entities
{
    internal enum FlightClass
    {
        Economy,
        Business,
        FirstClass
    }
    internal readonly struct Flight
    {
        private static int lastId = 1;
        public int FlightId { get; }
        public decimal Price { get; init; }
        public string DepartureCounrty { get; init; }
        public string DestinationCountry { get; init; }
        public DateOnly DepartureDate { get; init; }
        public string DepartureAirport { get; init; }
        public string ArrivalAirport { get; init; }
        public FlightClass FClass { get; init; }

        public Flight(decimal price, string departureCounrty, string destinationCountry, DateOnly departureDate, string departureAirport, string arrivalAirport, FlightClass fClass)
        {
            Price = price;
            DepartureCounrty = departureCounrty ?? throw new ArgumentNullException(nameof(departureCounrty));
            DestinationCountry = destinationCountry ?? throw new ArgumentNullException(nameof(destinationCountry));
            DepartureDate = departureDate;
            DepartureAirport = departureAirport ?? throw new ArgumentNullException(nameof(departureAirport));
            ArrivalAirport = arrivalAirport ?? throw new ArgumentNullException(nameof(arrivalAirport));
            FClass = fClass;

            FlightId = GenerateId(); 
        }
        static int GenerateId() => lastId++;
        public override string ToString()
        {
            return $"ID = {FlightId}, Price = {Price}, DepartureCountry = {DepartureCounrty}";
        }
    }
}
