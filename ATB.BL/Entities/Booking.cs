using System.Diagnostics;

namespace ATB.Entities
{
    internal class Booking
    {
        public Passenger passenger { get; set; }
        public Flight flight { get; set; }
        public  DateTime ReservationDate { get; } = DateTime.Now;
        public Booking(Flight? _flight, Passenger? _passenger )
        {
            flight = _flight ?? throw new ArgumentNullException( nameof( _flight ) );
            passenger = _passenger ?? throw new ArgumentNullException(nameof(_passenger));
        }

        public override string ToString()
        {
            return $"Passenger: {passenger}\nFlight: {flight}\n At: {ReservationDate}";
        }
        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj is not Booking)
            {
                return false;
            }
            return flight.Equals( ((Booking)obj ).flight ) &&
                passenger.Equals( ((Booking)obj ).passenger );
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
