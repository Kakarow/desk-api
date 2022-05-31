

using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using test_app.Models;
using test_app.Services;

[ApiController]
[Route("/api/v1/[controller]")]
[Authorize]
public class ReservationController : ControllerBase
{

    private readonly ReservationService _service;
    private readonly UserService _userService;
    private readonly DeskService _deskService;
    private readonly LocationService _locationService;
    public ReservationController(ReservationService service, UserService userService, DeskService deskService, LocationService locationService)
    {
        _service = service;
        _userService = userService;
        _deskService = deskService;
        _locationService = locationService;
    }

    [HttpGet]
    [Authorize(Roles = "ADMIN")]
    public IEnumerable<Reservation> GetAll() => _service.GetAll();

    [HttpGet("{id}")]
    [Authorize(Roles = "ADMIN")]
    public ActionResult<Reservation> GetById(int id)
    {
        var reservation = _service.GetById(id);
        if (reservation is null)
        {
            return NotFound("Reservation not found");
        }
        return reservation;
    }
    [Authorize(Roles = "ADMIN")]
    [HttpGet("GetByLocationId/{id}")]
    public IEnumerable<Reservation> GetByLocationId(int id) => _service.GetByLocationId(id);

    [Authorize(Roles = "ADMIN")]
    [HttpGet("GetByUserId/{id}")]
    public IEnumerable<Reservation> GetByUserIn(int id) => _service.GetByUserId(id);

    [HttpPost]
    public IActionResult Create(ReservationDto reservationDto)
    {
        var desk = _deskService.GetById(reservationDto.deskId);
        var reservations = _service.GetByDeskAndDate(reservationDto.reservationTime, reservationDto.days, reservationDto.deskId);
        if (desk is null)
        {
            return BadRequest("Desk not found");
        }
        if (reservations.Count() != 0)
        {
            return BadRequest("Desk is reserved that days");
        }
        if (desk is null || !desk.isAvailable || reservationDto.days > 7)
        {
            BadRequest("Can't create reservation");
        }
        var name = DecodeJwt(Request.Headers.Authorization);
        var user = _userService.GetByUsername(name);

        var reservation = new Reservation
        {
            userId = user.id,
            deskId = desk.id,
            days = reservationDto.days,
            reservationTime = reservationDto.reservationTime
        };
        _service.Create(reservation);
        return CreatedAtAction(nameof(Create), new { id = reservation.id }, reservation);
    }

    [HttpDelete]
    [Authorize]
    public IActionResult DeleteById(int id)
    {
        var reservation = _service.GetById(id);
        var name = DecodeJwt(Request.Headers.Authorization);
        var user = _userService.GetByUsername(name);
        if (reservation is null)
        {
            return NotFound("Reservation not found");
        }
        if (!user.username.Equals(name))
        {
            return Forbid("Not yours reservation");
        }
        _service.DeleteById(id);

        return Ok();
    }

    [HttpPut("{id}")]
    public ActionResult Change(int id, ReservationDto reservationDto)
    {
        var reservation = _service.GetById(id);
        var name = DecodeJwt(Request.Headers.Authorization);
        var user = _userService.GetByUsername(name);
        if (reservation is null)
        {
            return NotFound("Reservation not found");
        }
        if (!user.username.Equals(name))
        {
            return Forbid("Not yours reservation");
        }
        _service.Change(id, reservationDto);

        return Ok("Reservation updated");
    }

    private string DecodeJwt(string jwt)
    {
        jwt = jwt.Split(" ")[1];
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(jwt);
        var token = jsonToken as JwtSecurityToken;
        return token.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
    }
}