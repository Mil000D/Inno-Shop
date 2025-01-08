using EventBus;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserManagementService.DAL.Context;
using UserManagementService.DTOs;
using UserManagementService.Enums;
using UserManagementService.Models;

namespace UserManagementService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(UserDbContext context, IConfiguration configuration, IPublishEndpoint publishEndpoint) : ControllerBase
    {
        private readonly UserDbContext _context = context;
        private readonly IConfiguration _configuration = configuration;
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO login)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == login.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash))
            {
                return Unauthorized();
            }

            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }


        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("Role", "User"),
                new Claim(ClaimTypes.Role, "User")
            };

            if (user.Role == Role.Admin)
            {
                claims = new[] { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                                 new Claim("Role", "Admin"),
                                 new Claim("Role", "User"),
                                 new Claim(ClaimTypes.Role, "Admin"),
                                 new Claim(ClaimTypes.Role, "User")};
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("7CCA9FF4A0D5427FBB382974B5E2AC092BB38974B5E4B5E2AC092BB38974B5E2AC092AC09"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "issuer",
                audience: "audience",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
