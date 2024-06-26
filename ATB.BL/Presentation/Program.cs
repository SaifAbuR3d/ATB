﻿using ATB.DataAccess;
using ATB.Entities;
using ATB.Helpers;
using ATB.Services;

namespace ATB.Presentation
{
    internal class Program 
    {  
        static readonly IFlightRepository flightRepository = new FileFlightRepository();
        static readonly FlightService flightService = new FlightService(flightRepository);

        static readonly IBookingRepository bookingRepository = new FileBookingRepository();
        static readonly BookingService bookingService = new BookingService(bookingRepository);

        static readonly IPassengerRepository passengerRepository = new FilePassengerRepository();
        static void Main()
        {
            RunMainMenu();
        }

        private static void RunMainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to Airport Ticket Booking App!");
                Console.WriteLine("-------------------------------------");
                Console.WriteLine("1. Passenger");
                Console.WriteLine("2. Manager");
                Console.WriteLine("3. Exit");
                string mainChoice = UserInputHelper.GetValidString("Enter your choice: ");

                switch (mainChoice)
                {
                    case "1":
                        RunPassengerMenu();
                        break;
                    case "2":
                        RunManagerMenu();
                        break;
                    case "3":
                        Console.WriteLine("Exiting the application. Goodbye!");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        public static void RunPassengerMenu()
        {
            int passengerId = UserInputHelper.GetValidInt("Enter your Passenger ID: ");
            Passenger? passenger = passengerRepository.GetPassenger(passengerId);
            if (passenger is null)
            {
                Console.WriteLine("Invalid Passenger ID. Returning to main menu...");
                Thread.Sleep(2000);
                return;
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Passenger Menu");
                Console.WriteLine("--------------");
                Console.WriteLine("1. Book a Flight");
                Console.WriteLine("2. Search for Flights");
                Console.WriteLine("3. Cancel Booking");
                Console.WriteLine("4. Modify Booking");
                Console.WriteLine("5. View My Bookings");
                Console.WriteLine("6. Go Back");
                string passengerChoice = UserInputHelper.GetValidString("Enter your choice: ");

                switch (passengerChoice)
                {
                    case "1":
                        BookFlight(passenger);
                        break;
                    case "2":
                        SearchForFlights();
                        break;
                    case "3":
                        CancelBooking(passenger);
                        break;
                    case "4":
                        ModifyBooking(passenger);
                        break;
                    case "5":
                        ViewMyBookings(passenger);
                        break;
                    case "6":
                        RunMainMenu();
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void RunManagerMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Manager Menu");
                Console.WriteLine("------------");
                Console.WriteLine("1. Filter Bookings");
                Console.WriteLine("2. Import Flights from CSV");
                Console.WriteLine("3. Show Validation details For Flights"); 
                Console.WriteLine("4. Go Back");
                string managerChoice = UserInputHelper.GetValidString("Enter your choice: ");

                switch (managerChoice)
                {
                    case "1":
                        FilterBookings();
                        break;
                    case "2":
                        ImportFlightsFromCSV(); 
                        break;
                    case "3":
                        ShowValidationDetails(typeof(Flight));
                        break; 
                    case "4":
                        RunMainMenu();
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }


        #region Manager Methods
        private static void FilterBookings()
        {
            Console.Clear();
            Console.WriteLine("Filter Bookings");

            BookingSearchCriteria filterCriteria = GetSearchCriteriaFromManager();

            IEnumerable<Booking> filteredBookings = bookingService.FilterBookings(filterCriteria);

            if (filteredBookings.Any())
            {
                Console.WriteLine("Filtered Bookings:");
                foreach (var booking in filteredBookings)
                {
                    Console.WriteLine($"Passenger: {booking.Passenger.PassengerName}");
                    Console.WriteLine($"Flight ID: {booking.Flight.FlightId}");
                    Console.WriteLine($"Departure: {booking.Flight.DepartureCountry} - {booking.Flight.DepartureAirport}");
                    Console.WriteLine($"Destination: {booking.Flight.DestinationCountry} - {booking.Flight.ArrivalAirport}");
                    Console.WriteLine($"Departure Date: {booking.Flight.DepartureDate}");
                    Console.WriteLine($"Price: {booking.Flight.Price}");
                    Console.WriteLine($"Class: {booking.Flight.FClass}");
                    Console.WriteLine("--------------------------");
                }
            }
            else
            {
                Console.WriteLine("No bookings match the filter criteria.");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static BookingSearchCriteria GetSearchCriteriaFromManager()
        {
            BookingSearchCriteria criteria = new BookingSearchCriteria();

            Console.Write("Enter Price (or press Enter to skip): ");
            if (decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                criteria.Price = price;
            }

            Console.Write("Enter Departure Country (or press Enter to skip): ");

            criteria.DepartureCountry = Console.ReadLine();

            if (String.IsNullOrWhiteSpace(criteria.DepartureCountry))
            {
                criteria.DepartureCountry = null;
            }

            Console.Write("Enter Destination Country (or press Enter to skip): ");
            criteria.DestinationCountry = Console.ReadLine();

            if (String.IsNullOrWhiteSpace(criteria.DestinationCountry))
            {
                criteria.DestinationCountry = null;
            }

            Console.Write("Enter Departure Date (yyyy-MM-dd) (or press Enter to skip): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime departureDate))
            {
                criteria.DepartureDate = departureDate;
            }

            Console.Write("Enter Departure Airport (or press Enter to skip): ");
            criteria.DepartureAirport = Console.ReadLine();

            if (String.IsNullOrWhiteSpace(criteria.DepartureAirport))
            {
                criteria.DepartureAirport = null;
            }

            Console.Write("Enter Arrival Airport (or press Enter to skip): ");
            criteria.ArrivalAirport = Console.ReadLine();

            if (String.IsNullOrWhiteSpace(criteria.ArrivalAirport))
            {
                criteria.ArrivalAirport = null;
            }

            Console.Write("Enter Flight Class (Economy, Business, FirstClass) (or press Enter to skip): ");
            if (Enum.TryParse<FlightClass>(Console.ReadLine(), out FlightClass flightClass))
            {
                criteria.FClass = flightClass;
            }

            Console.Write("Enter Flight Id (or press Enter to skip): ");
            if (int.TryParse(Console.ReadLine(), out int flightId))
            {
                criteria.FlightId = flightId;
            }

            Console.Write("Enter Passenger Id (or press Enter to skip): ");
            if (int.TryParse(Console.ReadLine(), out int passengerId))
            {
                criteria.PassengerId = passengerId;
            }

            return criteria;
        }


        private static void ImportFlightsFromCSV()
        {
            Console.Clear();
            Console.WriteLine("Import Flights from CSV");

            string csvFilePath = UserInputHelper.GetValidString("Enter the path to the CSV file:");
            flightService.ImportFlightsFromCsv(csvFilePath);

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        private static void ShowValidationDetails(Type type)
        {
            ValidationDetailsHelper.DisplayValidationDetails(type);

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        #endregion

        #region Passenger Methods
        private static void BookFlight(Passenger passenger)
        {
            Console.Clear();
            Console.WriteLine("Book a Flight");

            int flightId = UserInputHelper.GetValidInt("Enter the Flight ID: ");
            FlightClass flightClass = UserInputHelper.GetValidFlightClass("Enter the Flight Class: ", "Invalid Flight Class: Must be one of the following (Economy, Business, FirstClass)");

            Flight? flight = flightService.GetFlight(flightId, flightClass);
            if (flight is null)
            {
                Console.WriteLine("There is no such a Flight. Returning...");
                Thread.Sleep(2000);
                return;
            }
            bookingService.AddBooking(passenger, (Flight)flight, flightClass);
            Console.WriteLine("Booking Added Successfully.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }  

        private static void SearchForFlights()
        {
            Console.Clear();
            Console.WriteLine("Search for Flights");

            FlightSearchCriteria searchCriteria = GetSearchCriteriaFromPassenger();

            IEnumerable<Flight> foundFlights = flightService.FilterFlights(searchCriteria);

            if (foundFlights.Any())
            {
                Console.WriteLine("Found Flights:");
                foreach (var flight in foundFlights)
                {
                    Console.WriteLine($"Flight ID: {flight.FlightId}");
                    Console.WriteLine($"Departure: {flight.DepartureCountry} - {flight.DepartureAirport}");
                    Console.WriteLine($"Destination: {flight.DestinationCountry} - {flight.ArrivalAirport}");
                    Console.WriteLine($"Departure Date: {flight.DepartureDate}");
                    Console.WriteLine($"Price: {flight.Price}");
                    Console.WriteLine($"Class: {flight.FClass}");
                    Console.WriteLine("--------------------------");
                }
            }
            else
            {
                Console.WriteLine("No flights found matching the search criteria.");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static FlightSearchCriteria GetSearchCriteriaFromPassenger()
        {
            FlightSearchCriteria criteria = new FlightSearchCriteria();

            Console.Write("Enter Price (or press Enter to skip): ");
            if (decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                criteria.Price = price;
            }

            Console.Write("Enter Departure Country (or press Enter to skip): ");

            criteria.DepartureCountry = Console.ReadLine();

            if(String.IsNullOrWhiteSpace(criteria.DepartureCountry))
            {
                criteria.DepartureCountry = null; 
            }

            Console.Write("Enter Destination Country (or press Enter to skip): ");
            criteria.DestinationCountry = Console.ReadLine();

            if (String.IsNullOrWhiteSpace(criteria.DestinationCountry))
            {
                criteria.DestinationCountry = null;
            }

            Console.Write("Enter Departure Date (yyyy-MM-dd) (or press Enter to skip): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime departureDate))
            {
                criteria.DepartureDate = departureDate;
            }

            Console.Write("Enter Departure Airport (or press Enter to skip): ");
            criteria.DepartureAirport = Console.ReadLine();

            if (String.IsNullOrWhiteSpace(criteria.DepartureAirport))
            {
                criteria.DepartureAirport = null;
            }

            Console.Write("Enter Arrival Airport (or press Enter to skip): ");
            criteria.ArrivalAirport = Console.ReadLine();

            if (String.IsNullOrWhiteSpace(criteria.ArrivalAirport))
            {
                criteria.ArrivalAirport = null;
            }

            Console.Write("Enter Flight Class (Economy, Business, FirstClass) (or press Enter to skip): ");
            if (Enum.TryParse<FlightClass>(Console.ReadLine(), out FlightClass flightClass))
            {
                criteria.FClass = flightClass;
            }

            return criteria;
        }

        private static void CancelBooking(Passenger passenger)
        {
            Console.Clear();
            Console.WriteLine("Cancel Booking");

            int flightId = UserInputHelper.GetValidInt("Enter the Flight ID: ");
            FlightClass flightClass = UserInputHelper.GetValidFlightClass("Enter the Flight Class: ", "Invalid Flight Class: Must be one of the following (Economy, Business, FirstClass)");
            
            Flight? flight = flightService.GetFlight(flightId, flightClass); 
            if (flight is null) 
            {
                Console.WriteLine("You Didn't book this Flight. Returning...");
                Thread.Sleep(2000);
                return;
            }

            bookingService.RemoveBooking(passenger, (Flight)flight, flightClass);
            Console.WriteLine("Booking Cancelled Successfully.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static void ModifyBooking(Passenger passenger) 
        {
            Console.Clear();
            Console.WriteLine("Modify Booking");

            int flightId = UserInputHelper.GetValidInt("Enter the Flight ID: ");
            FlightClass flightClass = UserInputHelper.GetValidFlightClass("Enter the Flight Class: ", "Invalid Flight Class: Must be one of the following (Economy, Business, FirstClass)");

            Flight? flight = flightService.GetFlight(flightId, flightClass);
            if (flight is null)
            {
                Console.WriteLine("There is No such a flight. Returning...");
                Thread.Sleep(2000);
                return;
            }

            FlightClass newFlightClass = UserInputHelper.GetValidFlightClass("Enter the new Flight Class: ", "Invalid Flight Class: Must be one of the following (Economy, Business, FirstClass)");

            var status = bookingService.UpdateBookingClass(passenger, (Flight)flight, flightClass, newFlightClass);
            if (status == BookingOperationStatus.Failed)
            {
                Console.WriteLine("You Didn't book this Flight. Returning...");
                Thread.Sleep(2000);
                return;
            }

            Console.WriteLine("Booking Updated Successfully.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static void ViewMyBookings(Passenger passenger)
        {
            Console.Clear();
            Console.WriteLine("View My Bookings");
            IEnumerable<Booking> bookings = bookingService.GetPassengerBookings(passenger); 
            foreach (Booking booking in bookings)
            {
                Console.WriteLine(booking);
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        #endregion
    }
}