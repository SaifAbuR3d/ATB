using ATB.DataAccess;
using ATB.Entities;
using ATB.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;

namespace ATB.Tests
{
    public class BookingServiceTests
    {
        private readonly IFixture fixture;
        private readonly Mock<IBookingRepository> mockBookingRepository;
        private readonly BookingService sut;
        public BookingServiceTests()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
            mockBookingRepository = fixture.Freeze<Mock<IBookingRepository>>();
            sut = new BookingService(mockBookingRepository.Object);
        }

        [Fact]
        public void GetAllBookings_ReturnsAllBookings()
        {
            var bookings = fixture.CreateMany<Booking>();
            mockBookingRepository.Setup(x => x.GetAllBookings()).Returns(bookings);

            var result = sut.GetAllBookings();

            Assert.Equal(bookings, result);
        }

        [Fact]
        public void AddBooking_PassengerHasNotBookedThisFlightBefore_ReturnsSuccess()
        {
            var passenger = fixture.Create<Passenger>();
            var flight = fixture.Create<Flight>();
            var flightClass = fixture.Create<FlightClass>();
            var booking = new Booking(flight, passenger, flightClass);
            var bookings = new List<Booking>();

            mockBookingRepository.Setup(x => x.AddBooking(booking));
            mockBookingRepository.Setup(x => x.GetAllBookings()).Returns(bookings);

            var result = sut.AddBooking(passenger, flight, flightClass);

            Assert.Equal(BookingOperationStatus.Success, result);
            mockBookingRepository.Verify(x => x.AddBooking(booking), Times.Once());

        }
        [Fact]
        public void AddBooking_PassengerHasBookedThisFlightBefore_ReturnsFailed()
        {
            var passenger = fixture.Create<Passenger>();
            var flight = fixture.Create<Flight>();
            var flightClass = fixture.Create<FlightClass>();
            var booking = new Booking(flight, passenger, flightClass);
            var bookings = new List<Booking>
            {
                booking
            };

            mockBookingRepository.Setup(x => x.AddBooking(booking));
            mockBookingRepository.Setup(x => x.GetAllBookings()).Returns(bookings);

            var result = sut.AddBooking(passenger, flight, flightClass);

            Assert.Equal(BookingOperationStatus.Failed, result);
            mockBookingRepository.Verify(x => x.AddBooking(booking), Times.Never());

        }
        [Fact]
        public void RemoveBooking_PassengerHasNotBookedThisFlightBefore_ReturnsFailed()
        {
            var passenger = fixture.Create<Passenger>();
            var flight = fixture.Create<Flight>();
            var flightClass = fixture.Create<FlightClass>();
            var booking = new Booking(flight, passenger, flightClass);
            var bookings = new List<Booking>();

            mockBookingRepository.Setup(x => x.RemoveBooking(booking));
            mockBookingRepository.Setup(x => x.GetAllBookings()).Returns(bookings);

            var result = sut.RemoveBooking(passenger, flight, flightClass);

            Assert.Equal(BookingOperationStatus.Failed, result);
            mockBookingRepository.Verify(x => x.RemoveBooking(booking), Times.Never());
        }
        [Fact]
        public void RemoveBooking_PassengerHasBookedThisFlightBefore_ReturnsSuccess()
        {
            var passenger = fixture.Create<Passenger>();
            var flight = fixture.Create<Flight>();
            var flightClass = fixture.Create<FlightClass>();
            var booking = new Booking(flight, passenger, flightClass);
            var bookings = new List<Booking>
            {
                booking
            };

            mockBookingRepository.Setup(x => x.RemoveBooking(booking));
            mockBookingRepository.Setup(x => x.GetAllBookings()).Returns(bookings);

            var result = sut.RemoveBooking(passenger, flight, flightClass);

            Assert.Equal(BookingOperationStatus.Success, result);
            mockBookingRepository.Verify(x => x.RemoveBooking(booking), Times.Once());
        }


        [Fact]
        public void GetPassengerBookings_ReturnsMatchingBookings()
        {
            var passenger = fixture.Create<Passenger>();

            var matchingBookings = new List<Booking>
            {
                new Booking(fixture.Create<Flight>(), passenger, fixture.Create<FlightClass>()),
                new Booking(fixture.Create<Flight>(), passenger, fixture.Create<FlightClass>())
            };

            var allBookings = new List<Booking>
            {
                new Booking(fixture.Create<Flight>(), fixture.Create<Passenger>(), fixture.Create<FlightClass>()),
                new Booking(fixture.Create<Flight>(), fixture.Create<Passenger>(), fixture.Create<FlightClass>()),
                new Booking(fixture.Create<Flight>(), fixture.Create<Passenger>(), fixture.Create<FlightClass>())
            };
            allBookings.AddRange(matchingBookings);

            mockBookingRepository.Setup(repo => repo.GetAllBookings()).Returns(allBookings);

            var result = sut.GetPassengerBookings(passenger);

            Assert.Equal(matchingBookings, result);
        }
        [Fact]
        public void GetPassengerBookings_NoMatchingBookings_ReturnsEmptyList()
        {
            var passenger = fixture.Create<Passenger>();

            var allBookings = new List<Booking>
            {
                new Booking(fixture.Create<Flight>(), fixture.Create<Passenger>(), fixture.Create<FlightClass>()),
                new Booking(fixture.Create<Flight>(), fixture.Create<Passenger>(), fixture.Create<FlightClass>()),
                new Booking(fixture.Create<Flight>(), fixture.Create<Passenger>(), fixture.Create<FlightClass>())
            };

            mockBookingRepository.Setup(repo => repo.GetAllBookings()).Returns(allBookings);

            var result = sut.GetPassengerBookings(passenger);

            Assert.Empty(result);
        }

        [Fact]
        public void UpdateBookingClass_PassengerHasBookedThisFlightBefore_ReturnsSuccess()
        {
            var booking = fixture.Create<Booking>();
            var bookings = new List<Booking> { booking };

            var newFlightClass = fixture.Create<FlightClass>();


            mockBookingRepository.Setup(x => x.UpdateBookingClass(booking, newFlightClass));
            mockBookingRepository.Setup(x => x.GetAllBookings()).Returns(bookings);

            var result = sut.UpdateBookingClass(booking, newFlightClass);

            Assert.Equal(BookingOperationStatus.Success, result);
            mockBookingRepository.Verify(x => x.UpdateBookingClass(booking, newFlightClass), Times.Once());
        }

        [Fact]
        public void UpdateBookingClass_EmptyBookings_ReturnsFailed()
        {
            var booking = fixture.Create<Booking>();
            var bookings = new List<Booking>();

            var newFlightClass = fixture.Create<FlightClass>();

            mockBookingRepository.Setup(x => x.UpdateBookingClass(booking, newFlightClass));
            mockBookingRepository.Setup(x => x.GetAllBookings()).Returns(bookings);

            var result = sut.UpdateBookingClass(booking, newFlightClass);

            Assert.Equal(BookingOperationStatus.Failed, result);
            mockBookingRepository.Verify(x => x.UpdateBookingClass(booking, newFlightClass), Times.Never());
        }
        [Fact]
        public void UpdateBookingClass_PassengerHasNotBookedThisFlightBefore_ReturnsFailed()
        {
            var passenger = fixture.Create<Passenger>();
            var flight = fixture.Create<Flight>();
            var oldFlightClass = fixture.Create<FlightClass>();

            var booking = new Booking(flight, passenger, oldFlightClass);  // real booking 
            var bookings = new List<Booking> { booking };
            mockBookingRepository.Setup(x => x.GetAllBookings()).Returns(bookings); // returns real bookings


            var otherPassenger = fixture.Create<Passenger>();
            var fakeBooking = new Booking(flight, otherPassenger, oldFlightClass);

            var newFlightClass = fixture.Create<FlightClass>();

            mockBookingRepository.Setup(x => x.UpdateBookingClass(fakeBooking, newFlightClass));

            var result = sut.UpdateBookingClass(fakeBooking, newFlightClass);

            Assert.Equal(BookingOperationStatus.Failed, result);
            mockBookingRepository.Verify(x => x.UpdateBookingClass(fakeBooking, newFlightClass), Times.Never());
        }

        [Fact]
        public void FilterBookings_WithNullCriteria_ReturnsAllBookings()
        {
            var allBookings = fixture.CreateMany<Booking>();
            mockBookingRepository.Setup(x => x.GetAllBookings()).Returns(allBookings);

            var searchCriteria = new BookingSearchCriteria();

            var result = sut.FilterBookings(searchCriteria);

            Assert.Equal(allBookings, result);
        }
        [Fact]
        public void FilterBookings_WithSpecificCriteriaAndOneMatchingBooking_ReturnsMatchingBooking()
        {
            var allBookings = fixture.CreateMany<Booking>().ToList();

            var flight = fixture.Build<Flight>()
                                .With(f => f.Price, 100.0m)
                                .Create();
            var matchingBooking = fixture.Build<Booking>()
                                .With(b => b.Flight, flight)
                                .Create();

            allBookings.Add(matchingBooking);

            mockBookingRepository.Setup(x => x.GetAllBookings()).Returns(allBookings);

            var sut = new BookingService(mockBookingRepository.Object);

            var searchCriteria = new BookingSearchCriteria
            {
                Price = 100.0m
            };

            var result = sut.FilterBookings(searchCriteria);
            Assert.Contains(matchingBooking, result);
        }
        [Fact]
        public void FilterBookings_WithMultipleCriteriaAndMultipleMatchingBookings_ReturnsMatchingBookings()
        {
            var allBookings = fixture.CreateMany<Booking>().ToList();

            var flight1 = fixture.Build<Flight>()
                .With(f => f.Price, 100.0m)
                .With(f => f.DestinationCountry, "USA")
                .With(f => f.FClass, FlightClass.Economy)
                .With(f => f.DepartureCountry, "PS")
                .Create();
            var matchingBooking1 = fixture.Build<Booking>()
                .With(b => b.Flight, flight1)
                .Create();

            var flight2 = fixture.Build<Flight>()
                .With(f => f.Price, 100.0m)
                .With(f => f.DestinationCountry, "USA")
                .With(f => f.FClass, FlightClass.Economy)
                .With(f => f.DepartureCountry, "PS")
                .Create();
            var matchingBooking2 = fixture.Build<Booking>()
                .With(b => b.Flight, flight2)
                .Create();

            allBookings.Add(matchingBooking1);
            allBookings.Add(matchingBooking2);

            mockBookingRepository.Setup(x => x.GetAllBookings()).Returns(allBookings);

            var searchCriteria = new BookingSearchCriteria
            {
                Price = 100.0m,
                DestinationCountry = "USA",
                FClass = FlightClass.Economy,
                DepartureCountry = "PS"
            };

            var result = sut.FilterBookings(searchCriteria);

            Assert.Equal(2, result.Count());
            Assert.Contains(matchingBooking1, result);
            Assert.Contains(matchingBooking2, result);
        }
    }
}