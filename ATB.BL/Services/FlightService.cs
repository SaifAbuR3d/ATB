using ATB.DataAccess;
using ATB.Entities;

namespace ATB.Services
{
    public class FlightService
    {
        private readonly IFlightRepository _flightRepository;
        public FlightService(IFlightRepository flightRepository)
        {
            _flightRepository = flightRepository;
        }
        public IEnumerable<Flight> GetAllFlights()
        {
            try
            {
                return _flightRepository.GetAllFlights();
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An error occurred while getting the flights: {ex.Message}");
                return new List<Flight>();
            }

        }

        /// <summary>
        /// Filters the collection of flights based on the specified search criteria.
        /// </summary>
        /// <param name="flightSearchCriteria">The criteria to filter flights.</param>
        /// <returns>
        /// An IEnumerable of <see cref="Flight"/> objects that match the search criteria.
        /// </returns>
        public IEnumerable<Flight> FilterFlights(FlightSearchCriteria flightSearchCriteria)
        { 
            var allFlights = GetAllFlights();

            bool IsNotSpecified<T>(T value) => value == null;

            bool FilterPredicate(Flight flight)
            {
                bool IsPriceMatch = flight.Price == flightSearchCriteria.Price || IsNotSpecified(flightSearchCriteria.Price);
                bool IsDepartureDateMatch = flight.DepartureDate == flightSearchCriteria.DepartureDate || IsNotSpecified(flightSearchCriteria.DepartureDate);
                bool IsFClassMatch = flight.FClass == flightSearchCriteria.FClass || IsNotSpecified(flightSearchCriteria.FClass);
                bool IsDestinationCountryMatch = flight.DestinationCountry == flightSearchCriteria.DestinationCountry || IsNotSpecified(flightSearchCriteria.DestinationCountry);
                bool IsArrivalAirportMatch = flight.ArrivalAirport == flightSearchCriteria.ArrivalAirport || IsNotSpecified(flightSearchCriteria.ArrivalAirport);
                bool IsDepartureCountryMatch = flight.DepartureCountry == flightSearchCriteria.DepartureCountry || IsNotSpecified(flightSearchCriteria.DepartureCountry);
                bool IsDepartureAirportMatch = flight.DepartureAirport == flightSearchCriteria.DepartureAirport || IsNotSpecified(flightSearchCriteria.DepartureAirport);

                return IsPriceMatch && IsDepartureDateMatch && IsFClassMatch &&
                       IsDestinationCountryMatch && IsArrivalAirportMatch &&
                       IsDepartureCountryMatch && IsDepartureAirportMatch;
            }

            var filteredFlights = allFlights.Where(FilterPredicate);

            return filteredFlights;
        }

        public void ImportFlightsFromCsv(string csvFilePath)
        {
            try
            {
                List<Flight> flights = CsvUtility.ParseFlightsFromCsv(csvFilePath).ToList();
                _flightRepository.AddAllFlights(flights);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An error occurred while importing the flights: {ex.Message}");
            }
        }

        public Flight? GetFlight(int flightId, FlightClass flightClass)
        {
            try
            {
                return _flightRepository.GetFlight(flightId, flightClass);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An error occurred while getting the flight: {ex.Message}");
                return null; 
            }
        }
    }
}
