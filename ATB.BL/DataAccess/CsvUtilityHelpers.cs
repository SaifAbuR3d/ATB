using ATB.DataAccess;
using ATB.Entities;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

internal static class CsvUtilityHelpers
{

    internal static IEnumerable<Flight> GetValidFlightGroups(IEnumerable<Flight> flights)
    {
        return flights
                .GroupBy(flight => flight.FlightId)
                .Where(group => group.Select(flight => flight.FClass).Distinct().Count() == 3)
                .SelectMany(group => group);
    }

    internal static void PrintFlightsWithMissingFlightClasses(IEnumerable<Flight> flightsWithThreeDifferentClasses, List<Flight> flights)
    {
        var originalFlightIds = flights.Select(flight => flight.FlightId).Distinct();
        var filteredFlightIds = flightsWithThreeDifferentClasses.Select(flight => flight.FlightId).Distinct();


        var excludedFlightIds = originalFlightIds.Except(filteredFlightIds);
        foreach (var flight in excludedFlightIds)
        {
            Console.WriteLine($"Error With Flight {flight}");
            Console.WriteLine($"Validation Error: Flight Must have three different classes (Economy, Business, First Class).");
        }

    }

    internal static FlightValidationResult ValidateFlightData(CsvReader csvReader)
    {
        FlightValidationResult validationResult = new FlightValidationResult();
        // Validate flightId uniqueness  ---- TODO check the uniqueness using the dictionary for better performance
        var existingFlights = CsvUtility.ReadFlightsFromCsv("files/Flights.csv");

        if (int.TryParse(csvReader.GetField(0), out int flightId))
        {
            if (existingFlights.Any(flight => flight.FlightId == flightId))
                validationResult.Errors.Add($"Invalid Flight ID \"{csvReader.GetField(0)}\": Already exists in the system");
        }
        else
        {
            validationResult.Errors.Add($"Invalid Flight ID \"{csvReader.GetField(0)}\": Must be an integer");
        }


        // Validate price
        if (!decimal.TryParse(csvReader.GetField(1), out decimal price) || price < 1.0m || price > 20000.0m)
        {
            validationResult.Errors.Add($"Invalid Price \"{csvReader.GetField(1)}\": Must be a nonnegative decimal in range (1.0 to 20000.0)");
        }

        // Validate non-empty alphabetical strings
        // used null-forgiving operator
        if (string.IsNullOrWhiteSpace(csvReader.GetField(2)) || !csvReader.GetField(2)!.All(char.IsLetter) || csvReader.GetField(2)!.Length > 20)
        {
            validationResult.Errors.Add($"Invalid departure country \"{csvReader.GetField(2) ?? String.Empty}\": Must be non-empty alphabetical string with at most 20 characters");
        }
        if (string.IsNullOrWhiteSpace(csvReader.GetField(3)) || !csvReader.GetField(3)!.All(char.IsLetter) || csvReader.GetField(3)!.Length > 20)
        {
            validationResult.Errors.Add($"Invalid destination country \"{csvReader.GetField(3) ?? String.Empty}\": Must be non-empty alphabetical string with at most 20 characters");
        }
        if (string.IsNullOrWhiteSpace(csvReader.GetField(5)) || !csvReader.GetField(5)!.All(char.IsLetter) || csvReader.GetField(5)!.Length > 20)
        {
            validationResult.Errors.Add($"Invalid departure airport \"{csvReader.GetField(5) ?? String.Empty}\": Must be non-empty alphabetical string with at most 20 characters");
        }
        if (string.IsNullOrWhiteSpace(csvReader.GetField(6)) || !csvReader.GetField(6)!.All(char.IsLetter) || csvReader.GetField(6)!.Length > 20)
        {
            validationResult.Errors.Add($"Invalid arrival airport \"{csvReader.GetField(6) ?? String.Empty}\": Must be non-empty alphabetical string with at most 20 characters");
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
            validationResult.Errors.Add($"Invalid FlightClass \"{csvReader.GetField(7) ?? String.Empty}\": Must be one of (Economy, Business, First Class)");
        }

        validationResult.IsValid = (validationResult.Errors.Count == 0);
        return validationResult;
    }

        internal static void PrintFlightEntityErrors(int currentIndex, List<string> errors)
        {
            Console.WriteLine($"Error At Line {currentIndex}");
            foreach (string error in errors)
            {
                Console.WriteLine($"Validation Error: {error}");
            }
            Console.WriteLine();
        }
}

internal class FlightValidationResult
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; } = new List<string>();
}