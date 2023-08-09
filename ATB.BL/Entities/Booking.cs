namespace ATB.Entities
{
    internal class Booking
    {
        public Flight flight { get; set; }
        public Passenger passenger { get; set; }
        public Booking(Flight? _flight, Passenger? _passenger )
        {
            flight = _flight ?? throw new ArgumentNullException( nameof( _flight ) );
            passenger = _passenger ?? throw new ArgumentNullException(nameof(_passenger));
        }


    }
}
