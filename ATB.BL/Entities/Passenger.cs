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

        public override string ToString()
        {
            return $"PassengerId = {PassengerId}, PassengerName = {PassengerName}";
        }
        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj is not Passenger)
            {
                return false;
            }
            return PassengerId.Equals( ((Passenger)obj ).PassengerId);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }

}
