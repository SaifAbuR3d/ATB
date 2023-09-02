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
            var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = false
            };

            using var streamReader = File.OpenText(csvFilePath);
            using var csvReader = new CsvReader(streamReader, csvConfig);
            var flightGroups = new Dictionary<int, List<Flight>>();

            int currentRow = 1;

            while (csvReader.Read())
            {
                var validationResult = InputFlightDataValidation.ValidateFlightData(csvReader);

                if (validationResult.IsValid)
                {
                    // the below code is null-safe.
                    var flightId = int.Parse(csvReader.GetField(0));
                    var price = decimal.Parse(csvReader.GetField(1));
                    var departureCountry = csvReader.GetField(2);
                    var destinationCountry = csvReader.GetField(3);
                    var departureDate = DateOnly.Parse(csvReader.GetField(4));
                    var departureAirport = csvReader.GetField(5);
                    var arrivalAirport = csvReader.GetField(6);
                    var fClass = Enum.Parse<FlightClass>(csvReader.GetField(7), true);

                    var flight = new Flight(flightId, price, departureCountry, destinationCountry, departureDate, departureAirport, arrivalAirport, fClass);

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

        public static IEnumerable<Flight> ReadFlightsFromCsv(string csvFilePath)
        {
            var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = false
            };

            using var streamReader = File.OpenText(csvFilePath);
            using var csvReader = new CsvReader(streamReader, csvConfig);
            var flights = new List<Flight>();

            while (csvReader.Read())
            {
                var flightId = int.Parse(csvReader.GetField(0));
                var price = decimal.Parse(csvReader.GetField(1));
                var departureCountry = csvReader.GetField(2);
                var destinationCountry = csvReader.GetField(3);
                var departureDate = DateOnly.Parse(csvReader.GetField(4));
                var DepartureAirport = csvReader.GetField(5);
                var arrivalAirport = csvReader.GetField(6);
                var fClass = Enum.Parse<FlightClass>(csvReader.GetField(7), true);

                Flight flight = new Flight(flightId, price, departureCountry, destinationCountry, departureDate, DepartureAirport, arrivalAirport, fClass);
                flights.Add(flight);
            }
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
                        string flightLine = $"{flight.FlightId},{flight.Price},{flight.DepartureCountry},{flight.DestinationCountry}," +
                                            $"{flight.DepartureDate},{flight.DepartureAirport},{flight.ArrivalAirport},{flight.FClass}";

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
