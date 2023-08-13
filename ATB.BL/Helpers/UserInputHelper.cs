using ATB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATB.Helpers
{
    // can add (optional) error messages and further input (range) validation
    public static class UserInputHelper
    {

        public static decimal GetValidDecimal(string promptMessage = "", string errorMessage = "Invalid input. Please enter a valid decimal.")
        {
            decimal result = 0;
            bool isValidInput = false;

            while (!isValidInput)
            {
                Console.Write(promptMessage);
                string? input = Console.ReadLine();

                if (decimal.TryParse(input, out result))
                {
                    isValidInput = true;
                }
                else
                {
                    Console.WriteLine(errorMessage);
                }
            }

            return result;
        }
        public static int GetValidInt(string promptMessage="", string errorMessage = "Invalid input. Please enter a valid integer.")
        {
            int result = 0;
            bool isValidInput = false;

            while (!isValidInput)
            {
                Console.Write(promptMessage);
                string? input = Console.ReadLine();

                if (int.TryParse(input, out result))
                {
                    isValidInput = true;
                }
                else
                {
                    Console.WriteLine(errorMessage);
                }
            }

            return result;
        }


        public static string GetValidString(string promptMessage="", string errorMessage = "Invalid input. Please enter a valid string.")
        {
            bool isValidInput = false;
            string result = String.Empty; 
            while (!isValidInput)
            {
                Console.Write(promptMessage);
                string? input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    isValidInput = true;
                    result = input; 
                }
                else
                {
                    Console.WriteLine(errorMessage);
                }
            }
            return result; 
        }

        internal static FlightClass GetValidFlightClass(string promptMessage = "", string errorMessage = "Invalid input. Please enter a valid Flight Class.")
        {
            FlightClass result = default(FlightClass); 
            bool isValidInput = false;

            while (!isValidInput)
            {
                Console.Write(promptMessage);
                string? input = Console.ReadLine();

                if (Enum.TryParse<FlightClass>(input,true, out result))
                {
                    isValidInput = true;
                }
                else
                {
                    Console.WriteLine(errorMessage);
                }
            }

            return result;
        }
    }
}