using ATB.DataAccess;
using ATB.Entities;
using ATB.Services;

namespace ATB.Presentation
{
    internal class Program
    {
        static void Main()
        {
            IFlightRepository flightRepository = new FileFlightRepository();
            FlightService flightService = new FlightService(flightRepository);

            List<Flight> list = flightService.GetAllFlights().ToList();

        }
    }
}