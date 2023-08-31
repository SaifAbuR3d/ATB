using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "Flight ID is required")]
        public int FlightId { get; init; }

        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Price is required")]
        [Range(1.0, 20000.0, ErrorMessage =
            "Price Must be a nonnegative decimal in range (1.0 to 20000.0).")]
        public decimal Price { get; init; }
        
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Departure country is required")]
        [StringLength(20, ErrorMessage =
            "Departure country should be less than or equal to 20 characters.")]
        public string DepartureCountry { get; init; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Destination country is required")]
        [StringLength(20, ErrorMessage =
            "Destination country should be less than or equal to 20 characters.")]
        public string DestinationCountry { get; init; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Departure date is required")]
        [Range(typeof(DateTime),"today", "future", ErrorMessage = "Allowed Range: today → future")]
        public DateOnly DepartureDate { get; init; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Departure airport is required")]
        [StringLength(20, ErrorMessage =
            "Departure airport should be less than or equal to 20 characters.")]
        public string DepartureAirport { get; init; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Arrival airport is required")]
        [StringLength(20, ErrorMessage =
            "Arrival airport should be less than or equal to 20 characters.")]
        public string ArrivalAirport { get; init; }

        [EnumDataType(typeof(FlightClass))]
        [Required(ErrorMessage = "FlightClass is required")]
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
        public override string ToString()
        {
            return $"FlightId = {FlightId}, Price = {Price}, DepartureCountry = {DepartureCountry}";
        }

        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }
            if (obj is not Flight)
            {
                return false;
            }
            return FlightId.Equals( ((Flight)obj ).FlightId)  &&  FClass.Equals(((Flight)obj).FClass); 
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
