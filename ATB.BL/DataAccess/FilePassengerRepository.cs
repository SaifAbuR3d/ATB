using ATB.Entities;

namespace ATB.DataAccess
{
    internal class FilePassengerRepository : IPassengerRepository
    {
        private string passengersFilePath = "files/Passengers.txt";

        private Dictionary<int, Passenger> passengerDictionary;

        public FilePassengerRepository()
        {
            InitializePassengerDictionary();

        }


        public Passenger? GetPassenger(int passengerId) // returns null if the passenger is not in the dictionary
        {
            if (passengerDictionary.TryGetValue((passengerId), out Passenger passenger))
            {
                return passenger;
            }
            return null;
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
