using ATB.Entities;
using System.Text;

namespace ATB.DataAccess
{
    internal class FileFlightRepository : IFlightRepository
    {
        private string flightsFilePath = "files/Flights.csv";
        private Dictionary<int, Flight> flightDictionary;

        public FileFlightRepository()
        {
            InitializeFlightDictionary();
        }

        public void AddAllFlights(IEnumerable<Flight> flights)
        {

            CsvUtility.WriteFlightsToCsv(flightsFilePath, flights);

        }

        public void AddFlight(Flight flight)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Flight> GetAllFlights()
        {
            return CsvUtility.ParseFlightsFromCsv(flightsFilePath);
        }

        public Flight GetFlightById(int flightId) // TODO : if the key is not here ?
        {
            return flightDictionary[flightId];
        }
        private void InitializeFlightDictionary()
        {
            flightDictionary = new Dictionary<int, Flight>();

            try
            {
                string[] lines = File.ReadAllLines(flightsFilePath);

                foreach (string line in lines)
                {
                    string[] values = line.Split(',');

                    if (values.Length >= 8 && int.TryParse(values[0], out int id))
                    {
                        decimal price = decimal.Parse(values[1]);
                        string departureCountry = values[2];
                        string destinationCountry = values[3];
                        DateOnly departureDate = DateOnly.Parse(values[4]);
                        string departureAirport = values[5];
                        string arrivalAirport = values[6];
                        FlightClass fClass = Enum.Parse<FlightClass>(values[7]);

                        flightDictionary[id] = new Flight
                        {
                            FlightId = id,
                            Price = price,
                            DepartureCountry = departureCountry,
                            DestinationCountry = destinationCountry,
                            DepartureDate = departureDate,
                            DepartureAirport = departureAirport,
                            ArrivalAirport = arrivalAirport,
                            FClass = fClass
                        };
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An error occurred while reading flights from the file: {ex.Message}");
            }
        }
    }
}
