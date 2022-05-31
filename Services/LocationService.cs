

using test_app.Data;
using test_app.Models;
using Microsoft.EntityFrameworkCore;

namespace test_app.Services;

public class LocationService
{
    private readonly ProjectContext _context;

    public LocationService(ProjectContext context)
    {
        _context = context;
    }

    public IEnumerable<Location> GetAll()
    {
        return _context.locations.AsNoTracking().ToList();
    }

    public Location? GetById(int id)
    {
        return _context.locations.AsNoTracking().SingleOrDefault(p => p.id == id);
    }

    public Location Create(Location location)
    {
        _context.locations.Add(location);
        _context.SaveChanges();

        return location;
    }

    public void DeleteById(int id)
    {
        var location = _context.locations.Find(id);
        if (location is not null)
        {
            _context.locations.Remove(location);
            _context.SaveChanges();
        }
    }

    public void Update(int id, string city)
    {
        var locationToUpdate = _context.locations.Find(id);

        if (locationToUpdate is null)
        {
            throw new InvalidOperationException("Location not found");
        }

        locationToUpdate.city = city;

        _context.SaveChanges();
    }
}