using Microsoft.EntityFrameworkCore;
using test_app.Models;

namespace test_app.Data;

public class ProjectContext : DbContext
{
    public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
    {
    }

    public DbSet<User> users => Set<User>();
    public DbSet<Location> locations => Set<Location>();
    public DbSet<Desk> desks => Set<Desk>();
    public DbSet<Reservation> reservations => Set<Reservation>();
}