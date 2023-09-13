using System.Globalization;
using System.Text;
using ATB.Entities;
using CsvHelper;
using CsvHelper.Configuration;

namespace ATB.DataAccess
{
    internal static class CsvUtility
    {
        public static IEnumerable<Flight> ParseFlightsFromCsv(string csvFilePath)
        {

            using var csvReader = CsvReaderFactory.CreateCsvReader(csvFilePath, false); 

            var flightGroups = new Dictionary<int, List<Flight>>();

            int currentRow = 1;

            while (csvReader.Read())
            {
                var validationResult = InputFlightDataValidation.ValidateFlightData(csvReader);

                if (validationResult.IsValid)
                {
                    var flight = FlightParser.ParseFlightFromCsv(csvReader);
                    var flightId = flight.FlightId; 

                    if (!flightGroups.ContainsKey(flightId))
                    {
                        flightGroups[flightId] = new List<Flight>();
                    }
                    flightGroups[flightId].Add(flight);
                }
                else
                {
                    CsvUtilityHelpers.PrintFlightRowErrors(currentRow, validationResult.Errors);
                }
                currentRow++;
            }

            List<Flight> flights = InputFlightDataValidation.GetConsistentFlights(flightGroups);

            return flights;
        }

        public static void AppendFlightsToCsv(string flightsFilePath, IEnumerable<Flight> flights)
        {
            if (flights.Count() == 0)
            {
                return; 
            }
            try 
            {
                using (StreamWriter writer = new StreamWriter(flightsFilePath, true, Encoding.UTF8))  // append the flights to file
                {
                    foreach (Flight flight in flights)
                    {
                        string flightLine = flight.GetFlightLine();

                        writer.WriteLine(flightLine);
                    }

                    Console.WriteLine("Flights added successfully!");
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An error occurred while writing flights to the file: {ex.Message}");
            }
        }
    }
}
