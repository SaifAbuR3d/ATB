﻿using ATB.Entities;

namespace ATB.DataAccess
{
    internal interface IBookingRepository
    {
        void AddBooking(Passenger passenger, Flight flight, FlightClass flightClass);
        void AddBooking(Booking booking);
        void UpdateBookingClass(Booking booking, FlightClass newClass);
        void RemoveBooking(Booking booking);
        void RemoveBooking(Passenger passenger, Flight flight, FlightClass flightClass);
        IEnumerable<Booking> GetPassengerBookings(Passenger passenger);
        IEnumerable<Booking> GetPassengerBookings(int passengerId);
        IEnumerable<Booking> GetAllBookings();

    }
}
