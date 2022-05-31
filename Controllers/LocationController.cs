


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using test_app.Models;
using test_app.Services;

namespace test_app.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
[Authorize]
public class LocationController : ControllerBase
{

    private readonly LocationService _service;
    private readonly DeskService _deskService;

    public LocationController(LocationService service, DeskService deskService)
    {
        _service = service;
        _deskService = deskService;
    }

    [HttpGet]
    public IEnumerable<Location> GetAll() => _service.GetAll();

    [HttpGet("{id}")]
    public ActionResult<Location> GetById(int id)
    {
        var location = _service.GetById(id);
        if (location is null)
        {
            return NotFound("Location not found");
        }
        return location;
    }

    [HttpPost]
    public IActionResult Create(LocationDto locationDto)
    {
        Location location = new Location { city = locationDto.city };
        _service.Create(location);
        return CreatedAtAction(nameof(Create), new { id = location.id }, location);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, LocationDto locationDto)
    {
        var locationToUpdate = _service.GetById(id);

        if (locationToUpdate is null)
        {
            return NotFound("Location not found");
        }

        _service.Update(id, locationDto.city);
        return Ok("City updated");
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var location = _service.GetById(id);
        var desks = _deskService.GetByLocationId(id);
        if (location is null)
        {
            return NotFound("Location not found");
        }
        else if (desks.Count() != 0)
        {
            return Conflict("Can't delete non-empty locations");
        }

        _service.DeleteById(id);

        return Ok("Location deleted");
    }
}