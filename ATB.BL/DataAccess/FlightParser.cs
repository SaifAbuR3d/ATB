using ATB.Entities;
using CsvHelper;

public class FlightParser
{
    public static Flight ParseFlightFromCsv(CsvReader csvReader)
    {
        var flightId = int.Parse(csvReader.GetField(0));
        var price = decimal.Parse(csvReader.GetField(1));
        var departureCountry = csvReader.GetField(2);
        var destinationCountry = csvReader.GetField(3);
        var departureDate = DateTime.Parse(csvReader.GetField(4));
        var departureAirport = csvReader.GetField(5);
        var arrivalAirport = csvReader.GetField(6);
        var fClass = Enum.Parse<FlightClass>(csvReader.GetField(7), true);

        return new Flight(flightId, price, departureCountry, destinationCountry, departureDate, departureAirport, arrivalAirport, fClass);
    }
}
