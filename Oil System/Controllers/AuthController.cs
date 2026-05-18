using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Oil_System.Contract.Response.Authentcation;
using Oil_System.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Oil_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                return BadRequest("Email and password are required.");

            if (request.Email != "admin@test.com" || request.Password != "123456")
                return Unauthorized("Invalid email or password");

            var expireAt = DateTime.Now.AddMinutes(60);
            var token = GenerateJwtToken(request.Email, expireAt);

            var response = new LoginResponse
            {
                Token = token,
                TokenType = "Bearer",
                ExpireAt = expireAt,
                Email = request.Email,
                Role = "Admin"
            };

            return Ok(response);
        }
        private string GenerateJwtToken(string email, DateTime expireAt)
        {
            var claims = new[]
            {
        new Claim(ClaimTypes.Email, email),
        new Claim(ClaimTypes.Name, email),
        new Claim(ClaimTypes.Role, "Admin")
    };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
            );

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expireAt,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}