﻿using MySpot.Api.Commands;
using MySpot.Api.DTO;
using MySpot.Api.Entities;
using MySpot.Api.Repositories;
using MySpot.Api.ValueObjects;

namespace MySpot.Api.Services
{
    public class ReservationsService : IReservationsService
    {
        private readonly IClock _clock;
        private readonly IWeeklyParkingSpotRepository _repository;

        public ReservationsService(IClock clock, IWeeklyParkingSpotRepository repository)
        {
            _clock = clock;
            _repository = repository;
        }

        public ReservationDto? Get(Guid id)
            => GetAllWeekly().SingleOrDefault(x => x.Id == id);

        public IEnumerable<ReservationDto> GetAllWeekly()
            => _repository.GetAll().SelectMany(x => x.Reservations)
            .Select(x => new ReservationDto
            {
                Id = x.Id,
                ParkingSpotId = x.ParkingSpotId,
                Employeename = x.EmployeeName,
                Date = x.Date.Value.Date
            });

        public Guid? Create(CreateReservation command)
        {
            var weeklyParkingSpot = _repository.Get(command.ParkingSpotId);
            if (weeklyParkingSpot is null)
            {
                return default;
            }

            var reservation = new Reservation(command.ReservationId, command.ParkingSpotId, command.EmployeeName, command.LicensePlate, new Date(command.Date));
            weeklyParkingSpot.AddReservation(reservation, new Date(_clock.Current()));

            return reservation.Id;
        }

        public bool Update(ChangeReservationLicensePlate command)
        {
            var weeklyParkingSpot = GetWeeklyParkingSpotByReservation(command.ReservationId);
            if (weeklyParkingSpot is null)
                return false;

            var existingReservation = weeklyParkingSpot.Reservations.SingleOrDefault(x => x.Id.Value == command.ReservationId);
            if (existingReservation is null)
                return false;

            if (existingReservation.Date <= new Date(_clock.Current()))
                return false;

            existingReservation.ChangeLicensePlate(command.LicensePlate);
            return true;
        }

        public bool Delete(DeleteReservation command)
        {
            var weeklyParkingSpot = GetWeeklyParkingSpotByReservation(command.ReservationId);
            if (weeklyParkingSpot is null)
                return false;

            var existingReservation = weeklyParkingSpot.Reservations.SingleOrDefault(x => x.Id.Value == command.ReservationId);
            if (existingReservation is null)
                return false;

            weeklyParkingSpot.RemoveReservation(command.ReservationId);
            return true;
        }

        private WeeklyParkingSpot? GetWeeklyParkingSpotByReservation(Guid reservationId)
           => _repository.GetAll()
                .SingleOrDefault(x => x.Reservations.Any(r => r.Id.Value == reservationId));
    }
}
