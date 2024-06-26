﻿using ATB.Entities;
using ATB.Services;

namespace ATB.DataAccess
{
    internal class FileBookingRepository : IBookingRepository
    {
        private string bookingsFilePath = "files/Bookings.txt";
        public void AddBooking(Booking booking)
        {
            var bookingLine = $"{booking.Passenger.PassengerId},{booking.Flight.FlightId},{booking.FClass}";
            File.AppendAllLines(bookingsFilePath, new[] { bookingLine });
            Console.WriteLine("Booking added successfully!");
        }

        public void AddBooking(Passenger passenger, Flight flight, FlightClass flightClass)
        {
            AddBooking(new Booking(flight, passenger, flightClass));
        }

        public IEnumerable<Booking> GetAllBookings()
        {
            IFlightRepository flightRepository = new FileFlightRepository();
            var flightService = new FlightService(flightRepository);

            IPassengerRepository passengerRepository = new FilePassengerRepository();

            var allBookings = new List<Booking>();
            string[] lines = File.ReadAllLines(bookingsFilePath);

            foreach (string line in lines)
            {
                string[] values = line.Split(',');

                if (values.Length >= 3 && int.TryParse(values[0], out int passengerId))
                {
                    var flightId = int.Parse(values[1]);
                    var passenger = passengerRepository.GetPassenger(passengerId);
                    var flightClass = Enum.Parse<FlightClass>(values[2], true); // true to ignore case
                    var flight = flightService.GetFlight(flightId, flightClass);

                    allBookings.Add(new Booking((Flight)flight, passenger, flightClass));
                }
            }

            return allBookings;
        }
        private void WriteBookingsIntoFile(IEnumerable<Booking> bookings) // OVERWRITES Existing Lines
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(bookingsFilePath, false)) // Open the file in write mode, which overwrites existing content
                {
                    foreach (var booking in bookings)
                    {
                        var line = $"{booking.Passenger.PassengerId},{booking.Flight.FlightId},{booking.FClass}"; 
                        writer.WriteLine(line);
                    }
                }
                Console.WriteLine("bookings written to the file successfully!");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An error occurred while writing to the file: {ex.Message}");
            }
        }
        public void RemoveBooking(Booking booking)
        {
            // Contains All Bookings Except For (booking)
            IEnumerable<Booking> updatedList = GetAllBookings().Where(_booking => !_booking.Equals(booking));

            // Replace the contents of bookings.txt with the new list 
            WriteBookingsIntoFile(updatedList);
        }
        public void RemoveBooking(Passenger passenger, Flight flight, FlightClass flightClass)
        {
            RemoveBooking(new Booking(flight, passenger, flightClass)); 
        }

        public void UpdateBookingClass(Booking booking, FlightClass newFlightClass)
        {
            var allBookingsList = GetAllBookings().ToList();

            // Find the index of the booking to update
            var bookingIndex = allBookingsList.FindIndex(_booking =>
                (_booking.Passenger.PassengerId == booking.Passenger.PassengerId) && (_booking.Flight.FlightId == booking.Flight.FlightId));

            if (bookingIndex != -1)
            {
                allBookingsList[bookingIndex] = new Booking(booking.Flight, booking.Passenger, newFlightClass);

                WriteBookingsIntoFile(allBookingsList);
                Console.WriteLine("Booking modified successfully!");
            }
            else
            {
                Console.WriteLine("the Booking was not found for modification.");
            }
        }
    }
}
