

using Microsoft.EntityFrameworkCore;
using test_app.Data;
using test_app.Models;

namespace test_app.Services;

public class DeskService
{

    private readonly ProjectContext _context;

    public DeskService(ProjectContext context)
    {
        _context = context;
    }

    public IEnumerable<Desk> GetAll()
    {
        return _context.desks.AsNoTracking().ToList();
    }

    public Desk? GetById(int id)
    {
        return _context.desks.AsNoTracking().SingleOrDefault(p => p.id == id);
    }

    public IEnumerable<Desk> GetAvailable()
    {
        return _context.desks.AsNoTracking().Where(p => p.isAvailable == true);
    }

    public IEnumerable<Desk> GetUnavailable()
    {
        return _context.desks.AsNoTracking().Where(p => p.isAvailable == false);
    }

    public Desk Create(Desk desk)
    {
        _context.desks.Add(desk);
        _context.SaveChanges();

        return desk;
    }

    public void DeleteById(int id)
    {
        var desk = _context.desks.Find(id);
        if (desk is not null)
        {
            _context.desks.Remove(desk);
            _context.SaveChanges();
        }
    }

    public void Change(int id, int locationId, bool isAvailable)
    {
        var deskToUpdate = _context.desks.SingleOrDefault(p => p.id == id);
        var locationToCheck = _context.locations.SingleOrDefault(p => p.id == locationId);

        if ((deskToUpdate is not null) && (locationToCheck is not null))
        {
            deskToUpdate.isAvailable = isAvailable;
            deskToUpdate.locationId = locationId;
            _context.SaveChanges();
        }
    }

    public IEnumerable<Desk> GetByLocationId(int id)
    {
        return _context.desks.AsNoTracking().Where(p => p.locationId == id).ToList();
    }
}