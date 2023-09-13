using ATB.Entities;
using CsvHelper;

public class FlightParser
{
    public static Flight ParseFlightFromCsvReader(CsvReader csvReader)
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

    public static Flight ParseFlightFromStrings(string[] values)
    {
        int id = int.Parse(values[0]);
        decimal price = decimal.Parse(values[1]);
        string departureCountry = values[2];
        string destinationCountry = values[3];
        DateTime departureDate = DateTime.Parse(values[4]);
        string departureAirport = values[5];
        string arrivalAirport = values[6];
        FlightClass fClass = Enum.Parse<FlightClass>(values[7], true);

        return new Flight
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
