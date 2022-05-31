using test_app.Models;
using test_app.Data;
using Microsoft.EntityFrameworkCore;

namespace test_app.Services;

public class UserService
{
    private readonly ProjectContext _context;

    public UserService(ProjectContext context)
    {
        _context = context;
    }

    public IEnumerable<User> GetAll()
    {
        return _context.users.AsNoTracking().ToList();
    }

    public User? GetById(int id)
    {
        return _context.users.AsNoTracking().SingleOrDefault(p => p.id == id);
    }

    public User? GetByUsername(string username)
    {
        return _context.users.AsNoTracking().SingleOrDefault(p => p.username.Equals(username));
    }

    public User Create(User user)
    {
        _context.users.Add(user);
        _context.SaveChanges();

        return user;
    }

    public void DeleteById(int id)
    {
        var user = _context.users.Find(id);
        if (user is not null)
        {
            _context.users.Remove(user);
            _context.SaveChanges();
        }
    }

    public void Update(int id, string username, roles role)
    {
        var userToUpdate = _context.users.Find(id);

        if (userToUpdate is null)
        {
            throw new InvalidOperationException("User not found");
        }

        userToUpdate.username = username;
        userToUpdate.role = role;

        _context.SaveChanges();
    }
}