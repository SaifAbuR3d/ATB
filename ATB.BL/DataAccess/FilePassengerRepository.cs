using ATB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATB.DataAccess
{
    internal class FilePassengerRepository : IFilePassengerRepository
    {
        private string passengersFilePath = "files/Passengers.txt";

        private Dictionary<int, Passenger> passengerDictionary;

        public FilePassengerRepository()
        {
            InitializePassengerDictionary();

        }


        public Passenger GetPassengerById(int passengerId) // TODO : if the key is not here ?  -> maybe handle that at PassengerService
        {
            return passengerDictionary[passengerId];
        }

        private void InitializePassengerDictionary()
        {
            passengerDictionary = new Dictionary<int, Passenger>();

            try
            {
                string[] lines = File.ReadAllLines(passengersFilePath);

                foreach (string line in lines)
                {
                    string[] values = line.Split(',');

                    if (values.Length >= 2 && int.TryParse(values[0], out int id))
                    {
                        string name = values[1];

                        passengerDictionary[id] = new Passenger(id, name);
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An error occurred while reading flights from the file: {ex.Message}");
            }
        }
    }
}
