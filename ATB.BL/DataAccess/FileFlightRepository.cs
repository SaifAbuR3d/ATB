using ATB.Entities;

namespace ATB.DataAccess
{
    internal class FileFlightRepository : IFlightRepository
    {
        private string flightsFilePath = "files/Flights.csv";

        private Dictionary<(int, FlightClass), Flight> flightDictionary = new();   // (id,class) -> Flight       

        public FileFlightRepository()
        {
            InitializeFlightDictionary();
        }

        public void AddAllFlights(IEnumerable<Flight> flights)
        {

            CsvUtility.AppendFlightsToCsv(flightsFilePath, flights);

        }
        public IEnumerable<Flight> GetAllFlights()
        {
            return flightDictionary.Values;
        }

        public Flight? GetFlight(int flightId, FlightClass flightClass) // returns null if the Flight is not in the dictionary
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
                    Flight flight = FlightParser.ParseFlightFromStrings(values);
                    flightDictionary[(flight.FlightId, flight.FClass)] = flight;
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An error occurred while reading flights from the file: {ex.Message}");
            }
        }
    }
}
