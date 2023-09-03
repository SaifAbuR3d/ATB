using ATB.Entities;

namespace ATB.DataAccess
{
    public interface IBookingRepository
    {
        void AddBooking(Passenger passenger, Flight flight, FlightClass flightClass);
        void AddBooking(Booking booking);
        void UpdateBookingClass(Booking booking, FlightClass newClass);
        void RemoveBooking(Booking booking);
        void RemoveBooking(Passenger passenger, Flight flight, FlightClass flightClass);
        IEnumerable<Booking> GetAllBookings();

    }
}
