using ATB.DataAccess;
using ATB.Entities;
using CsvHelper;

internal static class CsvUtilityHelpers
{

    internal static IEnumerable<Flight> GetValidFlightGroups(IEnumerable<Flight> flights)  // should this method be at Services ?
    {
        return flights
                .GroupBy(flight => flight.FlightId)
                .Where(group => group.Select(flight => flight.FClass).Distinct().Count() == 3)
                .SelectMany(group => group);
    }

    internal static void PrintFlightsWithMissingFlightClasses(IEnumerable<Flight> flightsWithThreeDifferentClasses, List<Flight> originalFlights)
    {
        var originalFlightsIds = originalFlights.Select(flight => flight.FlightId).Distinct();
        var filteredFlightsIds = flightsWithThreeDifferentClasses.Select(flight => flight.FlightId).Distinct();


        var excludedFlightsIds = originalFlightsIds.Except(filteredFlightsIds);
        foreach (var flight in excludedFlightsIds)
        {
            Console.WriteLine($"Error With Flight {flight}");
            Console.WriteLine($"Validation Error: Flight Must have three different classes (Economy, Business, First Class).");
        }

    }

    internal static FlightValidationResult ValidateFlightData(CsvReader csvReader)
    {
        FlightValidationResult validationResult = new FlightValidationResult();

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

        if (!decimal.TryParse(csvReader.GetField(1), out decimal price) || price < 1.0m || price > 20000.0m)
        {
            validationResult.Errors.Add($"Invalid Price \"{csvReader.GetField(1)}\": Must be a nonnegative decimal in range (1.0 to 20000.0)");
        }

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

        var now = DateOnly.FromDateTime(DateTime.Now);
        var oneYearFromNow = now.AddYears(1);
        if (!DateOnly.TryParse(csvReader.GetField(4), out DateOnly departureDate) || departureDate < now || departureDate > oneYearFromNow)
        {
            validationResult.Errors.Add($"Invalid departure date \"{csvReader.GetField(4)}\": Must be in range ({now} to {oneYearFromNow})");
        }

        if (!Enum.TryParse<FlightClass>(csvReader.GetField(7), true, out _))
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