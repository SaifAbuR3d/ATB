using ATB.DataAccess;
using ATB.Entities;
using ATB.Services;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace ATB.Presentation
{
    internal class Program
    {
        static void Main()
        {
            IFlightRepository flightRepository = new FileFlightRepository();
            FlightService flightService = new FlightService(flightRepository);


          //  flightService.ImportFlightsFromCsv("files/AddFlights.csv");

            List<Flight> list = flightService.GetAllFlights().Take(6).ToList();

            foreach (var flight in list) { Console.WriteLine(flight); }
            Console.WriteLine();
            Console.WriteLine(flightService.GetFlightById(0));

            //List<Flight> list = flightService.GetAllFlights().ToList();

            //IBookingRepository bookingRepository = new FileBookingRepository();
            //BookingService bookingService = new BookingService(bookingRepository); 

            //List<Booking> list = bookingService.GetAllBookings().ToList();
            //foreach (Booking booking in list)
            //{
            //    Console.WriteLine(booking);
            //}
            //Console.WriteLine();
            //Console.WriteLine();


            //var bookingSearchCriteria = new BookingSearchCriteria
            //{
            //    passenger = new Passenger(3, "a"), 
            //    DestinationCountry = "a"
            //};
            //var filteredList = bookingService.FilterBookings(bookingSearchCriteria);
            //foreach (Booking booking in filteredList)
            //{
            //    Console.WriteLine(booking);
            //}



        }
    }
}