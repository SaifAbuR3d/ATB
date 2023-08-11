using ATB.Entities;
using ATB.Services;

namespace ATB.DataAccess
{
    internal class FileBookingRepository : IBookingRepository
    {
        private string bookingsFilePath = "files/Bookings.txt";
        public void AddBooking(Passenger passenger, Flight flight)
        {
            throw new NotImplementedException();
        }

        public void AddBooking(Booking booking)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Booking> GetAllBookings()
        {
            IFlightRepository flightRepository = new FileFlightRepository();
            FlightService flightService = new FlightService(flightRepository);

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
                        Flight flight = (Flight)flightService.GetFlightById(flightId); 

                        // Continue Here .... TODO make getById for passengers ...
                       
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

        public void RemoveBooking(Booking booking)
        {
            throw new NotImplementedException();
        }

        public void RemoveBooking(Passenger passenger, Flight flight)
        {
            throw new NotImplementedException();
        }

        public void UpdateBookingClass(Booking booking, FlightClass newClass)
        {
            throw new NotImplementedException();
        }


    }
}
