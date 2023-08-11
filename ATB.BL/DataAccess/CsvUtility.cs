﻿using System;
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