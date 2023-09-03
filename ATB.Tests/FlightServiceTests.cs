using ATB.DataAccess;
using ATB.Entities;
using ATB.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;

namespace ATB.Tests
{
    public class FlightServiceTests
    {
        private readonly IFixture fixture;
        private readonly Mock<IFlightRepository> mockFlightRepository;
        private readonly FlightService sut;
        public FlightServiceTests()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
            mockFlightRepository = fixture.Freeze<Mock<IFlightRepository>>();
            sut = new FlightService(mockFlightRepository.Object);
        }
        [Fact]
        public void GetAllFlights_ReturnsAllFlights()
        {
            var flights = fixture.CreateMany<Flight>();
            mockFlightRepository.Setup(x => x.GetAllFlights()).Returns(flights);

            var result = sut.GetAllFlights();

            Assert.Equal(flights, result);
        }




        public void FilterFlights_WithNullCriteria_ReturnsAllFlights()
        {
            var allFlights = fixture.CreateMany<Flight>();
            mockFlightRepository.Setup(x => x.GetAllFlights()).Returns(allFlights);

            var searchCriteria = new FlightSearchCriteria();

            var result = sut.FilterFlights(searchCriteria);

            Assert.Equal(allFlights, result);
        }
        [Fact]
        public void FilterFlights_WithSpecificCriteriaAndOneMatchingFlight_ReturnsMatchingFlight()
        {
            var allFlights = fixture.CreateMany<Flight>().ToList();

            var matchingFlight = fixture.Build<Flight>()
                                .With(f => f.Price, 100.0m)
                                .Create();

            allFlights.Add(matchingFlight);

            mockFlightRepository.Setup(x => x.GetAllFlights()).Returns(allFlights);

            var sut = new FlightService(mockFlightRepository.Object);

            var searchCriteria = new FlightSearchCriteria
            {
                Price = 100.0m
            };

            var result = sut.FilterFlights(searchCriteria);
            Assert.Contains(matchingFlight, result);
        }
        [Fact]
        public void FilterFlights_WithMultipleCriteriaAndMultipleMatchingFlights_ReturnsMatchingFlights()
        {
            var allFlights = fixture.CreateMany<Flight>().ToList();

            var matchingFlight1 = fixture.Build<Flight>()
                .With(f => f.Price, 100.0m)
                .With(f => f.DestinationCountry, "USA")
                .With(f => f.FClass, FlightClass.Economy)
                .With(f => f.DepartureCountry, "PS")
                .Create();

            var matchingFlight2 = fixture.Build<Flight>()
                .With(f => f.Price, 100.0m)
                .With(f => f.DestinationCountry, "USA")
                .With(f => f.FClass, FlightClass.Economy)
                .With(f => f.DepartureCountry, "PS")
                .Create();


            allFlights.Add(matchingFlight1);
            allFlights.Add(matchingFlight2);

            mockFlightRepository.Setup(x => x.GetAllFlights()).Returns(allFlights);

            var searchCriteria = new FlightSearchCriteria
            {
                Price = 100.0m,
                DestinationCountry = "USA",
                FClass = FlightClass.Economy,
                DepartureCountry = "PS"
            };

            var result = sut.FilterFlights(searchCriteria);

            Assert.Equal(2, result.Count());
            Assert.Contains(matchingFlight1, result);
            Assert.Contains(matchingFlight2, result);
        }
    }
}
