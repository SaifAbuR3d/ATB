using System.Diagnostics.CodeAnalysis;

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
       // private static int lastId = 1;
        public int FlightId { get; init; }
        public decimal Price { get; init; }
        public string DepartureCountry { get; init; }
        public string DestinationCountry { get; init; }
        public DateOnly DepartureDate { get; init; }
        public string DepartureAirport { get; init; }
        public string ArrivalAirport { get; init; }
        public FlightClass FClass { get; init; }

        public Flight(int flightId, decimal price, string departureCountry, string destinationCountry, DateOnly departureDate, string departureAirport, string arrivalAirport, FlightClass fClass)
        {
            Price = price;
            DepartureCountry = departureCountry ?? throw new ArgumentNullException(nameof(departureCountry));
            DestinationCountry = destinationCountry ?? throw new ArgumentNullException(nameof(destinationCountry));
            DepartureDate = departureDate;
            DepartureAirport = departureAirport ?? throw new ArgumentNullException(nameof(departureAirport));
            ArrivalAirport = arrivalAirport ?? throw new ArgumentNullException(nameof(arrivalAirport));
            FClass = fClass;

            FlightId = flightId; 
        }
      //  static int GenerateId() => lastId++;
        public override string ToString()
        {
            return $"FlightId = {FlightId}, Price = {Price}, DepartureCountry = {DepartureCountry}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj is not Flight)
            {
                return false;
            }
            return this.FlightId.Equals( ((Flight)obj ).FlightId); 
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
