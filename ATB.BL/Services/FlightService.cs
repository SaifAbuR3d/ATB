using ATB.DataAccess;
using ATB.Entities;
using CsvHelper;

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

        public static bool DontCare(object? obj)
        {
            return obj is null;
        }
        public IEnumerable<Flight> FilterFlights(FlightSearchCriteria flightSearchCriteria)
        { 
            var allFlights = GetAllFlights();
            return allFlights
                .Where(flight =>
                         ((flight.Price.Equals(flightSearchCriteria.Price)) || DontCare(flightSearchCriteria.Price)) &&
                         ((flight.DepartureDate.Equals(flightSearchCriteria.DepartureDate)) || DontCare(flightSearchCriteria.DepartureDate)) &&
                         ((flight.FClass.Equals(flightSearchCriteria.FClass)) || DontCare(flightSearchCriteria.FClass)) &&
                         ((flight.DestinationCountry.Equals(flightSearchCriteria.DestinationCountry)) || DontCare(flightSearchCriteria.DestinationCountry)) &&
                         ((flight.ArrivalAirport.Equals(flightSearchCriteria.ArrivalAirport)) || DontCare(flightSearchCriteria.ArrivalAirport)) &&
                         ((flight.DepartureCountry.Equals(flightSearchCriteria.DepartureCountry)) || DontCare(flightSearchCriteria.DepartureCountry)) &&
                         ((flight.DepartureAirport.Equals(flightSearchCriteria.DepartureAirport)) || DontCare(flightSearchCriteria.DepartureAirport))
                         ); 
        }

        public void ImportFlightsFromCsv(string csvFilePath)
        {
            List<Flight> flights = CsvUtility.ParseFlightsFromCsv(csvFilePath).ToList();
            _flightRepository.AddAllFlights(flights);
        }

        public Flight? GetFlightById(int flightId)
        {
            return _flightRepository.GetFlightById(flightId);
        }


    }
}
