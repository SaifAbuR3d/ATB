namespace ATB.Entities
{
    internal class Passenger : User
    {
      public int PassengerId { get; set; }
      public string PassengerName { get; set; }
      public IEnumerable<Booking> Bookings { get; set; }

        public Passenger(int passengerId, string passengerName)
        {
            PassengerId = passengerId;
            PassengerName = passengerName;
            Bookings = new HashSet<Booking>();
        }
    }
}
