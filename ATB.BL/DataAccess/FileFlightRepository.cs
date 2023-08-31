using ATB.Entities;

namespace ATB.DataAccess
{
    internal class FileFlightRepository : IFlightRepository
    {
        private string flightsFilePath = "files/Flights.csv";

        private Dictionary<(int, FlightClass), Flight> flightDictionary;   // (id,class) -> flight

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
            return CsvUtility.ReadFlightsFromCsv(flightsFilePath);
        }

        public Flight? GetFlight(int flightId, FlightClass flightClass) // returns null if the flight is not in the dictionary
        {
            if(flightDictionary.TryGetValue((flightId, flightClass), out Flight flight))
            {
                return flight;
            }
            return null; 
        }
        private void InitializeFlightDictionary()
        {
            flightDictionary = new Dictionary<(int, FlightClass), Flight>();

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
                        FlightClass fClass = Enum.Parse<FlightClass>(values[7],true); // true to ignore case 

                        flightDictionary[(id, fClass)] = new Flight
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
