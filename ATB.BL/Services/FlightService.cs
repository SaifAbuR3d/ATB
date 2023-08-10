using ATB.DataAccess;
using ATB.Entities;
using System.Xml.Serialization;

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

        public bool DontCare(Object? obj)
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
                         ((flight.DepartureCounrty.Equals(flightSearchCriteria.DepartureCounrty)) || DontCare(flightSearchCriteria.DepartureCounrty)) &&
                         ((flight.DepartureAirport.Equals(flightSearchCriteria.DepartureAirport)) || DontCare(flightSearchCriteria.DepartureAirport))
                         ); 
        }
    }
}
