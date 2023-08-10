using ATB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATB.DataAccess
{
    internal class FileFlightRepository : IFlightRepository
    {
        public void AddAllFlights(IEnumerable<Flight> flights)
        {
            throw new NotImplementedException();
        }

        public void AddFlight(Flight flight)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Flight> GetAllFlights()
        {
            // Hard-Coded List, for Testing purposes 
            return new List<Flight>()
            {
                new Flight
                {
                    Price = 1,
                    ArrivalAirport = "A",
                    DepartureAirport = "B",
                    DepartureCounrty = "A",
                    DepartureDate =DateOnly.Parse("1/1/2001"),
                    DestinationCountry = "A",
                    FClass = FlightClass.FirstClass,
                },
                new Flight
                {
                    Price = 1,
                    ArrivalAirport = "AA",
                    DepartureAirport = "Bd",
                    DepartureCounrty = "Ad",
                    DepartureDate =DateOnly.Parse("11/1/2001"),
                    DestinationCountry = "Ac",
                    FClass = FlightClass.Economy
                },
                new Flight
                {
                    Price = 1,
                    ArrivalAirport = "AA",
                    DepartureAirport = "BA",
                    DepartureCounrty = "AA",
                    DepartureDate =DateOnly.Parse("1/1/2561"),
                    DestinationCountry = "AA",
                    FClass = FlightClass.Business
                },
                new Flight
                {
                    Price = 145,
                    ArrivalAirport = "Ab",
                    DepartureAirport = "bB",
                    DepartureCounrty = "bA",
                    DepartureDate =DateOnly.Parse("1/15/2061"),
                    DestinationCountry = "A",
                    FClass = FlightClass.FirstClass
                },
                new Flight
                {
                    Price = 121,
                    ArrivalAirport = "AA",
                    DepartureAirport = "B",
                    DepartureCounrty = "AA",
                    DepartureDate =DateOnly.Parse("1/1/2521"),
                    DestinationCountry = "AA",
                    FClass = FlightClass.FirstClass
                },
            };
        }

        public Flight GetFlightById(int flightId)
        {
            throw new NotImplementedException();
        }
    }
}
