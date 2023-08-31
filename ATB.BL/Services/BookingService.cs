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
        public IEnumerable<Booking> FilterBookings(BookingSearchCriteria bookingSearchCriteria)
        {
            var allBookings = GetAllBookings();
            return allBookings
                .Where(booking =>
                     (booking.flight.Price.Equals(bookingSearchCriteria.Price) || (bookingSearchCriteria.Price is null) ) &&
                     (booking.flight.DepartureDate.Equals(bookingSearchCriteria.DepartureDate) || (bookingSearchCriteria.DepartureDate is null)) &&
                     (booking.flight.FClass.Equals(bookingSearchCriteria.FClass) || (bookingSearchCriteria.FClass is null)) &&
                     (booking.flight.DestinationCountry.Equals(bookingSearchCriteria.DestinationCountry) || (bookingSearchCriteria.DestinationCountry is null)) &&
                     (booking.flight.ArrivalAirport.Equals(bookingSearchCriteria.ArrivalAirport) || (bookingSearchCriteria.ArrivalAirport is null)) &&
                     (booking.flight.DepartureCountry.Equals(bookingSearchCriteria.DepartureCountry) || (bookingSearchCriteria.DepartureCountry is null)) &&
                     (booking.flight.DepartureAirport.Equals(bookingSearchCriteria.DepartureAirport) || (bookingSearchCriteria.DepartureAirport is null)) &&

                     (booking.flight.FlightId.Equals(bookingSearchCriteria.FlightId) || (bookingSearchCriteria.FlightId is null)) &&
                     (booking.passenger.PassengerId.Equals(bookingSearchCriteria.PassengerId) || (bookingSearchCriteria.PassengerId is null))
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
        private bool hasBookedThisFlight(Passenger passenger, Flight flight)
        {
            return GetAllBookings().Any(booking => booking.passenger.Equals(passenger)
                                     && booking.flight.Equals(flight));
        }
        public void AddBooking(Passenger passenger, Flight flight, FlightClass flightClass)
        {
            if (!hasBookedThisFlight(passenger, flight))
            {
                _bookingRepository.AddBooking(new Booking(flight, passenger, flightClass));
            }
        }
        public void RemoveBooking(Passenger passenger, Flight flight, FlightClass flightClass)
        {
            if (hasBookedThisFlight(passenger, flight))
            {
                _bookingRepository.RemoveBooking(new Booking(flight, passenger, flightClass));
            }
        }

        public void UpdateBookingClass(Booking booking, FlightClass newFlightClass)
        {
            _bookingRepository.UpdateBookingClass(booking, newFlightClass);
        }

        public void UpdateBookingClass(Passenger passenger, Flight flight, FlightClass flightClass, FlightClass newFlightClass)
        {
            UpdateBookingClass(new Booking(flight, passenger, flightClass), newFlightClass); 
        }
    }
}
