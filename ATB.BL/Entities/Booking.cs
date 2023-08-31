namespace ATB.Entities
{
    internal class Booking
    {
        public Passenger Passenger { get; set; }
        public Flight Flight { get; set; }
        public FlightClass FClass { get; set; }
        public  DateTime ReservationDate { get; } = DateTime.Now;
        public Booking(Flight flight, Passenger passenger, FlightClass fClass)
        {
            Flight = flight;
            Passenger = passenger;
            FClass = fClass;
        }

        public override string ToString()
        {
            return $"Passenger: {Passenger}\nFlight: {Flight} with Class {FClass} \n At: {ReservationDate}";
        }
        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }
            if (obj is not Booking)
            {
                return false;
            }
            return Flight.Equals( ((Booking)obj ).Flight ) &&
                Passenger.Equals( ((Booking)obj ).Passenger );
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
