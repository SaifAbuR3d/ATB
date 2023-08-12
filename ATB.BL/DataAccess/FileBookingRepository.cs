using ATB.Entities;
using ATB.Services;
using System.Collections;

namespace ATB.DataAccess
{
    internal class FileBookingRepository : IBookingRepository
    {
        private string bookingsFilePath = "files/Bookings.txt";
        public void AddBooking(Booking booking)
        {
            try
            {
                string bookingLine = $"{booking.passenger.PassengerId},{booking.flight.FlightId}";
                File.AppendAllLines(bookingsFilePath, new[] { bookingLine });
                Console.WriteLine("Booking added successfully!");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An error occurred while adding the booking: {ex.Message}");
            }
        }



        public IEnumerable<Booking> GetAllBookings()
        {
            IFlightRepository flightRepository = new FileFlightRepository();
            FlightService flightService = new FlightService(flightRepository);

            FilePassengerRepository passengerRepository = new FilePassengerRepository();

            List<Booking> allBookings = new List<Booking>();

            try
            {
                string[] lines = File.ReadAllLines(bookingsFilePath);

                foreach (string line in lines)
                {
                    string[] values = line.Split(',');

                    if (values.Length >= 2 && int.TryParse(values[0], out int passengerId))
                    {
                        int flightId = int.Parse(values[1]);
                        Flight flight = flightService.GetFlightById(flightId); 
                        Passenger passenger = passengerRepository.GetPassengerById(passengerId);
                        
                        allBookings.Add(new Booking(flight, passenger));
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An error occurred while reading the bookings file: {ex.Message}");
            }

            return allBookings;
        }
        public IEnumerable<Booking> GetPassengerBookings(Passenger passenger)
        {
            return GetAllBookings().
                Where(booking => booking.passenger.Equals(passenger)); 
        }

        public IEnumerable<Booking> GetPassengerBookings(int passengerId)
        {
            return GetAllBookings().
                Where(booking => booking.passenger.PassengerId.Equals(passengerId));
        }


        private void WriteBookingsIntoFile(IEnumerable<Booking> bookings)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(bookingsFilePath, false)) // Open the file in write mode, which overwrites existing content
                {
                    foreach (Booking booking in bookings)
                    {
                        string line = $"{booking.passenger.PassengerId},{booking.flight.FlightId}"; 
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

            // replace the contents of bookings.txt with the new list 
            WriteBookingsIntoFile(updatedList);
        }
        public void RemoveBooking(Passenger passenger, Flight flight)
        {
            RemoveBooking(new Booking(flight, passenger)); 
        }

        public void UpdateBookingClass(Booking booking, FlightClass newClass)
        {
            throw new NotImplementedException();
        }


        
        // not needed, remove it from the interface
        public void AddBooking(Passenger passenger, Flight flight)
        {
            throw new NotImplementedException();
        }
    }
}
