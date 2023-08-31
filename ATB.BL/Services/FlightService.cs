using ATB.DataAccess;
using ATB.Entities;

namespace ATB.Services
{
    internal class FlightService
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

        public IEnumerable<Flight> FilterFlights(FlightSearchCriteria flightSearchCriteria)
        { 
            var allFlights = GetAllFlights();
            return allFlights
                .Where(flight =>
                         ((flight.Price.Equals(flightSearchCriteria.Price)) || (flightSearchCriteria.Price is null)) &&
                         ((flight.DepartureDate.Equals(flightSearchCriteria.DepartureDate)) || (flightSearchCriteria.DepartureDate is null)) &&
                         ((flight.FClass.Equals(flightSearchCriteria.FClass)) || (flightSearchCriteria.FClass is null)) &&
                         ((flight.DestinationCountry.Equals(flightSearchCriteria.DestinationCountry)) || (flightSearchCriteria.DestinationCountry is null)) &&
                         ((flight.ArrivalAirport.Equals(flightSearchCriteria.ArrivalAirport)) || (flightSearchCriteria.ArrivalAirport is null)) &&
                         ((flight.DepartureCountry.Equals(flightSearchCriteria.DepartureCountry)) || (flightSearchCriteria.DepartureCountry is null)) &&
                         ((flight.DepartureAirport.Equals(flightSearchCriteria.DepartureAirport)) || (flightSearchCriteria.DepartureAirport is null))
                         ); 
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
