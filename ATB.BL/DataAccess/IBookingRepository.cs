using ATB.Entities;

namespace ATB.DataAccess
{
    internal interface IBookingRepository
    {
        void AddBooking(Passenger passenger, Flight flight);
        void AddBooking(Booking booking);
        void UpdateBookingClass(Booking booking, FlightClass newClass);
        void RemoveBooking(Booking booking);
        void RemoveBooking(Passenger passenger, Flight flight);
        IEnumerable<Booking> GetPassengerBookings(Passenger passenger);

        IEnumerable<Booking> GetAllBookings();

    }
}
