
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using test_app.Models;
using test_app.Services;

namespace test_app.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
[Authorize(Roles = "ADMIN")]
public class UserController : ControllerBase
{

    private readonly UserService _service;
    private readonly AuthController _authController;
    public UserController(UserService service, AuthController authController)
    {
        _service = service;
        _authController = authController;
    }

    [HttpGet]
    public IEnumerable<User> GetAll() => _service.GetAll();

    [HttpGet("{id}")]
    public ActionResult<User> GetByID(int id)
    {
        var user = _service.GetById(id);
        if (user is null)
        {
            return NotFound();
        }
        return user;
    }

    [HttpPost]
    public IActionResult Create(UserDto userDto)
    {
        int[] roleValues = (int[])System.Enum.GetValues(typeof(roles));
        if (!Array.Exists<int>(roleValues, element => element == ((int)userDto.role)))
        {
            return BadRequest("Wrong role ID");
        }
        if (_service.GetByUsername(userDto.username) is null)
        {
            var user = new User
            {
                username = userDto.username,
                role = userDto.role
            };
            using (var hmac = new HMACSHA512())
            {
                user.passwordSalt = hmac.Key;
                user.passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(userDto.password));
            }
            _service.Create(user);
            return CreatedAtAction(nameof(Create), new { id = user.id }, user);
        }
        return Conflict("User with this username exists");
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, UserDto userDto)
    {
        var userToUpdate = _service.GetById(id);
        int[] roleValues = (int[])System.Enum.GetValues(typeof(roles));
        if (!Array.Exists<int>(roleValues, element => element == ((int)userDto.role)))
        {
            return BadRequest("Wrong role ID");
        }
        if (userToUpdate is null)
        {
            return NotFound("User not found");
        }
        var user = GetWithUsername(userDto.username);
        if (user is not null && user.id != id)
        {
            return BadRequest("User with this username exists");
        }
        _service.Update(id, userDto.username, userDto.role);
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var user = _service.GetById(id);
        if (user is null)
        {
            return NotFound();
        }

        _service.DeleteById(id);

        return Ok();
    }

    private User? GetWithUsername(string username)
    {
        return _service.GetByUsername(username);
    }
}