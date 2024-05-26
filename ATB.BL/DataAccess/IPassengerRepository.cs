using ATB.Entities;

namespace ATB.DataAccess
{
    internal interface IPassengerRepository
    {
        Passenger? GetPassenger(int passengerId);
    }
}