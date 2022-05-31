
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using test_app.Models;
using test_app.Services;

namespace test_app.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;

        public AuthController(IConfiguration configuration, UserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [HttpPost("register")]
        public ActionResult<User> Register(Auth auth)
        {

            CreatePasswordHash(auth.password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User();
            if (_userService.GetByUsername(auth.username) is null)
            {
                user.passwordHash = passwordHash;
                user.passwordSalt = passwordSalt;

                user.username = auth.username;
                user.role = Models.roles.EMPLOYEE;
                _userService.Create(user);
                return Ok(user);
            }
            return BadRequest("User with this username exists");
        }

        [HttpPost("login")]
        public ActionResult<string> Login(Auth auth)
        {
            var user = _userService.GetByUsername(auth.username);
            if (user is null)
            {
                return BadRequest("User not found");
            }

            if (!VerifyPasswordHash(auth.password, user.passwordHash, user.passwordSalt))
            {
                return BadRequest("Wrong password");
            }
            string token = CreateToken(user);
            return Ok(token);
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.username),
                new Claim(ClaimTypes.Role, user.role.ToString())
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            var jwd = new JwtSecurityTokenHandler().WriteToken(token);
            return jwd;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var ComputeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return ComputeHash.SequenceEqual(passwordHash);
            }
        }
    }
}