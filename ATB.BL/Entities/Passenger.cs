namespace ATB.Entities
{
    public class Passenger
    {
      public int PassengerId { get; set; }
      public string PassengerName { get; set; }

        public Passenger(int passengerId, string passengerName)
        {
            PassengerId = passengerId;
            PassengerName = passengerName;
        }

        public override string ToString()
        {
            return $"PassengerId = {PassengerId}, PassengerName = {PassengerName}";
        }
        public override bool Equals(object? obj)
        {
            if (obj is null)
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
