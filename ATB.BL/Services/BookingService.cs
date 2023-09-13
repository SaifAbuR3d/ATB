using ATB.DataAccess;
using ATB.Entities;
namespace ATB.Services
{
    public class BookingService
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

        /// <summary>
        /// Filters the collection of bookings based on the specified search criteria.
        /// </summary>
        /// <param name="bookingSearchCriteria">The criteria to filter bookings.</param>
        /// <returns>
        /// An IEnumerable of <see cref="Booking"/> objects that match the search criteria.
        /// </returns>
        /// 
        public IEnumerable<Booking> FilterBookings(BookingSearchCriteria bookingSearchCriteria)
        {
            var allBookings = GetAllBookings();

            bool IsNotSpecified<T>(T value) => value == null;

            bool FilterPredicate(Booking booking)
            {
                var flight = booking.Flight;

                bool IsPriceMatch = flight.Price.Equals(bookingSearchCriteria.Price) || IsNotSpecified(bookingSearchCriteria.Price);
                bool IsDepartureDateMatch = flight.DepartureDate.Equals(bookingSearchCriteria.DepartureDate) || IsNotSpecified(bookingSearchCriteria.DepartureDate);
                bool IsFClassMatch = flight.FClass.Equals(bookingSearchCriteria.FClass) || IsNotSpecified(bookingSearchCriteria.FClass);
                bool IsDestinationCountryMatch = flight.DestinationCountry.Equals(bookingSearchCriteria.DestinationCountry) || IsNotSpecified(bookingSearchCriteria.DestinationCountry);
                bool IsArrivalAirportMatch = flight.ArrivalAirport.Equals(bookingSearchCriteria.ArrivalAirport) || IsNotSpecified(bookingSearchCriteria.ArrivalAirport);
                bool IsDepartureCountryMatch = flight.DepartureCountry.Equals(bookingSearchCriteria.DepartureCountry) || IsNotSpecified(bookingSearchCriteria.DepartureCountry);
                bool IsDepartureAirportMatch = flight.DepartureAirport.Equals(bookingSearchCriteria.DepartureAirport) || IsNotSpecified(bookingSearchCriteria.DepartureAirport);
                bool IsFlightIdMatch = flight.FlightId.Equals(bookingSearchCriteria.FlightId) || IsNotSpecified(bookingSearchCriteria.FlightId);
                bool IsPassengerIdMatch = booking.Passenger.PassengerId.Equals(bookingSearchCriteria.PassengerId) || IsNotSpecified(bookingSearchCriteria.PassengerId);

                return IsPriceMatch && IsDepartureDateMatch && IsFClassMatch &&
                       IsDestinationCountryMatch && IsArrivalAirportMatch &&
                       IsDepartureCountryMatch && IsDepartureAirportMatch &&
                       IsFlightIdMatch && IsPassengerIdMatch;
            }

            var filteredBookings = allBookings.Where(FilterPredicate);

            return filteredBookings;
        }

        public IEnumerable<Booking> GetPassengerBookings(Passenger passenger)
        {
            return GetAllBookings().
                Where(booking => booking.Passenger.Equals(passenger));
        }
        public IEnumerable<Booking> GetPassengerBookings(int passengerId)
        {
            return GetAllBookings().
                Where(booking => booking.Passenger.PassengerId.Equals(passengerId));
        }
        private bool HasAlreadyBookedThisFlight(Passenger passenger, Flight flight)
        {
            return GetAllBookings().Any(booking => booking.Passenger.Equals(passenger)
                                                && booking.Flight.Equals(flight));
        }
        public BookingOperationStatus AddBooking(Passenger passenger, Flight flight, FlightClass flightClass)
        {
            if (!HasAlreadyBookedThisFlight(passenger, flight))
            {
                _bookingRepository.AddBooking(new Booking(flight, passenger, flightClass));
                return BookingOperationStatus.Success;
            }
            return BookingOperationStatus.Failed;
        }
        public BookingOperationStatus RemoveBooking(Passenger passenger, Flight flight, FlightClass flightClass)
        {
            if (HasAlreadyBookedThisFlight(passenger, flight))
            {
                _bookingRepository.RemoveBooking(new Booking(flight, passenger, flightClass));
                return BookingOperationStatus.Success;
            }
            return BookingOperationStatus.Failed;
        }
        public BookingOperationStatus UpdateBookingClass(Booking booking, FlightClass newFlightClass)
        {
            if (HasAlreadyBookedThisFlight(booking.Passenger, booking.Flight))
            {
                _bookingRepository.UpdateBookingClass(booking, newFlightClass);
                return BookingOperationStatus.Success; 
            }
            return BookingOperationStatus.Failed; 
        }

        public BookingOperationStatus UpdateBookingClass(Passenger passenger, Flight flight, FlightClass flightClass, FlightClass newFlightClass)
        {
            return UpdateBookingClass(new Booking(flight, passenger, flightClass), newFlightClass);
        }
    }
}
