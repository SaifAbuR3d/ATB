using ATB.Entities;

namespace ATB.DataAccess
{
    internal interface IFilePassengerRepository
    {
        Passenger GetPassengerById(int passengerId);
    }
}