using ATB.Entities;
namespace ATB.DataAccess
{
    internal interface IFlightRepository
    {
        void AddFlight(Flight flight);
        Flight? GetFlight(int flightId, FlightClass flightClass);
        IEnumerable<Flight> GetAllFlights();
        void AddAllFlights(IEnumerable<Flight> flights);
    }
}
