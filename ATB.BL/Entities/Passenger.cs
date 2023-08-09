namespace ATB.Entities
{
    internal class Passenger : User
    {
      public int PassengerId { get; set; }
      public string PassengerName { get; set; }
      public HashSet<Booking> Bookings { get; set; } = new HashSet<Booking>();

        public Passenger(int passengerId, string passengerName)
        {
            PassengerId = passengerId;
            PassengerName = passengerName;
        }
    }
}
