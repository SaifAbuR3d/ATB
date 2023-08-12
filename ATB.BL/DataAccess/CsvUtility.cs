using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ATB.Entities;
using CsvHelper;
using CsvHelper.Configuration;

namespace ATB.DataAccess
{

    internal class FlightValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
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
            var flights = new List<Flight>();
            int currentIndex = 1; 
            while (csvReader.Read())
            {
                FlightValidationResult validationResult = ValidateFlightData(csvReader);

                if (validationResult.IsValid)
                {
                    // the below code is safe. all fields are parsable  
                    var flightId = int.Parse(csvReader.GetField(0));
                    var price = decimal.Parse(csvReader.GetField(1));
                    var departureCountry = csvReader.GetField(2);
                    var destinationCountry = csvReader.GetField(3);
                    var departureDate = DateOnly.Parse(csvReader.GetField(4));
                    var departureAirport = csvReader.GetField(5);
                    var arrivalAirport = csvReader.GetField(6);
                    var fClass = Enum.Parse<FlightClass>(csvReader.GetField(7));

                    Flight flight = new Flight(flightId, price, departureCountry, destinationCountry, departureDate, departureAirport, arrivalAirport, fClass);
                    flights.Add(flight);
                }
                else
                {
                    Console.WriteLine($"Error At Line {currentIndex}"); 
                    foreach (string error in validationResult.Errors)
                    {
                        Console.WriteLine($"Validation Error: {error}");
                    }
                    Console.WriteLine(); 
                }
                currentIndex++;
            }

            return flights;
        }

        private static FlightValidationResult ValidateFlightData(CsvReader csvReader)
        {
            FlightValidationResult validationResult = new FlightValidationResult();
            // Validate flightId uniqueness  ---- TODO check the uniqueness using the dictionary for better performance
            var existingFlights = ReadFlightsFromCsv("files/Flights.csv");

            if (!int.TryParse(csvReader.GetField(0), out int flightId) || existingFlights.Any(flight => flight.FlightId == flightId))
            {
                validationResult.Errors.Add($"Invalid Flight ID \"{csvReader.GetField(0)}\": Must be unique integer");
            }

            // Validate price
            if (!decimal.TryParse(csvReader.GetField(1), out decimal price) || price < 1.0m || price > 20000.0m)
            {
                validationResult.Errors.Add($"Invalid Price \"{csvReader.GetField(1)}\": Must be a nonnegative decimal in range (1.0 to 20000.0)");
            }

            // Validate non-empty alphabetical strings
            // used null-forgiving operator
            if (string.IsNullOrWhiteSpace(csvReader.GetField(2)) || !csvReader.GetField(2)!.All(char.IsLetter))
            {
                validationResult.Errors.Add($"Invalid departure country \"{csvReader.GetField(2) ?? String.Empty}\": Must be non-empty alphabetical string");
            }
            if (string.IsNullOrWhiteSpace(csvReader.GetField(3)) || !csvReader.GetField(3)!.All(char.IsLetter))
            {
                validationResult.Errors.Add($"Invalid destination country \"{csvReader.GetField(3) ?? String.Empty}\": Must be non-empty alphabetical string");
            }
            if (string.IsNullOrWhiteSpace(csvReader.GetField(5)) || !csvReader.GetField(5)!.All(char.IsLetter))
            {
                validationResult.Errors.Add($"Invalid departure airport \"{csvReader.GetField(5) ?? String.Empty}\": Must be non-empty alphabetical string");
            }
            if (string.IsNullOrWhiteSpace(csvReader.GetField(6)) || !csvReader.GetField(6)!.All(char.IsLetter))
            {
                validationResult.Errors.Add($"Invalid arrival airport \"{csvReader.GetField(6) ?? String.Empty}\": Must be non-empty alphabetical string");
            }

            // Validate departure date range
            var now = DateOnly.FromDateTime(DateTime.Now);
            var oneYearFromNow = now.AddYears(1);
            if (!DateOnly.TryParse(csvReader.GetField(4), out DateOnly departureDate) || departureDate < now || departureDate > oneYearFromNow)
            {
                validationResult.Errors.Add($"Invalid departure date \"{csvReader.GetField(4)}\": Must be in range ({now} to {oneYearFromNow})");
            }

            // Validate fClass
            if (!Enum.TryParse<FlightClass>(csvReader.GetField(7), out _))
            {
                validationResult.Errors.Add($"Invalid Flight Class \"{csvReader.GetField(7) ?? String.Empty}\": Must be one of (Economy, Business, First Class)");
            }

            validationResult.IsValid = (validationResult.Errors.Count == 0);
            return validationResult;
        }


        public static IEnumerable<Flight> ReadFlightsFromCsv(string csvFilePath) // without validation  (data is checked and valid)
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
                var fClass = Enum.Parse<FlightClass>(csvReader.GetField(7));

                Flight flight = new Flight(flightId, price, departureCountry, destinationCountry, departureDate, DepartureAirport, arrivalAirport, fClass);
                flights.Add(flight);
            }
            return flights;
        }

        public static void WriteFlightsToCsv(string flightsFilePath, IEnumerable<Flight> flights)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(flightsFilePath, true, Encoding.UTF8))
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
