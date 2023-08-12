using ATB.DataAccess;
using ATB.Entities;

namespace ATB.Services
{
    internal class BookingService
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public IEnumerable<Booking> GetAllBookings()
        {
            return _bookingRepository.GetAllBookings();
        }
        public bool DontCare(object? obj)
        {
            return obj is null;
        }
        public IEnumerable<Booking> FilterBookings(BookingSearchCriteria bookingSearchCriteria)
        {
            var allBookings = GetAllBookings();
            return allBookings
                .Where(booking =>
                     (booking.flight.Price.Equals(bookingSearchCriteria.Price) || DontCare(bookingSearchCriteria.Price)) &&
                     (booking.flight.DepartureDate.Equals(bookingSearchCriteria.DepartureDate) || DontCare(bookingSearchCriteria.DepartureDate)) &&
                     (booking.flight.FClass.Equals(bookingSearchCriteria.FClass) || DontCare(bookingSearchCriteria.FClass)) &&
                     (booking.flight.DestinationCountry.Equals(bookingSearchCriteria.DestinationCountry) || DontCare(bookingSearchCriteria.DestinationCountry)) &&
                     (booking.flight.ArrivalAirport.Equals(bookingSearchCriteria.ArrivalAirport) || DontCare(bookingSearchCriteria.ArrivalAirport)) &&
                     (booking.flight.DepartureCountry.Equals(bookingSearchCriteria.DepartureCountry) || DontCare(bookingSearchCriteria.DepartureCountry)) &&
                     (booking.flight.DepartureAirport.Equals(bookingSearchCriteria.DepartureAirport) || DontCare(bookingSearchCriteria.DepartureAirport)) &&
                     (booking.flight.Equals(bookingSearchCriteria.flight) || DontCare(bookingSearchCriteria.flight)) &&
                     (booking.passenger.Equals(bookingSearchCriteria.passenger) || DontCare(bookingSearchCriteria.passenger))
                     );
        }
        public IEnumerable<Booking> GetPassengerBookings(Passenger passenger)
        {
            return _bookingRepository.GetPassengerBookings(passenger);

        }
        public IEnumerable<Booking> GetPassengerBookings(int passengerId)
        {
            return _bookingRepository.GetPassengerBookings(passengerId);

        }

        // TODO - Add checks if the passenger has already booked this flight.
        public void AddBooking(Passenger passenger, Flight flight, FlightClass flightClass)
        {
            _bookingRepository.AddBooking(new Booking(flight, passenger, flightClass));
        }
        public void AddBooking(Booking booking)
        {
            _bookingRepository.AddBooking(booking);
        }

        // TODO - Add checks if the passenger hasn't booked this flight

        public void RemoveBooking(Booking booking)
        {
            _bookingRepository.RemoveBooking(booking);

        }

        public void RemoveBooking(Passenger passenger, Flight flight, FlightClass flightClass)
        {
            _bookingRepository.RemoveBooking(new Booking(flight, passenger, flightClass));
        }
    }
}
