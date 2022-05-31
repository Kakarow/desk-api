


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using test_app.Models;
using test_app.Services;

[ApiController]
[Route("/api/v1/[controller]")]
//[Authorize(Roles = "Admin")]
public class DeskController : ControllerBase
{

    private readonly DeskService _service;
    private readonly LocationService _locationService;
    private readonly ReservationService _reservationService;
    public DeskController(DeskService service, LocationService locationservice, ReservationService reservationService)
    {
        _service = service;
        _locationService = locationservice;
        _reservationService = reservationService;
    }


    [HttpGet]
    [Authorize(Roles = "ADMIN")]
    public IEnumerable<Desk> GetAll() => _service.GetAll();

    [HttpGet("{id}")]
    [Authorize(Roles = "ADMIN")]
    public ActionResult<Desk> GetById(int id)
    {
        var desk = _service.GetById(id);
        if (desk is null)
        {
            return NotFound("Desk not found");
        }
        return desk;
    }

    [HttpGet("getByLocation/{id}")]
    [Authorize]
    public IEnumerable<Desk> GetByLocationId(int id)
    {
        return _service.GetByLocationId(id);
    }

    [HttpGet("GetAvailable")]
    [Authorize]
    public IEnumerable<Desk> GetAvailable()
    {
        return _service.GetAvailable();
    }

    [HttpGet("GetUnavailable")]
    [Authorize]
    public IEnumerable<Desk> GetUnavailable()
    {
        return _service.GetUnavailable();
    }

    [HttpPost]
    [Authorize(Roles = "ADMIN")]
    public IActionResult Create(DeskDto deskDto)
    {
        var location = _locationService.GetById(deskDto.locationId);
        if (location is null)
        {
            return NotFound("Location not found");
        }
        var desk = new Desk { isAvailable = deskDto.isAvailable, locationId = deskDto.locationId };
        _service.Create(desk);
        return CreatedAtAction(nameof(Create), new { id = desk.id }, desk);
    }

    [HttpDelete]
    [Authorize(Roles = "ADMIN")]
    public IActionResult DeleteById(int id)
    {
        var desk = _service.GetById(id);
        if (desk is null)
        {
            return NotFound("Desk not found");
        }

        var reservations = _reservationService.GetByDeskId(id);
        if (reservations.Count() == 0)
        {
            _service.DeleteById(id);
            return Ok("Desk deleted");
        }

        return Conflict("Can't delete, desk has reservations");
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "ADMIN")]
    public IActionResult Change(DeskDto deskDto, int id)
    {
        var deskToChange = _service.GetById(id);
        var locationToCheck = _locationService.GetById(deskDto.locationId);
        if (deskToChange is null || locationToCheck is null)
        {
            return NotFound("Location/desk not exist");
        }
        _service.Change(id, deskDto.locationId, deskDto.isAvailable);
        return Ok("Desk changed");
    }
}