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
            return _flightRepository.GetAllFlights();
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
            List<Flight> flights = CsvUtility.ParseFlightsFromCsv(csvFilePath).ToList();
            _flightRepository.AddAllFlights(flights);
        }

        public Flight? GetFlight(int flightId, FlightClass flightClass)
        {
            return _flightRepository.GetFlight(flightId, flightClass);
        }


    }
}
