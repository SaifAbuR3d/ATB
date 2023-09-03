using ATB.DataAccess;
using ATB.Entities;
using CsvHelper;

internal static class InputFlightDataValidation
{

    internal static FlightValidationResult ValidateFlightData(CsvReader csvReader)
    {
        FlightValidationResult validationResult = new();

        if (!int.TryParse(csvReader.GetField(0), out int flightId))
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

        var now = DateTime.Now;
        var oneYearFromNow = now.AddYears(1);
        if (!DateTime.TryParse(csvReader.GetField(4), out DateTime departureDate) || departureDate < now || departureDate > oneYearFromNow)
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

    public static List<Flight> GetConsistentFlights(Dictionary<int, List<Flight>> flightGroups)
    {

        var allFlights = new FileFlightRepository().GetAllFlights();
        var ConsistentFlights = new List<Flight>();
        foreach (var flightGroup in flightGroups)
        {
            if (allFlights.Any(flight => flight.FlightId == flightGroup.Key))
            {
                Console.WriteLine($"Flight ID {flightGroup.Key} Already Exists.");
            }
            else if (flightGroup.Value.Count != 3)
            {
                Console.WriteLine($"Inconsistent data for Flight ID {flightGroup.Key}. Expected exactly 3 VALID Flights, but found {flightGroup.Value.Count}.");
            }
            else if (AreFlightsConsistent(flightGroup.Value))
            {
                ConsistentFlights.AddRange(flightGroup.Value);
            }
            else
            {
                Console.WriteLine($"Inconsistent data for Flight ID {flightGroup.Key}. Flights have different data or duplicated flight class.");
            }
        }

        return ConsistentFlights;
    }

    private static bool AreFlightsConsistent(List<Flight> flights)
    {
        // Check if ConsistentFlights have the same data but different FlightClasses
        var referenceFlight = flights[0];
        if (flights[0].FClass == flights[1].FClass || flights[1].FClass == flights[2].FClass)
        {
            return false;
        }
        return flights.All(flight =>
            flight.DepartureCountry == referenceFlight.DepartureCountry &&
            flight.DestinationCountry == referenceFlight.DestinationCountry &&
            flight.DepartureDate == referenceFlight.DepartureDate &&
            flight.DepartureAirport == referenceFlight.DepartureAirport &&
            flight.ArrivalAirport == referenceFlight.ArrivalAirport
            );
    }
}