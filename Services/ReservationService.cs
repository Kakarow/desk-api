


using Microsoft.EntityFrameworkCore;
using test_app.Data;
using test_app.Models;

namespace test_app.Services;

public class ReservationService
{
    private readonly ProjectContext _context;

    public ReservationService(ProjectContext context)
    {
        _context = context;
    }

    public IEnumerable<Reservation> GetAll()
    {
        return _context.reservations
        .AsNoTracking()
        .Where(p => p.reservationTime.AddDays(p.days) > DateTime.Now)
        .ToList();
    }

    public Reservation? GetById(int id)
    {
        return _context.reservations.AsNoTracking().SingleOrDefault(p => p.id == id);
    }

    public IEnumerable<Reservation> GetByDeskId(int id)
    {
        return _context.reservations
        .AsNoTracking()
        .Where(p => p.deskId == id)
        .Where(p => p.reservationTime.AddDays(p.days) > DateTime.Now)
        .ToList();
    }

    public IEnumerable<Reservation> GetByLocationId(int id)
    {
        return _context.reservations
        .AsNoTracking()
        .Where(p => p.desk.location.id == id)
        .Where(p => p.reservationTime.AddDays(p.days) > DateTime.Now)
        .ToList();
    }

    public IEnumerable<Reservation> GetByUserId(int id)
    {
        return _context.reservations
        .AsNoTracking()
        .Where(p => p.user.id == id)
        .Where(p => p.reservationTime.AddDays(p.days) > DateTime.Now)
        .ToList();
    }

    public IEnumerable<Reservation> GetByDeskAndDate(DateTime date, int days, int id)
    {
        return _context.reservations
        .AsNoTracking()
        .Where(p => p.deskId == id)
        .Where(p => ((p.reservationTime >= date) && (date.AddDays(days) >= p.reservationTime))
        || ((p.reservationTime <= date) && (p.reservationTime.AddDays(p.days) >= date)))
        .ToList();
    }

    public Reservation Create(Reservation reservation)
    {
        _context.reservations.Add(reservation);
        _context.SaveChanges();

        return reservation;
    }

    public void DeleteById(int id)
    {
        var reservation = _context.reservations.SingleOrDefault(p => p.id == id);
        if (reservation is not null)
        {
            _context.reservations.Remove(reservation);
            _context.SaveChanges();
        }
    }

    public void Change(int id, ReservationDto reservationDto)
    {
        var reservation = _context.reservations.SingleOrDefault(p => p.id == id);
        if (reservation is not null)
        {
            reservation.days = reservationDto.days;
            reservation.reservationTime = reservationDto.reservationTime;
            reservation.deskId = reservationDto.deskId;
            _context.SaveChanges();
        }
    }
}