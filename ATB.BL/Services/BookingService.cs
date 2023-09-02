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
                     (booking.Flight.Price.Equals(bookingSearchCriteria.Price) || (bookingSearchCriteria.Price is null) ) &&
                     (booking.Flight.DepartureDate.Equals(bookingSearchCriteria.DepartureDate) || (bookingSearchCriteria.DepartureDate is null)) &&
                     (booking.Flight.FClass.Equals(bookingSearchCriteria.FClass) || (bookingSearchCriteria.FClass is null)) &&
                     (booking.Flight.DestinationCountry.Equals(bookingSearchCriteria.DestinationCountry) || (bookingSearchCriteria.DestinationCountry is null)) &&
                     (booking.Flight.ArrivalAirport.Equals(bookingSearchCriteria.ArrivalAirport) || (bookingSearchCriteria.ArrivalAirport is null)) &&
                     (booking.Flight.DepartureCountry.Equals(bookingSearchCriteria.DepartureCountry) || (bookingSearchCriteria.DepartureCountry is null)) &&
                     (booking.Flight.DepartureAirport.Equals(bookingSearchCriteria.DepartureAirport) || (bookingSearchCriteria.DepartureAirport is null)) &&

                     (booking.Flight.FlightId.Equals(bookingSearchCriteria.FlightId) || (bookingSearchCriteria.FlightId is null)) &&
                     (booking.Passenger.PassengerId.Equals(bookingSearchCriteria.PassengerId) || (bookingSearchCriteria.PassengerId is null))
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
            return GetAllBookings().Any(booking => booking.Passenger.Equals(passenger)
                                     && booking.Flight.Equals(flight));
        }
        public BookingStatus AddBooking(Passenger passenger, Flight flight, FlightClass flightClass)
        {
            if (!hasBookedThisFlight(passenger, flight))
            {
                _bookingRepository.AddBooking(new Booking(flight, passenger, flightClass));
                return BookingStatus.Failed; 
            }
            return BookingStatus.Success; 
        }
        public BookingStatus RemoveBooking(Passenger passenger, Flight flight, FlightClass flightClass)
        {
            if (hasBookedThisFlight(passenger, flight))
            {
                _bookingRepository.RemoveBooking(new Booking(flight, passenger, flightClass));
                return BookingStatus.Failed;
            }
            return BookingStatus.Success;
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
