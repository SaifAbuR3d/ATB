using ATB.DataAccess;
using ATB.Entities;
using ATB.Helpers;
using ATB.Services;
using CsvHelper;
using CsvHelper.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;

namespace ATB.Presentation
{
    internal class Program
    {
        static void Main()
        {
            ValidationDetailsHelper.DisplayValidationDetails(typeof(Flight)); 
            //IFlightRepository flightRepository = new FileFlightRepository();
            //FlightService flightService = new FlightService(flightRepository);

            //IBookingRepository bookingRepository = new FileBookingRepository(); 
            //BookingService bookingService = new BookingService(bookingRepository);
            
            //FilePassengerRepository passengerRepository = new FilePassengerRepository();

            //foreach (var flight in flightService.GetAllFlights())
            //{
            //    Console.WriteLine(flight);
            //}

            //Console.WriteLine();
            //Console.WriteLine();

            //flightService.ImportFlightsFromCsv("files/AddFlights.csv");

            //foreach (var flight in flightService.GetAllFlights())
            //{
            //    Console.WriteLine(flight);
            //}

            //var list = bookingService.GetPassengerBookings(1);
            //foreach (Booking item in list) { Console.WriteLine(item); }

            //bookingService.RemoveBooking(passengerRepository.GetPassengerById(1), flightService.GetFlightById(1));

            //Console.WriteLine();
            //list = bookingService.GetPassengerBookings(1);
            //foreach (Booking item in list) { Console.WriteLine(item); }

            //bookingService.AddBooking(passengerRepository.GetPassengerById(2), flightService.GetFlightById(3));
            //Console.WriteLine();
            //list = bookingService.GetPassengerBookings(2);
            //foreach (Booking item in list) { Console.WriteLine(item); }
            //  flightService.ImportFlightsFromCsv("files/AddFlights.csv");

            //List<Flight> list = flightService.GetAllFlights().Take(6).ToList();

            //foreach (var flight in list) { Console.WriteLine(flight); }
            //Console.WriteLine();
            //Console.WriteLine(flightService.GetFlightById(0));

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