using ATB.Entities;
namespace ATB.DataAccess
{
    internal interface IFlightRepository
    {
        void AddFlight(Flight flight);
        Flight GetFlightById(int flightId);
        IEnumerable<Flight> GetAllFlights();
        void AddAllFlights(IEnumerable<Flight> flights);
    }
}
