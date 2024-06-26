﻿using System.Globalization;
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

            var currentRow = 1;

            while (csvReader.Read())
            {
                var validationResult = InputFlightDataValidation.ValidateFlightData(csvReader);

                if (validationResult.IsValid)
                {
                    var flight = FlightParser.ParseFlightFromCsvReader(csvReader);
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

            var flights = InputFlightDataValidation.GetConsistentFlights(flightGroups);

            return flights;
        }

        public static void AppendFlightsToCsv(string flightsFilePath, IEnumerable<Flight> flights)
        {
            if (flights.Count() == 0)
            {
                return;
            }
            using (StreamWriter writer = new StreamWriter(flightsFilePath, true, Encoding.UTF8))  // append the flights to file
            {
                foreach (Flight flight in flights)
                {
                    var flightLine = flight.GetFlightLine();

                    writer.WriteLine(flightLine);
                }

                Console.WriteLine("Flights added successfully!");
            }
        }
    }
}
